using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_unit")]
    public class M_Unit : Base_Related_Field
    {
        [Required(ErrorMessage = "UnitCode|Unit Code Is Required!!")]
        [Display(Name = "Unit Code")]
        [MaxLength(30)]
        public string UnitCode { get; set; }

        [Required(ErrorMessage = "UnitName|Unit Name Is Required!!")]
        [Display(Name = "Unit Name")]
        public string UnitName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string UnitDesc { get; set; }
    }
}
