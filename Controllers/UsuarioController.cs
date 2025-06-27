using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnidadGenero.Class;
using Usuario.Models;
namespace Usuario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;
        private UsuarioModels Usuario { get; set; }

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
            Usuario = new UsuarioModels();
        }

        [HttpPost]
        public ActionResult<UsuarioResponse> Crear([FromBody] UsuarioRequest datos)
        {
            if (datos == null)
            {
                return BadRequest("Datos de usuario inválidos.");
            }

            try
            {
                var resultado = Usuario.Crear(datos);
                return Ok(resultado);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, "Error interno al crear usuario.");
            }
        }

        [HttpGet("findByCorreo")]
        public IActionResult FindByCorreo([FromQuery] String correo)
        {
            try
            {
                var usuario = Usuario.BuscarPorCorreo(correo);
                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("findAll")]
        public ActionResult<List<UsuarioResponse>> ObtenerTodos()
        {
            try
            {
                var usuarios = Usuario.ObtenerTodos();
                return Ok(usuarios);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de usuarios");
                return StatusCode(500, "Error interno al obtener usuarios.");
            }
        }

        [HttpPut("edit")]
        public ActionResult<UsuarioResponse> ActualizarUsuario([FromBody] UsuarioRequest datos)
        {
            try
            {
                var usuarioActualizado = Usuario.Editar(datos);
                return Ok(usuarioActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar usuario: {ex.Message}");
            }
        }

        [HttpPatch("changeStatus")]
        public ActionResult<UsuarioResponse> ActualizarActivo([FromQuery] long idUsuario, [FromQuery] int activo)
        {
            try
            {
                var usuarioActualizado = Usuario.ActualizarActivo(idUsuario, activo);
                return Ok(usuarioActualizado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar estado activo: {ex.Message}");
            }
        }

    }
}
