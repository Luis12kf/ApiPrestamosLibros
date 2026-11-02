using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController(EstudianteServices estudianteService) : ControllerBase
{
    private readonly EstudianteServices _estudianteService = estudianteService;

    // GET: api/estudiantes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstudianteRecordGet>>> GetList()
    {
        var lista = await _estudianteService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/estudiantes/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EstudianteRecordGet>> GetById(int id)
    {
        var estudiante = await _estudianteService.GetByIdAsync(id);
        
        if (estudiante is null)
        {
            return NotFound(new { mensaje = $"No se encontró el estudiante con ID {id}." });
        }

        return Ok(estudiante);
    }

    // POST: api/estudiantes
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] EstudianteRecordSet estudianteDto)
    {
        if (estudianteDto is null || string.IsNullOrWhiteSpace(estudianteDto.Matricula) || string.IsNullOrWhiteSpace(estudianteDto.Nombre))
        {
            return BadRequest(new { mensaje = "El nombre y la matrícula del estudiante son obligatorios." });
        }

        var resultado = await _estudianteService.SaveAsync(estudianteDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el estudiante." });
        }

        return StatusCode(201, new { mensaje = "Estudiante registrado exitosamente." });
    }

    // PUT: api/estudiantes/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EstudianteRecordGet>> Update(int id, [FromBody] EstudianteRecordSet estudianteDto)
    {
        var estudianteActualizado = await _estudianteService.UpdateAsync(id, estudianteDto);

        if (estudianteActualizado is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. El estudiante con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Estudiante actualizado correctamente.", data = estudianteActualizado });
    }

    // DELETE: api/estudiantes/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _estudianteService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. El estudiante con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Estudiante eliminado correctamente." });
    }
}