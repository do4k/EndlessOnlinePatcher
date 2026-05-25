# Endless Online Patcher

## Project Overview
A WinForms patcher for the MMO [Endless Online](https://endless-online.com). The primary deployment target is **Steam Deck via Proton** — the `.exe` is added to Steam as a non-Steam game with a Proton compatibility layer, allowing Steam Deck users to patch and launch EO without leaving the Steam environment.

## Architecture

```
src/
  EoPatcher.Core/   — net8.0, business logic (no Windows dependency)
  EoPatcher.UI/     — net8.0-windows, WinForms custom-skinned UI
```

### Core Components
| File | Role |
|---|---|
| `ServerVersionFetcher` | TCP-connects to `game.endless-online.com:8078`, sends an EO init packet, reads the server's reported client version number |
| `LocalVersionRepository` | Reads/writes `version.txt` (relative to CWD) to track the locally installed version |
| `HttpService` | Scrapes `endless-online.com/client/download.html` for the latest zip link; downloads with progress reporting |
| `FileService` | Extracts the patch zip and copies files to the EO install directory; skips `config/` to preserve local settings |
| `PatchOrchestrator` | Coordinates: clean temp files → download → extract → save version; implements `IDisposable` for cleanup |
| `EndlessOnlineDirectory` | Resolves EO install path: checks `../Endless.exe` first (patcher expected at `Endless Online/Patcher/eopatcher.exe`), falls back to `C:/Program Files (x86)/Endless Online/` |
| `Windows.StartEO` | Launches `Endless.exe` via Win32 token duplication so EO runs as the desktop (non-elevated) user |

### UI
The form is borderless and custom-skinned with EO assets. Window dragging is handled manually via `MouseDown/Move/Up`. All image buttons swap to hover variants on `MouseEnter`.

Button states:
- On load: all hidden while fetching versions
- Up to date: Launch + Exit shown
- Update available: Patch + Skip + Exit shown (Skip = launch without patching)
- Error connecting: Launch + Exit shown (allows launching without version check)

## Build

Single self-contained Windows exe:
```bash
dotnet publish src/EoPatcher.UI/EoPatcher.UI.csproj \
  -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -o ./publish
```
Output: `publish/eopatcher.exe`

## Releasing

Push to `main` triggers `.github/workflows/release.yml` which builds and uploads `eopatcher.exe` to a GitHub Release tagged `v{AssemblyVersion}`. To cut a new versioned release, bump `AssemblyVersion` and `FileVersion` in `src/EoPatcher.UI/EoPatcher.UI.csproj` before pushing.

## Steam Deck / Proton Notes

- The patcher is a Windows `.exe` running under Proton — add it to Steam as a non-Steam game, set Proton as the compatibility tool
- `EndlessOnlineDirectory` resolves paths using Windows-style paths (`C:/Program Files (x86)/...`), which map correctly to the Wine prefix under Proton
- `Windows.StartEO` uses Win32 token duplication to launch EO as a non-elevated process. **Under Proton, this may not work** if Wine's Explorer shell (which provides the shell window handle) is absent. If EO fails to launch, the fallback would be a simple `Process.Start(EndlessOnlineDirectory.GetExe())`

## Known Issues / Todo
- No EO directory picker UI — only auto-detects `../Endless.exe` or falls back to default install path
- No auto-launch config option (launch after patch without clicking)
- `Windows.StartEO` may need a Proton-compatible fallback (see above)
- No test suite
- News embed not implemented
