// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System;
using SixLabors.ImageSharp.Metadata.Profiles.Icc;
using Xunit;

namespace SixLabors.ImageSharp.Tests.Metadata.Profiles.ICC.DataReader
{
    [Trait("Profile", "Icc")]
    public class IccDataReaderLutTests
    {
        [Theory]
        [MemberData(nameof(IccTestDataLut.ClutTestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadClut(byte[] data, IccClut expected, int inChannelCount, int outChannelCount, bool isFloat)
        {
            IccDataReader reader = CreateReader(data);

            IccClut output = reader.ReadClut(inChannelCount, outChannelCount, isFloat);

            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(IccTestDataLut.Clut8TestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadClut8(byte[] data, IccClut expected, int inChannelCount, int outChannelCount, byte[] gridPointCount)
        {
            IccDataReader reader = CreateReader(data);

            IccClut output = reader.ReadClut8(inChannelCount, outChannelCount, gridPointCount);

            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(IccTestDataLut.Clut16TestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadClut16(byte[] data, IccClut expected, int inChannelCount, int outChannelCount, byte[] gridPointCount)
        {
            IccDataReader reader = CreateReader(data);

            IccClut output = reader.ReadClut16(inChannelCount, outChannelCount, gridPointCount);

            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(IccTestDataLut.ClutF32TestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadClutF32(byte[] data, IccClut expected, int inChannelCount, int outChannelCount, byte[] gridPointCount)
        {
            IccDataReader reader = CreateReader(data);

            IccClut output = reader.ReadClutF32(inChannelCount, outChannelCount, gridPointCount);

            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(IccTestDataLut.Lut8TestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadLut8(byte[] data, IccLut expected)
        {
            IccDataReader reader = CreateReader(data);

            IccLut output = reader.ReadLut8();

            Assert.Equal(expected, output);
        }

        [Theory]
        [MemberData(nameof(IccTestDataLut.Lut16TestData), MemberType = typeof(IccTestDataLut))]
        internal void ReadLut16(byte[] data, IccLut expected, int count)
        {
            IccDataReader reader = CreateReader(data);

            IccLut output = reader.ReadLut16(count);

            Assert.Equal(expected, output);
        }

        // GHSA-gwg2-r3hj-4w44: CLUT dimensions are untrusted and must not drive allocations
        // larger than the data that is actually present.
        [Theory]
        [InlineData(15, 15, 3, 1)]
        [InlineData(15, 15, 3, 2)]
        [InlineData(15, 15, 3, 4)]
        [InlineData(16, 65535, 255, 4)]
        internal void ReadClut_TruncatedData_Throws(int inChannelCount, int outChannelCount, byte gridPoints, int bytesPerValue)
        {
            byte[] gridPointCount = new byte[inChannelCount];
            gridPointCount.AsSpan().Fill(gridPoints);
            IccDataReader reader = CreateReader(new byte[16]);

            Assert.Throws<InvalidIccProfileException>(() =>
            {
                switch (bytesPerValue)
                {
                    case 1:
                        reader.ReadClut8(inChannelCount, outChannelCount, gridPointCount);
                        break;
                    case 2:
                        reader.ReadClut16(inChannelCount, outChannelCount, gridPointCount);
                        break;
                    default:
                        reader.ReadClutF32(inChannelCount, outChannelCount, gridPointCount);
                        break;
                }
            });
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(17, 1)]
        [InlineData(1, 0)]
        internal void ReadClut_InvalidChannelCount_Throws(int inChannelCount, int outChannelCount)
        {
            IccDataReader reader = CreateReader(new byte[64]);

            Assert.Throws<InvalidIccProfileException>(() => reader.ReadClut(inChannelCount, outChannelCount, true));
        }

        [Fact]
        public void IccProfile_TruncatedMpetClut_TagIsSkipped()
        {
            // An ICC v4 profile with an A2B0 mpet tag whose CLUT declares 15 input and 15 output channels
            // with 3 grid points each (3^15 * 15 values), but ends right after the grid descriptor.
            const int tagOffset = 144;
            const int elementOffset = 168;
            byte[] profile = new byte[200];
            WriteUInt32(profile, 0, (uint)profile.Length);
            WriteUInt32(profile, 8, 0x04000000);
            WriteUInt32(profile, 12, 0x6D6E7472); // mntr
            WriteUInt32(profile, 16, 0x52474220); // RGB
            WriteUInt32(profile, 20, 0x58595A20); // XYZ
            WriteUInt32(profile, 36, 0x61637370); // acsp
            WriteUInt32(profile, 128, 1);
            WriteUInt32(profile, 132, 0x41324230); // A2B0
            WriteUInt32(profile, 136, tagOffset);
            WriteUInt32(profile, 140, 56);
            WriteUInt32(profile, tagOffset, 0x6D706574); // mpet
            WriteUInt32(profile, tagOffset + 12, 1);
            WriteUInt32(profile, tagOffset + 16, 24);
            WriteUInt32(profile, tagOffset + 20, 32);
            WriteUInt32(profile, elementOffset, 0x636C7574); // clut
            profile[elementOffset + 5] = 15;
            profile[elementOffset + 7] = 15;
            for (int i = 0; i < 15; i++)
            {
                profile[elementOffset + 8 + i] = 3;
            }

            var iccProfile = new IccProfile(profile);

            Assert.Empty(iccProfile.Entries);
        }

        private static void WriteUInt32(byte[] data, int offset, uint value)
            => System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(data.AsSpan(offset, 4), value);

        private static IccDataReader CreateReader(byte[] data)
        {
            return new IccDataReader(data);
        }
    }
}
