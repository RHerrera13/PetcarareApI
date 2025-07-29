using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPA_DATOS;
using CAPA_ENTIDADES;
using Microsoft.Extensions.Configuration;

namespace CAPA_NEGOCIO
{
    public class CN_USUARIOS
    {
        private readonly CD_USUARIOS _CDUsuarios;

        public CN_USUARIOS(IConfiguration configuration)
        {
            _CDUsuarios = new CD_USUARIOS(configuration);
        }

        public void CN_InsertarUsuario(CE_USUARIOS usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

            _CDUsuarios.INSERTAR_USUARIO(usuario);
        }

        public void CN_ActualizarUsuario(CE_USUARIOS usuario, out int num, out string msg)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");

            _CDUsuarios.ACTUALIZAR_USUARIO(usuario, out num, out msg);
        }

        public void CN_EliminarUsuario(int id)
        {
            CE_USUARIOS usuario = new CE_USUARIOS { UsuarioID = id };
            _CDUsuarios.ELIMINAR_USUARIO(usuario);
        }

        public List<CE_USUARIOS> CN_ListarUsuarios()
        {
            return _CDUsuarios.LISTAR_USUARIOS();
        }
    }
}
