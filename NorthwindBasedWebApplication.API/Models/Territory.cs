using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NorthwindBasedWebApplication.API.Models.Base;

namespace NorthwindBasedWebApplication.API.Models
{
    public class Territory : BaseEntity
    {
        [Required]
        [Display(Name = "Territory Description")]
        public string TerritoryDescription { get; set; }



        [Display(Name = "PictureUrl")]
        public string? PictureUrl { get; set; }



        //Navigations
        public ICollection<EmployeeTerritory> EmployeeTerritory { get; set; }



        [Required]
        [Display(Name = "Region")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Region Region { get; set; }

    }
}
