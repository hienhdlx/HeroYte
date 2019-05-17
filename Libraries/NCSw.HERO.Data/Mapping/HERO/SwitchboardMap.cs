using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;

namespace NCSw.HERO.Data.Mapping
{
    public partial class SwitchboardMap : NopEntityTypeConfiguration<Appointment>
    {
        public override void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable(nameof(Appointment), "hero");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ListOfAppointmentCreatedBy)
                .HasForeignKey(x => x.CreatedBy)
                .IsRequired();
            builder.HasOne(x => x.UpdatedByUser)
                .WithMany(x => x.ListOfAppointmentUpdatedBy)
                .HasForeignKey(x => x.UpdatedBy);

            base.Configure(builder);
        }
    }
}
