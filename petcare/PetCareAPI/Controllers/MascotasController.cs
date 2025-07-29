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
    public class MascotasController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_MASCOTAS _cnMascotas;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public MascotasController(IConfiguration configuration)
        {
            _cnMascotas = new CN_MASCOTAS(configuration);
        }

        [HttpGet("GetMascotas")]
        public ActionResult<List<CE_MASCOTAS>> GetMascotas()
        {
            try
            {
                var lista = _cnMascotas.CN_ListarMascotas();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener mascotas: {ex.Message}");
            }
        }

        [HttpPost("PostMascotas")]
        public IActionResult CrearMascota([FromBody] CE_MASCOTAS mascota)
        {
            try
            {
                _cnMascotas.CN_InsertarMascota(mascota);
                return Ok("Mascota creada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear mascota: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarMascota([FromBody] CE_MASCOTAS mascota)
        {
            try
            {
                _cnMascotas.CN_ActualizarMascota(mascota, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar mascota: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarMascota(int id)
        {
            try
            {
                _cnMascotas.CN_EliminarMascota(id);
                return Ok("Mascota eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar mascota: {ex.Message}");
            }
        }
    }
}
