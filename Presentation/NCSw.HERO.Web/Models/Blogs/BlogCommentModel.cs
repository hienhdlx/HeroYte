﻿using System;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Blogs
{
    public partial class BlogCommentModel : BaseNopEntityModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAvatarUrl { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }
    }
}