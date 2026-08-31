using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    // Alle endpoints kræver JWT-login
    [Authorize]

    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    [Route("api/[controller]")]
    public class ExerciseSetsController : ControllerBase
    {
        private readonly IExerciseSetService _exerciseSetService;
        private readonly ITrainingDayExerciseService _trainingDayExerciseService;
        private readonly ITrainingDayService _trainingDayService;
        private readonly ITrainingProgramService _trainingProgramService;


        public ExerciseSetsController(
            IExerciseSetService exerciseSetService,
            ITrainingDayExerciseService trainingDayExerciseService,
            ITrainingDayService trainingDayService,
            ITrainingProgramService trainingProgramService)
        {
            _exerciseSetService = exerciseSetService;
            _trainingDayExerciseService = trainingDayExerciseService;
            _trainingDayService = trainingDayService;
            _trainingProgramService = trainingProgramService;
        }


        private int GetUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            return int.Parse(userId!);
        }



        [HttpGet("exercise/{trainingDayExerciseId}")]
        public async Task<IActionResult> GetByTrainingDayExercise(
            int trainingDayExerciseId)
        {
            var userId = GetUserId();


            // Finder den valgte øvelse på træningsdagen
            var trainingDayExercise =
                await _trainingDayExerciseService.GetByIdAsync(
                    trainingDayExerciseId
                );


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


            // Finder programmet dagen tilhører
            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );


            // Sikrer at brugeren kun kan se egne data
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            // Henter sættene
            var sets =
                await _exerciseSetService
                    .GetByTrainingDayExerciseIdAsync(
                        trainingDayExerciseId
                    );


            // Laver modellerne om til DTO'er
            var result = sets.Select(set =>
                new ExerciseSetDto
                {
                    Id = set.Id,
                    TrainingDayExerciseId =
                        set.TrainingDayExerciseId,
                    Reps = set.Reps,
                    Weight = set.Weight,
                    RestBetweenSets =
                        set.RestBetweenSets
                });


            return Ok(result);
        }



        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateExerciseSetDto dto)
        {
            var userId = GetUserId();


            // Finder den valgte øvelse
            var trainingDayExercise =
                await _trainingDayExerciseService.GetByIdAsync(
                    dto.TrainingDayExerciseId
                );


            if (trainingDayExercise == null)
            {
                return NotFound(new
                {
                    message = "Den valgte øvelse blev ikke fundet."
                });
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


            // Sikrer at brugeren kun kan tilføje
            // sæt til sine egne programmer
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            // Opretter sættet
            var exerciseSet = new ExerciseSet
            {
                TrainingDayExerciseId =
                    dto.TrainingDayExerciseId,

                Reps =
                    dto.Reps,

                Weight =
                    dto.Weight,

                RestBetweenSets =
                    dto.RestBetweenSets
            };


            // Gemmer sættet
            var created =
                await _exerciseSetService.AddAsync(
                    exerciseSet
                );


            // Sender DTO tilbage
            var result = new ExerciseSetDto
            {
                Id = created.Id,
                TrainingDayExerciseId =
                    created.TrainingDayExerciseId,
                Reps = created.Reps,
                Weight = created.Weight,
                RestBetweenSets =
                    created.RestBetweenSets
            };


            return Ok(result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateExerciseSetDto dto)
        {
            var userId = GetUserId();


            // Finder sættet
            var exerciseSet =
                await _exerciseSetService.GetByIdAsync(id);


            if (exerciseSet == null)
            {
                return NotFound();
            }


            // Finder den valgte øvelse
            var trainingDayExercise =
                await _trainingDayExerciseService.GetByIdAsync(
                    exerciseSet.TrainingDayExerciseId
                );


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


            // Sikrer at brugeren kun kan ændre egne sæt
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            // Opdaterer sættet
            exerciseSet.Reps =
                dto.Reps;

            exerciseSet.Weight =
                dto.Weight;

            exerciseSet.RestBetweenSets =
                dto.RestBetweenSets;


            var updated =
                await _exerciseSetService.UpdateAsync(
                    exerciseSet
                );


            var result = new ExerciseSetDto
            {
                Id = updated.Id,
                TrainingDayExerciseId =
                    updated.TrainingDayExerciseId,
                Reps = updated.Reps,
                Weight = updated.Weight,
                RestBetweenSets =
                    updated.RestBetweenSets
            };


            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();


            // Finder sættet
            var exerciseSet =
                await _exerciseSetService.GetByIdAsync(id);


            if (exerciseSet == null)
            {
                return NotFound();
            }


            // Finder den valgte øvelse
            var trainingDayExercise =
                await _trainingDayExerciseService.GetByIdAsync(
                    exerciseSet.TrainingDayExerciseId
                );


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


            // Sikrer at brugeren kun kan slette egne sæt
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            var deleted =
                await _exerciseSetService.DeleteAsync(id);


            if (!deleted)
            {
                return BadRequest(new
                {
                    message = "Sættet kunne ikke slettes."
                });
            }


            return NoContent();
        }
    }
}