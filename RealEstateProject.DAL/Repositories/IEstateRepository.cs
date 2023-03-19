using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL.Repositories
{
    public interface IEstateRepository
    {
        Task<IEnumerable<Estate>> GetAllEstates();
        Task<Estate> GetEstateById(int id);
        Task<Estate> AddEstate(Estate estate);
        Task<bool> UpdateEstate(Estate estate);
        Task<bool> DeleteEstate(Estate estate);
    }
}
