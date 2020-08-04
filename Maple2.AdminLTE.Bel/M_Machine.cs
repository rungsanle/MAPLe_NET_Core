using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_machine")]
    public class M_Machine : Base_Related_Field
    {

        [Display(Name = "Machine Code")]
        [Required(ErrorMessage = "MachineCode|Machine Code Is Required!!")]
        [MaxLength(30)]
        public string MachineCode { get; set; }

        [Display(Name = "Machine Name")]
        [Required(ErrorMessage = "MachineName|Machine Name Is Required!!")]
        public string MachineName { get; set; }

        [Display(Name = "Machine Prod. Type")]
        public int? MachineProdType { get; set; }

        [NotMapped]
        [Display(Name = "Prod. Type")]
        public string MachineProdTypeName { get; set; }

        [Display(Name = "Machine Size")]
        public string MachineSize { get; set; }

        [Display(Name = "Machine Remark")]
        [DataType(DataType.MultilineText)]
        public string MachineRemark { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
