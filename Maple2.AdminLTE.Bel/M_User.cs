using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_user")]
    public class M_User :  Base_Related_Field
    {

        [Display(Name = "User Code")]
        [Required(ErrorMessage = "UserCode|User Code Is Required!!")]
        [MaxLength(30)]
        public string UserCode { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "UserName|User Name Is Required!!")]
        public string UserName { get; set; }

        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; }

        [Display(Name = "Department Id")]
        public int? DeptId { get; set; }

        [NotMapped]
        [Display(Name = "Department")]
        public string DeptName { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [Display(Name = "Login User Id")]
        [MaxLength(255)]
        public string aspnetuser_Id { get; set; }

        [Display(Name = "User Image")]
        public string UserImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Company Logo")]
        public string CompanyLogoPath { get; set; }
    }
}
