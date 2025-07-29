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
    public class EstadosController : ControllerBase
    {
        int num;
        string msg = string.Empty; // Al momento de trabajar con errores, es recomendable inicializar el mensaje de error.
        APIModels.AM_Errorescs _modelsApi = new APIModels.AM_Errorescs();
        private readonly CN_ESTADOS _cnEstados;

        public EstadosController(IConfiguration configuration)
        {
            _cnEstados = new CN_ESTADOS(configuration);
        }

        [HttpGet ("GetEstados")]
        public ActionResult<List<CE_ESTADOS>> GetEstados()
        {
            try
            {
                var estados = _cnEstados.CN_ListarEstados();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener estados: {ex.Message}");
            }
        }

        [HttpPost("PostEstados")]
        public IActionResult CrearEstado([FromBody] CE_ESTADOS estado)
        {
            try
            {
                _cnEstados.CN_InsertarEstado(estado);
                return Ok("Estado creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear estado: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarEstado([FromBody] CE_ESTADOS estado)
        {
            try
            {
                _cnEstados.CN_ActualizarEstado(estado, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;
                if (num == -1)
                {
                    return BadRequest(_modelsApi);
                }
                else
                {
                    return Ok(_modelsApi);
                }
                   
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar estado: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarEstado(int id)
        {
            try
            {
                _cnEstados.CN_EliminarEstado(id);
                return Ok("Estado eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar estado: {ex.Message}");
            }
        }
    }
}
