using EndlessOnlinePatcher.Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace EndlessOnlinePatcher.Services;

public interface IFileService
{
    public Task ExtractPatch(Version version);
}

public class FileService : IFileService
{
    private Action<string> _setPatchTextCallback;

    public FileService(Action<string> setPatchTextCallback)
    {
        _setPatchTextCallback = setPatchTextCallback;
    }

    public async Task ExtractPatch(Version version)
    {
        var localDirectory = EndlessOnlineDirectory.Get().FullName;
        var patchFolder = $"patch-{version}/";

        ZipFile.ExtractToDirectory("patch.zip", patchFolder);
        var patchFiles = Directory.EnumerateFiles(patchFolder, "*", SearchOption.AllDirectories)
            .Select(x => x.Remove(0, patchFolder.Length))
            .Where(x => !x.StartsWith("config", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var completed = 0;

        var extractTasks = patchFiles
            .Select(file => Task.Run(() =>
            {
                var relativeDirectory = $"{localDirectory}/{GetDirectoryFrom(file)}";
                if (!Directory.Exists(relativeDirectory))
                    Directory.CreateDirectory(relativeDirectory);

                File.Copy($"{patchFolder}/{file}", $"{localDirectory}/{file}", true);

                var count = Interlocked.Increment(ref completed);
                var percent = 100f * count / patchFiles.Count;
#if DEBUG
                Debug.WriteLine($"Copying {file} to {localDirectory}/{file} {count}/{patchFiles.Count} {percent}%");
#endif
                _setPatchTextCallback($"Extracting... {(int)percent}%");
            }));

        await Task.WhenAll(extractTasks);

        _setPatchTextCallback($"Patch applied! You are now on the latest version v{version}. Enjoy!");
    }

    private static string GetDirectoryFrom(string filePath)
        => new Regex("(.*)\\\\").Match(filePath).Value;
}
