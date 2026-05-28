namespace EndlessOnlinePatcher.Models;

public record EoSettings
{
    public string Host { get; init; } = "game.endless-online.com";
    public int Port { get; init; } = 8078;
    public int MusicVolume { get; init; } = 50;
    public int SoundVolume { get; init; } = 50;
    public int SfxVolume { get; init; } = 50;
    public int InstrumentsVolume { get; init; } = 50;
    public bool WasdKeys { get; init; } = false;
}
