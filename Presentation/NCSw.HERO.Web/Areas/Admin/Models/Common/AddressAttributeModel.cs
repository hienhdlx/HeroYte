using System.Collections.Generic;
using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Validators.Common;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an address attribute model
    /// </summary>
    [Validator(typeof(AddressAttributeValidator))]
    public partial class AddressAttributeModel : BaseNopEntityModel, ILocalizedModel<AddressAttributeLocalizedModel>
    {
        #region Ctor

        public AddressAttributeModel()
        {
            Locales = new List<AddressAttributeLocalizedModel>();
            AddressAttributeValueSearchModel = new AddressAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<AddressAttributeLocalizedModel> Locales { get; set; }

        public AddressAttributeValueSearchModel AddressAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class AddressAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}