using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;

namespace NCSw.HERO.Data.Mapping
{
    public partial class HospitalMap : NopEntityTypeConfiguration<Hospital>
    {
        public override void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.ToTable(nameof(Hospital), "hero");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Logo).HasMaxLength(500);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Address).HasMaxLength(500);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ListOfHospitalCreatedBy)
                .HasForeignKey(x => x.CreatedBy)
                .IsRequired();
            builder.HasOne(x => x.UpdatedByUser)
                .WithMany(x => x.ListOfHospitalUpdatedBy)
                .HasForeignKey(x => x.UpdatedBy);

            base.Configure(builder);
        }
    }
}
