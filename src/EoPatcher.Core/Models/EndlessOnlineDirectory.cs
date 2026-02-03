using System.Runtime.InteropServices;

namespace EoPatcher.Models;

public static class EndlessOnlineDirectory
{
    public static DirectoryInfo Get()
    {
        if (File.Exists("Endless.exe")) return new DirectoryInfo(Directory.GetCurrentDirectory());
        if (File.Exists("../Endless.exe")) return Directory.GetParent(Directory.GetCurrentDirectory())!;
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
             return new DirectoryInfo("C:/Program Files (x86)/Endless Online/");
        }

        return new DirectoryInfo(Directory.GetCurrentDirectory());
    }

    public static string GetExe() => Path.Combine(Get().FullName, "Endless.exe");
}
