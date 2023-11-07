using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.Account
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter valid email")]
        public string Email { get; set; }
    }
}
