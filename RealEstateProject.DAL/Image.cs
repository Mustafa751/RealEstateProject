using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int EstateId { get; set; }
        public virtual Estate Estate { get; set; }
    }
}
