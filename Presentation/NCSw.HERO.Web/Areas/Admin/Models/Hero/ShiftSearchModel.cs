using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Models
{
    public class ShiftSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Hero.Admin.Shifts.SearchName")]
        public string SearchName { get; set; }
    }
}
