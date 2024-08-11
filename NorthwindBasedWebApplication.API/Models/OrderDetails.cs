using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NorthwindBasedWebApplication.API.Models.Base;

namespace NorthwindBasedWebApplication.API.Models
{
    public class OrderDetails : BaseEntity
    {
        [Display(Name = "Unit Price")]
        [Required(ErrorMessage = "Unit Price is required field!")]
        public decimal UnitPrice { get; set; }



        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required field!")]
        public short Quantity { get; set; }



        [Display(Name = "Discount")]
        [Required(ErrorMessage = "Discount is required field!")]
        public float Discount { get; set; }



        //Navigation Properties 
        [Required(ErrorMessage = "Order Id is required field!")]
        [ForeignKey("Order")]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }
        public Order Order { get; set; }



        [Display(Name = "Product Id")]
        [Required(ErrorMessage = "Product Id is required field!")]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }
    }
}
