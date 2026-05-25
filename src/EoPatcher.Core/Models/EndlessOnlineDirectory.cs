namespace EoPatcher.Models;

public static class EndlessOnlineDirectory
{
    public static DirectoryInfo Get()
    {
        if (File.Exists("../Endless.exe")) return Directory.GetParent(Directory.GetCurrentDirectory())!;
        else return new DirectoryInfo("C:/Program Files (x86)/Endless Online/");
    }

    public static string GetExe() => Get().FullName + "/Endless.exe";
}
