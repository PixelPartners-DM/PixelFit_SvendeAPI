using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]

    [ApiController]

    [Route("api/[controller]")]
    public class MuscleGroupsController : ControllerBase
    {
        private readonly IMuscleGroupService _service;


        // Får service-laget gennem Dependency Injection
        public MuscleGroupsController(
            IMuscleGroupService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Henter muskelgrupperne fra service-laget
            var muscleGroups =
                await _service.GetAllAsync();


            // Laver database-modellerne om til DTO'er
            var result = muscleGroups.Select(
                muscleGroup =>
                    new MuscleGroupDto
                    {
                        Id = muscleGroup.Id,
                        Name = muscleGroup.Name
                    }
            );


            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Finder muskelgruppen
            var muscleGroup =
                await _service.GetByIdAsync(id);


            // Hvis den ikke findes
            if (muscleGroup == null)
            {
                return NotFound();
            }


            // Laver database-modellen om til DTO
            var result = new MuscleGroupDto
            {
                Id = muscleGroup.Id,
                Name = muscleGroup.Name
            };


            return Ok(result);
        }
    }
}