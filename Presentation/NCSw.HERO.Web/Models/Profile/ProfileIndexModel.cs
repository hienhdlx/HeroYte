﻿using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Profile
{
    public partial class ProfileIndexModel : BaseNopModel
    {
        public int CustomerProfileId { get; set; }
        public string ProfileTitle { get; set; }
        public int PostsPage { get; set; }
        public bool PagingPosts { get; set; }
        public bool ForumsEnabled { get; set; }
    }
}