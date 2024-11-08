using System.IO;

namespace ProvisionPadel.Api.Features.CameraNVRAPIIntegration.ExtractLast30Seconds;

public record ExtractLast30SecondsResult(FileStream Stream);

public record ExtractLast30SecondsCommand(string Name) : ICommand<ExtractLast30SecondsResult>;

public class ExtractLast30SecondsHandler
    (IConfiguration configuration) : ICommandHandler<ExtractLast30SecondsCommand, ExtractLast30SecondsResult>
{
    private readonly IConfiguration _configuration = configuration;
    public async Task<ExtractLast30SecondsResult> Handle(ExtractLast30SecondsCommand command, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_configuration["FFmpeg:VideoDirectory"]!, $"{command.Name}.mp4");

        var fileOutPut = Path.Combine(_configuration["FFmpeg:VideoDirectory"]!, command.Name + $"_last30seconds_{Guid.NewGuid()}.mp4");

        var ffmpegArgs = $"-sseof -30 -i \"{filePath}\" -t 30 -c:v copy -an {fileOutPut}";

        var ffmpegPath = _configuration["FFmpeg:FFmpegPath"];

        Process.Start($"{ffmpegPath}", $"{ffmpegArgs}");

        var stream = DownloadVideo(fileOutPut);

        return new ExtractLast30SecondsResult(stream);
    }

    private FileStream DownloadVideo(string fileOutPut)
    {
        return new FileStream(fileOutPut, FileMode.Open, FileAccess.Read);
    }

    private void DeleteVideo(string fileOutPut)
    {
        if (System.IO.File.Exists(fileOutPut))
        {
            System.IO.File.Delete(fileOutPut);
        }
    }
}