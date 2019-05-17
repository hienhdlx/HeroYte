using NCSw.HERO.Core.Domain.Tax;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Common
{
    public partial class TaxTypeSelectorModel : BaseNopModel
    {
        public TaxDisplayType CurrentTaxType { get; set; }
    }
}