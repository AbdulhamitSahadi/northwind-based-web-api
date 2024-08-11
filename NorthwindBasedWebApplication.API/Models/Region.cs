using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models
{
    public class Region : BaseEntity
    {
        [Required(ErrorMessage = "Region description is required!")]
        [Display(Name = "Region Description")]
        public string RegionDescription { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }



        //Navigation Properties
        public ICollection<Territory> Territories { get; set; }
    }
}
