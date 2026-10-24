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


## 🌐 Experimental LAN/VPN VTC room

This branch adds an **early command-line companion**, separate from the game and the Windows logger. Drivers can exchange nickname/status messages over a trusted **Radmin VPN** or **LogMeIn Hamachi** network.

**Not implemented:** in-game shared trucks, truck movement, docking, traffic, deliveries, world state, save synchronisation, or gameplay co-op.

### Quick start

Install the .NET 8 SDK and connect both machines to the same private VPN. On the **host**:

```powershell
dotnet run --project experiments/VtcLanRoom -- host 7777
```

On a **friend's PC**, substitute the host's VPN IPv4 address:

```powershell
dotnet run --project experiments/VtcLanRoom -- join 25.1.2.3 7777
```

The address above is **illustrative only**; use the actual address shown in Radmin/Hamachi. Enter a nickname and type status messages. Allow inbound **TCP 7777** on the host only for trusted VPN peers. **Do not port-forward to the public Internet.** This prototype has **no authentication or transport encryption**; only use it with people you trust.

See [the companion's own README](experiments/VtcLanRoom/README.md) for details.

### Experimental roadmap

1. Shared driver status and session presence.
2. Invite/authentication, connection robustness and security.
3. Integrate **verified** real haul events from the logger.
4. Research game mod hooks and feasibility of true vehicle-state synchronisation.
5. Validate game stability, back up saves, and only then consider a multiplayer gameplay alpha.

## Roadmap and limitations

The project currently combines working concepts and unfinished prototypes. Some telemetry logic assumes files produced by a separate hook/mod that has not been validated. Some progress calculations are simulated; they do not reflect real gameplay. Do not merge experimental features or report them as fully functional without game, network, and Windows-build testing.

## Credits and licence

Created by **TTVytangelofhype** as a personal fan project. Please read [LICENSE.md](LICENSE.md) before copying, modifying, or redistributing anything; retain attribution and relevant notices.

**Keep on truckin' across the stars!** 🚀
