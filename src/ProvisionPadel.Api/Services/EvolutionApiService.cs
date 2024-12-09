using ProvisionPadel.Api.Data.Models;

namespace ProvisionPadel.Api.Services;

public class EvolutionApiService : IEvolutionApiService
{
    private readonly HttpClient _httpClient;
    private readonly EvoluctionApi _evolutionApi;

    public EvolutionApiService(IOptions<EvoluctionApi> evolutionApi)
    {
        _evolutionApi = evolutionApi.Value;

        _httpClient = new HttpClient { BaseAddress = new Uri(_evolutionApi.BaseUrl) };
        _httpClient.DefaultRequestHeaders.Add("apikey", _evolutionApi.GlobalApikey);
    }

    public async Task<bool> SendVideo(string destination, string video)
    {
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
            Console.WriteLine($"Erro ao enviar vídeo. Código do erro: {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        return response.IsSuccessStatusCode;
    }
}