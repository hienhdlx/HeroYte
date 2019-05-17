using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Areas.Admin.Models.Plugins
{
    /// <summary>
    /// Represents an official feed plugin model
    /// </summary>
    public partial class OfficialFeedPluginModel : BaseNopModel
    {
        #region Properties

        public string Url { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string SupportedVersions { get; set; }

        public string PictureUrl { get; set; }

        public string Price { get; set; }

        #endregion
    }
}