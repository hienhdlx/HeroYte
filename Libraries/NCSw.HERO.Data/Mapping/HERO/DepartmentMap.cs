using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;

namespace NCSw.HERO.Data.Mapping
{
    public partial class DepartmentMap : NopEntityTypeConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable(nameof(Department), "hero");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Ignore(x => x.PathName);
            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ListOfDepartmentCreatedBy)
                .HasForeignKey(x => x.CreatedBy)
                .IsRequired();
            builder.HasOne(x => x.UpdatedByUser)
                .WithMany(x => x.ListOfDepartmentUpdatedBy)
                .HasForeignKey(x => x.UpdatedBy);

            base.Configure(builder);
        }
    }
}
