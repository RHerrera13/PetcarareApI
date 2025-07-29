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
    public class RegistrosMedicosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_REGISTROSMEDICOS _cnRegistros;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public RegistrosMedicosController(IConfiguration configuration)
        {
            _cnRegistros = new CN_REGISTROSMEDICOS(configuration);
        }

        [HttpGet("GetRegistros")]
        public ActionResult<List<CE_REGISTROSMEDICOS>> GetRegistros()
        {
            try
            {
                var lista = _cnRegistros.CN_ListarRegistros();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener registros médicos: {ex.Message}");
            }
        }

        [HttpPost("PostRegistro")]
        public IActionResult CrearRegistro([FromBody] CE_REGISTROSMEDICOS registro)
        {
            try
            {
                _cnRegistros.CN_InsertarRegistro(registro);
                return Ok("Registro médico creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear registro médico: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarRegistro([FromBody] CE_REGISTROSMEDICOS registro)
        {
            try
            {
                _cnRegistros.CN_ActualizarRegistro(registro, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar registro médico: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarRegistro(int id)
        {
            try
            {
                _cnRegistros.CN_EliminarRegistro(id);
                return Ok("Registro médico eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar registro médico: {ex.Message}");
            }
        }
    }
}