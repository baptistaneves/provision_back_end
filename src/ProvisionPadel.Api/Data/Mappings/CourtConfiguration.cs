namespace ProvisionPadel.Api.Data.Mappings;

public class CourtConfiguration : IEntityTypeConfiguration<Court>
{
    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Description)
            .HasColumnType("text")
            .IsRequired();

        builder.ToTable("Courts");
    }
}