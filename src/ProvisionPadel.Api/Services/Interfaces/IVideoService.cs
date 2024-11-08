namespace ProvisionPadel.Api.Services.Interfaces;

public interface IVideoService
{
    Task Create(string name, DateTime startTime, int ChannelId, CancellationToken cancellationToken);
    Task Update(string name, DateTime endTime, string size, CancellationToken cancellationToken);
    Task<IEnumerable<Video>> GetAll(CancellationToken cancellationToken);
}