namespace ProvisionPadel.Api.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid,
    IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IApplicationDbContext
{
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Camera> Cameras => Set<Camera>();
    public DbSet<Court> Courts => Set<Court>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}