using System;
using System.Collections.Generic;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class M_UserObj
    {
        public int Id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string EmpCode { get; set; }
        public int? DeptId { get; set; }
        public string DeptName { get; set; }
        public string Position { get; set; }
        public string CompanyCode { get; set; }
        public string aspnetuser_Id { get; set; }
        public string UserImagePath { get; set; }
        public string CompanyLogoPath { get; set; }
        public bool Is_Active { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Created_By { get; set; }
        public DateTime? Updated_Date { get; set; }
        public int? Updated_By { get; set; }
    }
}
