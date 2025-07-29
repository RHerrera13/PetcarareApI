using CAPA_ENTIDADES;
using CAPA_NEGOCIO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using PetCareAPI.APIModels;

namespace PetCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_USUARIOS _cnUsuarios;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public UsuariosController(IConfiguration configuration)
        {
            _cnUsuarios = new CN_USUARIOS(configuration);
        }

        [HttpGet("GetUsuarios")]
        public ActionResult<List<CE_USUARIOS>> GetUsuarios()
        {
            try
            {
                var usuarios = _cnUsuarios.CN_ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener usuarios: {ex.Message}");
            }
        }

        [HttpPost("PostUsuario")]
        public IActionResult CrearUsuario([FromBody] CE_USUARIOS usuario)
        {
            try
            {
                _cnUsuarios.CN_InsertarUsuario(usuario);
                return Ok("Usuario creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear usuario: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarUsuario([FromBody] CE_USUARIOS usuario)
        {
            try
            {
                _cnUsuarios.CN_ActualizarUsuario(usuario, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                {
                    return BadRequest(_modelsApi);
                }

                return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar usuario: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                _cnUsuarios.CN_EliminarUsuario(id);
                return Ok("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar usuario: {ex.Message}");
            }
        }
    }
}
