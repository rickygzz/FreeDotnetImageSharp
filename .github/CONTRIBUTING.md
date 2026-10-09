# Contributing to FreeDotnetImageSharp

FreeDotnetImageSharp is a community-maintained fork of SixLabors.ImageSharp 2.1.13. Its goal is to keep the 2.1.x API available under the Apache License 2.0, with security and compatibility fixes.

## Scope

- Security fixes, bug fixes and dependency updates for the 2.1.x API are welcome.
- New features are only accepted if they don't break binary compatibility. The assembly name (`SixLabors.ImageSharp`), assembly version (2.0.0.0) and public API stay the same, so the package remains a drop-in replacement.

## Licensing of contributions

- All contributions are licensed under the [Apache License 2.0](../LICENSE).
- **Do not copy code from SixLabors.ImageSharp 3.x or later.** Those versions use the Six Labors Split License, which is not compatible with this project. Fixes for issues that were also fixed upstream must be written independently.
- Keep the existing copyright headers in files you change. You may add your own line below them.

## Building and testing

```
dotnet build -c Release
dotnet test tests/ImageSharp.Tests/ImageSharp.Tests.csproj -c Release -f net10.0
```

The test images are stored in Git LFS. Run `git lfs pull` after cloning.

## Reporting security issues

Do not open a public issue. Use GitHub's private vulnerability reporting on this repository.
