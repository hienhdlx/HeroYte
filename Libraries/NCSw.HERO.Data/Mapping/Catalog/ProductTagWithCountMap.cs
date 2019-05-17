using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NCSw.HERO.Core.Domain.Catalog;

namespace NCSw.HERO.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product tag with count mapping configuration
    /// </summary>
    public partial class ProductTagWithCountMap : NopQueryTypeConfiguration<ProductTagWithCount>
    {
    }
}