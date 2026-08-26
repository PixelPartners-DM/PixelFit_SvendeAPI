using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelFit_SvendeAPI.Data;
using PixelFit_SvendeAPI.DTOS;
using PixelFit_SvendeAPI.Models;
using PixelFit_SvendeAPI.Services.Interfaces;
using System.Security.Claims;

namespace PixelFit_SvendeAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingDaysController : ControllerBase
    {
        private readonly ITrainingDayService _trainingDayService;
        private readonly ITrainingProgramService _trainingProgramService;
        private readonly ApplicationDbContext _context;


        // Får services gennem Dependency Injection
        public TrainingDaysController(
            ITrainingDayService trainingDayService,
            ITrainingProgramService trainingProgramService,
            ApplicationDbContext context)
        {
            _trainingDayService = trainingDayService;
            _trainingProgramService = trainingProgramService;
            _context = context;
        }



        private int GetUserId()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );


            return int.Parse(
                userId!
            );
        }



        [HttpGet("program/{trainingProgramId}")]
        public async Task<IActionResult> GetByProgram(
            int trainingProgramId)
        {
            var userId =
                GetUserId();


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


            // Brugeren må kun hente egne programmer
            if (program.UserId != userId)
            {
                return NotFound();
            }


            // Henter alle træningsdage til programmet
            var days =
                await _trainingDayService.GetByProgramIdAsync(
                    trainingProgramId
                );


            var result =
                days.Select(
                    day =>
                        new TrainingDayDto
                        {
                            Id =
                                day.Id,

                            TrainingProgramId =
                                day.TrainingProgramId,

                            DayName =
                                day.DayName
                        }
                );


            return Ok(
                result
            );
        }



        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateTrainingDayDto dto)
        {
            var userId =
                GetUserId();



            var program =
                await _trainingProgramService.GetByIdAsync(
                    dto.TrainingProgramId
                );


            if (program == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Træningsprogrammet blev ikke fundet."
                    }
                );
            }


            // Brugeren må kun ændre sit eget program
            if (program.UserId != userId)
            {
                return NotFound();
            }



            if (dto.DayName == null)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Vælg en gyldig ugedag."
                    }
                );
            }



            var dayName =
                dto.DayName.Value;


            var dayAlreadyExists =
                await _context.TrainingDays
                    .AnyAsync(
                        day =>
                            day.DayName == dayName &&
                            _context.TrainingPrograms.Any(
                                trainingProgram =>
                                    trainingProgram.Id ==
                                        day.TrainingProgramId &&
                                    trainingProgram.UserId ==
                                        userId
                            )
                    );


            if (dayAlreadyExists)
            {
                return Conflict(
                    new
                    {
                        message =
                            "Du har allerede en træningsplan for denne ugedag."
                    }
                );
            }



            var trainingDay =
                new TrainingDay
                {
                    TrainingProgramId =
                        dto.TrainingProgramId,

                    DayName =
                        dayName
                };


            var createdDay =
                await _trainingDayService.CreateAsync(
                    trainingDay
                );


            var result =
                new TrainingDayDto
                {
                    Id =
                        createdDay.Id,

                    TrainingProgramId =
                        createdDay.TrainingProgramId,

                    DayName =
                        createdDay.DayName
                };


            return Ok(
                result
            );
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateTrainingDayDto dto)
        {
            var userId =
                GetUserId();



            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    id
                );


            if (trainingDay == null)
            {
                return NotFound();
            }



            var program =
                await _trainingProgramService.GetByIdAsync(
                    trainingDay.TrainingProgramId
                );


            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }



            if (dto.DayName == null)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Vælg en gyldig ugedag."
                    }
                );
            }


            var dayName =
                dto.DayName.Value;



            var dayAlreadyExists =
                await _context.TrainingDays
                    .AnyAsync(
                        day =>
                            day.Id != id &&
                            day.DayName == dayName &&
                            _context.TrainingPrograms.Any(
                                trainingProgram =>
                                    trainingProgram.Id ==
                                        day.TrainingProgramId &&
                                    trainingProgram.UserId ==
                                        userId
                            )
                    );


            if (dayAlreadyExists)
            {
                return Conflict(
                    new
                    {
                        message =
                            "Du har allerede en træningsplan for denne ugedag."
                    }
                );
            }



            trainingDay.DayName =
                dayName;


            var updatedDay =
                await _trainingDayService.UpdateAsync(
                    trainingDay
                );


            var result =
                new TrainingDayDto
                {
                    Id =
                        updatedDay.Id,

                    TrainingProgramId =
                        updatedDay.TrainingProgramId,

                    DayName =
                        updatedDay.DayName
                };


            return Ok(
                result
            );
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            var userId =
                GetUserId();


            // Finder træningsdagen
            var trainingDay =
                await _trainingDayService.GetByIdAsync(
                    id
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


            // Brugeren må kun slette egne dage
            if (program == null ||
                program.UserId != userId)
            {
                return NotFound();
            }


            var deleted =
                await _trainingDayService.DeleteAsync(
                    id
                );


            if (!deleted)
            {
                return BadRequest(
                    new
                    {
                        message =
                            "Træningsdagen kunne ikke slettes."
                    }
                );
            }


            return NoContent();
        }
    }
}