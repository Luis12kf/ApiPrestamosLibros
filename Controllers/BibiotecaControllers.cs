using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BibliotecasController(BibliotecaServices bibliotecaService) : ControllerBase
{
    private readonly BibliotecaServices _bibliotecaService = bibliotecaService;

    // GET: api/bibliotecas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BibliotecaRecordGet>>> GetList()
    {
        var lista = await _bibliotecaService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/bibliotecas/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BibliotecaRecordGet>> GetById(int id)
    {
        var biblioteca = await _bibliotecaService.GetByIdAsync(id);
        
        if (biblioteca is null)
        {
            return NotFound(new { mensaje = $"No se encontró la biblioteca con ID {id}." });
        }

        return Ok(biblioteca);
    }

    // POST: api/bibliotecas
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] BibliotecaRecordSet bibliotecaDto)
    {
        if (bibliotecaDto is null || string.IsNullOrWhiteSpace(bibliotecaDto.Direccion))
        {
            return BadRequest(new { mensaje = "La dirección de la biblioteca es obligatoria." });
        }

        // Asegurarse de que la tabla esté inicializada (o puedes llamarlo globalmente en Program.cs)
        await _bibliotecaService.InitializeAsync();

        var resultado = await _bibliotecaService.SaveAsync(bibliotecaDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al guardar la biblioteca." });
        }

        return StatusCode(201, new { mensaje = "Biblioteca creada exitosamente." });
    }

    // PUT: api/bibliotecas/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BibliotecaRecordGet>> Update(int id, [FromBody] BibliotecaRecordSet bibliotecaDto)
    {
        var bibliotecaActualizada = await _bibliotecaService.UpdateAsync(id, bibliotecaDto);

        if (bibliotecaActualizada is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. La biblioteca con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Biblioteca actualizada correctamente.", data = bibliotecaActualizada });
    }

    // DELETE: api/bibliotecas/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _bibliotecaService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. La biblioteca con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Biblioteca eliminada correctamente." });
    }
}