using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    // Alle endpoints i controlleren kræver et gyldigt JWT-token
    [Authorize]

    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    // Alle endpoints starter med api/TrainingDays
    [Route("api/[controller]")]
    public class TrainingDaysController : ControllerBase
    {
        private readonly ITrainingDayService _trainingDayService;
        private readonly ITrainingProgramService _trainingProgramService;


        // Får services gennem Dependency Injection
        public TrainingDaysController(
            ITrainingDayService trainingDayService,
            ITrainingProgramService trainingProgramService)
        {
            _trainingDayService = trainingDayService;
            _trainingProgramService = trainingProgramService;
        }


        private int GetUserId()
        {
            // Finder brugerens id som blev gemt i JWT-tokenet
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            return int.Parse(userId!);
        }


        // Henter alle træningsdage til et bestemt program
        [HttpGet("program/{trainingProgramId}")]
        public async Task<IActionResult> GetByProgram(
            int trainingProgramId)
        {
            // Finder den loggede bruger
            var userId = GetUserId();


            // Finder træningsprogrammet
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingProgramId
                );


            // Hvis programmet ikke findes
            if (program == null)
            {
                return NotFound();
            }


            // Sikrer at programmet tilhører
            // den bruger som er logget ind
            if (program.UserId != userId)
            {
                return NotFound();
            }


            // Henter alle dage til programmet
            var days =
                await _trainingDayService.GetByProgramIdAsync(
                    trainingProgramId
                );


            // Konverterer database-modeller til DTO'er
            var result = days.Select(day =>
                new TrainingDayDto
                {
                    Id = day.Id,
                    TrainingProgramId = day.TrainingProgramId,
                    DayName = day.DayName
                });


            return Ok(result);
        }


        // Opretter en ny træningsdag
        [HttpPost]
        public async Task<IActionResult> Create(
    [FromBody] CreateTrainingDayDto dto)
        {
            // Finder den loggede bruger
            var userId = GetUserId();


            // Finder programmet dagen skal tilhøre
            var program =
                await _trainingProgramService.GetByIdAsync(
                    dto.TrainingProgramId
                );


            // Hvis programmet ikke findes
            if (program == null)
            {
                return NotFound(new
                {
                    message = "Træningsprogrammet blev ikke fundet."
                });
            }


            // Sikrer at brugeren kun kan ændre sit eget program
            if (program.UserId != userId)
            {
                return NotFound();
            }


            // Henter de dage der allerede findes i programmet
            var existingDays =
                await _trainingDayService.GetByProgramIdAsync(
                    dto.TrainingProgramId
                );


            // Tjekker om den valgte dag allerede er tilføjet
            var dayAlreadyExists =
                existingDays.Any(day =>
                    day.DayName == dto.DayName!.Value
                );


            // Den samme dag må kun bruges én gang pr. program
            if (dayAlreadyExists)
            {
                return Conflict(new
                {
                    message =
                        "Denne ugedag er allerede tilføjet til programmet."
                });
            }


            // Opretter træningsdagen
            var trainingDay = new TrainingDay
            {
                TrainingProgramId = dto.TrainingProgramId,
                DayName = dto.DayName!.Value
            };


            // Gemmer dagen
            var createdDay =
                await _trainingDayService.CreateAsync(
                    trainingDay
                );


            // Laver response DTO
            var result = new TrainingDayDto
            {
                Id = createdDay.Id,
                TrainingProgramId = createdDay.TrainingProgramId,
                DayName = createdDay.DayName
            };


            return Ok(result);
        }


        // Ændrer hvilken ugedag træningsdagen ligger på
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateTrainingDayDto dto)
        {
            // Finder den loggede bruger
            var userId = GetUserId();


            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(id);


            // Hvis dagen ikke findes
            if (trainingDay == null)
            {
                return NotFound();
            }


            // Finder programmet som dagen tilhører
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );


            // Sikrer at programmet tilhører
            // den loggede bruger
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            // Henter de dage der allerede findes i programmet
            var existingDays =
                await _trainingDayService.GetByProgramIdAsync(
                    trainingDay.TrainingProgramId
                );


            // Tjekker om den valgte dag allerede bruges
            // af en anden træningsdag i samme program
            var dayAlreadyExists =
                existingDays.Any(day =>
                    day.Id != id &&
                    day.DayName == dto.DayName!.Value
                );


            // Den samme ugedag må ikke bruges to gange
            if (dayAlreadyExists)
            {
                return Conflict(new
                {
                    message =
                        "Denne ugedag er allerede tilføjet til programmet."
                });
            }


            // Opdaterer ugedagen
            trainingDay.DayName = dto.DayName!.Value;


            // Gemmer ændringen
            var updatedDay =
                await _trainingDayService.UpdateAsync(
                    trainingDay
                );


            // Laver response DTO
            var result = new TrainingDayDto
            {
                Id = updatedDay.Id,
                TrainingProgramId = updatedDay.TrainingProgramId,
                DayName = updatedDay.DayName
            };


            return Ok(result);
        }


        // Sletter en træningsdag
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Finder den loggede bruger
            var userId = GetUserId();


            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(id);


            // Hvis dagen ikke findes
            if (trainingDay == null)
            {
                return NotFound();
            }


            // Finder programmet dagen tilhører
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );


            // Sikrer at brugeren kun kan
            // slette dage fra sine egne programmer
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            // Sletter dagen
            var deleted =
                await _trainingDayService.DeleteAsync(id);


            // Hvis sletningen fejler
            if (!deleted)
            {
                return BadRequest(new
                {
                    message = "Træningsdagen kunne ikke slettes."
                });
            }


            // uden noget response body
            return NoContent();
        }
    }
}