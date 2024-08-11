using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NorthwindBasedWebApplication.API.Models
{
    public class CustomerDemographic : BaseEntity
    {
        [Display(Name = "Customer Description")]
        public string? CustomerDescription { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }



        //Navigation properties
        public ICollection<CustomerCustomerDemographic> CustomerCustomerDemographic { get; set; }
    }
}
