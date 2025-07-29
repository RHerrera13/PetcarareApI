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
    public class VacunasController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_VACUNAS _cnVacunas;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public VacunasController(IConfiguration configuration)
        {
            _cnVacunas = new CN_VACUNAS(configuration);
        }

        [HttpGet("GetVacunas")]
        public ActionResult<List<CE_VACUNAS>> GetVacunas()
        {
            try
            {
                var lista = _cnVacunas.CN_ListarVacunas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener vacunas: {ex.Message}");
            }
        }

        [HttpPost("PostVacuna")]
        public IActionResult CrearVacuna([FromBody] CE_VACUNAS vacuna)
        {
            try
            {
                _cnVacunas.CN_InsertarVacuna(vacuna);
                return Ok("Vacuna creada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear vacuna: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarVacuna([FromBody] CE_VACUNAS vacuna)
        {
            try
            {
                _cnVacunas.CN_ActualizarVacuna(vacuna, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar vacuna: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarVacuna(int id)
        {
            try
            {
                _cnVacunas.CN_EliminarVacuna(id);
                return Ok("Vacuna eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar vacuna: {ex.Message}");
            }
        }
    }
}
