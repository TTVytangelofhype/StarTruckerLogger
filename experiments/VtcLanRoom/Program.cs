using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

// Experimental VPN-only VTC companion, NOT in-game multiplayer.
// dotnet run -- host 7777
// dotnet run -- join 25.1.2.3 7777
const int maxLineLength = 2048;
if (args.Length == 0)
{
    Console.WriteLine("Usage: host [port] OR join <VPN-IP> [port]");
    return;
}
if (args[0].Equals("host", StringComparison.OrdinalIgnoreCase))
{
    int port = args.Length > 1 ? int.Parse(args[1]) : 7777;
    var listener = new TcpListener(IPAddress.Any, port);
    listener.Start();
    Console.WriteLine($"VTC room on TCP {port}. Only allow trusted VPN peers.");
    var clients = new List<TcpClient>();
    var gate = new object();
    while (true)
    {
        var peer = await listener.AcceptTcpClientAsync();
        lock (gate) clients.Add(peer);
        _ = Task.Run(async () =>
        {
            try
            {
                using (peer)
                using (var stream = peer.GetStream())
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    while (true)
                    {
                        var line = await reader.ReadLineAsync();
                        if (line == null) break;
                        if (line.Length > maxLineLength) break;
                        string output;
                        try
                        {
                            var msg = JsonSerializer.Deserialize<RoomMessage>(line);
                            if (msg == null || string.IsNullOrWhiteSpace(msg.Driver)) continue;
                            output = JsonSerializer.Serialize(new RoomMessage(
                                msg.Driver[..Math.Min(msg.Driver.Length, 32)],
                                (msg.Status ?? "")[..Math.Min((msg.Status ?? "").Length, 256)],
                                DateTimeOffset.UtcNow));
                        }
                        catch (JsonException) { continue; }
                        List<TcpClient> current;
                        lock (gate) current = clients.ToList();
                        foreach (var target in current)
                        {
                            if (target == peer) continue;
                            try
                            {
                                var bytes = Encoding.UTF8.GetBytes(output + "\n");
                                await target.GetStream().WriteAsync(bytes);
                            }
                            catch (IOException) { }
                            catch (ObjectDisposedException) { }
                        }
                        Console.WriteLine(output);
                    }
                }
            }
            catch (IOException) { }
            finally { lock (gate) clients.Remove(peer); }
        });
    }
}
else if (args[0].Equals("join", StringComparison.OrdinalIgnoreCase) && args.Length > 1)
{
    int port = args.Length > 2 ? int.Parse(args[2]) : 7777;
    using var tcp = new TcpClient();
    await tcp.ConnectAsync(args[1], port);
    using var stream = tcp.GetStream();
    using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };
    using var reader = new StreamReader(stream, Encoding.UTF8);
    Console.Write("Driver nickname: ");
    var name = (Console.ReadLine() ?? "Driver").Trim();
    if (name.Length > 32) name = name[..32];
    Console.WriteLine("Type a status to share with the room, /quit to leave.");
    _ = Task.Run(async () =>
    {
        try
        {
            while (await reader.ReadLineAsync() is { } line) Console.WriteLine("ROOM: " + line);
        }
        catch (IOException) { }
    });
    while (true)
    {
        var status = Console.ReadLine();
        if (status == null || status == "/quit") break;
        await writer.WriteLineAsync(JsonSerializer.Serialize(new RoomMessage(name, status, DateTimeOffset.UtcNow)));
    }
}
else Console.WriteLine("Usage: host [port] OR join <VPN-IP> [port]");

record RoomMessage(string Driver, string? Status, DateTimeOffset Timestamp);
