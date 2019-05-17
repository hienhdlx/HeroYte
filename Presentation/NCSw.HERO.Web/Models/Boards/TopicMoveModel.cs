﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Boards
{
    public partial class TopicMoveModel : BaseNopEntityModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int ForumSelected { get; set; }
        public string TopicSeName { get; set; }
        
        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}