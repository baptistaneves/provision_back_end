namespace ProvisionPadel.Api.Data.Mappings;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasOne(a => a.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(a => a.Role)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(u => u.RoleId);
    }
}