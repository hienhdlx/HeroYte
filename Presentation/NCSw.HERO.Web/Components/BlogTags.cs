﻿using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Blogs;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class BlogTagsViewComponent : NopViewComponent
    {
        private readonly BlogSettings _blogSettings;
        private readonly IBlogModelFactory _blogModelFactory;

        public BlogTagsViewComponent(BlogSettings blogSettings, IBlogModelFactory blogModelFactory)
        {
            this._blogSettings = blogSettings;
            this._blogModelFactory = blogModelFactory;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = _blogModelFactory.PrepareBlogPostTagListModel();
            return View(model);
        }
    }
}