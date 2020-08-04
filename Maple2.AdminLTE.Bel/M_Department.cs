using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_department")]
    public class M_Department : Base_Related_Field
    {
        [Display(Name = "Dept. Code")]
        [Required(ErrorMessage = "DeptCode|Dept. Code Is Required!!")]
        [MaxLength(30)]
        public string DeptCode { get; set; }

        [Display(Name = "Dept. Name")]
        [Required(ErrorMessage = "DeptName|Dept. Name Is Required!!")]
        public string DeptName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string DeptDesc { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
