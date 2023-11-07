using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
      
    }
}
