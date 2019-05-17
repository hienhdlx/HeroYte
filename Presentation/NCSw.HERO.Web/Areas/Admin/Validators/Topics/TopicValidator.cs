using FluentValidation;
using NCSw.HERO.Web.Areas.Admin.Models.Topics;
using NCSw.HERO.Core.Domain.Topics;
using NCSw.HERO.Data;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Services.Seo;
using NCSw.HERO.Web.Framework.Validators;

namespace NCSw.HERO.Web.Areas.Admin.Validators.Topics
{
    public partial class TopicValidator : BaseNopValidator<TopicModel>
    {
        public TopicValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.SeName).Length(0, NopSeoDefaults.ForumTopicLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), NopSeoDefaults.ForumTopicLength));

            SetDatabaseValidationRules<Topic>(dbContext);
        }
    }
}
