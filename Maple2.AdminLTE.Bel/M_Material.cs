using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_material")]
    public class M_Material : Base_Related_Field
    {

        [Required(ErrorMessage = "MaterialCode|Material Code Is Required!!")]
        [Display(Name = "Material Code")]
        [MaxLength(30)]
        public string MaterialCode { get; set; }

        [Required(ErrorMessage = "MaterialName|Material Name Is Required!!")]
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; }

        [Display(Name = "Description 1")]
        public string MaterialDesc1 { get; set; }

        [Display(Name = "Description 2")]
        public string MaterialDesc2 { get; set; }

        [Display(Name = "Raw MAT. Type")]
        public int? RawMatTypeId { get; set; }

        [NotMapped]
        [Display(Name = "Raw MAT. Type")]
        public string RawMatType { get; set; }

        [Display(Name = "Unit")]
        public int? UnitId { get; set; }

        [NotMapped]
        [Display(Name = "Unit")]
        public string Unit { get; set; }


        [Display(Name = "Package")]
        [DisplayFormat(DataFormatString = "{0:#,##0.00}", ApplyFormatInEditMode = true)]
        public decimal? PackageStdQty { get; set; }

        [Display(Name = "W/H")]
        public int? WarehouseId { get; set; }

        [NotMapped]
        [Display(Name = "W/H")]
        public string Warehouse { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [NotMapped]
        [Display(Name = "Location")]
        public string Location { get; set; }


        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [Display(Name = "Material Image")]
        public string MaterialImagePath { get; set; }
    }
}
