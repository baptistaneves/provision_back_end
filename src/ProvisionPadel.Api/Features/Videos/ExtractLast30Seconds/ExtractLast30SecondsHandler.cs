namespace ProvisionPadel.Api.Features.Videos.ExtractLast30Seconds;

public record ExtractLast30SecondsResult(bool IsSuccess);

public record ExtractLast30SecondsCommand(string FileName, string InstanceName, string Destination) : ICommand<Result<bool>>;

public class ExtractLast30SecondsHandler
    (IEvolutionApiService evolutionApiService,
     IOptions<Fmpeg> ffmpeg) : ICommandHandler<ExtractLast30SecondsCommand, Result<bool>>
{
    private readonly Fmpeg _ffmpeg = ffmpeg.Value;
    private readonly IEvolutionApiService _evolutionApiService = evolutionApiService;

    public async Task<Result<bool>> Handle(ExtractLast30SecondsCommand command, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_ffmpeg.VideoDirectory, $"{command.FileName}.mp4");

        var fileOutPut = Path.Combine(_ffmpeg.VideoDirectory, command.FileName + $"_last30seconds_{Guid.NewGuid()}.mp4");

        var ffmpegArgs = $"-sseof -30 -i \"{filePath}\" -t 30 -c:v copy -an {fileOutPut}";

        Process.Start($"{_ffmpeg.FFmpegPath}", $"{ffmpegArgs}");

        await Task.Delay(1000);

        var media = ConvertToBase64(fileOutPut);

        var result = await _evolutionApiService.SendVideo(command.Destination, command.InstanceName, media);

        if (result.IsSuccess) DeleteVideo(fileOutPut);

        return Result<bool>.Success(true);
    }

    private void DeleteVideo(string fileOutPut)
    {
        if (File.Exists(fileOutPut))
        {
            File.Delete(fileOutPut);
        }
    }

    private string ConvertToBase64(string fileOutPut)
    {
        byte[] fileBytes = File.ReadAllBytes(fileOutPut);

        return Convert.ToBase64String(fileBytes);
    }
}