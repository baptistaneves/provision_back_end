namespace ProvisionPadel.Api.Entities;

public class Camera : Entity
{
    public int Channel { get; private set; }
    public bool IsRecording { get; private set; }
    public IEnumerable<Video> Videos { get; private set; }

    public static Camera Create(int channel)
    {
        return new Camera
        {
           Channel = channel,
           IsRecording = false
        };
    }

    public void StartCameraRecording() => IsRecording = true;
    public void StopCameraRecording() => IsRecording = false;
}
