namespace ProvisionPadel.Api.Services.Interfaces;

public interface IEvolutionApiService
{
    Task<bool> SendVideo(string destination, string instanceName, string video);

    Task<bool> CreateInstance(string instanceName);
    Task<QrcodeDto> InstanceConnect(string instanceName);
    Task<IEnumerable<InstanceDto>> FetchInstances();
    Task<InstanceDto> FetchInstanceById(Guid instanceId);
    Task<bool> LogoutInstance(string instanceName);
    Task<bool> DeleteInstance(string instanceName);
}