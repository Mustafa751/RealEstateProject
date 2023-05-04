using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateProject.DAL
{
    public class Estate
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string EstateType { get; set; }
        public double Price { get; set; }
        public int? MainImageId { get; set; }
        public virtual Image? MainImage { get; set; }
        public virtual ICollection<Image>? Images { get; set; } = new List<Image>();
    }
}