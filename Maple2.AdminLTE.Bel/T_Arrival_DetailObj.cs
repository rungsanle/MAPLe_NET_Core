using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class T_Arrival_DetailObj
    {
        public int Id { get; set; }
        public int? ArrivalId { get; set; }
        public int? LineNo { get; set; }
        public int? PoLineNo { get; set; }
        public int? MaterialId { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDesc { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? RecvQty { get; set; }
        public string LotNo { get; set; }
        public DateTime? LotDate { get; set; }
        public string DetailRemark { get; set; }
        public string GenLabelStatus { get; set; }
        public int? NoOfLabel { get; set; }
        public string CompanyCode { get; set; }
        public int RecordFlag { get; set; }
        public decimal? PackageStdQty { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
