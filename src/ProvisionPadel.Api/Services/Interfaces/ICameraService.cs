namespace ProvisionPadel.Api.Services.Interfaces;

public interface ICameraService
{
    Task Create(int channel, Guid courtId, CancellationToken cancellationToken);
    Task<bool> Update(Guid id, int channel, Guid courtId, CancellationToken cancellationToken);
    Task<Camera> StartCameraRecording(int channel, CancellationToken cancellationToken);
    Task<Camera> StopCameraRecording(int channel, CancellationToken cancellationToken);

    Task<bool> IsCameraRecording(int channel, CancellationToken cancellationToken);

    Task<IEnumerable<CameraDto>> GetAll(CancellationToken cancellationToken);
    Task<bool> Remove(Guid id, CancellationToken cancellationToken);
}