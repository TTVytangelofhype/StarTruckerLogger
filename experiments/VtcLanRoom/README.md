# Experimental LAN/VPN VTC Room

**This is not playable Star Trucker multiplayer.** It is a standalone companion prototype that exchanges driver status messages. It does not modify the game, add trucks to the world, synchronize physics, or share missions.

## Run on Windows with the .NET 8 SDK
Host: `dotnet run --project experiments/VtcLanRoom -- host 7777`

Friend: `dotnet run --project experiments/VtcLanRoom -- join <HOST-VPN-IP> 7777`

Connect through the same **Radmin VPN** or **LogMeIn Hamachi** network. Allow inbound TCP 7777 on the host's Windows Firewall for the trusted VPN interface only. Do **not** forward the port publicly. This prototype has **no authentication or encryption** and must only be used with trusted people on a private VPN.

## Milestones
1. Shared nickname/status presence (initial prototype).
2. Add secure invitation codes, authenticated sessions, reconnects and clean shutdown.
3. Connect to actual validated logger events, not simulated progress.
4. Investigate game instrumentation/modding; determine if entities can spawn remotely and whether truck movement, trailers and mission state can safely sync.
5. Only describe it as in-game multiplayer when those capabilities have been demonstrated on real game installations.

Do not synchronize or overwrite original save files. Back up saves before any later mod testing.
