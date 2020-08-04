using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_rawmaterialtype")]
    public class M_RawMaterialType : Base_Related_Field
    {
        [Required(ErrorMessage = "RawMatTypeCode|Raw MAT. Type Code Is Required!!")]
        [Display(Name = "Raw MAT. Type Code")]
        [MaxLength(30)]
        public string RawMatTypeCode { get; set; }

        [Required(ErrorMessage = "RawMatTypeName|Raw MAT. Type Name Is Required!!")]
        [Display(Name = "Raw MAT. Type Name")]
        public string RawMatTypeName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string RawMatTypeDesc { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

    }
}
