namespace ProvisionPadel.Api.Data.Mappings;

public class CameraConfiguration : IEntityTypeConfiguration<Camera>
{
    public void Configure(EntityTypeBuilder<Camera> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Channel)
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(e => e.IsRecording)
            .HasColumnType("boolean")
            .IsRequired();

        builder.HasMany(x => x.Videos)
            .WithOne(x => x.Camera)
            .HasForeignKey(x => x.CameraId);

        builder.ToTable("Cameras");
    }
}