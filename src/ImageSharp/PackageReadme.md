# FreeDotnetImageSharp

FreeDotnetImageSharp is a community-maintained fork of [SixLabors.ImageSharp](https://github.com/SixLabors/ImageSharp) 2.1.13, the last release line published under the Apache License 2.0. It ships security fixes for the 2.1.x API while staying under Apache-2.0.

## Drop-in replacement

The assembly name (`SixLabors.ImageSharp`), strong-name identity (version 2.0.0.0), namespaces and public API are unchanged. Replace the package reference and your code keeps working:

```
dotnet remove package SixLabors.ImageSharp
dotnet add package FreeDotnetImageSharp
```

Do not reference both `FreeDotnetImageSharp` and `SixLabors.ImageSharp` in the same project. Documentation written for ImageSharp 2.x applies unchanged.

## Security fixes

| Version | Advisory | Issue |
|---|---|---|
| 2.1.14 | [GHSA-j9gm-c75j-xc9q](https://github.com/advisories/GHSA-j9gm-c75j-xc9q) | TIFF CCITT T4 encoder writes past its output buffer |
| 2.1.14 | [GHSA-jjfr-hcj7-qf5w](https://github.com/advisories/GHSA-jjfr-hcj7-qf5w) | TIFF CCITT T6 encoder writes past its output buffer |
| 2.1.14 | [GHSA-j3p4-wp97-rph4](https://github.com/advisories/GHSA-j3p4-wp97-rph4) | HistogramEqualization uses an unchecked luminance as a histogram index |
| 2.1.14 | [GHSA-gwg2-r3hj-4w44](https://github.com/advisories/GHSA-gwg2-r3hj-4w44) | ICC CLUT parsing allocates from untrusted dimensions |
| 2.1.14 | [GHSA-wmxv-xphr-5c9g](https://github.com/advisories/GHSA-wmxv-xphr-5c9g) | BigTIFF IFD entry count can cause a non-progressing loop |

## License and credits

Licensed under the [Apache License 2.0](https://github.com/rickygzz/FreeDotnetImageSharp/blob/main/LICENSE). All credit for the original work goes to Six Labors and the ImageSharp contributors. This project is not affiliated with or endorsed by Six Labors. Security fixes are independent implementations; no code from ImageSharp 3.x or later (Six Labors Split License) is included.

Source, issues and security reports: https://github.com/rickygzz/FreeDotnetImageSharp
