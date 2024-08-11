using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models
{
    public class Shipper : BaseEntity
    {
        [Required(ErrorMessage = "Company name is required field!")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }



        [Required(ErrorMessage = "Shipper phone is required field!")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }



        //Navigation Properties
        public ICollection<Order> Orders { get; set; }
    }
}
