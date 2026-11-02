using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevolucionesController(DevolucionServices devolucionService) : ControllerBase
{
    private readonly DevolucionServices _devolucionService = devolucionService;

    // GET: api/devoluciones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DevolucionRecordGet>>> GetList()
    {
        var lista = await _devolucionService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/devoluciones/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DevolucionRecordGet>> GetById(int id)
    {
        var devolucion = await _devolucionService.GetByIdAsync(id);
        
        if (devolucion is null)
        {
            return NotFound(new { mensaje = $"No se encontró la devolución con ID {id}." });
        }

        return Ok(devolucion);
    }

    // POST: api/devoluciones
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] DevolucionRecordSet devolucionDto)
    {
        if (devolucionDto is null || devolucionDto.PrestamoID <= 0)
        {
            return BadRequest(new { mensaje = "El PrestamoID es obligatorio y debe ser válido." });
        }

        var resultado = await _devolucionService.SaveAsync(devolucionDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al registrar la devolución." });
        }

        return StatusCode(201, new { mensaje = "Devolución registrada exitosamente." });
    }

    // PUT: api/devoluciones/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<DevolucionRecordGet>> Update(int id, [FromBody] DevolucionRecordSet devolucionDto)
    {
        var devolucionActualizada = await _devolucionService.UpdateAsync(id, devolucionDto);

        if (devolucionActualizada is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. La devolución con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Devolución actualizada correctamente.", data = devolucionActualizada });
    }

    // DELETE: api/devoluciones/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _devolucionService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. La devolución con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Devolución eliminada correctamente." });
    }
}