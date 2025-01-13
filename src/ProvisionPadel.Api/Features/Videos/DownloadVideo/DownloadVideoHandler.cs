using Microsoft.AspNetCore.Http.HttpResults;

namespace ProvisionPadel.Api.Features.Videos.DownloadVideo;

public record DownloadVideoResult();
public record DownloadVideoCommand
    (int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime) : ICommand<Result<byte[] Content>>;

public class DownloadVideoHandler
    (HikvisionHttpClient hikvisionHttpClient,
     INotifier notifier) : ICommandHandler<DownloadVideoCommand, Result<byte[]>>
{
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;
    private readonly INotifier _notifier = notifier;
    public async Task<Result<byte[]>> Handle(DownloadVideoCommand command, CancellationToken cancellationToken)
    {
        var url = $"/ISAPI/ContentMgmt/download?playbackURI={_hikvisionHttpClient.Rtsp}/Streaming/tracks" +
            $"/{command.ChannelId}/?starttime={command.StartTime}&amp;endtime={command.EndTime}&amp;name={command.Name}&amp;size={command.Size}";

        var response = await _hikvisionHttpClient.Client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<byte[]>.Failure(new Error(ErrorMessages.ErrorDownloadingVideo));
        }

        var videoStream = await response.Content.ReadAsByteArrayAsync();

        return Result<byte[]>.Success(videoStream);
    }
}