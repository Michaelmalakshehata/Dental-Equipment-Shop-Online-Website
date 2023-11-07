using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentaEquip.BL.ViewModels.FilterProduct
{
    public class FilterProductViewModel
    {
        public List<string>  brandFilters { get; set; }
        public List<string>  shades { get; set; }
        public List<string>  cureTypes { get; set; }
        public List<string> countryOfOrigins { get; set; }
        public List<string> bureColorCodes { get; set; }
        public List<string>  bureMaximumRPMs { get; set; }
        public List<string> bureShapes { get; set; }
        public List<string> BureOrderNumbers { get; set; }
        public List<string> fileMaterials { get; set; }
        public List<string> fileShapes { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public bool hasvalue {
            get
            {
                if(brandFilters is null && shades is null&& cureTypes is null
                    && countryOfOrigins is null &&
                    bureColorCodes is null && bureMaximumRPMs is null && 
                    bureShapes is null && BureOrderNumbers is null && fileMaterials is null && fileShapes is null && HighPrice==0 && LowPrice==0)
                {
                    return false;
                }
                return true;
            }
        }

    }
   
}
