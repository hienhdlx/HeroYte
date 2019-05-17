using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;

namespace NCSw.HERO.Data.Mapping
{
    public partial class ServiceMap : NopEntityTypeConfiguration<Service>
    {
        public override void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable(nameof(Service), "hero");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);

            builder.HasOne(x => x.Hospital)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.HospitalId)
                .IsRequired();
            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ListOfServiceCreatedBy)
                .HasForeignKey(x => x.CreatedBy)
                .IsRequired();
            builder.HasOne(x => x.UpdatedByUser)
                .WithMany(x => x.ListOfServiceUpdatedBy)
                .HasForeignKey(x => x.UpdatedBy);

            base.Configure(builder);
        }
    }
}
