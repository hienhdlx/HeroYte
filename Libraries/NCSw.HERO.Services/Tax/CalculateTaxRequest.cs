using NCSw.HERO.Core.Domain.Catalog;
using NCSw.HERO.Core.Domain.Common;
using NCSw.HERO.Core.Domain.Customers;

namespace NCSw.HERO.Services.Tax
{
    /// <summary>
    /// Represents a request for tax calculation
    /// </summary>
    public partial class CalculateTaxRequest
    {
        /// <summary>
        /// Gets or sets a customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets a product
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Gets or sets an address
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// Gets or sets a tax category identifier
        /// </summary>
        public int TaxCategoryId { get; set; }

        /// <summary>
        /// Gets or sets a price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a current store identifier
        /// </summary>
        public int CurrentStoreId { get; set; }
    }
}
