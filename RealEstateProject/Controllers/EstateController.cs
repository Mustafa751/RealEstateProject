using Microsoft.AspNetCore.Mvc;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;
using RealEstateProject.DAL.Repositories;

namespace RealEstateProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstateController : Controller
    {
        private readonly IEstateService _estateService;
        private readonly IImageRepository _imageRepository;

        public EstateController(IEstateService estateService, IImageRepository imageRepository)
        {
            _estateService = estateService;
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEstates()
        {
            var estates = await _estateService.GetAllEstates();
            return Ok(estates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstateById(int id)
        {
            var estate = await _estateService.GetEstateById(id);
            if (estate == null)
            {
                return BadRequest();
            }
            else
                return Ok(estate);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstate()
        {
            string city = Request.Form["city"];
            string estateType = Request.Form["estate"];
            string address = Request.Form["address"];
            int price = int.Parse(Request.Form["price"]);
            string description = Request.Form["description"];

            // Access uploaded files
            IFormFile? image1 = Request.Form.Files["image1"];
            List<IFormFile> image2 = Request.Form.Files
                .Where(f => f.Name == "image2")
                .ToList();

            var estate = new Estate
            {
                City = city,
                EstateType = estateType,
                Address = address,
                Price = price,
                Description = description
            };

            using (var memoryStream = new MemoryStream())
            {
                await image1.CopyToAsync(memoryStream);
                var image = new Image()
                {
                    Bytes = memoryStream.ToArray(),
                    Description = image1.Name,
                    FileExtension = Path.GetExtension(image1.Name),
                    Size = image1.Length
                };
                estate.MainImage = image;
            }

            foreach (var file in image2)
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                Image image = new Image()
                {
                    Bytes = memoryStream.ToArray(),
                    Description = file.Name,
                    FileExtension = Path.GetExtension(file.Name),
                    Size = file.Length,
                    Estate = estate
                };
                await _imageRepository.AddImage(image);
                estate.Images.Add(image);
            }


            var createdEstate = await _estateService.CreateEstate(estate);
            if (createdEstate == null)
            {
                return BadRequest();
            }
            else
                return Ok(createdEstate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEstate(int id, Estate estate)
        {
            var updatedEstate = await _estateService.UpdateEstate(id, estate);
            if (updatedEstate)
                return Ok(updatedEstate);
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstate(int id)
        {
            var deletedEstate = await _estateService.DeleteEstate(id);
            if (deletedEstate)
                return Ok(deletedEstate);
            else
                return BadRequest();
        }
    }
}