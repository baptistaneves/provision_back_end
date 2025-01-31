namespace ProvisionPadel.Api.Features.Bunnies.UploadVideo;
public record UploadVideoCommand(int ChannelId, string Name, string Size, DateTime StartTime, DateTime EndTime) : ICommand<Result<bool>>;

public class UploadVideoHandler
    (IBunnyService bunnyService,
    IHikvisionService hikvisionService) : ICommandHandler<UploadVideoCommand, Result<bool>>
{
    private readonly IBunnyService _bunnyservice = bunnyService;
    private readonly IHikvisionService _hikvisionService  = hikvisionService;

    public async Task<Result<bool>> Handle(UploadVideoCommand command, CancellationToken cancellationToken)
    {
        var result = await _hikvisionService
            .DownloadVideo(command.ChannelId, command.Name, command.Size, command.StartTime, command.EndTime);

        if (!result.IsSuccess)
            return Result<bool>.Failure(new Error("Erro ao baixar o video do equipamento de gravação"));

        var videoData = result.Value;

        var isSuccessfullyUploaded = await _bunnyservice.UploadVideo(command.Name, videoData!);

        return Result<bool>.Success(isSuccessfullyUploaded);
    }
}