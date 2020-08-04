using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_ProductObj
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductNameRef { get; set; }
        public string ProductDesc { get; set; }
        public int? MaterialTypeId { get; set; }
        public string MaterialType { get; set; }
        public int? ProductionTypeId { get; set; }
        public string ProductionType { get; set; }
        public int? MachineId { get; set; }
        public string Machine { get; set; }
        public int? UnitId { get; set; }
        public string Unit { get; set; }
        public decimal? PackageStdQty { get; set; }
        public decimal? SalesPrice1 { get; set; }
        public decimal? SalesPrice2 { get; set; }
        public decimal? SalesPrice3 { get; set; }
        public decimal? SalesPrice4 { get; set; }
        public decimal? SalesPrice5 { get; set; }
        public string GLSalesAccount { get; set; }
        public string GLInventAccount { get; set; }
        public string GLCogsAccount { get; set; }
        public int? RevisionNo { get; set; }
        public int? WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public string CompanyCode { get; set; }
        public string ProductImagePath { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }

    }
}
