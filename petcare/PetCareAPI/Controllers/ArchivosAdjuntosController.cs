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
    public class ArchivosAdjuntosController : ControllerBase
    {
        int num;
        string msg = string.Empty;
        private readonly CN_ARCHIVOSADJUNTOS _cnArchivos;
        private readonly AM_Errorescs _modelsApi = new AM_Errorescs();

        public ArchivosAdjuntosController(IConfiguration configuration)
        {
            _cnArchivos = new CN_ARCHIVOSADJUNTOS(configuration);
        }

        [HttpGet("GetArchivos")]
        public ActionResult<List<CE_ARCHIVOSADJUNTOS>> GetArchivos()
        {
            try
            {
                var lista = _cnArchivos.CN_ListarArchivos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener archivos adjuntos: {ex.Message}");
            }
        }

        [HttpPost("PostArchivo")]
        public IActionResult CrearArchivo([FromBody] CE_ARCHIVOSADJUNTOS archivo)
        {
            try
            {
                _cnArchivos.CN_InsertarArchivo(archivo);
                return Ok("Archivo adjunto creado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear archivo adjunto: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult ActualizarArchivo([FromBody] CE_ARCHIVOSADJUNTOS archivo)
        {
            try
            {
                _cnArchivos.CN_ActualizarArchivo(archivo, out num, out msg);
                _modelsApi.idError = num;
                _modelsApi.error = msg;

                if (num == -1)
                    return BadRequest(_modelsApi);
                else
                    return Ok(_modelsApi);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar archivo adjunto: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarArchivo(int id)
        {
            try
            {
                _cnArchivos.CN_EliminarArchivo(id);
                return Ok("Archivo adjunto eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar archivo adjunto: {ex.Message}");
            }
        }
    }
}
