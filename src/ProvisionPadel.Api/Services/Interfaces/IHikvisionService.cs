namespace ProvisionPadel.Api.Services.Interfaces;

public interface IHikvisionService
{
    Task<string> ExtractRtspFromXml(string trackID, DateTime startTime, DateTime endTime);
    Task<(string startTime, string name)> ExtractNameAndStartTimeFromXml(string trackID, DateTime startTime, DateTime endTime);
    Task<(string endTime, string size)> ExtractSizeAndEndTimeFromXml(string trackID, DateTime startTime, DateTime endTime);
    Task<Result<byte[]>> DownloadVideo(int channelId, string name, string size, DateTime startTime, DateTime endTime);
}