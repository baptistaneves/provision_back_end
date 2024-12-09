namespace ProvisionPadel.Api.Features.Bunnies.UploadVideo;

public record UploadVideoResult(bool IsSuccess);

public record UploadVideoCommand(string VideoName, Guid LibraryId) : ICommand<UploadVideoResult>;

public class UploadVideoHandler
    (IOptions<Fmpeg> ffmpeg,
    IBunnyService bunnyService,
    IApplicationDbContext context) : ICommandHandler<UploadVideoCommand, UploadVideoResult>
{
    private readonly Fmpeg _ffmpeg = ffmpeg.Value;
    private readonly IBunnyService _bunnyservice = bunnyService;
    private readonly IApplicationDbContext _context = context;

    public async Task<UploadVideoResult> Handle(UploadVideoCommand command, CancellationToken cancellationToken)
    {
        var fileName = $"{command.VideoName}.mp4";

        var filePath = Path.Combine(_ffmpeg.VideoDirectory, fileName);

        var videoData = await ConvertToBase64(filePath);

        var isSuccessfullyUploaded = await _bunnyservice.UploadVideo(fileName, videoData);

        return new UploadVideoResult(isSuccessfullyUploaded);
    }

    private async Task<byte[]> ConvertToBase64(string filePath)
    {
        return await File.ReadAllBytesAsync(filePath);
    }
}