using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs
{
    public class CreateTerritoryDto
    {


        [Required(ErrorMessage = "Territory Description is required!")]
        [Display(Name = "Territory Description")]
        public string TerritoryDescription { get; set; }


        [Display(Name = "PictureUrl")]
        public string? PictureUrl { get; set; }



        [Required(ErrorMessage = "Region is required field!")]
        [Display(Name = "Region")]

        public int RegionId { get; set; }

    }
}
