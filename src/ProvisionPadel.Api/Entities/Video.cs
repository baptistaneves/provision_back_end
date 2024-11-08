namespace ProvisionPadel.Api.Entities;

public class Video : Entity
{
    public string Name { get; private set; }
    public int Channel { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Size { get; private set; }
    public bool IsRecording { get; private set; }

    public static Video Create(string name, DateTime startTime, int channel)
    {
        return new Video
        {
            Name = name,
            StartTime = startTime,
            Channel = channel,
            IsRecording = true
        };
    }

    public void Update(DateTime endTime, string size)
    {
        EndTime = endTime;
        Size = size;
        IsRecording = false;
    }
}