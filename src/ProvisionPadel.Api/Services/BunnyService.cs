namespace ProvisionPadel.Api.Services;

public class BunnyService
    (IOptions<Bunny> bunny) : IBunnyService
{
    private HttpClient _httpClient;
    private readonly Bunny _bunny = bunny.Value;

    public async Task<bool> UploadVideo(string fileName, byte[] videoData)
    {
        SetHeader(_bunny.BaseUrl, _bunny.StorageZoneKey);

        var requestUri = $"/{_bunny.StorageZone}/{fileName}";

        using var content = new ByteArrayContent(videoData);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        var response = await _httpClient.PutAsync(requestUri, content);

        return response.IsSuccessStatusCode;
    }

    private void SetHeader(string url, string apiKey)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(url);

        _httpClient.DefaultRequestHeaders.Add("AccessKey", apiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}