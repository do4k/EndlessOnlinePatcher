using EoPatcher.Models;
using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace EoPatcher.Core.Services;

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
        var patchFolder = $"patch-{version}";

        ZipFile.ExtractToDirectory("patch.zip", patchFolder);
        var patchFiles = Directory.EnumerateFiles(patchFolder, "*", SearchOption.AllDirectories).ToList();

        var completed = 0;
        var total = patchFiles.Count;

        var extractTasks = patchFiles
            .Select(fullPath => Task.Run(() =>
            {
                var relativePath = Path.GetRelativePath(patchFolder, fullPath);
                var destPath = Path.Combine(localDirectory, relativePath);
                var destDir = Path.GetDirectoryName(destPath);

                if (destDir != null && !Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                File.Copy(fullPath, destPath, true);

                Interlocked.Increment(ref completed);

                var percent = (int)(100f * completed / total);
#if DEBUG
                Debug.WriteLine($"Copying {relativePath} to {destPath} {completed}/{total} {percent}%");
#endif
                _setPatchTextCallback($"Extracting... {percent}%");
            }));

        await Task.WhenAll(extractTasks);

        _setPatchTextCallback($"Patch applied! You are now on the latest version v{version}. Enjoy!");
    }
}
