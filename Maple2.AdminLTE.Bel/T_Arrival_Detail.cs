using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("t_arrival_dtl")]
    public class T_Arrival_Detail : Base_Related_Field
    {
        [Display(Name = "ARRIVAL ID")]
        public int? ArrivalId { get; set; }

        [Display(Name = "Line No.")]
        public int? LineNo { get; set; }

        [Display(Name = "PO Distrib")]
        public int? PoLineNo { get; set; }

        [Display(Name = "Item Id")]
        public int? MaterialId { get; set; }

        [Display(Name = "Item Code")]
        public string MaterialCode { get; set; }

        [Display(Name = "Item Name")]
        public string MaterialName { get; set; }

        [Display(Name = "Description")]
        public string MaterialDesc { get; set; }

        [Display(Name = "Order Qty")]
        [DataType("decimal(18 ,4")]
        public decimal? OrderQty { get; set; }

        [Display(Name = "Receive Qty")]
        [DataType("decimal(18 ,4")]
        public decimal? RecvQty { get; set; }

        [Display(Name = "Lot #")]
        public string LotNo { get; set; }

        [Display(Name = "Lot Date")]
        public DateTime? LotDate { get; set; }

        [Display(Name = "Detail Remark")]
        public string DetailRemark { get; set; }

        [Display(Name = "Gen. Status")]
        public string GenLabelStatus { get; set; }

        [Display(Name = "No Of Label")]
        public int? NoOfLabel { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [NotMapped]
        public int RecordFlag { get; set; }

        [NotMapped]
        public decimal? PackageStdQty { get; set; }
    }
}
