﻿using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Catalog
{
    public partial class SearchBoxModel : BaseNopModel
    {
        public bool AutoCompleteEnabled { get; set; }
        public bool ShowProductImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
    }
}