﻿using DentaEquip.DAL.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ValidationAttributes.Products
{
    public class UniqueProductName:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            EntityContext context = (EntityContext)validationContext.GetService(typeof(EntityContext));
            if (value is null)
                return null;
            string newname = value.ToString();
            if (context.Product.FirstOrDefault(s => s.Name.Equals(newname)) is not null)
            {
                return new ValidationResult("Name Already Exist in Products or restore it from deleted");
            }
            return ValidationResult.Success;
        }
    }
}
