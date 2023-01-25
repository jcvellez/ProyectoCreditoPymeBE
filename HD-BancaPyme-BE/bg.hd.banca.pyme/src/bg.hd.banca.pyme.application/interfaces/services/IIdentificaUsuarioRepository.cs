using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.domain.entities.persona;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IIdentificaUsuarioRepository
    {
        Task<IdentificaUsuarioResponse> IdentificarUsuario(string identificacion, string codDactilar, string tipoId, bool obfuscate, bool vieneSolicitud);
    }
}
