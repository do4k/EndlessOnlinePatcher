namespace EndlessOnlinePatcher.Services;

public interface ILocalVersionRepository
{
    Version Get();
    void Save(Version version);
}

public class LocalVersionRepository : ILocalVersionRepository
{
    public Version Get()
    {
        if (File.Exists("version.txt") is false)
            return new Version(0, 0, 0);

        var storedVersion = File.ReadAllText("version.txt");
        return new Version(storedVersion);
    }

    public void Save(Version version)
    {
        File.WriteAllText("version.txt", $"{version.Major}.{version.Minor}.{version.Build}");
    }
}
