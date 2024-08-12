
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.RegionDTOs
{
    public class ReadRegionDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Region Description")]
        public string RegionDescription { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }


    }
}
