using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Validators.Localization;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource model
    /// </summary>
    [Validator(typeof(LanguageResourceValidator))]
    public partial class LocaleResourceModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        public string Value { get; set; }

        public int LanguageId { get; set; }

        #endregion
    }
}