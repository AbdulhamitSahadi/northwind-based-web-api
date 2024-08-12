using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.CustomerDemographicDTOs
{
    public class ReadCustomerDemographicDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Customer Description")]
        public string? CustomerDescription { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }
    }
}
