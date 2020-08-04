using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_productiontype")]
    public class M_ProductionType : Base_Related_Field
    {
        [Required(ErrorMessage = "ProdTypeCode|Prod. Type Code Is Required!!")]
        [Display(Name = "Prod. Type Code")]
        [MaxLength(30)]
        public string ProdTypeCode { get; set; }

        [Required(ErrorMessage = "ProdTypeName|Prod. Type Name Is Required!!")]
        [Display(Name = "Prod. Type Name")]
        public string ProdTypeName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string ProdTypeDesc { get; set; }

        [Display(Name = "Seq")]
        public int? ProdTypeSeq { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

    }
}
