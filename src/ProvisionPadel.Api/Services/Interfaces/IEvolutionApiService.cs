namespace ProvisionPadel.Api.Services.Interfaces;

public interface IEvolutionApiService
{
    Task<Result<bool>> SendVideo(string destination, string instanceName, string video);

    Task<Result<bool>> CreateInstance(string instanceName);
    Task<Result<QrcodeDto>> InstanceConnect(string instanceName);
    Task<IEnumerable<InstanceDto>> FetchInstances();
    Task<Result<InstanceDto>> FetchInstanceById(Guid instanceId);
    Task<bool> LogoutInstance(string instanceName);
    Task<Result<bool>> DeleteInstance(string instanceName);
}