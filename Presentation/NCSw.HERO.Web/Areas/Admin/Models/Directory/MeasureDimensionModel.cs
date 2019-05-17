﻿using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Validators.Directory;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a measure dimension model
    /// </summary>
    [Validator(typeof(MeasureDimensionValidator))]
    public partial class MeasureDimensionModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.SystemKeyword")]
        public string SystemKeyword { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.Ratio")]
        public decimal Ratio { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Shipping.Measures.Dimensions.Fields.IsPrimaryDimension")]
        public bool IsPrimaryDimension { get; set; }

        #endregion
    }
}