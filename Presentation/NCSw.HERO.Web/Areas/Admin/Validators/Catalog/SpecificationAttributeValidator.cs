﻿using FluentValidation;
using NCSw.HERO.Web.Areas.Admin.Models.Catalog;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Catalog
{
    public partial class SpecificationAttributeValidator : BaseNopValidator<SpecificationAttributeModel>
    {
        public SpecificationAttributeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Attributes.SpecificationAttributes.Fields.Name.Required"));
        }
    }
}