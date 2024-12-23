
namespace ProvisionPadel.Api.Services;

public class VideoService(IApplicationDbContext context) : IVideoService
{
    private readonly IApplicationDbContext _context = context;

    public async Task Create(string name, string videoDownloadUrl, DateTime startTime, Guid cameraId, CancellationToken cancellationToken)
    {
        var newVideo = Video.Create(name, videoDownloadUrl, startTime, cameraId);

        _context.Videos.Add(newVideo);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VideoDto>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Videos
            .AsNoTracking()
            .Include(x => x.Camera)
            .Select(x => new VideoDto
            {
                Id = x.Id,
                CameraId = x.CameraId,
                Name = x.Name,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                VideoDownloadUrl = x.VideoDownloadUrl,
                Size = x.Size,
                IsRecording = x.IsRecording,
                CameraChannel = x.Camera.Channel
            })
            .ToListAsync(cancellationToken);
    }

    public async Task Update(string name, DateTime endTime, string size, CancellationToken cancellationToken)
    {
        var video = await _context.Videos.SingleOrDefaultAsync(x => x.Name == name);

        if (video is null) return;

        video.Update(endTime, size);
        await _context.SaveChangesAsync(cancellationToken);
    }
}