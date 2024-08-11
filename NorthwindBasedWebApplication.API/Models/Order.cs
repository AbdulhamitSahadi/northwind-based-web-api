using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models
{
    public class Order : BaseEntity
    {
        [Required(ErrorMessage = "Order date is required field!")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }



        [Required(ErrorMessage = "Required date is required field!")]
        [Display(Name = "Required Date")]
        public DateTime RequiredDate { get; set; }



        [Display(Name = "Shipped Date")]
        public DateTime? ShippedDate { get; set; }



        [Required(ErrorMessage = "Freight is required field!")]
        [Display(Name = "Freight")]
        public decimal Freight { get; set; }



        [Required(ErrorMessage = "Ship name is required field!")]
        [Display(Name = "Ship Name")]
        public string ShipName { get; set; }



        [Required(ErrorMessage = "Shipped address is required field!")]
        [Display(Name = "Shipped Address")]
        public string ShipAddress { get; set; }



        [Required(ErrorMessage = "Ship city is required field!")]
        [Display(Name = "Ship City")]
        public string ShipCity { get; set; }



        [Display(Name = "Ship Region")]
        public string? ShipRegion { get; set; }


        [Display(Name = "Ship Postal Code")]
        public string? ShipPostalCode { get; set; }



        [Required(ErrorMessage = "Ship country is required field!")]
        [Display(Name = "Ship Country")]
        public string ShipCountry { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }



        //Navigation Properties
        [Required(ErrorMessage = "Employee Id is required field!")]
        [Display(Name = "Employee Id")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        [Required(ErrorMessage = "Ship Via is required field!")]
        [Display(Name = "Ship Via")]
        [ForeignKey("Shipper")]
        public int ShipVia { get; set; }
        public Shipper Shipper { get; set; }


        [Required(ErrorMessage = "Customer Id is required field!")]
        [Display(Name = "Customer Id")]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }


        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
