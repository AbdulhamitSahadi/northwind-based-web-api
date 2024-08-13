
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs
{
    public class ReadTerritoryDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Territory Description")]
        public string TerritoryDescription { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }


        [Display(Name = "Rojin")]
        public int RegionId { get; set; }


    }
}
