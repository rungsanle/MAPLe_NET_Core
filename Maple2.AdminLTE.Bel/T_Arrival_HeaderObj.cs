using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class T_Arrival_HeaderObj
    {
        public int Id { get; set; }
        public string ArrivalNo { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public int? RawMatTypeId { get; set; }
        public string RawMatTypeName { get; set; }
        public int? VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public int? ArrivalTypeId { get; set; }
        public string ArrivalTypeName { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string DocRefNo { get; set; }
        public DateTime? DocRefDate { get; set; }
        public string ArrivalRemark { get; set; }
        public string CompanyCode { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
