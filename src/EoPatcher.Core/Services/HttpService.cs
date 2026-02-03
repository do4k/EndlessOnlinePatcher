using EoPatcher.Extensions;
using OneOf;
using OneOf.Types;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EoPatcher.Core.Services;

public interface IHttpService
{
    public Task<OneOf<Success, Error<string>>> DownloadLatestPatchAsync(Version version);
}

public partial class HttpService : IHttpService
{
    private readonly Action<string> _setPatchTextCallback;

    public HttpService(Action<string> setPatchTextCallback)
    {
        _setPatchTextCallback = setPatchTextCallback;
    }

    private async Task<OneOf<string, Error<string>>> GetLatestDownloadLinkAsync()
    {
        using var httpClient = new HttpClient();
        var clientUrl = "https://www.endless-online.com/client/download.html";
        try
        {
            var endlessHomePage = await httpClient.GetStringAsync(clientUrl);
            var regexVersionMatches = new Regex("href=\"([^\\\"]*EndlessOnline[^\\\"]*\\.zip)\"").Match(endlessHomePage);
            if (!regexVersionMatches.Success)
            {
                return new Error<string>($"Could not find download link in {clientUrl}");
            }
            var downloadLink = regexVersionMatches.Groups[1].Value;
            return downloadLink;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Could not get the latest download link: {ex.Message}");
            return new Error<string>($"Could not get the latest download link from {clientUrl}");
        }
    }

    public async Task<OneOf<Success, Error<string>>> DownloadLatestPatchAsync(Version version)
    {
        var link = await GetLatestDownloadLinkAsync();
        if (link.IsT1)
        {
            return link.AsT1;
        }

        try
        {
            var progress = new Progress<int>(x => _setPatchTextCallback($"Downloading... {x}%"));
            using var httpClient = new HttpClient();
            using var fileStream = new FileStream("patch.zip", FileMode.Create, FileAccess.Write);
            await httpClient.DownloadAsync(link.AsT0, fileStream, progress);
            fileStream.Close();
            return new Success();
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Could not download latest patch from {link.AsT0}: {e.Message}");
            return new Error<string>($"Could not download latest patch from {link.AsT0}");
        }
    }
}