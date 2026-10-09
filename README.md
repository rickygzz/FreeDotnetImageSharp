# FreeDotnetImageSharp

[![License: Apache 2.0](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE)

**FreeDotnetImageSharp is a community-maintained fork of [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 2.1.13, the last release line published under the Apache License 2.0.** It exists to ship security fixes for the 2.1.x API while staying under Apache-2.0. It is used by [FreeDotnetPOI](https://github.com/rickygzz/FreeDotnetPOI).

The assembly name (`SixLabors.ImageSharp`), strong-name identity (version 2.0.0.0) and namespaces are unchanged, so it is a drop-in replacement for `SixLabors.ImageSharp` 2.1.x:

```
dotnet add package FreeDotnetImageSharp
```

Do not reference both `FreeDotnetImageSharp` and `SixLabors.ImageSharp` in the same project.

## Security fixes

| Version | Advisory | Issue |
|---|---|---|
| 2.1.14 | [GHSA-j9gm-c75j-xc9q](https://github.com/advisories/GHSA-j9gm-c75j-xc9q) | TIFF CCITT T4 encoder writes past its output buffer |
| 2.1.14 | [GHSA-jjfr-hcj7-qf5w](https://github.com/advisories/GHSA-jjfr-hcj7-qf5w) | TIFF CCITT T6 encoder writes past its output buffer |
| 2.1.14 | [GHSA-j3p4-wp97-rph4](https://github.com/advisories/GHSA-j3p4-wp97-rph4) | HistogramEqualization uses an unchecked luminance as a histogram index |
| 2.1.14 | [GHSA-gwg2-r3hj-4w44](https://github.com/advisories/GHSA-gwg2-r3hj-4w44) | ICC CLUT parsing allocates from untrusted dimensions |
| 2.1.14 | [GHSA-wmxv-xphr-5c9g](https://github.com/advisories/GHSA-wmxv-xphr-5c9g) | BigTIFF IFD entry count can cause a non-progressing loop |

This project is not affiliated with or endorsed by Six Labors. All credit for the original work goes to Six Labors and the ImageSharp contributors. Security fixes here are independent implementations; no code from ImageSharp 3.x or later (Six Labors Split License) is included. See [NOTICE](NOTICE).

## About

ImageSharp is a fully managed, cross-platform 2D graphics library for .NET. This fork targets .NET Framework 4.7.2, .NET Standard 2.0/2.1 and .NET Core 2.1/3.1, so it runs on any modern .NET version.

The API is identical to ImageSharp 2.1.x. Documentation and samples written for ImageSharp 2.x apply unchanged. When reading upstream documentation, make sure it is for version 2.x, because 3.x and later changed parts of the API.

## Building

```
git lfs pull
dotnet build -c Release
dotnet test tests/ImageSharp.Tests/ImageSharp.Tests.csproj -c Release -f net10.0
```

The test images are stored in Git LFS, so `git lfs pull` is needed before running the tests.

## Contributing

See [CONTRIBUTING.md](.github/CONTRIBUTING.md). Contributions must not include code from ImageSharp 3.x or later.

## License

Licensed under the [Apache License 2.0](LICENSE). See [NOTICE](NOTICE) and [THIRD-PARTY-NOTICES.TXT](THIRD-PARTY-NOTICES.TXT).
