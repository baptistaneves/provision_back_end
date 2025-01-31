namespace ProvisionPadel.Api.Features.Videos.DownloadVideo;

public record DownloadVideoCommand
    (int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime) : ICommand<Result<byte[]>>;

public class DownloadVideoHandler
    (IHikvisionService hikvisionService,
     INotifier notifier) : ICommandHandler<DownloadVideoCommand, Result<byte[]>>
{
    private readonly IHikvisionService _hikvisionService = hikvisionService;

    public async Task<Result<byte[]>> Handle(DownloadVideoCommand command, CancellationToken cancellationToken)
    {
        var result = await _hikvisionService
            .DownloadVideo(command.ChannelId, command.Name, command.Size, command.StartTime, command.EndTime);

        if(!result.IsSuccess) 
            return result;

        var videoStream = result.Value;

        return Result<byte[]>.Success(videoStream!);
    }
}