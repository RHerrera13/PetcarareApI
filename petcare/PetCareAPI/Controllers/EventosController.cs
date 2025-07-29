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
    public class EventosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_EVENTOS _cnEventos;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public EventosController(IConfiguration configuration)
        {
            _cnEventos = new CN_EVENTOS(configuration);
        }

        [HttpGet("GetEventos")]
        public ActionResult<List<CE_EVENTOS>> GetEventos()
        {
            try
            {
                var lista = _cnEventos.CN_ListarEventos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener eventos: {ex.Message}");
            }
        }

        [HttpPost("PostEventos")]
        public IActionResult CrearEvento([FromBody] CE_EVENTOS evento)
        {
            try
            {
                _cnEventos.CN_InsertarEvento(evento);
                return Ok("Evento creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear evento: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarEvento([FromBody] CE_EVENTOS evento)
        {
            try
            {
                _cnEventos.CN_ActualizarEvento(evento, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar evento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarEvento(int id)
        {
            try
            {
                _cnEventos.CN_EliminarEvento(id);
                return Ok("Evento eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar evento: {ex.Message}");
            }
        }
    }
}
