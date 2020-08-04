using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_MaterialObj
    {
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDesc1 { get; set; }
        public string MaterialDesc2 { get; set; }
        public int? RawMatTypeId { get; set; }
        public string RawMatType { get; set; }
        public int? UnitId { get; set; }
        public string Unit { get; set; }
        public decimal? PackageStdQty { get; set; }
        public int? WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public string CompanyCode { get; set; }
        public string MaterialImagePath { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
