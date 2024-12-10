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

    public async Task<bool> CreateInstance(string name)
    {
        var payload = new
        {
            instanceName = name,
            qrcode = false,
            integration = "WHATSAPP-BAILEYS"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/instance/create", jsonContent);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteInstance(string instanceName)
    {
        var response = await _httpClient.DeleteAsync($"/instance/delete/{instanceName}");

        return response.IsSuccessStatusCode;
    }

    public async Task<InstanceDto?> FetchInstanceById(Guid instanceId)
    {
        var response = await _httpClient.GetAsync($"/instance/fetchInstances?instanceId={instanceId}");

        if (!response.IsSuccessStatusCode) return null;

        var jsonString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<InstanceDto>>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?.FirstOrDefault();
    }

    public async Task<IEnumerable<InstanceDto>> FetchInstances()
    {
        var response = await _httpClient.GetAsync($"/instance/fetchInstances");

        if (!response.IsSuccessStatusCode) return null;

        var jsonString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<IEnumerable<InstanceDto>>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result;
    }

    public async Task<QrcodeDto> InstanceConnect(string instanceName)
    {
        var response = await _httpClient.GetAsync($"/instance/connect/{instanceName}");

        if (!response.IsSuccessStatusCode) return null;

        var jsonString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<QrcodeDto>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result;
    }

    public async Task<bool> LogoutInstance(string instanceName)
    {
        var response = await _httpClient.DeleteAsync($"/instance/logout/{instanceName}");

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SendVideo(string destination, string instanceName, string video)
    {
        var payload = new
        {
            number = destination,
            mediatype = "video",
            caption = "Ucall - Provision Padel",
            media = video
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/message/sendMedia/{instanceName}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Erro ao enviar vídeo. Código do erro: {(int)response.StatusCode} - {response.ReasonPhrase}");
        }

        return response.IsSuccessStatusCode;
    }
}