using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL
{
    public class Filter
    {
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
    }
}
