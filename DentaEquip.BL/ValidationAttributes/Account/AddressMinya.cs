using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ValidationAttributes.Account
{
    public class AddressMinya:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is null)
                return null;
            string address = value.ToString().ToLower();
            if (address.Contains("minya") is false)
            {
                return new ValidationResult("This Website Available In Minya Only Enter Your Address In Minya ( Contain minya Keyword)");
            }
            return ValidationResult.Success;
        }
    }
}
