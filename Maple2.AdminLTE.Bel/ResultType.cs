using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Maple2.AdminLTE.Bel
{
    public class ResultType
    {
        [NotMapped]
        public object ResultValue {get; set;}
    }
}
