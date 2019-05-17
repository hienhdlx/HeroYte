using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;

namespace NCSw.HERO.Data.Mapping
{
    public partial class DoctorMap : NopEntityTypeConfiguration<Doctor>
    {
        public override void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable(nameof(Doctor), "hero");
            builder.HasKey(h => h.Id);

            builder.Property(p => p.Code).HasMaxLength(50).IsRequired();
            builder.Property(p => p.FullName).HasMaxLength(255).IsRequired();
            builder.Property(p => p.IdentityCard).HasMaxLength(50).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(255);
            builder.Property(p => p.PhoneNumber).HasMaxLength(255).IsRequired();
            builder.Property(p => p.Address).HasMaxLength(500);

            base.Configure(builder);
        }
    }
}
