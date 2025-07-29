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
    public class MedicamentosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_MEDICAMENTOS _cnMedicamentos;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public MedicamentosController(IConfiguration configuration)
        {
            _cnMedicamentos = new CN_MEDICAMENTOS(configuration);
        }

        [HttpGet("GetMedicamentos")]
        public ActionResult<List<CE_MEDICAMENTOS>> GetMedicamentos()
        {
            try
            {
                var lista = _cnMedicamentos.CN_ListarMedicamentos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener medicamentos: {ex.Message}");
            }
        }

        [HttpPost("PostMedicamento")]
        public IActionResult CrearMedicamento([FromBody] CE_MEDICAMENTOS medicamento)
        {
            try
            {
                _cnMedicamentos.CN_InsertarMedicamento(medicamento);
                return Ok("Medicamento creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear medicamento: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarMedicamento([FromBody] CE_MEDICAMENTOS medicamento)
        {
            try
            {
                _cnMedicamentos.CN_ActualizarMedicamento(medicamento, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar medicamento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarMedicamento(int id)
        {
            try
            {
                _cnMedicamentos.CN_EliminarMedicamento(id);
                return Ok("Medicamento eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar medicamento: {ex.Message}");
            }
        }
    }
}