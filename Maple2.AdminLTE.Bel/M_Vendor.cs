using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_vendor")]
    public class M_Vendor : Base_Related_Field
    {

        [Display(Name = "Vendor Code")]
        [Required(ErrorMessage = "VendorCode|Vendor Code Is Required!!")]
        [MaxLength(30)]
        public string VendorCode { get; set; }

        [Display(Name = "Vendor Name")]
        [Required(ErrorMessage = "VendorName|Vendor Name Is Required!!")]
        public string VendorName { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddressL1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressL2 { get; set; }

        [Display(Name = "Address Line 3")]
        public string AddressL3 { get; set; }

        [Display(Name = "Address Line 4")]
        public string AddressL4 { get; set; }

        [Display(Name = "Tel.")]
        public string Telephone { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Email")]
        public string VendorEmail { get; set; }

        [Display(Name = "Contact")]
        public string VendorContact { get; set; }

        [Display(Name = "Credit Term")]
        [DefaultValue(0)]
        public int? CreditTerm { get; set; }

        [Display(Name = "Price Level")]
        [DefaultValue(0)]
        public int? PriceLevel { get; set; }

        [Display(Name = "Tax Id")]
        public string VendorTaxId { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

    }
}
