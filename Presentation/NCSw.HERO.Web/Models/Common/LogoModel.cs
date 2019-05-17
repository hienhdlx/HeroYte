using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Common
{
    public partial class LogoModel : BaseNopModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}