using FluentValidation;
using NCSw.HERO.Web.Areas.Admin.Models.Shipping;
using NCSw.HERO.Core.Domain.Shipping;
using NCSw.HERO.Data;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Shipping
{
    public partial class WarehouseValidator : BaseNopValidator<WarehouseModel>
    {
        public WarehouseValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Warehouses.Fields.Name.Required"));

            SetDatabaseValidationRules<Warehouse>(dbContext);
        }
    }
}