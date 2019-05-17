using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Catalog
{
    public partial class ProductTagModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public int ProductCount { get; set; }
    }
}