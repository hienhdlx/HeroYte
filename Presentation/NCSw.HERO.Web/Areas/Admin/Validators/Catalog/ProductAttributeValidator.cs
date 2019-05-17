using FluentValidation;
using NCSw.HERO.Web.Areas.Admin.Models.Catalog;
using NCSw.HERO.Core.Domain.Catalog;
using NCSw.HERO.Data;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Catalog
{
    public partial class ProductAttributeValidator : BaseNopValidator<ProductAttributeModel>
    {
        public ProductAttributeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Attributes.ProductAttributes.Fields.Name.Required"));
            SetDatabaseValidationRules<ProductAttribute>(dbContext);
        }
    }
}