﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL.Repositories
{
    public interface IEstateRepository
    {
        Task<IEnumerable<Estate>> GetAllEstates();
        Task<IEnumerable<Estate>> GetEstates(double minPrice, double maxPrice, string city, string type);
        Task<Estate> GetEstateById(int id);
        Task<Estate> AddEstate(Estate estate);
        Task<bool> DeleteEstate(Estate estate);
        Task SaveChangesAsync();
    }
}
