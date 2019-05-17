using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    public partial class ServiceSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Hero.Admin.Services.Fields.Name")]
        public string SearchName { get; set; }
    }
}
