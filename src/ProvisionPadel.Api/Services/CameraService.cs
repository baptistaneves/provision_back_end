
using MediatR;
using System.Threading;

namespace ProvisionPadel.Api.Services;

public class CameraService(IApplicationDbContext context) : BaseService, ICameraService
{
    private readonly IApplicationDbContext _context = context;
    public async Task<Result<bool>> Create(CreateCameraRequest request, CancellationToken cancellationToken)
    {
        var errors = Validate(new CreateCameraValidator(), request);

        if(errors.Any()) 
            return Result<bool>.Failure(errors);

        if (await _context.Cameras.AnyAsync(x => x.Channel == request.Channel, cancellationToken))
            return Result<bool>.Failure(new Error("Já existe uma camera cadastrada com este número"));

        var newCamera = Camera.Create(request.Channel, request.CourtId);

        _context.Cameras.Add(newCamera);

        return Result<bool>.Success(await _context.SaveChangesAsync(cancellationToken) > 0);
    }

    public async Task<IEnumerable<CameraDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Cameras
                        .AsNoTracking()
                        .Include(x => x.Court)
                        .Select(x => new CameraDto
                        (
                            x.Id,
                            x.CourtId,
                            x.Court.Description,
                            x.Channel,
                            x.IsRecording
                         ))
                        .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsCameraRecording(int channel, CancellationToken cancellationToken)
    {
        var isCameraRecording = await _context.Cameras.AnyAsync(x => x.Channel == channel && x.IsRecording == true);

        if(isCameraRecording) return true;

        return false;
    }

    public async Task<Result<bool>> Remove(Guid id, CancellationToken cancellationToken)
    {
        var camera = await GetCameraById(id);

        if (camera is null)
            return Result<bool>.Failure(new Error(ErrorMessages.CameraNotFound));

        if(await IsCameraRecording(camera.Channel, cancellationToken))
            return Result<bool>.Failure(new Error(ErrorMessages.CameraRecordingCanNotBeRemoved));

        if(camera.Videos.Any())
            return Result<bool>.Failure(new Error(ErrorMessages.CameraWithVideoCanNotBeRemoved));

        _context.Cameras.Remove(camera);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
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

    public async Task<Result<bool>> Update(UpdateCameraRequest request, CancellationToken cancellationToken)
    {
        var errors = Validate(new UpdateCameraValidator(), request);

        if (errors.Any())
            return Result<bool>.Failure(errors);

        var camera = await _context.Cameras.SingleOrDefaultAsync(x => x.Id == request.Id);

        if (camera is null)
            return Result<bool>.Failure(new Error(ErrorMessages.CameraNotFound));

        if (await _context.Cameras.AnyAsync(x => x.Channel == request.Channel && x.Id != request.Id, cancellationToken))
            return Result<bool>.Failure(new Error("Já existe uma camera cadastrada com este número"));

        camera.Update(request.Channel, request.CourtId);

        _context.Cameras.Update(camera);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    private async Task<Camera> GetCameraById(Guid id)
    {
        return await _context.Cameras
                        .AsNoTracking()
                        .Include(x => x.Court)
                        .Include(x => x.Videos)
                        .SingleOrDefaultAsync(x => x.Id == id);
    }
}