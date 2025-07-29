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
    public class TratamientosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_TRATAMIENTOS _cnTratamientos;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public TratamientosController(IConfiguration configuration)
        {
            _cnTratamientos = new CN_TRATAMIENTOS(configuration);
        }

        [HttpGet("GetTratamientos")]
        public ActionResult<List<CE_TRATAMIENTOS>> GetTratamientos()
        {
            try
            {
                var lista = _cnTratamientos.CN_ListarTratamientos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener tratamientos: {ex.Message}");
            }
        }

        [HttpPost("PostTratamiento")]
        public IActionResult CrearTratamiento([FromBody] CE_TRATAMIENTOS tratamiento)
        {
            try
            {
                _cnTratamientos.CN_InsertarTratamiento(tratamiento);
                return Ok("Tratamiento creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear tratamiento: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarTratamiento([FromBody] CE_TRATAMIENTOS tratamiento)
        {
            try
            {
                _cnTratamientos.CN_ActualizarTratamiento(tratamiento, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar tratamiento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTratamiento(int id)
        {
            try
            {
                _cnTratamientos.CN_EliminarTratamiento(id);
                return Ok("Tratamiento eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar tratamiento: {ex.Message}");
            }
        }
    }
}