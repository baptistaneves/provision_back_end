namespace ProvisionPadel.Api.Services;

public class CameraService(IApplicationDbContext context) : ICameraService
{
    private readonly IApplicationDbContext _context = context;
    public async Task Create(int channel, Guid courtId, CancellationToken cancellationToken)
    {
        var newCamera = Camera.Create(channel, courtId);

        _context.Cameras.Add(newCamera);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Camera>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Cameras.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<bool> IsCameraRecording(int channel, CancellationToken cancellationToken)
    {
        var isCameraRecording = await _context.Cameras.AnyAsync(x => x.Channel == channel && x.IsRecording == true);

        if(isCameraRecording) return true;

        return false;
    }

    public async Task<Camera> StartCameraRecording(int channel, CancellationToken cancellationToken)
    {
        var camera = await _context.Cameras.SingleOrDefaultAsync(x => x.Channel == channel);

        if(camera is null) return null;

        camera.StartCameraRecording();

        await _context.SaveChangesAsync(cancellationToken);
        return camera;
    }

    public async Task<Camera> StopCameraRecording(int channel, CancellationToken cancellationToken)
    {
        var camera = await _context.Cameras.SingleOrDefaultAsync(x => x.Channel == channel);

        if (camera is null) return null;

        camera.StopCameraRecording();

        await _context.SaveChangesAsync(cancellationToken);

        return camera;
    }
}