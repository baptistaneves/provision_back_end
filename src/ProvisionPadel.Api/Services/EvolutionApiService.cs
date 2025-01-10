using ProvisionPadel.Api.Services.Validators;
using ProvisionPadel.Api.Shared.Results;

namespace ProvisionPadel.Api.Services;

public class EvolutionApiService : BaseService, IEvolutionApiService
{
    private readonly HttpClient _httpClient;
    private readonly EvoluctionApi _evolutionApi;

    public EvolutionApiService(IOptions<EvoluctionApi> evolutionApi)
    {
        _evolutionApi = evolutionApi.Value;

        _httpClient = new HttpClient { BaseAddress = new Uri(_evolutionApi.BaseUrl) };
        _httpClient.DefaultRequestHeaders.Add("apikey", _evolutionApi.GlobalApikey);
    }

    public async Task<Result<bool>> CreateInstance(string name)
    {
        var errors = Validate(new CreateInstanceValidator(), name);

        if (errors.Any())
            return Result<bool>.Failure(errors);

        var payload = new
        {
            instanceName = name,
            qrcode = false,
            integration = "WHATSAPP-BAILEYS"
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/instance/create", jsonContent);

        if(!response.IsSuccessStatusCode)
            return Result<bool>.Failure(new Error("Erro ao criar a conexão, tente novamente!"));

        return Result<bool>.Success(response.IsSuccessStatusCode);
    }

    public async Task<Result<bool>> DeleteInstance(string instanceName)
    {
        var response = await _httpClient.DeleteAsync($"/instance/delete/{instanceName}");

        if (!response.IsSuccessStatusCode)
            return Result<bool>.Failure(new Error("Erro ao excluir a conexão, tente novamente!"));

        return Result<bool>.Success(response.IsSuccessStatusCode);
    }

    public async Task<Result<InstanceDto>> FetchInstanceById(Guid instanceId)
    {
        var response = await _httpClient.GetAsync($"/instance/fetchInstances?instanceId={instanceId}");

        if (!response.IsSuccessStatusCode)
            return Result<InstanceDto>.Failure(new Error($"A conexão solicitada não foi encontrada")); ;

        var jsonString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<InstanceDto>>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Result<InstanceDto>.Success(result?.FirstOrDefault()!);
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

    public async Task<Result<QrcodeDto>> InstanceConnect(string instanceName)
    {
        var response = await _httpClient.GetAsync($"/instance/connect/{instanceName}");

        if (!response.IsSuccessStatusCode)
            return Result<QrcodeDto>.Failure(new Error($"Erro ao obter o QrCode. Tente novamente"));

        var jsonString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<QrcodeDto>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return Result<QrcodeDto>.Success(result!);
    }

    public async Task<bool> LogoutInstance(string instanceName)
    {
        var response = await _httpClient.DeleteAsync($"/instance/logout/{instanceName}");

        return response.IsSuccessStatusCode;
    }

    public async Task<Result<bool>> SendVideo(string destination, string instanceName, string video)
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
           return Result<bool>.Failure(new Error($"Erro ao enviar vídeo. Tente novamente"));

        return Result<bool>.Success(response.IsSuccessStatusCode);
    }
}