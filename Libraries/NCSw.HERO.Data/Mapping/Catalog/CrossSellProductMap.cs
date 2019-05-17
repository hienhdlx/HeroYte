using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain.Catalog;

namespace NCSw.HERO.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a cross-sell product mapping configuration
    /// </summary>
    public partial class CrossSellProductMap : NopEntityTypeConfiguration<CrossSellProduct>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CrossSellProduct> builder)
        {
            builder.ToTable(nameof(CrossSellProduct));
            builder.HasKey(product => product.Id);

            base.Configure(builder);
        }

        #endregion
    }
}