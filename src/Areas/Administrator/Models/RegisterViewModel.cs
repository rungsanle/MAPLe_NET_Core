using Maple2.AdminLTE.Bel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maple2.AdminLTE.Uil.Areas.Administrator.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email|{0} IS REQUIRED!!")]
        [EmailAddress(ErrorMessage = "Email|{0} is invalid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password|{0} IS REQUIRED!!")]
        [StringLength(100, ErrorMessage = "Password|{0} must be at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword|{0} IS REQUIRED!!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "ConfirmPassword|Confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "NameIdentifier|{0} IS REQUIRED!!")]
        [StringLength(100, ErrorMessage = "NameIdentifier|{0} must be at least {2} characters.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "User Name")]
        public string NameIdentifier { get; set; }

        [Display(Name = "Agreement")]
        [IsTrueRequired(ErrorMessage = "IsAgree|You must agree the terms.")]
        public bool IsAgree { get; set; }
    }
}
