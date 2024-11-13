namespace ProvisionPadel.Api.Services.Interfaces;

public interface IEvolutionApiService
{
    Task<bool> SendVideo(string destination, string video);
}