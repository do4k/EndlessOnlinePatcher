namespace EndlessOnlinePatcher.Extensions;

public static class HttpClientExtensions
{
    public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<int>? progress = null, CancellationToken cancellationToken = default)
    {
        using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        var contentLength = response.Content.Headers.ContentLength;

        using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (progress == null || !contentLength.HasValue)
        {
            await download.CopyToAsync(destination, cancellationToken);
            return;
        }

        await download.CopyToAsync(destination, 81920, contentLength.Value, progress, cancellationToken);
    }
}
