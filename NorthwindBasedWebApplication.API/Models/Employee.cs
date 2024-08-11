using NorthwindBasedWebApplication.API.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models
{
    public class Employee : BaseEntity
    {
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required field!")]
        public string LastName { get; set; }



        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name is required field!")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "Title is required field!")]
        [Display(Name = "Title")]
        public string Title { get; set; }



        [Required(ErrorMessage = "Title of Courtesy is required field!")]
        [Display(Name = "Title Of Courtesy")]
        public string TitleOfCourtesy { get; set; }



        [Required(ErrorMessage = "Birth date is required field!")]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }



        [Required(ErrorMessage = "Hire Date is required field!")]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }



        [Required(ErrorMessage = "Address is required field!")]
        [Display(Name = "Address")]
        public string Address { get; set; }



        [Required(ErrorMessage = "City is required field!")]
        [Display(Name = "City")]
        public string City { get; set; }



        [Display(Name = "Region")]
        public string? Region { get; set; }



        [Required(ErrorMessage = "Postal Code is required field!")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }



        [Required(ErrorMessage = "Country is required field!")]
        [Display(Name = "Country")]
        public string Country { get; set; }



        [Required(ErrorMessage = "Home phone is required field!")]
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }



        [Required(ErrorMessage = "Extension is required field!")]
        [Display(Name = "Extension")]
        public string? Extension { get; set; }



        [Display(Name = "PictureUrl")]
        public string? PictureUrl { get; set; }



        [Display(Name = "Notes")]
        public string? Notes { get; set; }



        //Navigations Properties
        [Display(Name = "Reports To")]
        [ForeignKey("Report")]
        public int? ReportsTo { get; set; }
        public Employee? Report { get; set; }



        public ICollection<EmployeeTerritory> EmployeeTerritory { get; set; }



        public ICollection<Order> Orders { get; set; }
    }
}
