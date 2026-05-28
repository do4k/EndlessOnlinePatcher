namespace EndlessOnlinePatcher.Models;

public static class EoConfigReader
{
    private static string ConfigPath =>
        Path.Combine(EndlessOnlineDirectory.Get().FullName, "config", "setup.ini");

    public static (string Host, int Port) ReadConnectionSettings()
    {
        var s = ReadSettings();
        return (s.Host, s.Port);
    }

    public static EoSettings ReadSettings()
    {
        var defaults = new EoSettings();
        if (!File.Exists(ConfigPath))
            return defaults;

        string? host = null;
        int? port = null, music = null, sound = null, sfx = null, instruments = null;
        bool? wasd = null;
        var section = "";

        foreach (var line in File.ReadLines(ConfigPath))
        {
            var t = line.Trim();
            if (t.StartsWith('['))
            {
                section = t[1..^1].ToUpperInvariant();
                continue;
            }
            if (t.StartsWith('#') || !t.Contains('=')) continue;

            var eq = t.IndexOf('=');
            var key = t[..eq].Trim();
            var val = t[(eq + 1)..].Trim();

            switch (section)
            {
                case "CONNECTION":
                    if (key.Equals("Host", StringComparison.OrdinalIgnoreCase)) host = val;
                    else if (key.Equals("Port", StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out var p)) port = p;
                    break;
                case "SOUND":
                    if      (key.Equals("Music",       StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out var m)) music = m;
                    else if (key.Equals("Sound",       StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out var s)) sound = s;
                    else if (key.Equals("Sfx",         StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out var x)) sfx = x;
                    else if (key.Equals("Instruments", StringComparison.OrdinalIgnoreCase) && int.TryParse(val, out var i)) instruments = i;
                    break;
                case "INPUTS":
                    if (key.Equals("WasdKeys", StringComparison.OrdinalIgnoreCase)) wasd = val.Equals("on", StringComparison.OrdinalIgnoreCase);
                    break;
            }
        }

        return new EoSettings
        {
            Host = host ?? defaults.Host,
            Port = port ?? defaults.Port,
            MusicVolume = music ?? defaults.MusicVolume,
            SoundVolume = sound ?? defaults.SoundVolume,
            SfxVolume = sfx ?? defaults.SfxVolume,
            InstrumentsVolume = instruments ?? defaults.InstrumentsVolume,
            WasdKeys = wasd ?? defaults.WasdKeys,
        };
    }
}
