using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    [Table("m_process")]
    public class M_Process : Base_Related_Field
    {
        [Required(ErrorMessage = "ProcessCode|Process Code Is Required!!")]
        [Display(Name = "Process Code")]
        [MaxLength(30)]
        public string ProcessCode { get; set; }

        [Required(ErrorMessage = "ProcessName|Process Name Is Required!!")]
        [Display(Name = "Process Name")]
        public string ProcessName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string ProcessDesc { get; set; }

        [Display(Name = "Seq")]
        public int? ProcessSeq { get; set; }

        [Display(Name = "Company")]
        [MaxLength(30)]
        public string CompanyCode { get; set; }
    }
}
