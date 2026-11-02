using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController(LibroServices libroService) : ControllerBase
{
    private readonly LibroServices _libroService = libroService;

    // GET: api/libros
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibroRecordGet>>> GetList()
    {
        var lista = await _libroService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/libros/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroRecordGet>> GetById(int id)
    {
        var libro = await _libroService.GetByIdAsync(id);
        
        if (libro is null)
        {
            return NotFound(new { mensaje = $"No se encontró el libro con ID {id}." });
        }

        return Ok(libro);
    }

    // POST: api/libros
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] LibroRecordSet libroDto)
    {
        if (libroDto is null || string.IsNullOrWhiteSpace(libroDto.Titulo) || string.IsNullOrWhiteSpace(libroDto.Isbn))
        {
            return BadRequest(new { mensaje = "El título y el ISBN del libro son obligatorios." });
        }

        var resultado = await _libroService.SaveAsync(libroDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el libro." });
        }

        return StatusCode(201, new { mensaje = "Libro registrado exitosamente." });
    }

    // PUT: api/libros/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<LibroRecordGet>> Update(int id, [FromBody] LibroRecordSet libroDto)
    {
        var libroActualizado = await _libroService.UpdateAsync(id, libroDto);

        if (libroActualizado is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. El libro con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Libro actualizado correctamente.", data = libroActualizado });
    }

    // DELETE: api/libros/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _libroService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. El libro con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Libro eliminado correctamente." });
    }
}