using FluentValidation;
using NCSw.HERO.Services.Localization;
using NCSw.HERO.Web.Framework.Validators;
using NCSw.HERO.Web.Models.Blogs;

namespace NCSw.HERO.Web.Validators.Blogs
{
    public partial class BlogPostValidator : BaseNopValidator<BlogPostModel>
    {
        public BlogPostValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.AddNewComment.CommentText).NotEmpty().WithMessage(localizationService.GetResource("Blog.Comments.CommentText.Required")).When(x => x.AddNewComment != null);
        }
    }
}