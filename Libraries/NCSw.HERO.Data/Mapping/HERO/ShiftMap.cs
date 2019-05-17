using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCSw.HERO.Data.Mapping
{
    public partial class ShiftMap : NopEntityTypeConfiguration<Shift>
    {
        public override void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable(nameof(Shift), "hero");
            builder.HasKey(h => h.Id);

            builder.Property(p => p.Code).HasMaxLength(255).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(255).IsRequired();
            builder.Property(p => p.Description).HasMaxLength(255);

            base.Configure(builder);
        }
    }
}
