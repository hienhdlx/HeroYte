using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseNopModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}