using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.ShipperDTOs
{
    public class ReadShipperDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }




        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }

    }
}
