using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_LocationObj
    {
        public int Id { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationDesc { get; set; }
        public int? WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string CompanyCode { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
