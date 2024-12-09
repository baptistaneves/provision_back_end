namespace ProvisionPadel.Api.Features.Cameras.StartRecording;

public record StartRecordingResult(string Content);

public record StartRecordingCommand(int ChannelId) : ICommand<StartRecordingResult>;

public class StartRecordingHandler
    (HikvisionHttpClient hikvisionHttpClient,
     IHikvisionService hikvisionService,
     ICameraService cameraService,
     IVideoService videoService,
     IOptions<Bunny> bunny,
     INotifier notifier
    ) : ICommandHandler<StartRecordingCommand, StartRecordingResult>
{
    private readonly Bunny _bunny = bunny.Value;
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;
    private readonly IHikvisionService _hikvisionService = hikvisionService;
    private readonly ICameraService _cameraService = cameraService;
    private readonly IVideoService _videoService = videoService;
    private readonly INotifier _notifier = notifier;

    public async Task<StartRecordingResult> Handle(StartRecordingCommand command, CancellationToken cancellationToken)
    {
        if (await IsCameraAlreadyRecording(command.ChannelId, cancellationToken))
            return new StartRecordingResult(ErrorMessages.ErrorStartingRecord);

        var response = await StartRecording(command.ChannelId);

        await ChangeCameraStatus(command.ChannelId, response, cancellationToken);

        return new StartRecordingResult(await response.Content.ReadAsStringAsync());
    }

    private async Task<bool> IsCameraAlreadyRecording(int ChannelId, CancellationToken cancellationToken)
    {
        if (await _cameraService.IsCameraRecording(ChannelId, cancellationToken))
        {
            _notifier.Add(ErrorMessages.ChannelIsRecording);

            return true;
        }

        return false;
    }

    private async Task<HttpResponseMessage> StartRecording(int channelId)
    {
        var url = $"/ISAPI/ContentMgmt/record/control/manual/start/tracks/{channelId}";

        return await _hikvisionHttpClient.Client.PutAsync(url, null);
    }

    private async Task ChangeCameraStatus(int channelId, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var start = DateTime.Now;

        if (response.IsSuccessStatusCode)
        {
            await Task.Delay(3000);

            var (startTime, name) = await _hikvisionService
                .ExtractNameAndStartTimeFromXml(channelId.ToString(), start.AddMinutes(-2), DateTime.Now.AddMinutes(1));

            var camera = await _cameraService.StartCameraRecording(channelId, cancellationToken);

            var videoDownloadUrl = $"/{_bunny.BaseUrl}/{_bunny.StorageZone}/{name}?accessKey={_bunny.StorageZoneKey}&download";

            await SaveVideo(name, videoDownloadUrl, startTime.ConvertToUtcDateTime().AddHours(-1), camera.Id, cancellationToken);
        }
    }

    private async Task SaveVideo(string name, string videoDownloadUrl, DateTime startTime, Guid cameraId, CancellationToken cancellationToken)
    {
        await _videoService.Create(name, videoDownloadUrl, startTime, cameraId, cancellationToken);
    }
}