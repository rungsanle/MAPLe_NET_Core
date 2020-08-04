﻿using System.ComponentModel.DataAnnotations;

namespace Maple2.AdminLTE.Bel.DataAnnotations
{
    public class IsTrueRequired : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }

            return false;
        }
    }
}
