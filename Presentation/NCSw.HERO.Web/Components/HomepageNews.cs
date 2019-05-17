﻿using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.News;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class HomepageNewsViewComponent : NopViewComponent
    {
        private readonly INewsModelFactory _newsModelFactory;
        private readonly NewsSettings _newsSettings;

        public HomepageNewsViewComponent(INewsModelFactory newsModelFactory, NewsSettings newsSettings)
        {
            this._newsModelFactory = newsModelFactory;
            this._newsSettings = newsSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowNewsOnMainPage)
                return Content("");

            var model = _newsModelFactory.PrepareHomePageNewsItemsModel();
            return View(model);
        }
    }
}
