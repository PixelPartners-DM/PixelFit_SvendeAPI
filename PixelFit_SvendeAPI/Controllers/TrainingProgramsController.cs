using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    // Alle endpoints i denne controller kræver login/JWT
    [Authorize]

    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    // Endpoint starter med api/TrainingPrograms
    [Route("api/[controller]")]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly ITrainingProgramService _service;

        // Får adgang til service-laget gennem Dependency Injection
        public TrainingProgramsController(
            ITrainingProgramService service)
        {
            _service = service;
        }


        // Henter brugerens id fra JWT-tokenet
        private int GetUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            return int.Parse(userId!);
        }



        // GET: api/TrainingPrograms
        // Henter alle programmer for den loggede bruger
        [HttpGet]
        public async Task<IActionResult> GetPrograms()
        {
            // Henter brugerens id fra JWT
            var userId = GetUserId();

            // Henter kun programmer som tilhører brugeren
            var programs =
                await _service.GetByUserIdAsync(userId);

            // Laver database-modellerne om til DTO'er
            var result = programs.Select(program =>
                new TrainingProgramDto
                {
                    Id = program.Id,
                    Name = program.Name
                });

            return Ok(result);
        }



        // POST: api/TrainingPrograms
        // Opretter et nyt træningsprogram
        [HttpPost]
        public async Task<IActionResult> CreateProgram(
            [FromBody] CreateTrainingProgramDto dto)
        {
            // Henter brugerens id fra JWT
            var userId = GetUserId();

            // Opretter database-modellen
            var program = new TrainingProgram
            {
                Name = dto.Name,

                // UserId kommer fra JWT og ikke fra MAUI
                UserId = userId
            };

            // Gemmer programmet gennem service-laget
            var createdProgram =
                await _service.CreateAsync(program);

            // Sender kun nødvendige data tilbage
            var result = new TrainingProgramDto
            {
                Id = createdProgram.Id,
                Name = createdProgram.Name
            };

            return Ok(result);
        }


        // PUT: api/TrainingPrograms/5
        // Opdaterer navnet på et træningsprogram
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(
            int id,
            [FromBody] CreateTrainingProgramDto dto)
        {
            // Finder den bruger som er logget ind
            var userId = GetUserId();

            // Finder programmet i databasen
            var program = await _service.GetByIdAsync(id);

            // Hvis programmet ikke findes
            if (program == null)
            {
                return NotFound();
            }

            // Sikrer at programmet tilhører den loggede bruger
            if (program.UserId != userId)
            {
                return NotFound();
            }

            // Ændrer programmets navn
            program.Name = dto.Name;

            // Gemmer ændringen
            var updatedProgram =
                await _service.UpdateAsync(program);

            // Returnerer de data MAUI har brug for
            var result = new TrainingProgramDto
            {
                Id = updatedProgram.Id,
                Name = updatedProgram.Name
            };

            return Ok(result);
        }


        // DELETE: api/TrainingPrograms/5
        // Sletter et træningsprogram
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id)
        {
            // Finder den bruger som er logget ind
            var userId = GetUserId();

            // Finder programmet
            var program = await _service.GetByIdAsync(id);

            // Hvis programmet ikke findes
            if (program == null)
            {
                return NotFound();
            }

            // Sikrer at brugeren kun kan slette sine egne programmer
            if (program.UserId != userId)
            {
                return NotFound();
            }

            // Sletter programmet
            var deleted =
                await _service.DeleteAsync(id);

            if (!deleted)
            {
                return BadRequest(new
                {
                    message = "Programmet kunne ikke slettes."
                });
            }

            // 204 betyder at handlingen lykkedes,
            // men der ikke er noget indhold at sende tilbage
            return NoContent();
        }
    }
}