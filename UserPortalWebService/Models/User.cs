using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserPortalWebService.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        [Required]
        [StringLength(250)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [StringLength(150)]
        [DisplayName("Phone")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid phone number")]
        [MinLength(11)]
        public string Phone { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "You must provide an Email address")]
        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "You must provide Password")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }




        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }


    }
}