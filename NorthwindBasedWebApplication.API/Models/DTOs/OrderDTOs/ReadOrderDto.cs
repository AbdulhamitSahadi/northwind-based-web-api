using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace NorthwindBasedWebApplication.API.Models.DTOs.OrderDTOs
{
    public class ReadOrderDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }



        [Display(Name = "Required Date")]
        public DateTime RequiredDate { get; set; }



        [Display(Name = "Shipped Date")]
        public DateTime? ShippedDate { get; set; }



 
        [Display(Name = "Freight")]
        public decimal Freight { get; set; }



    
        [Display(Name = "Ship Name")]
        public string ShipName { get; set; }




        [Display(Name = "Shipped Address")]
        public string ShipAddress { get; set; }



       
        [Display(Name = "Ship City")]
        public string ShipCity { get; set; }



        [Display(Name = "Ship Region")]
        public string? ShipRegion { get; set; }


        [Display(Name = "Ship Postal Code")]
        public string? ShipPostalCode { get; set; }



    
        [Display(Name = "Ship Country")]
        public string ShipCountry { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }




        [Required(ErrorMessage = "Employee Id is required field!")]
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }




        [Required(ErrorMessage = "Ship Via is required field!")]
        [Display(Name = "Ship Via")]
        public int ShipVia { get; set; }




        [Required(ErrorMessage = "Customer Id is required field!")]
        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }
    }
}
