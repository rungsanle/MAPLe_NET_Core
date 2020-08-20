using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("t_arrival_hdr")]
    public class T_Arrival_Header : Base_Related_Field
    {
        [Display(Name = "Arrival #")]
        [MaxLength(30)]
        public string ArrivalNo { get; set; }

        [Required(ErrorMessage = "ArrivalDate|ARRIVAL DATE IS REQUIRED!!")]
        [Display(Name = "Arrival Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ArrivalDate { get; set; }

        [Display(Name = "Raw MAT. Type")]
        public int? RawMatTypeId { get; set; }

        [NotMapped]
        [Display(Name = "Raw MAT. Type")]
        public string RawMatTypeName { get; set; }

        public int? VendorId { get; set; }

        [NotMapped]
        [Display(Name = "Vendor Code")]
        public string VendorCode { get; set; }

        [NotMapped]
        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }

        [NotMapped]
        [Display(Name = "Vendor Address")]
        [DataType(DataType.MultilineText)]
        public string VendorAddress { get; set; }

        [Display(Name = "Arrival Type")]
        public int? ArrivalTypeId { get; set; }

        [NotMapped]
        [Display(Name = "Arrival Type")]
        public string ArrivalTypeName { get; set; }

        [Display(Name = "PO #")]
        [MaxLength(30)]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Document #")]
        public string DocRefNo { get; set; }

        [Display(Name = "Ref. Date")]
        public DateTime? DocRefDate { get; set; }

        [Display(Name = "Remark")]
        public string ArrivalRemark { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [NotMapped]
        public List<T_Arrival_Detail> ArrivalDetails { get; set; }
    }
}
