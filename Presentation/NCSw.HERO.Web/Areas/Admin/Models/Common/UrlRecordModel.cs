using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record model
    /// </summary>
    public partial class UrlRecordModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.System.SeNames.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.EntityId")]
        public int EntityId { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }

        #endregion
    }
}