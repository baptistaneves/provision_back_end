namespace ProvisionPadel.Api.Features.Cameras.StopRecording;

public record StopRecordingCommand(int ChannelId) : ICommand<Result<string>>;

public class StopRecordingHandler
    (HikvisionHttpClient hikvisionHttpClient,
     IApplicationDbContext context,
     IHikvisionService hikvisionService,
     ICameraService cameraService,
     IVideoService videoService
    ) : ICommandHandler<StopRecordingCommand, Result<string>>
{
    private readonly HikvisionHttpClient _hikvisionHttpClient = hikvisionHttpClient;
    private readonly IHikvisionService _hikvisionService = hikvisionService;
    private readonly ICameraService _cameraService = cameraService;
    private readonly IVideoService _videoService = videoService;
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<string>> Handle(StopRecordingCommand command, CancellationToken cancellationToken)
    {
        var url = $"/ISAPI/ContentMgmt/record/control/manual/stop/tracks/{command.ChannelId}";

        var response = await _hikvisionHttpClient.Client.PutAsync(url, null);

        if (!response.IsSuccessStatusCode)
            return Result<string>.Failure(new Error(ErrorMessages.ErrorStopRecording));

        await ChangeCameraStatus(command.ChannelId, cancellationToken);

        return Result<string>.Success(await response.Content.ReadAsStringAsync());
    }

    private async Task ChangeCameraStatus(int channelId, CancellationToken cancellationToken)
    {
        var camera = await _cameraService.StopCameraRecording(channelId, cancellationToken);

        var video = await _context.Videos
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.CameraId == camera.Id && x.IsRecording == true);

        if (video is not null)
        {
            var stop = DateTime.Now;

            var (endTime, size) = await _hikvisionService
                .ExtractSizeAndEndTimeFromXml(channelId.ToString(), video.StartTime, stop.AddMinutes(-2));

            await _videoService.Update(video.Name, endTime.ConvertToUtcDateTime(), size, cancellationToken);
        }
    }
}