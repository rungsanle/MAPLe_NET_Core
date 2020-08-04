using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_location")]
    public class M_Location : Base_Related_Field
    {
        [Required(ErrorMessage = "LocationCode|Location Code Is Required!!")]
        [Display(Name = "Location Code")]
        [MaxLength(30)]
        public string LocationCode { get; set; }

        [Required(ErrorMessage = "LocationName|Location Name Is Required!!")]
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string LocationDesc { get; set; }

        [Display(Name = "W/H")]
        public int? WarehouseId { get; set; }

        [NotMapped]
        [Display(Name = "W/H Name")]
        public string WarehouseName { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
