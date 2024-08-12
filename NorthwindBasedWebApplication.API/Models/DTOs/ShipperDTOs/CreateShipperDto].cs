using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.ShipperDTOs
{
    public class CreateShipperDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }



        [Required(ErrorMessage = "Shipper phone is required field!")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }


    }
}
