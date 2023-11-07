using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.DAL.Entities
{
    public class Comment:BaseModel
    {
      
        [Required]
        public string Comments { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual Products  Products { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
