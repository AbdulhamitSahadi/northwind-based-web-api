using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.ShipperDTOs
{
    public class UpdateShipperDto
    {

        [Display(Name = "Id")]
        [Required(ErrorMessage = "Id is required field!")]
        public int Id { get; set; }


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
