using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Services.Forums;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class ForumLastPostViewComponent : NopViewComponent
    {
        private readonly IForumModelFactory _forumModelFactory;
        private readonly IForumService _forumService;

        public ForumLastPostViewComponent(IForumModelFactory forumModelFactory, IForumService forumService)
        {
            this._forumModelFactory = forumModelFactory;
            this._forumService = forumService;
        }

        public IViewComponentResult Invoke(int forumPostId, bool showTopic)
        {
            var forumPost = _forumService.GetPostById(forumPostId);
            var model = _forumModelFactory.PrepareLastPostModel(forumPost, showTopic);

            return View(model);
        }
    }
}
