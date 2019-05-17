using System.Collections.Generic;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Boards
{
    public partial class BoardsIndexModel : BaseNopModel
    {
        public BoardsIndexModel()
        {
            this.ForumGroups = new List<ForumGroupModel>();
        }
        
        public IList<ForumGroupModel> ForumGroups { get; set; }
    }
}