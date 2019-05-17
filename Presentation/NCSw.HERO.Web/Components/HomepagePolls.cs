using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class HomepagePollsViewComponent : NopViewComponent
    {
        private readonly IPollModelFactory _pollModelFactory;

        public HomepagePollsViewComponent(IPollModelFactory pollModelFactory)
        {
            this._pollModelFactory = pollModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _pollModelFactory.PrepareHomePagePollModels();
            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
