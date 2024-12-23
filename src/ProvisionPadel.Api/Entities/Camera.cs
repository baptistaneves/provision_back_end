namespace ProvisionPadel.Api.Entities;

public class Camera : Entity
{
    public Guid CourtId { get; private set; }
    public int Channel { get; private set; }
    public bool IsRecording { get; private set; }

    public Court Court { get; private set; }
    public IEnumerable<Video> Videos { get; set; }

    public static Camera Create(int channel, Guid courtId)
    {
        return new Camera
        {
            CourtId = courtId,
           Channel = channel,
           IsRecording = false
        };
    }

    public void Update(int channel, Guid courtId)
    {
        Channel = channel;
        CourtId = courtId;
    }

    public void StartCameraRecording() => IsRecording = true;
    public void StopCameraRecording() => IsRecording = false;
}
