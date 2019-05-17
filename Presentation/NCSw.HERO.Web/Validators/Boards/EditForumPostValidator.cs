using FluentValidation;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;
using NCSw.HERO.Web.Models.Boards;

namespace NCSw.HERO.Web.Validators.Boards
{
    public partial class EditForumPostValidator : BaseNopValidator<EditForumPostModel>
    {
        public EditForumPostValidator(ILocalizationService localizationService)
        {            
            RuleFor(x => x.Text).NotEmpty().WithMessage(localizationService.GetResource("Forum.TextCannotBeEmpty"));
        }
    }
}