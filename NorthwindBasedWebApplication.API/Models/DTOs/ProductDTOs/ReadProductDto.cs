
using System.ComponentModel.DataAnnotations;


namespace NorthwindBasedWebApplication.API.Models.DTOs.ProductDTOs
{
    public class ReadProductDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }




        
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }



       
        [Display(Name = "Quantity Per Unit")]
        public string QuantityPerUnit { get; set; }



        
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }



        
        [Display(Name = "Units In Stock")]
        public short UnitsInStock { get; set; }



        
        [Display(Name = "Units On Order")]
        public short UnitsOnOrder { get; set; }



       
        [Display(Name = "Reorder Level")]
        public short ReorderLevel { get; set; }



        
        [Required(ErrorMessage = "Discontinued is required field!")]
        public bool Discontinued { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }



      
    }
}
