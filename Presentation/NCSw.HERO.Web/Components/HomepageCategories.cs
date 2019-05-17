using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class HomepageCategoriesViewComponent : NopViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public HomepageCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _catalogModelFactory.PrepareHomepageCategoryModels();
            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
