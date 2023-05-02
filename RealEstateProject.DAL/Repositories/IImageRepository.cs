using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL.Repositories
{
    public interface IImageRepository
    {
        Task<Image> AddImage(Image image);
        Task SaveChangesAsync();
    }
}
