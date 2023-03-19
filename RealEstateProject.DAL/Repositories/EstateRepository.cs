using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.DAL.Repositories
{
    public class EstateRepository : IEstateRepository
    {
        private readonly DatabaseContext _context;

        public EstateRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Estate> AddEstate(Estate estate)
        {
            await _context.estates.AddAsync(estate);
            await _context.SaveChangesAsync();
            return estate;
        }

        public async Task<bool> DeleteEstate(Estate estate)
        {
            _context.estates.Remove(estate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Estate>> GetAllEstates()
        {
            return await _context.estates.ToListAsync();
        }

        public async Task<Estate> GetEstateById(int id)
        {
            return await _context.estates.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
