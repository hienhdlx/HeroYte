using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseNopModel
    {
        public string WidgetViewComponentName { get; set; }
        public object WidgetViewComponentArguments { get; set; }
    }
}