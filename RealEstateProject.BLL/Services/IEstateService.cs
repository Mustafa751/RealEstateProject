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
        Task<IEnumerable<Estate>> GetEstates(Filter filter);
        Task<Estate> GetEstateById(int id);
        Task<Estate> CreateEstate(Estate estate);
        Task<bool> UpdateEstate(int id, Estate estate);
        Task<bool> DeleteEstate(int id);
    }
}
