using APILab12.Data;
using APILab12.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APILab12.Controllers;

[ApiController]
[Route("api/estudiantes")]
public class EstudiantesController(AcademiaDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<Estudiante>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Estudiante>>> GetEstudiantes()
    {
        return Ok(await context.Estudiantes.AsNoTracking().OrderBy(x => x.IdEstudiante).ToListAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Estudiante>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
    {
        var estudiante = await context.Estudiantes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdEstudiante == id);

        return estudiante is null ? NotFound() : Ok(estudiante);
    }

    [HttpPost]
    [ProducesResponseType<Estudiante>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
    {
        if (await context.Estudiantes.AnyAsync(x => x.Email == estudiante.Email))
        {
            return Conflict(new ProblemDetails { Title = "El correo del estudiante ya está registrado." });
        }

        estudiante.IdEstudiante = 0;
        if (estudiante.FechaRegistro == default)
        {
            estudiante.FechaRegistro = DateTime.UtcNow;
        }

        context.Estudiantes.Add(estudiante);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEstudiante), new { id = estudiante.IdEstudiante }, estudiante);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
    {
        if (id != estudiante.IdEstudiante)
        {
            return BadRequest(new ProblemDetails { Title = "El id de la ruta no coincide con el cuerpo." });
        }

        if (!await context.Estudiantes.AnyAsync(x => x.IdEstudiante == id))
        {
            return NotFound();
        }

        if (await context.Estudiantes.AnyAsync(x => x.Email == estudiante.Email && x.IdEstudiante != id))
        {
            return Conflict(new ProblemDetails { Title = "El correo del estudiante ya está registrado." });
        }

        context.Entry(estudiante).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteEstudiante(int id)
    {
        var estudiante = await context.Estudiantes.FindAsync(id);
        if (estudiante is null)
        {
            return NotFound();
        }

        if (await context.Matriculas.AnyAsync(x => x.IdEstudiante == id))
        {
            return Conflict(new ProblemDetails { Title = "No se puede eliminar un estudiante que tiene matrículas." });
        }

        context.Estudiantes.Remove(estudiante);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
