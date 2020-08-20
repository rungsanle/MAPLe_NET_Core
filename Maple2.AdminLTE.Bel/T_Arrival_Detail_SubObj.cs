using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class T_Arrival_Detail_SubObj
    {
        public int Id { get; set; }
        public int? ArrivalId { get; set; }
        public int? DtlLineNo { get; set; }
        public int? SubLineNo { get; set; }
        public int? MaterialId { get; set; }
        public int? NoOfLabel { get; set; }
        public decimal? LabelQty { get; set; }
        public decimal? TotalQty { get; set; }
        public string SubDetail { get; set; }
        public string CompanyCode { get; set; }
        public int RecordFlag { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
