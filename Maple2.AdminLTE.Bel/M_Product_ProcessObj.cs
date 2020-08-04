using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_Product_ProcessObj
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProcessId { get; set; }
        public int? ProcessSeq { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public bool Is_Active { get; set; }

        //public DateTime? Created_Date { get; set; }
        //public int? Created_By { get; set; }
        //public DateTime? Updated_Date { get; set; }
        //public int? Updated_By { get; set; }
    }
}
