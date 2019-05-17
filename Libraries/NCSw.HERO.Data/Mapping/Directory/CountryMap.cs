using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain.Directory;

namespace NCSw.HERO.Data.Mapping.Directory
{
    /// <summary>
    /// Represents a country mapping configuration
    /// </summary>
    public partial class CountryMap : NopEntityTypeConfiguration<Country>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country));
            builder.HasKey(country => country.Id);

            builder.Property(country => country.Name).HasMaxLength(100).IsRequired();
            builder.Property(country => country.TwoLetterIsoCode).HasMaxLength(2);
            builder.Property(country => country.ThreeLetterIsoCode).HasMaxLength(3);

            base.Configure(builder);
        }

        #endregion
    }
}