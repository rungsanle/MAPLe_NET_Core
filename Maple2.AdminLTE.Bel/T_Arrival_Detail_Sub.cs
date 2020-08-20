using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("t_arrival_dtl_sub")]
    public class T_Arrival_Detail_Sub : Base_Related_Field
    {
        [Display(Name = "Arrival Id")]
        public int? ArrivalId { get; set; }

        [Display(Name = "Detail Line #")]
        public int? DtlLineNo { get; set; }

        [Display(Name = "Sub Line #")]
        public int? SubLineNo { get; set; }

        [Display(Name = "Item Id")]
        public int? MaterialId { get; set; }

        [Display(Name = "No. Of Label")]
        public int? NoOfLabel { get; set; }

        [Display(Name = "LABEL QTY")]
        [DataType("decimal(18 ,4")]
        public decimal? LabelQty { get; set; }

        [NotMapped]
        public decimal? TotalQty { get; set; }

        [Display(Name = "Lot #")]
        public string SubDetail { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }

        [NotMapped]
        public int RecordFlag { get; set; }
    }
}
