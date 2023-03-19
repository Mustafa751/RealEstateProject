using RealEstateProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.BLL.Services
{
    public interface IEstateService
    {
        Task<IEnumerable<Estate>> GetAllEstates();
        Task<Estate> GetEstateById(int id);
        Task<Estate> CreateEstate(Estate estate);
        Task<bool> UpdateEstate(int id);
        Task<bool> DeleteEstate(int id);
    }
}
