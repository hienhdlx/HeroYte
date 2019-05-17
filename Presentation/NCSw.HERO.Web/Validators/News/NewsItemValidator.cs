using FluentValidation;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;
using NCSw.HERO.Web.Models.News;

namespace NCSw.HERO.Web.Validators.News
{
    public partial class NewsItemValidator : BaseNopValidator<NewsItemModel>
    {
        public NewsItemValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AddNewComment.CommentTitle).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.CommentTitle.Required")).When(x => x.AddNewComment != null);
            RuleFor(x => x.AddNewComment.CommentTitle).Length(1, 200).WithMessage(string.Format(localizationService.GetResource("News.Comments.CommentTitle.MaxLengthValidation"), 200)).When(x => x.AddNewComment != null && !string.IsNullOrEmpty(x.AddNewComment.CommentTitle));
            RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessage(localizationService.GetResource("News.Comments.CommentText.Required")).When(x => x.AddNewComment != null);
        }
    }
}