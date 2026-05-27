using EndlessOnlinePatcher.Models;
using Moffat.EndlessOnline.SDK.Data;
using OneOf;
using OneOf.Types;
using System.Diagnostics;
using System.Net.Sockets;

namespace EndlessOnlinePatcher.Services;

public interface IServerVersionFetcher
{
    OneOf<Version, Error<string>> Get();
}

public partial class ServerVersionFetcher : IServerVersionFetcher
{
    public OneOf<Version, Error<string>> Get()
    {
        var (serverAddress, serverPort) = EoConfigReader.ReadConnectionSettings();
        var client = new TcpClient();
        Debug.WriteLine($"Connecting to {serverAddress}...");
        try
        {
            var connected = client.ConnectAsync(serverAddress, serverPort).Wait(TimeSpan.FromSeconds(10));
            if (!connected)
            {
                Debug.WriteLine($"Timed out connecting to {serverAddress}:{serverPort}");
                return new Error<string>($"Timed out connecting to {serverAddress}:{serverPort}");
            }
            Debug.WriteLine("Connected!");

            byte[] buf = [0xFF, 0xFF, 0x1E, 0x12, 0xFE, 0x1, 0x5, 0x2, 0x72, 0xB, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30];

            var sizeBytes = NumberEncoder.EncodeNumber(buf.Length);

            byte[] payload = [sizeBytes[0], sizeBytes[1]];
            payload = payload.Concat(buf).ToArray();

            Debug.WriteLine("Sending version payload...");
            client.Client.Send(payload);

            sizeBytes = new byte[2];
            client.Client.Receive(sizeBytes);

            var size = NumberEncoder.DecodeNumber(sizeBytes);

            buf = new byte[size];

            client.Client.Receive(buf);

            var major = NumberEncoder.DecodeNumber([buf[3]]);
            var minor = NumberEncoder.DecodeNumber([buf[4]]);
            var build = NumberEncoder.DecodeNumber([buf[5]]);

            return new Version(major, minor, build);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception thrown at connecting to {serverAddress}:{serverPort}: {e.Message}");
            return new Error<string>($"Could not connect to {serverAddress}:{serverPort}");
        }
    }
}
