using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a popular search term model
    /// </summary>
    public partial class PopularSearchTermModel : BaseNopModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.SearchTermReport.Keyword")]
        public string Keyword { get; set; }

        [NopResourceDisplayName("Admin.SearchTermReport.Count")]
        public int Count { get; set; }

        #endregion
    }
}
