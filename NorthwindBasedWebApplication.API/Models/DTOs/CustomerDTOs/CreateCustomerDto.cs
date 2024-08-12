using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.CustomerDTOs
{
    public class CreateCustomerDto
    {

        [Required(ErrorMessage = "Customer Id name is required field!")]
        [Display(Name = "Customer ID")]
        public string CustomerId { get; set; }



        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company name is required field!")]
        public string CompanyName { get; set; }



        [Required(ErrorMessage = "Contact name is required field!")]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }



        [Required(ErrorMessage = "Contact title is required field!")]
        [Display(Name = "Contact Title")]
        public string ContactTitle { get; set; }



        [Required(ErrorMessage = "Address is required field!")]
        [Display(Name = "Address")]
        public string Address { get; set; }



        [Display(Name = "City")]
        public string? City { get; set; }



        [Display(Name = "Region")]
        public string? Region { get; set; }



        [Required(ErrorMessage = "Postal code is required field!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }



        [Required(ErrorMessage = "Country is required field!")]
        [Display(Name = "Country")]
        public string Country { get; set; }



        [Required(ErrorMessage = "Phone is required field")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Display(Name = "Fax")]
        public string? Fax { get; set; }



        [Display(Name = "Picture")]
        public string? PictureUrl { get; set; }
    }
}
