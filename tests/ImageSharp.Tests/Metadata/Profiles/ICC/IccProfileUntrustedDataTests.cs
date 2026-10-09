// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Metadata.Profiles.Icc
{
    /// <summary>
    /// Counts and sizes in an ICC profile are untrusted. Parsing a small, malformed profile must not allocate
    /// memory far beyond the profile's size. Before FreeDotnetImageSharp 2.1.15, a profile of a few hundred bytes
    /// could make the tag readers allocate gigabytes (the same class of issue as GHSA-gwg2-r3hj-4w44).
    /// </summary>
    [Trait("Profile", "Icc")]
    public class IccProfileUntrustedDataTests
    {
        private const long MaxAllowedAllocation = 16_000_000;

        // Names of the test data sets whose type signature has a different name.
        private static readonly Dictionary<string, string> SignatureAliases = new()
        {
            ["Fix16Array"] = nameof(IccTypeSignature.S15Fixed16Array),
            ["UFix16Array"] = nameof(IccTypeSignature.U16Fixed16Array),
        };

        public static IEnumerable<object[]> TagTypes =>
            typeof(IccTestDataTagDataEntry).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.Name.EndsWith("TagDataEntryTestData", StringComparison.Ordinal) && f.FieldType == typeof(object[][]))
                .Select(f => f.Name.Substring(0, f.Name.Length - "TagDataEntryTestData".Length))
                .Where(name => TryGetSignature(name, out _))
                .Distinct()
                .Select(name => new object[] { name });

        [Theory]
        [MemberData(nameof(TagTypes))]
        public void ParsingTagWithLargeCounts_DoesNotAllocateExcessively(string tagTypeName)
        {
            Assert.True(TryGetSignature(tagTypeName, out IccTypeSignature signature));
            FieldInfo field = typeof(IccTestDataTagDataEntry).GetField(tagTypeName + "TagDataEntryTestData");
            var failures = new List<string>();

            foreach (object[] row in (object[][])field.GetValue(null))
            {
                byte[] body = (byte[])row[0];

                // Overwrite each position in turn with large values that a count or size field could hold.
                for (int position = 0; position < body.Length; position++)
                {
                    foreach ((int width, uint value) in new[] { (1, 0xFFu), (2, 0xFFFFu), (4, 0x00FFFFFFu), (4, 0x0FFFFFFFu), (4, 0x7FFFFFFFu), (4, 0xFFFFFFFFu) })
                    {
                        if (position + width > body.Length)
                        {
                            continue;
                        }

                        byte[] mutated = (byte[])body.Clone();
                        for (int i = 0; i < width; i++)
                        {
                            mutated[position + i] = (byte)(value >> (8 * (width - 1 - i)));
                        }

                        long allocated = ParseAndMeasure(BuildProfile(signature, mutated));
                        if (allocated > MaxAllowedAllocation)
                        {
                            failures.Add($"position {position}, value 0x{value:X}: {allocated:N0} bytes allocated");
                        }
                    }
                }
            }

            Assert.True(failures.Count == 0, $"{tagTypeName}: {string.Join("; ", failures.Take(5))}");
        }

        private static bool TryGetSignature(string tagTypeName, out IccTypeSignature signature)
        {
            string name = SignatureAliases.TryGetValue(tagTypeName, out string alias) ? alias : tagTypeName;
            return Enum.TryParse(name, true, out signature) && signature != IccTypeSignature.Unknown;
        }

        private static long ParseAndMeasure(byte[] profile)
        {
#if NETCOREAPP
            long before = GC.GetAllocatedBytesForCurrentThread();
#endif
            try
            {
                _ = new IccProfile(profile).Entries;
            }
            catch (Exception)
            {
                // Malformed tags may throw; only the resources used while parsing matter here.
            }

#if NETCOREAPP
            return GC.GetAllocatedBytesForCurrentThread() - before;
#else
            // GC.GetAllocatedBytesForCurrentThread is not available on .NET Framework; the cases still run.
            return 0;
#endif
        }

        private static byte[] BuildProfile(IccTypeSignature signature, byte[] body)
        {
            // 128-byte header, a tag table with one entry, then the tag (type signature, 4 reserved bytes, body).
            const int tagOffset = 128 + 4 + 12;
            byte[] profile = new byte[tagOffset + 8 + body.Length];
            WriteUInt32(profile, 0, (uint)profile.Length);
            WriteUInt32(profile, 36, 0x61637370); // 'acsp'
            WriteUInt32(profile, 128, 1);
            WriteUInt32(profile, 132, 0x41324230); // 'A2B0'; the tag signature does not restrict the tag type
            WriteUInt32(profile, 136, tagOffset);
            WriteUInt32(profile, 140, (uint)(8 + body.Length));
            WriteUInt32(profile, tagOffset, (uint)signature);
            Buffer.BlockCopy(body, 0, profile, tagOffset + 8, body.Length);
            return profile;
        }

        private static void WriteUInt32(byte[] data, int offset, uint value)
        {
            data[offset] = (byte)(value >> 24);
            data[offset + 1] = (byte)(value >> 16);
            data[offset + 2] = (byte)(value >> 8);
            data[offset + 3] = (byte)value;
        }
    }
}
