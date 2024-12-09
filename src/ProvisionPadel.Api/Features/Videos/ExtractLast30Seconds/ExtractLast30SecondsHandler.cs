namespace ProvisionPadel.Api.Features.Videos.ExtractLast30Seconds;

public record ExtractLast30SecondsResult(bool IsSuccess);

public record ExtractLast30SecondsCommand(string Name) : ICommand<ExtractLast30SecondsResult>;

public class ExtractLast30SecondsHandler
    (IEvolutionApiService evolutionApiService,
     IOptions<Fmpeg> ffmpeg) : ICommandHandler<ExtractLast30SecondsCommand, ExtractLast30SecondsResult>
{
    private readonly Fmpeg _ffmpeg = ffmpeg.Value;
    private readonly IEvolutionApiService _evolutionApiService = evolutionApiService;

    public async Task<ExtractLast30SecondsResult> Handle(ExtractLast30SecondsCommand command, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_ffmpeg.VideoDirectory, $"{command.Name}.mp4");

        var fileOutPut = Path.Combine(_ffmpeg.VideoDirectory, command.Name + $"_last30seconds_{Guid.NewGuid()}.mp4");

        var ffmpegArgs = $"-sseof -30 -i \"{filePath}\" -t 30 -c:v copy -an {fileOutPut}";

        Process.Start($"{_ffmpeg.FFmpegPath}", $"{ffmpegArgs}");

        await Task.Delay(1000);

        var media = ConvertToBase64(fileOutPut);

        var isSuccess = await _evolutionApiService.SendVideo("244934569038", media);

        if (isSuccess) DeleteVideo(fileOutPut);

        return new ExtractLast30SecondsResult(isSuccess);
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