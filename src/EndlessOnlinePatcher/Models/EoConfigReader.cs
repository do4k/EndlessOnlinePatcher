namespace EndlessOnlinePatcher.Models;

public static class EoConfigReader
{
    public static (string Host, int Port) ReadConnectionSettings()
    {
        const string defaultHost = "game.endless-online.com";
        const int defaultPort = 8078;

        var configPath = Path.Combine(EndlessOnlineDirectory.Get().FullName, "config", "setup.ini");
        if (!File.Exists(configPath))
            return (defaultHost, defaultPort);

        string? host = null;
        int? port = null;
        var inConnectionSection = false;

        foreach (var line in File.ReadLines(configPath))
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith('['))
            {
                inConnectionSection = trimmed.Equals("[CONNECTION]", StringComparison.OrdinalIgnoreCase);
                continue;
            }
            if (!inConnectionSection || trimmed.StartsWith('#') || !trimmed.Contains('='))
                continue;

            var eq = trimmed.IndexOf('=');
            var key = trimmed[..eq].Trim();
            var value = trimmed[(eq + 1)..].Trim();

            if (key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                host = value;
            else if (key.Equals("Port", StringComparison.OrdinalIgnoreCase) && int.TryParse(value, out var p))
                port = p;

            if (host != null && port != null) break;
        }

        return (host ?? defaultHost, port ?? defaultPort);
    }
}
