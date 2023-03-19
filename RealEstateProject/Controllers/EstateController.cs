using Microsoft.AspNetCore.Http;
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

        public EstateController(IEstateService estateService)
        {
                _estateService = estateService;
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
                return Ok(estate);
            }

        [HttpPost]
            public async Task<IActionResult> CreateEstate(Estate estate)
        {
                var createdEstate = await _estateService.CreateEstate(estate);
                return Ok(createdEstate);
            }

        [HttpPut("{id}")]
            public async Task<IActionResult> UpdateEstate(int id, Estate estate)
        {
                var updatedEstate = await _estateService.UpdateEstate(id, estate);
                return Ok(updatedEstate);
            }

        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteEstate(int id)
        {
                var deletedEstate = await _estateService.DeleteEstate(id);
                return Ok(deletedEstate);
            }
    }
}
