using Microsoft.AspNetCore.Mvc;
using NCSw.HERO.Core.Domain.Tax;
using NCSw.HERO.Web.Factories;
using NCSw.HERO.Web.Framework.Components;

namespace NCSw.HERO.Web.Components
{
    public class TaxTypeSelectorViewComponent : NopViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly TaxSettings _taxSettings;

        public TaxTypeSelectorViewComponent(ICommonModelFactory commonModelFactory,
            TaxSettings taxSettings)
        {
            this._commonModelFactory = commonModelFactory;
            this._taxSettings = taxSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_taxSettings.AllowCustomersToSelectTaxDisplayType)
                return Content("");

            var model = _commonModelFactory.PrepareTaxTypeSelectorModel();
            return View(model);
        }
    }
}
