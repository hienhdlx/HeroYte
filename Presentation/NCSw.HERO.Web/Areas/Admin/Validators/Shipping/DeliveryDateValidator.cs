using FluentValidation;
using NCSw.HERO.Web.Areas.Admin.Models.Shipping;
using NCSw.HERO.Core.Domain.Shipping;
using NCSw.HERO.Data;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Shipping
{
    public partial class DeliveryDateValidator : BaseNopValidator<DeliveryDateModel>
    {
        public DeliveryDateValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.DeliveryDates.Fields.Name.Required"));

            SetDatabaseValidationRules<DeliveryDate>(dbContext);
        }
    }
}