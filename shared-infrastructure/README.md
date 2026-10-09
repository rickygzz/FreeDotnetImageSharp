# Shared infrastructure

Build configuration and shared helper sources used by FreeDotnetImageSharp:

| Path | Purpose |
|---|---|
| `msbuild/props`, `msbuild/targets` | `FreeDotnet.*` MSBuild files imported by the repository's `Directory.Build.props/targets` |
| `src/SharedInfrastructure/` | Shared helper code (`Guard`, `DebugGuard`, ...) compiled into the library through `SharedInfrastructure.projitems` |
| `SixLabors.snk` | Strong-name key. Keeps the assembly identity compatible with the original package |
| `freedotnet.ruleset`, `freedotnet.tests.ruleset`, `stylecop.json` | Code analysis settings |
| `.editorconfig`, `.gitattributes` | Copied to the repository root on every build. Edit them here, not in the root |

This directory was vendored from [SixLabors/SharedInfrastructure](https://github.com/SixLabors/SharedInfrastructure) at commit `59ce17f` (Apache License 2.0) and has since been modified. The standalone SharedInfrastructure solution and its test project were removed; they are not part of this repository's build. See the [NOTICE](../NOTICE) file in the repository root.
