using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_warehouse")]
    public class M_Warehouse : Base_Related_Field
    {
        [Required(ErrorMessage = "WarehouseCode|W/H Code Is Required!!")]
        [Display(Name = "W/H Code")]
        [MaxLength(30)]
        public string WarehouseCode { get; set; }

        [Required(ErrorMessage = "WarehouseName|W/H Name Is Required!!")]
        [Display(Name = "W/H Name")]
        public string WarehouseName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string WarehouseDesc { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
