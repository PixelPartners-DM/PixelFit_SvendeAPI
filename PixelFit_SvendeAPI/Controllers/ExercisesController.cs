using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Services.Interfaces;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]

    [ApiController]

    [Route("api/[controller]")]
    public class ExercisesController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;


        public ExercisesController(
            IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }



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



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Finder øvelsen
            var exercise =
                await _exerciseService.GetByIdAsync(id);


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