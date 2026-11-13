using APILab12.Data;
using APILab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APILab12.Controllers;

[ApiController]
[Route("api/instructores")]
public class InstructoresController(AcademiaDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<Instructor>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Instructor>>> GetInstructores()
    {
        return Ok(await context.Instructores.AsNoTracking().OrderBy(x => x.IdInstructor).ToListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Instructor>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Instructor>> GetInstructor(int id)
    {
        var instructor = await context.Instructores.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdInstructor == id);

        return instructor is null ? NotFound() : Ok(instructor);
    }

    [HttpPost]
    [ProducesResponseType<Instructor>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
    {
        if (await context.Instructores.AnyAsync(x => x.Email == instructor.Email))
        {
            return Conflict(new ProblemDetails { Title = "El correo del instructor ya está registrado." });
        }

        instructor.IdInstructor = 0;
        context.Instructores.Add(instructor);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInstructor), new { id = instructor.IdInstructor }, instructor);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PutInstructor(int id, Instructor instructor)
    {
        if (id != instructor.IdInstructor)
        {
            return BadRequest(new ProblemDetails { Title = "El id de la ruta no coincide con el cuerpo." });
        }

        if (!await context.Instructores.AnyAsync(x => x.IdInstructor == id))
        {
            return NotFound();
        }

        if (await context.Instructores.AnyAsync(x => x.Email == instructor.Email && x.IdInstructor != id))
        {
            return Conflict(new ProblemDetails { Title = "El correo del instructor ya está registrado." });
        }

        context.Entry(instructor).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        var instructor = await context.Instructores.FindAsync(id);
        if (instructor is null)
        {
            return NotFound();
        }

        if (await context.Cursos.AnyAsync(x => x.IdInstructor == id))
        {
            return Conflict(new ProblemDetails { Title = "No se puede eliminar un instructor que tiene cursos." });
        }

        context.Instructores.Remove(instructor);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
