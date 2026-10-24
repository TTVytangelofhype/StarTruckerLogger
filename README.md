# 🚛 StarTruckerLogger

**An independent, community-made Virtual Trucking Company (VTC) logger and experimental companion for _Star Trucker_.** Built by **TTVytangelofhype**. Not affiliated with the game's developers or publisher.

> ⚠️ **Development software.** The 1.75.x compatibility work and LAN/VPN experiment are not verified as release-ready. This project does not currently add in-game multiplayer.

## Project overview

StarTruckerLogger aims to help players record hauls, track deliveries, and report VTC activity to a connected website. The existing code includes a Windows Forms application, website login/API client, manual haul reporting, and prototype file-based delivery watchers.

**Important:** The legacy console client generates simulated journey progress. The watcher paths (`C:\Games\StarTrucker\logs`) refer to assumed external files and are **not a verified built-in Star Trucker telemetry API**. Automatic game telemetry should not be advertised until tested against real game output.

## Development branches

| Branch | Purpose | Status |
| --- | --- | --- |
| [master](https://github.com/TTVytangelofhype/StarTruckerLogger/tree/master) | Original project | Preserved |
| [update/star-trucker-1.75](https://github.com/TTVytangelofhype/StarTruckerLogger/tree/update/star-trucker-1.75) | Logger modernisation and latest-game compatibility investigation | In development |
| [experimental/lan-multiplayer](https://github.com/TTVytangelofhype/StarTruckerLogger/tree/experimental/lan-multiplayer) | VPN-connected multiplayer *companion* | Early prototype |

## Current development goals

- 🛠️ Modernise the Windows logger for .NET 8.
- 📦 Investigate detection of real delivery completion and newer contract types.
- 🛰️ Replace hard-coded telemetry paths with configurable sources.
- 💾 Queue unsent events and guard against duplicate submissions.
- 🔐 Improve login, API authentication, validation, and error handling.
- 👥 Explore optional shared VTC sessions across a trusted VPN.
- 🎮 **Long-term research only:** assess whether real in-game truck and physics synchronisation is technically possible.

## Build the logger

On Windows, install the **.NET 8 SDK** (or Visual Studio with .NET desktop development). Clone this repository and use the compatibility branch:

```powershell
git clone https://github.com/TTVytangelofhype/StarTruckerLogger.git
cd StarTruckerLogger
git switch update/star-trucker-1.75
dotnet restore
dotnet build StarTruckerLogger.csproj
```

The Windows Forms project targets `net8.0-windows` on the compatibility branch. **A clean Windows build and live game integration have not yet been verified.** The VTC login client is configured for the project's website API; review the source/configuration before use. Never publish real passwords or tokens in logs or bug reports.

## Compatibility

- **Star Trucker:** v1.75.x is the target for compatibility testing, **not yet confirmed as supported**.
- **Windows:** required for the current Windows Forms application.
- **Runtime:** .NET 8 on the modernisation branch.
- **Other game versions:** not verified.

See [the compatibility audit](https://github.com/TTVytangelofhype/StarTruckerLogger/blob/update/star-trucker-1.75/docs/COMPATIBILITY_1_75.md) for known gaps and test requirements.

## 📌 Compatibility branch progress

**Committed:** .NET 8 Windows target, explicit WinForms entry point, and a documented v1.75.x compatibility audit. Actual job parsing, verified 1.75.x telemetry, and complete testing remain outstanding.

## Roadmap and limitations

The project currently combines working concepts and unfinished prototypes. Some telemetry logic assumes files produced by a separate hook/mod that has not been validated. Some progress calculations are simulated; they do not reflect real gameplay. Do not merge experimental features or report them as fully functional without game, network, and Windows-build testing.

## Credits and licence

Created by **TTVytangelofhype** as a personal fan project. Please read [LICENSE.md](LICENSE.md) before copying, modifying, or redistributing anything; retain attribution and relevant notices.

**Keep on truckin' across the stars!** 🚀
