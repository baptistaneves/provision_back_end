namespace ProvisionPadel.Api.Features.Videos.DownloadVideo;

public record DownloadVideoResult(byte[] Content);

public record DownloadVideoCommand
    (int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime) : ICommand<Result<DownloadVideoResult>>;

public class DownloadVideoHandler
    (HikvisionHttpClient hikvisionHttpClient,
     INotifier notifier) : ICommandHandler<DownloadVideoCommand, Result<DownloadVideoResult>>
{
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;
    private readonly INotifier _notifier = notifier;
    public async Task<Result<DownloadVideoResult>> Handle(DownloadVideoCommand command, CancellationToken cancellationToken)
    {
        var url = $"/ISAPI/ContentMgmt/download?playbackURI={_hikvisionHttpClient.Rtsp}/Streaming/tracks" +
            $"/{command.ChannelId}/?starttime={command.StartTime}&amp;endtime={command.EndTime}&amp;name={command.Name}&amp;size={command.Size}";

        var response = await _hikvisionHttpClient.Client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<DownloadVideoResult>.Failure(new Error(ErrorMessages.ErrorDownloadingVideo));
        }

        var videoStream = await response.Content.ReadAsByteArrayAsync();

        return Result<DownloadVideoResult>.Success( new DownloadVideoResult(videoStream));
    }
}