namespace ProvisionPadel.Api.Data.Mappings;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
           .HasMany(user => user.UserRoles)
           .WithOne(userRole => userRole.User)
           .HasForeignKey(userRole => userRole.UserId);
    }
}