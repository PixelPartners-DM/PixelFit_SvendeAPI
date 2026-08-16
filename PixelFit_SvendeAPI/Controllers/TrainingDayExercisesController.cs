using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    // Alle endpoints kræver login med JWT
    [Authorize]

    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    // Endpoint starter med api/TrainingDayExercises
    [Route("api/[controller]")]
    public class TrainingDayExercisesController : ControllerBase
    {
        private readonly ITrainingDayExerciseService _trainingDayExerciseService;
        private readonly ITrainingDayService _trainingDayService;
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly IExerciseService _exerciseService;


        // Får alle services gennem Dependency Injection
        public TrainingDayExercisesController(
            ITrainingDayExerciseService trainingDayExerciseService,
            ITrainingDayService trainingDayService,
            ITrainingProgramService trainingProgramService,
            IExerciseService exerciseService)
        {
            _trainingDayExerciseService = trainingDayExerciseService;
            _trainingDayService = trainingDayService;
            _trainingProgramService = trainingProgramService;
            _exerciseService = exerciseService;
        }


        // Henter brugerens id fra JWT-tokenet
        private int GetUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            return int.Parse(userId!);
        }


        // GET: api/TrainingDayExercises/day/1
        // Henter alle valgte øvelser til en bestemt træningsdag
        [HttpGet("day/{trainingDayId}")]
        public async Task<IActionResult> GetByTrainingDay(
            int trainingDayId)
        {
            var userId = GetUserId();

            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    trainingDayId
                );

            if (trainingDay == null)
            {
                return NotFound();
            }

            // Finder programmet dagen tilhører
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );

            // Sikrer at brugeren kun kan se sine egne data
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }

            // Henter øvelserne på dagen
            var exercises =
                await _trainingDayExerciseService
                    .GetByTrainingDayIdAsync(
                        trainingDayId
                    );

            // Laver database-modeller om til DTO'er
            var result = exercises.Select(x =>
                new TrainingDayExerciseDto
                {
                    Id = x.Id,
                    TrainingDayId = x.TrainingDayId,
                    ExerciseId = x.ExerciseId,
                    ExerciseName = x.Exercise.Name,
                    ImageUrl = x.Exercise.ImageUrl,
                    RestBetweenExercises =
                        x.RestBetweenExercises,
                    Order = x.Order
                });

            return Ok(result);
        }


        // POST: api/TrainingDayExercises
        // Tilføjer en øvelse til en træningsdag
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTrainingDayExerciseDto dto)
        {
            var userId = GetUserId();

            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    dto.TrainingDayId
                );

            if (trainingDay == null)
            {
                return NotFound(new
                {
                    message = "Træningsdagen blev ikke fundet."
                });
            }

            // Finder programmet dagen tilhører
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );

            // Sikrer at brugeren kun kan ændre sit eget program
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }

            // Finder øvelsen i øvelsesbiblioteket
            var exercise =
                await _exerciseService.GetByIdAsync(
                    dto.ExerciseId
                );

            if (exercise == null)
            {
                return NotFound(new
                {
                    message = "Øvelsen blev ikke fundet."
                });
            }

            // Tjekker om øvelsen allerede er tilføjet til dagen
            var existingExercises =
                await _trainingDayExerciseService
                    .GetByTrainingDayIdAsync(
                        dto.TrainingDayId
                    );

            var alreadyExists =
                existingExercises.Any(x =>
                    x.ExerciseId == dto.ExerciseId
                );

            if (alreadyExists)
            {
                return Conflict(new
                {
                    message =
                        "Øvelsen er allerede tilføjet til denne træningsdag."
                });
            }

            // Opretter koblingen mellem dag og øvelse
            var trainingDayExercise =
                new TrainingDayExercise
                {
                    TrainingDayId =
                        dto.TrainingDayId,

                    ExerciseId =
                        dto.ExerciseId,

                    RestBetweenExercises =
                        dto.RestBetweenExercises,

                    Order =
                        dto.Order
                };

            // Gemmer den valgte øvelse
            var created =
                await _trainingDayExerciseService.AddAsync(
                    trainingDayExercise
                );

            // Sender resultat tilbage til MAUI
            var result = new TrainingDayExerciseDto
            {
                Id = created.Id,
                TrainingDayId = created.TrainingDayId,
                ExerciseId = created.ExerciseId,

                // Vi har allerede hentet øvelsen ovenfor
                ExerciseName = exercise.Name,
                ImageUrl = exercise.ImageUrl,

                RestBetweenExercises =
                    created.RestBetweenExercises,

                Order = created.Order
            };

            return Ok(result);
        }


        // PUT: api/TrainingDayExercises/1
        // Opdaterer pause eller rækkefølge på en valgt øvelse
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateTrainingDayExerciseDto dto)
        {
            var userId = GetUserId();

            // Finder den valgte øvelse
            var trainingDayExercise =
                await _trainingDayExerciseService
                    .GetByIdAsync(id);

            if (trainingDayExercise == null)
            {
                return NotFound();
            }

            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    trainingDayExercise.TrainingDayId
                );

            if (trainingDay == null)
            {
                return NotFound();
            }

            // Finder programmet
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );

            // Sikkerhed: kun ejer må redigere
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }

            // Opdaterer de felter brugeren må ændre
            trainingDayExercise.RestBetweenExercises =
                dto.RestBetweenExercises;

            trainingDayExercise.Order =
                dto.Order;

            var updated =
                await _trainingDayExerciseService.UpdateAsync(
                    trainingDayExercise
                );

            var result = new TrainingDayExerciseDto
            {
                Id = updated.Id,
                TrainingDayId = updated.TrainingDayId,
                ExerciseId = updated.ExerciseId,
                ExerciseName = updated.Exercise.Name,
                ImageUrl = updated.Exercise.ImageUrl,
                RestBetweenExercises =
                    updated.RestBetweenExercises,
                Order = updated.Order
            };

            return Ok(result);
        }


        // DELETE: api/TrainingDayExercises/1
        // Fjerner en øvelse fra træningsdagen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            // Finder den valgte øvelse
            var trainingDayExercise =
                await _trainingDayExerciseService
                    .GetByIdAsync(id);

            if (trainingDayExercise == null)
            {
                return NotFound();
            }

            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    trainingDayExercise.TrainingDayId
                );

            if (trainingDay == null)
            {
                return NotFound();
            }

            // Finder programmet
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );

            // Sikrer at brugeren kun kan slette fra sit eget program
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }

            var deleted =
                await _trainingDayExerciseService.DeleteAsync(
                    id
                );

            if (!deleted)
            {
                return BadRequest(new
                {
                    message = "Øvelsen kunne ikke fjernes."
                });
            }

            return NoContent();
        }
    }
}