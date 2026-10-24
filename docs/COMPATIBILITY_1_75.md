# Star Trucker v1.75.x compatibility work

This branch is reserved for updating the Windows VTC logger without changing the stable `master` branch.

## Audit (24 October 2026)
- Latest verified official game patch: **1.75.2** (21 September 2026).
- New job families introduced in 1.73 and 1.75 include Handyman, Courier and Cultivation ventures.
- `StarTruckerClient.cs` currently fakes haul progress with `Random`; this is **not** game telemetry.
- `JobWatcher.cs` and `DeliveryLogger.cs` expect files in `C:\Games\StarTrucker\logs`. These are assumed external/mod output files, not a verified game API.
- `DeliveryLogger.cs` assumes positional pipe-delimited event text and can throw on unexpected fields.
- The WinForms project targets .NET 6 Windows, and multiple `Main` methods need an explicit entry-point/build audit.

## Acceptance criteria before release
1. Validate actual game version, mod-loader version, and event source on a **real 1.75.2 installation**.
2. Record redacted sample event data for original hauls and the new ventures, with consent.
3. Replace hard-coded watch paths with user settings; never create dummy game telemetry folders.
4. Parse all inputs defensively, add deduplication and durable offline queue.
5. Post completed hauls only after genuine completion events; clearly label manual/simulated entries.
6. Replace user-ID-only API writes with properly authenticated server-side authorization.
7. Test clean Windows build and regression coverage before merging into master.

**Current status:** compatibility audit and plan only. The branch does not claim verified 1.75.2 telemetry support.
