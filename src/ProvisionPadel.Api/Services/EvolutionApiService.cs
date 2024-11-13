namespace ProvisionPadel.Api.Services;

public class EvolutionApiService
    (IOptions<EvoluctionApi> evolutionApi) : IEvolutionApiService
{
    public HttpClient _httpClient { get; } = new HttpClient();
    private readonly EvoluctionApi _evolutionApi = evolutionApi.Value;

    public async Task<bool> SendVideo(string destination, string video)
    {
        SetHeader();

        var payload = new
        {
            number = destination,
            mediatype = "video",
            caption = "Ucall - Provision Padel",
            media = video
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/message/sendMedia/{_evolutionApi.Instance}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro ao enviar vídeo. Código do erro: {(int)response.StatusCode} - {response.RequestMessage}");
        }

        return response.IsSuccessStatusCode;
    }

    private void SetHeader()
    {
        _httpClient.BaseAddress = new Uri(_evolutionApi.BaseUrl);

        _httpClient.DefaultRequestHeaders.Add("apikey", _evolutionApi.GlobalApikey);
    }
}