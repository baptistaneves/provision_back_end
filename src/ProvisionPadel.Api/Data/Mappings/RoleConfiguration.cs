namespace ProvisionPadel.Api.Data.Mappings;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
           .HasMany(role => role.UserRoles)
           .WithOne(userRole => userRole.Role)
           .HasForeignKey(userRole => userRole.RoleId);
    }
}