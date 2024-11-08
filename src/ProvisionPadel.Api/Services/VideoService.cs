
namespace ProvisionPadel.Api.Services;

public class VideoService(IApplicationDbContext context) : IVideoService
{
    private readonly IApplicationDbContext _context = context;

    public async Task Create(string name, DateTime startTime, int channelId, CancellationToken cancellationToken)
    {
        var newVideo = Video.Create(name, startTime, channelId);

        _context.Videos.Add(newVideo);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Video>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Videos.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task Update(string name, DateTime endTime, string size, CancellationToken cancellationToken)
    {
        var video = await _context.Videos.SingleOrDefaultAsync(x => x.Name == name);

        if (video is null) return;

        video.Update(endTime, size);
        await _context.SaveChangesAsync(cancellationToken);
    }
}