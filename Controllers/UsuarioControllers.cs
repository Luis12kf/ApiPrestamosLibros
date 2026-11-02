using ApiPrestamoLibro.Models;
using ApiPrestamoLibro.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiPrestamoLibro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosBController(UsuarioBServices usuarioService) : ControllerBase
{
    private readonly UsuarioBServices _usuarioService = usuarioService;

    // GET: api/usuariosb
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioBRecordGet>>> GetList()
    {
        var lista = await _usuarioService.GetListAsync();
        return Ok(lista);
    }

    // GET: api/usuariosb/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioBRecordGet>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        
        if (usuario is null)
        {
            return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });
        }

        return Ok(usuario);
    }

    // POST: api/usuariosb
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UsuarioBRecordSet usuarioDto)
    {
        if (usuarioDto is null || string.IsNullOrWhiteSpace(usuarioDto.NombreU) || string.IsNullOrWhiteSpace(usuarioDto.ContraseNa))
        {
            return BadRequest(new { mensaje = "El nombre de usuario y la contraseña son obligatorios." });
        }

        var resultado = await _usuarioService.SaveAsync(usuarioDto);

        if (!resultado)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error al registrar el usuario." });
        }

        return StatusCode(201, new { mensaje = "Usuario registrado exitosamente." });
    }

    // PUT: api/usuariosb/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioBRecordGet>> Update(int id, [FromBody] UsuarioBRecordSet usuarioDto)
    {
        var usuarioActualizado = await _usuarioService.UpdateAsync(id, usuarioDto);

        if (usuarioActualizado is null)
        {
            return NotFound(new { mensaje = $"No se pudo actualizar. El usuario con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Usuario actualizado correctamente.", data = usuarioActualizado });
    }

    // DELETE: api/usuariosb/{id}
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var eliminado = await _usuarioService.DeleteAsync(id);

        if (eliminado is null)
        {
            return NotFound(new { mensaje = $"No se pudo eliminar. El usuario con ID {id} no existe." });
        }

        return Ok(new { mensaje = "Usuario eliminado correctamente." });
    }
}