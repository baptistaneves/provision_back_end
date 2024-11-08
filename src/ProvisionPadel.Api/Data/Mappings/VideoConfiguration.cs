namespace ProvisionPadel.Api.Data.Mappings;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.Size)
            .HasColumnType("varchar(255)")
            .IsRequired(false);

        builder.Property(e => e.StartTime)
            .IsRequired();

        builder.Property(e => e.EndTime)
            .IsRequired();

        builder.ToTable("Videos");
    }
}