﻿
using System.ComponentModel.DataAnnotations;

namespace NorthwindBasedWebApplication.API.Models.DTOs.SupplierDTOs
{
    public class UpdateSupplierDto
    {

        [Required(ErrorMessage = "Id is required field!")]
        public int Id { get; set; }



        [Required(ErrorMessage = "Company name is required!")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }



        [Required(ErrorMessage = "Contact name is required field!")]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }




        [Required(ErrorMessage = "Contact Title is required field!")]
        [Display(Name = "Contact Title")]
        public string ContactTitle { get; set; }



        [Required(ErrorMessage = "Address is required field!")]
        [Display(Name = "Address")]
        public string Address { get; set; }



        [Required(ErrorMessage = "City is required field!")]
        [Display(Name = "City")]
        public string City { get; set; }




        [Display(Name = "Region")]
        public string? Region { get; set; }


        [Required(ErrorMessage = "Postal code is required field!")]
        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }




        [Required(ErrorMessage = "Country is required field!")]
        [Display(Name = "Country")]
        public string Country { get; set; }



        [Required(ErrorMessage = "Phone is required field!")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }



        [Display(Name = "Fax")]
        public string? Fax { get; set; }


        [Display(Name = "Home Page")]
        public string? HomePage { get; set; }


        [Display(Name = "Picture")]
        public string? Picture { get; set; }
    }
}