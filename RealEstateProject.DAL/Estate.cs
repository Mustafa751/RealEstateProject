namespace RealEstateProject.DAL
{
    public class Estate
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}