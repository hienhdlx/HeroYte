using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Models.PrivateMessages
{
    public partial class PrivateMessageListModel : BaseNopModel
    {
        public IList<PrivateMessageModel> Messages { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}