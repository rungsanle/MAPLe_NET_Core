using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_company")]
    public class M_Company : Base_Related_Field
    {
        [Display(Name = "Company Code")]
        [Required(ErrorMessage = "CompanyCode|Company Code Is Required!!")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "CompanyName|Company Name Is Required!!")]
        public string CompanyName { get; set; }

        [Display(Name = "Logo File")]
        public string CompanyLogoPath { get; set; }

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

        [Display(Name = "Tax Id")]
        public string CompanyTaxId { get; set; }

    }
}
