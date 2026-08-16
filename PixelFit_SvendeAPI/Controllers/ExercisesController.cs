using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Controllers
{
    // Alle endpoints kræver login med JWT
    [Authorize]

    // Fortæller ASP.NET Core at dette er en API-controller
    [ApiController]

    // Endpoint starter med api/Exercises
    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;


        // Får ExerciseService gennem Dependency Injection
        public ExercisesController(
            IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }


        // =====================================================
        // GET ALLE ØVELSER
        // =====================================================

        // GET: api/Exercises
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Henter alle øvelser
            var exercises =
                await _exerciseService.GetAllAsync();


            // Laver database-modeller om til DTO'er
            var result = exercises.Select(exercise =>
                new ExerciseDto
                {
                    Id = exercise.Id,

                    Name = exercise.Name,

                    MuscleGroupId =
                        exercise.MuscleGroupId,

                    MuscleGroupName =
                        exercise.MuscleGroup.Name,

                    ImageUrl =
                        exercise.ImageUrl
                });


            return Ok(result);
        }


        // =====================================================
        // GET ÉN ØVELSE
        // =====================================================

        // GET: api/Exercises/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Finder øvelsen
            var exercise =
                await _exerciseService.GetByIdAsync(id);


            // Hvis øvelsen ikke findes
            if (exercise == null)
            {
                return NotFound();
            }


            // Laver database-modellen om til DTO
            var result = new ExerciseDto
            {
                Id = exercise.Id,

                Name = exercise.Name,

                MuscleGroupId =
                    exercise.MuscleGroupId,

                MuscleGroupName =
                    exercise.MuscleGroup.Name,

                ImageUrl =
                    exercise.ImageUrl
            };


            return Ok(result);
        }


        // =====================================================
        // GET ØVELSER EFTER MUSKELGRUPPE
        // =====================================================

        // GET: api/Exercises/muscle-group/1
        [HttpGet("muscle-group/{muscleGroupId}")]
        public async Task<IActionResult> GetByMuscleGroup(
            int muscleGroupId)
        {
            // Henter alle øvelser der tilhører
            // den valgte muskelgruppe
            var exercises =
                await _exerciseService
                    .GetByMuscleGroupIdAsync(
                        muscleGroupId
                    );


            // Laver database-modeller om til DTO'er
            var result = exercises.Select(exercise =>
                new ExerciseDto
                {
                    Id = exercise.Id,

                    Name = exercise.Name,

                    MuscleGroupId =
                        exercise.MuscleGroupId,

                    MuscleGroupName =
                        exercise.MuscleGroup.Name,

                    ImageUrl =
                        exercise.ImageUrl
                });


            return Ok(result);
        }
    }
}