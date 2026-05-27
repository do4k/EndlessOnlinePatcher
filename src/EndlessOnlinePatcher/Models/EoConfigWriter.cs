namespace EndlessOnlinePatcher.Models;

public static class EoConfigWriter
{
    private static string ConfigPath =>
        Path.Combine(EndlessOnlineDirectory.Get().FullName, "config", "setup.ini");

    public static void WriteSettings(EoSettings settings)
    {
        if (!File.Exists(ConfigPath)) return;

        var updates = new Dictionary<(string Section, string Key), string>
        {
            [("CONNECTION", "Host")] = settings.Host,
            [("CONNECTION", "Port")] = settings.Port.ToString(),
            [("SOUND", "Music")]     = settings.MusicVolume.ToString(),
            [("SOUND", "Sound")]     = OnOff(settings.SoundEnabled),
            [("SOUND", "Sfx")]       = OnOff(settings.SfxEnabled),
            [("INPUTS", "WasdKeys")] = OnOff(settings.WasdKeys),
        };

        var lines = File.ReadAllLines(ConfigPath);
        var section = "";

        for (var i = 0; i < lines.Length; i++)
        {
            var t = lines[i].Trim();
            if (t.StartsWith('['))
            {
                section = t[1..^1].ToUpperInvariant();
                continue;
            }
            if (t.StartsWith('#') || !t.Contains('=')) continue;

            var eq = t.IndexOf('=');
            var key = t[..eq].Trim();

            if (updates.TryGetValue((section, key), out var newVal))
                lines[i] = $"{key}={newVal}";
        }

        File.WriteAllLines(ConfigPath, lines);
    }

    private static string OnOff(bool value) => value ? "on" : "off";
}
