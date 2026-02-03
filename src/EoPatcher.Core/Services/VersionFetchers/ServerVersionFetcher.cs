using Moffat.EndlessOnline.SDK.Data;
using OneOf;
using OneOf.Types;
using System.Diagnostics;
using System.Net.Sockets;

namespace EoPatcher.Services.VersionFetchers;

public interface IServerVersionFetcher
{
    OneOf<Version, Error<string>> Get();
}

public partial class ServerVersionFetcher : IServerVersionFetcher
{
    public OneOf<Version, Error<string>> Get()
    {
        var serverAddress = "game.endless-online.com";
        var serverPort = 8078;
        var client = new TcpClient();
        Debug.WriteLine($"Connecting to {serverAddress}...");
        try
        {
            client.Connect(serverAddress, serverPort);
            Debug.WriteLine("Connected!");

            byte[] buf = [0xFF, 0xFF, 0x1E, 0x12, 0xFE, 0x1, 0x5, 0x2, 0x72, 0xB, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30];

            var sizeBytes = NumberEncoder.EncodeNumber(buf.Length);

            byte[] payload = [sizeBytes[0], sizeBytes[1]];
            payload = payload.Concat(buf).ToArray();

            Debug.WriteLine("Sending version payload...");
            client.Client.Send(payload);

            sizeBytes = new byte[2];
            var lengthBytes = client.Client.Receive(sizeBytes);
            if (lengthBytes == 0)
            {
                return new Error<string>("Connection closed by server");
            }

            var size = NumberEncoder.DecodeNumber(sizeBytes);

            buf = new byte[size];

            client.Client.Receive(buf);

            if (buf.Length < 6)
            {
                 return new Error<string>($"Received invalid data size from server. Expected at least 6 bytes but got {buf.Length}");
            }

            var major = NumberEncoder.DecodeNumber([buf[3]]);
            var minor = NumberEncoder.DecodeNumber([buf[4]]);
            var build = NumberEncoder.DecodeNumber([buf[5]]);

            return new Version(major, minor, build);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Exception thrown at connecting to {serverAddress}:{serverPort}: {e.Message}");
            return new Error<string>($"Could not connect to {serverAddress}:{serverPort} - {e.Message}");
        }
    }
}