namespace ProvisionPadel.Api.Services.Interfaces;

public interface IBunnyService
{
    Task<bool> UploadVideo(string fileName, byte[] videoData);
}