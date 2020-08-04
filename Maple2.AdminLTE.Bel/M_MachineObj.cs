using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_MachineObj
    {
        public int Id { get; set; }
        public string MachineCode { get; set; }
        public string MachineName { get; set; }
        public int? MachineProdType { get; set; }
        public string MachineProdTypeName { get; set; }
        public string MachineSize { get; set; }
        public string MachineRemark { get; set; }
        public string CompanyCode { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
