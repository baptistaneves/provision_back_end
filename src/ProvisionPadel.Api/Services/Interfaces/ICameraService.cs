namespace ProvisionPadel.Api.Services.Interfaces;

public interface ICameraService
{
    Task Create(int channel, CancellationToken cancellationToken);
    Task StartCameraRecording(int channel, CancellationToken cancellationToken);
    Task StopCameraRecording(int channel, CancellationToken cancellationToken);

    Task<bool> IsCameraRecording(int channel, CancellationToken cancellationToken);

    Task<IEnumerable<Camera>> GetAll(CancellationToken cancellationToken);
}