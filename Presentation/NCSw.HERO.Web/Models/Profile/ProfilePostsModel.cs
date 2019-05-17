using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Models.Common;

namespace NCSw.HERO.Web.Models.Profile
{
    public partial class ProfilePostsModel : BaseNopModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}