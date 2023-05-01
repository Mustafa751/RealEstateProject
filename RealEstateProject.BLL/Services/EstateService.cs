using RealEstateProject.DAL;
using RealEstateProject.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateProject.BLL.Services
{
    public class EstateService : IEstateService
    {
        private readonly IEstateRepository _estateRepository;

        public EstateService(IEstateRepository estateRepository)
        {
            _estateRepository = estateRepository;
        }

        public async Task<Estate> CreateEstate(Estate estate)
        {
            List<Image> photolist = new List<Image>();

            if(estate.Files.Count > 0)
            {
                foreach(var file in estate.Files)
                {
                    if(file.Length > 0)
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            Image image = new Image()
                            {
                                Bytes = memoryStream.ToArray(),
                                Description = file.Name,
                                FileExtension = Path.GetExtension(file.Name),
                                Size = file.Length
                            };

                            photolist.Add(image);
                        }
                    }
                }
            }

            estate.Images = photolist;
            estate.MainImage = photolist.FirstOrDefault();


            return await _estateRepository.AddEstate(estate);
        }

        public async Task<bool> DeleteEstate(int id)
        {
            Estate estate = await _estateRepository.GetEstateById(id);

            if (estate == null)
            {
                return false;
            }

            return await _estateRepository.DeleteEstate(estate);
        }

        public async Task<IEnumerable<Estate>> GetAllEstates()
        {
            return await _estateRepository.GetAllEstates();
        }

        public async Task<Estate> GetEstateById(int id)
        {
            return await _estateRepository.GetEstateById(id);
        }

        public async Task<bool> UpdateEstate(int id, Estate newEstate)
        {
            Estate estate = await _estateRepository.GetEstateById(id);

            if (estate == null)
            {
                return false;
            }

            estate.Price = newEstate.Price;
            estate.Address = newEstate.Address;

            await _estateRepository.SaveChangesAsync();

            return true;
        }
    }
}
