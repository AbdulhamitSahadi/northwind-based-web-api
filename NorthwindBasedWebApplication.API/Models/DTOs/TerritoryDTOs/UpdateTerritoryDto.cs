using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.TerritoryDTOs
{
    public class UpdateTerritoryDto
    {

        [Required(ErrorMessage = "Id is required field")]
        public int Id { get; set; }


        [Required(ErrorMessage = "Territory Description is required!")]
        [Display(Name = "Territory Description")]
        public string TerritoryDescription { get; set; }


        [Display(Name = "PictureUrl")]
        public string? PictureUrl { get; set; }




    }
}
