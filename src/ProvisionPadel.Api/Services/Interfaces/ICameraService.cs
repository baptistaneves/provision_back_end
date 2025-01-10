namespace ProvisionPadel.Api.Services.Interfaces;

public interface ICameraService
{
    Task<Result<bool>> Create(CreateCameraRequest request, CancellationToken cancellationToken);
    Task<Result<bool>> Update(UpdateCameraRequest request, CancellationToken cancellationToken);
    Task<Camera> StartCameraRecording(int channel, CancellationToken cancellationToken);
    Task<Camera> StopCameraRecording(int channel, CancellationToken cancellationToken);

    Task<bool> IsCameraRecording(int channel, CancellationToken cancellationToken);

    Task<IEnumerable<CameraDto>> GetAll(CancellationToken cancellationToken);
    Task<Result<bool>> Remove(Guid id, CancellationToken cancellationToken);
}