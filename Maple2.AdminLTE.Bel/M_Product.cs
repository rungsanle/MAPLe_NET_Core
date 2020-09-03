using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_product")]
    public class M_Product : Base_Related_Field
    {
        [Required(ErrorMessage = "ProductCode|Product Code Is Required!!")]
        [Display(Name = "Product Code")]
        [MaxLength(30)]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "ProductName|Product Name Is Required!!")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Name Ref.")]
        public string ProductNameRef { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string ProductDesc { get; set; }

        [Display(Name = "Material Type")]
        public int? MaterialTypeId { get; set; }

        [NotMapped]
        [Display(Name = "MAT. Type")]
        public string MaterialType { get; set; }

        [Display(Name = "Production Type")]
        public int? ProductionTypeId { get; set; }

        [NotMapped]
        [Display(Name = "PROD. TYPE")]
        public string ProductionType { get; set; }

        [Display(Name = "Machine")]
        public int? MachineId { get; set; }

        [NotMapped]
        [Display(Name = "Machine")]
        public string Machine { get; set; }

        [Display(Name = "UNIT")]
        public int? UnitId { get; set; }

        [NotMapped]
        [Display(Name = "UNIT")]
        public string Unit { get; set; }

        [Display(Name = "Package")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? PackageStdQty { get; set; }

        [Display(Name = "Sales Price 1")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SalesPrice1 { get; set; }

        [Display(Name = "Sales Price 2")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SalesPrice2 { get; set; }

        [Display(Name = "Sales Price 3")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SalesPrice3 { get; set; }

        [Display(Name = "Sales Price 4")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SalesPrice4 { get; set; }

        [Display(Name = "Sales Price 5")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18,4)")]
        public decimal? SalesPrice5 { get; set; }

        [Required(ErrorMessage = "GLSalesAccount|G/L Sales Account Is Required!!")]
        [Display(Name = "G/L Sales Account")]
        public string GLSalesAccount { get; set; }

        [Display(Name = "G/L Inventory Account")]
        public string GLInventAccount { get; set; }

        [Required(ErrorMessage = "GLCogsAccount|G/L COGS/Salary Acct Is Required!!")]
        [Display(Name = "G/L COGS/Salary Account")]
        public string GLCogsAccount { get; set; }

        [Display(Name = "Revision Number")]
        public int? RevisionNo { get; set; }

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

        [Display(Name = "Product Image")]
        public string ProductImagePath { get; set; }

        [NotMapped]
        public List<M_Product_Process> ProdProcess { get; set; }

    }
}
