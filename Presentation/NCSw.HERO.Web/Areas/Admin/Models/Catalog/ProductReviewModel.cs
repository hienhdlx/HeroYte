﻿using System;
using FluentValidation.Attributes;
using NCSw.HERO.Web.Areas.Admin.Validators.Catalog;
using NCSw.HERO.Web.Framework.Models;
using NCSw.HERO.Web.Framework.Mvc.ModelBinding;

namespace NCSw.HERO.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review model
    /// </summary>
    [Validator(typeof(ProductReviewValidator))]
    public partial class ProductReviewModel : BaseNopEntityModel
    {
        #region Ctor

        public ProductReviewModel()
        {
            ProductReviewReviewTypeMappingSearchModel = new ProductReviewReviewTypeMappingSearchModel();            
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Store")]
        public string StoreName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public int ProductId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Customer")]
        public int CustomerId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Customer")]
        public string CustomerInfo { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Title")]
        public string Title { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReviewText")]
        public string ReviewText { get; set; }
        
        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReplyText")]
        public string ReplyText { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Rating")]
        public int Rating { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [NopResourceDisplayName("Admin.Catalog.ProductReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //vendor
        public bool IsLoggedInAsVendor { get; set; }

        public ProductReviewReviewTypeMappingSearchModel ProductReviewReviewTypeMappingSearchModel { get; set; }

        #endregion
    }
}