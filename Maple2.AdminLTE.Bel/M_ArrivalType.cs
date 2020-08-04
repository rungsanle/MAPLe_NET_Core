using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_arrivaltype")]
    public class M_ArrivalType : Base_Related_Field
    {

        [Display(Name = "Arr. Type Code")]
        [Required(ErrorMessage = "ArrivalTypeCode|Arrival Type Code Is Required!!")]
        [MaxLength(30)]
        public string ArrivalTypeCode { get; set; }

        [Display(Name = "Arr. Type Name")]
        [Required(ErrorMessage = "ArrivalTypeName|Arrival Type Name Is Required!!")]
        public string ArrivalTypeName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string ArrivalTypeDesc { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
