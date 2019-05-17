using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Common
{
    public partial class CurrencyModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string CurrencySymbol { get; set; }
    }
}