using System.ComponentModel.DataAnnotations;
using NCSw.HERO.Web.Framework.Models;

namespace NCSw.HERO.Web.Models.Newsletter
{
    public partial class NewsletterBoxModel : BaseNopModel
    {
        [DataType(DataType.EmailAddress)]
        public string NewsletterEmail { get; set; }
        public bool AllowToUnsubscribe { get; set; }
    }
}