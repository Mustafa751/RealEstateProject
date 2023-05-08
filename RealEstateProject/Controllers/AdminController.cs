﻿using Microsoft.AspNetCore.Mvc;
using RealEstateProject.BLL.Services;
using RealEstateProject.DAL;
using static System.Int32;

namespace RealEstateProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IEstateService _estateService;
        private readonly IWebHostEnvironment _webHost;

        public AdminController(IEstateService estateService, IWebHostEnvironment webHostEnvironment)
        {
            _estateService = estateService;
            _webHost = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEstates()
        {
            return View("Admin");
        }

       

        [HttpPost]
        public async Task<IActionResult> CreateEstate()
        {
            string city = Request.Form["city"];
            string name = Request.Form["name"];
            string estateType = Request.Form["estate"];
            string address = Request.Form["address"];
            int price = Parse(Request.Form["price"]);
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
                Name = name,
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
        
    }
}