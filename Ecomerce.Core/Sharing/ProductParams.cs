using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.Sharing
{
    public class ProductParams
    {
        public string? sort { get; set; }
        public int pagesize { get; set; }
        public int pagenumber { get; set; }
        public string? Search { get; set; }
    }
}
