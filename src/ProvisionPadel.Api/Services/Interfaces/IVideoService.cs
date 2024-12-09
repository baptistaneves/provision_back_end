namespace ProvisionPadel.Api.Services.Interfaces;

public interface IVideoService
{
    Task Create(string name, string videoDownloadUrl, DateTime startTime, Guid cameraId, CancellationToken cancellationToken);
    Task Update(string name, DateTime endTime, string size, CancellationToken cancellationToken);
    Task<IEnumerable<Video>> GetAll(CancellationToken cancellationToken);
}