using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Models
{
    public class AppUserViewModel
    {
        [Required(ErrorMessage = "Id|{0} IS REQUIRED!!")]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required(ErrorMessage = "UserName|{0} IS REQUIRED!!")]
        [StringLength(100, ErrorMessage = "UserName|{0} must be at least {2} characters.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email|{0} IS REQUIRED!!")]
        [EmailAddress(ErrorMessage = "Email|{0} is invalid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Password|{0} must be at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Phone #")]
        public string PhoneNumber { get; set; }

        [Display(Name = "User Roles")]
        public List<string> Roles { get; set; }
    }
}
