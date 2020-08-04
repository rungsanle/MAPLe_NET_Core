using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_materialtype")]
    public class M_MaterialType : Base_Related_Field
    {

        [Required(ErrorMessage = "MatTypeCode|MAT. Type Code Is Required!!")]
        [Display(Name = "MAT. Type Code")]
        [MaxLength(30)]
        public string MatTypeCode { get; set; }

        [Required(ErrorMessage = "MatTypeName|MAT. Type Name Is Required!!")]
        [Display(Name = "MAT. Type Name")]
        public string MatTypeName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string MatTypeDesc { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
