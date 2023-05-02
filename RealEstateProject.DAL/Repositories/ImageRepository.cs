using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly DatabaseContext _context;

        public ImageRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Image> AddImage(Image image)
        {
            await _context.images.AddAsync(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
