namespace RealEstateProject.DAL
{
    public class Estate
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public int MainImageId { get; set; }
        public virtual Image MainImage { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}