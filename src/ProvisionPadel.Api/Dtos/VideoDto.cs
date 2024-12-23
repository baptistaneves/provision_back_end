namespace ProvisionPadel.Api.Dtos;

public class VideoDto
{
    public Guid Id { get; set; }
    public Guid CameraId { get; set; }
    public int CameraChannel { get; set; }
    public string Name { get; set; }
    public string VideoDownloadUrl { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Size { get; set; }
    public bool IsRecording { get; set; }
}
