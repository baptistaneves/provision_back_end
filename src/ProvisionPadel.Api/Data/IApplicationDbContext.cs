namespace ProvisionPadel.Api.Data;

public interface IApplicationDbContext
{
    public DbSet<Video> Videos { get; }
    public DbSet<Camera> Cameras { get; }
    public DbSet<Court> Courts { get; }
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
