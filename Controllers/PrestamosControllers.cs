using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController(PrestamoServices prestamoService) : ControllerBase
{
    private readonly PrestamoServices _prestamoService = prestamoService;

    // GET: api/prestamos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrestamoRecordGet>>> GetList()
    {
        var lista = await _prestamoService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/prestamos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PrestamoRecordGet>> GetById(int id)
    {
        var prestamo = await _prestamoService.GetByIdAsync(id);
        
        if (prestamo is null)
        {
            return NotFound(new { mensaje = $"No se encontró el préstamo con ID {id}." });
        }

        return Ok(prestamo);
    }

    // POST: api/prestamos
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] PrestamoRecordSet prestamoDto)
    {
        if (prestamoDto is null || prestamoDto.IdLibro <= 0 || prestamoDto.IdEstudiante <= 0)
        {
            return BadRequest(new { mensaje = "El ID del libro y el ID del estudiante son obligatorios y deben ser válidos." });
        }

        var resultado = await _prestamoService.SaveAsync(prestamoDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el préstamo." });
        }

        return StatusCode(201, new { mensaje = "Préstamo registrado exitosamente." });
    }

    // PUT: api/prestamos/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<PrestamoRecordGet>> Update(int id, [FromBody] PrestamoRecordSet prestamoDto)
    {
        var prestamoActualizado = await _prestamoService.UpdateAsync(id, prestamoDto);

        if (prestamoActualizado is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. El préstamo con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Préstamo actualizado correctamente.", data = prestamoActualizado });
    }

    // DELETE: api/prestamos/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _prestamoService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. El préstamo con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Préstamo eliminado correctamente." });
    }
}