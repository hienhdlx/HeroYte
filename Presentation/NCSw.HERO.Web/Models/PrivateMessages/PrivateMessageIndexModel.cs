using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.PrivateMessages
{
    public partial class PrivateMessageIndexModel : BaseNopModel
    {
        public int InboxPage { get; set; }
        public int SentItemsPage { get; set; }
        public bool SentItemsTabSelected { get; set; }
    }
}