namespace EndlessOnlinePatcher.Models;

public record EoSettings
{
    public string Host { get; init; } = "game.endless-online.com";
    public int Port { get; init; } = 8078;
    public int MusicVolume { get; init; } = 50;
    public bool SoundEnabled { get; init; } = true;
    public bool SfxEnabled { get; init; } = true;
    public bool WasdKeys { get; init; } = false;
}
