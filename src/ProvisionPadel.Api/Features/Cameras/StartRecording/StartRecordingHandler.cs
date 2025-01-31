namespace ProvisionPadel.Api.Features.Cameras.StartRecording;

public record StartRecordingResult(string Content);

public record StartRecordingCommand(int ChannelId) : ICommand<Result<string>>;

public class StartRecordingHandler
    (HikvisionHttpClient hikvisionHttpClient,
     IHikvisionService hikvisionService,
     ICameraService cameraService,
     IVideoService videoService,
     IOptions<Bunny> bunny
    ) : ICommandHandler<StartRecordingCommand, Result<string>>
{
    private readonly Bunny _bunny = bunny.Value;
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;
    private readonly IHikvisionService _hikvisionService = hikvisionService;
    private readonly ICameraService _cameraService = cameraService;
    private readonly IVideoService _videoService = videoService;

    public async Task<Result<string>> Handle(StartRecordingCommand command, CancellationToken cancellationToken)
    {
        if (await _cameraService.IsCameraRecording(command.ChannelId, cancellationToken))
            return Result<string>.Failure(new Error(ErrorMessages.ChannelIsRecording));

        var response = await StartRecording(command.ChannelId);

        if (!response.IsSuccessStatusCode)
            return Result<string>.Failure(new Error(ErrorMessages.ErrorStartingRecord));

        var changeCameraStatusResult = await ChangeCameraStatus(command.ChannelId, response, cancellationToken);

        if (!changeCameraStatusResult.IsSuccess)
            return Result<string>.Failure(new Error(changeCameraStatusResult.Error!.Message));

        return Result<string>.Success(await response.Content.ReadAsStringAsync());
    }

    private async Task<HttpResponseMessage> StartRecording(int channelId)
    {
        var url = $"/ISAPI/ContentMgmt/record/control/manual/start/tracks/{channelId}";

        return await _hikvisionHttpClient.Client.PutAsync(url, null);
    }

    private async Task<Result<bool>> ChangeCameraStatus(int channelId, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var start = DateTime.Now;

        await Task.Delay(3000);

        var (startTime, name) = await _hikvisionService
            .ExtractNameAndStartTimeFromXml(channelId.ToString(), start.AddMinutes(-2), DateTime.Now.AddMinutes(1));

        if(startTime == null && name == null)
            return Result<bool>.Failure(new Error(ErrorMessages.ErrorStartingRecord));

        var camera = await _cameraService.StartCameraRecording(channelId, cancellationToken);

        if (camera is null)
            return Result<bool>.Failure(new Error("Nenhuma camara com a canal informado foi encontrada"));

        var videoDownloadUrl = $"/{_bunny.BaseUrl}/{_bunny.StorageZone}/{name}?accessKey={_bunny.StorageZoneKey}&download";

        await SaveVideo(name, videoDownloadUrl, startTime.ConvertToUtcDateTime().AddHours(-1), camera.Id, cancellationToken);

        return Result<bool>.Success(true);
    }

    private async Task SaveVideo(string name, string videoDownloadUrl, DateTime startTime, Guid cameraId, CancellationToken cancellationToken)
    {
        await _videoService.Create(name, videoDownloadUrl, startTime, cameraId, cancellationToken);
    }
}