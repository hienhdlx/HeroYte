using FluentValidation;
using FluentValidation.Validators;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Areas.Admin.Models;
using NCSw.HERO.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCSw.HERO.Web.Areas.Admin.Validators
{
    public class ShiftValidator : BaseNopValidator<ShiftModel>
    {
        public ShiftValidator(ILocalizationService localizationService)
        {

            RuleFor(x => x.Code).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Shifts.Fields.Code")))
                .SetValidator(new MaximumLengthValidator(50))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Shifts.Fields.Code"), 50));

            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Shifts.Fields.Name")))
                .SetValidator(new MaximumLengthValidator(255))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Shifts.Fields.Name"), 255));

            RuleFor(x => x.Description).SetValidator(new MaximumLengthValidator(500))
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.Characters.MaxLength"), localizationService.GetResource("Hero.Admin.Shifts.Fields.Description"), 500));

            RuleFor(x => x.FromHour).NotEmpty()
                .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Shifts.Fields.FromHour")));

            RuleFor(x => x.ToHour).NotEmpty()
            .WithMessage(string.Format(localizationService.GetResource("Hero.Validators.InputFields.Required"), localizationService.GetResource("Hero.Admin.Shifts.Fields.ToHour")));

            RuleFor(x => x.ToHour).Must((x, context) =>
            {
                return CheckBreakTime(x.FromHour, x.ToHour);
            }).WithMessage(string.Format(localizationService.GetResource("Hero.Admin.Shifts.CheckTime"), localizationService.GetResource("Hero.Admin.Shifts.CheckTime")));

            RuleFor(x => x.BreakTimeTo).Must((x, context) =>
            {
                return CheckBreakTime(x.BreakTimeFrom, x.BreakTimeTo);
            }).WithMessage(string.Format(localizationService.GetResource("Hero.Admin.Shifts.CheckBreakTime"), localizationService.GetResource("Hero.Admin.Shifts.CheckBreakTime"))); ; 
        }

        public static bool CheckBreakTime(TimeSpan? t1, TimeSpan? t2)
        {
            if (t1 == null || t2 == null)
                return true;
            if (TimeSpan.Compare(t2.Value, t1.Value) == 1)
                return true;
            return false;
        }
    }
}
