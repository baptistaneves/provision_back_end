namespace ProvisionPadel.Api.Data.Mappings;

public class CourtConfiguration : IEntityTypeConfiguration<Court>
{
    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.HasMany(x => x.Cameras)
            .WithOne(x => x.Court)
            .HasForeignKey(x => x.CourtId);

        builder.ToTable("Courts");
    }
}