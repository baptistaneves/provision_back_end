namespace ProvisionPadel.Api.Entities;

public class Video : Entity
{
    public Guid CameraId { get; private set; }
    public string Name { get; private set; }
    public string VideoDownloadUrl { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Size { get; private set; }
    public bool IsRecording { get; private set; }

    public Camera Camera { get; private set; }

    public static Video Create(string name, string videoDownloadUrl, DateTime startTime, Guid cameraId)
    {
        return new Video
        {
            Name = name,
            StartTime = startTime,
            CameraId = cameraId,
            IsRecording = true,
            VideoDownloadUrl = videoDownloadUrl
        };
    }

    public void Update(DateTime endTime, string size)
    {
        EndTime = endTime;
        Size = size;
        IsRecording = false;
    }
}