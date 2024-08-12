
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs
{
    public class ReadSupplierDto
    {

        [Display(Name = "Id")]
        public int Id { get; set; }



        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }



        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }



 
        [Display(Name = "Contact Title")]
        public string ContactTitle { get; set; }




        [Display(Name = "Address")]
        public string Address { get; set; }




        [Display(Name = "City")]
        public string City { get; set; }



        [Display(Name = "Region")]
        public string? Region { get; set; }



        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }



   
        [Display(Name = "Country")]
        public string Country { get; set; }



 
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Display(Name = "Fax")]
        public string? Fax { get; set; }


        [Display(Name = "Home Page")]
        public string? HomePage { get; set; }


        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }

    }
}
