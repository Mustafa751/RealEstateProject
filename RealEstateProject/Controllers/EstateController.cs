using Microsoft.AspNetCore.Mvc;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;

namespace RealEstateProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstateController : Controller
    {
        private readonly IEstateService _estateService;
        private readonly IWebHostEnvironment _webHost;

        public EstateController(IEstateService estateService, IWebHostEnvironment webHostEnvironment)
        {
            _estateService = estateService;
            _webHost = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEstates()
        {
            var estates = await _estateService.GetAllEstates();
            List<EstateDTO> result = new List<EstateDTO>();

            foreach (var estate in estates)
            {
                List<ImageDTO> images = new List<ImageDTO>();
                foreach(var image in estate.Images)
                {
                    images.Add(new ImageDTO
                    {
                        Id = image.Id,
                        Path = image.Path,
                        Description = image.Description
                    });
                }

                result.Add(new EstateDTO
                {
                    Id = estate.Id,
                    Address = estate.Address,
                    Name = estate.Name,
                    City = estate.City,
                    Description = estate.Description,
                    EstateType = estate.EstateType,
                    Price = estate.Price,
                    Images = images
                });

            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEstateById(int id)
        {
            var estate = await _estateService.GetEstateById(id);

            List<ImageDTO> images = new List<ImageDTO>();            

            foreach (var image in estate.Images)
            {
                images.Add(new ImageDTO
                {
                    Id = image.Id,
                    Path = image.Path,
                    Description = image.Description
                });
            }

            EstateDTO estateDTO = new EstateDTO()
            {
                Id = estate.Id,
                Address = estate.Address,
                City = estate.City,
                Description = estate.Description,
                EstateType = estate.EstateType,
                Name = estate.Name,
                Price = estate.Price,
                Images = images
            };

            if (estate == null)
            {
                return BadRequest();
            }
            else
                return Ok(estateDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstate()
        {
            string city = Request.Form["city"];
            string estateType = Request.Form["estate"];
            string address = Request.Form["address"];
            int price = int.Parse(Request.Form["price"]);
            string description = Request.Form["description"];

            string folderPath = @"/Images";

            if (!Directory.Exists(folderPath))
            {
                // Create the folder
                Directory.CreateDirectory(folderPath);
            }

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

            //using (var memoryStream = new MemoryStream())
            //{
            //    await image1.CopyToAsync(memoryStream);
            //    var image = new Image()
            //    {
            //        Description = image1.Name,
            //        Size = image1.Length,
            //        Estate = estate
            //    };
            //    //estate.MainImage = image;
            //}

            foreach (var file in image2)
            {
                Guid guid = Guid.NewGuid();
                string extension = file.FileName.Split('.')[1];
                string filename = folderPath + "/" + guid + "." + extension;
                using var memoryStream = System.IO.File.OpenWrite(_webHost.WebRootPath + filename);
                await file.CopyToAsync(memoryStream);
                Image image = new Image()
                {
                    Description = file.Name,
                    Size = file.Length,
                    Estate = estate,
                    Path = filename
                };
                estate.Images.Add(image);
            }


            var createdEstate = await _estateService.CreateEstate(estate);
            if (createdEstate == null)
            {
                return BadRequest();
            }
            else
                return Ok();
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