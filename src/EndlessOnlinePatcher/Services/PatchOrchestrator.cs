using OneOf;
using OneOf.Types;

namespace EndlessOnlinePatcher.Services;

internal interface IPatchOrchestrator
{
    public Task<OneOf<Success, Error<string>>> Patch(Version version);
}

public sealed class PatchOrchestrator : IPatchOrchestrator, IDisposable
{
    private IHttpService _httpService;
    private IFileService _fileService;
    private ILocalVersionRepository _localVersionRepository;

    public PatchOrchestrator(Action<string> setPatchTextCallback)
    {
        _httpService = new HttpService(setPatchTextCallback);
        _fileService = new FileService(setPatchTextCallback);
        _localVersionRepository = new LocalVersionRepository();
    }

    public async Task<OneOf<Success, Error<string>>> Patch(Version version)
    {
        Clean();
        var downloadResult = await _httpService.DownloadLatestPatchAsync(version);
        if (downloadResult.IsT1)
            return downloadResult.AsT1;

        await _fileService.ExtractPatch(version);
        _localVersionRepository.Save(version);
        return new Success();
    }

    public void Dispose()
    {
        Clean();
    }

    private static void Clean()
    {
        var patchDirectories = Directory.EnumerateDirectories("./").Where(x => x.Contains("patch"));
        foreach (var directory in patchDirectories)
            Directory.Delete(directory, true);

        var patchZips = Directory.EnumerateFiles("./", "*.zip");
        foreach (var zip in patchZips)
            File.Delete(zip);
    }
}
