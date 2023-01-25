using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.informacionCliente;

namespace bg.hd.banca.pyme.application.services
{
    public class IdentificaUsuarioRepository : IIdentificaUsuarioRepository
    {
        private readonly IIdentificaUsuarioRestRepository _identificaUsuarioRestRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRestRepository;

        public IdentificaUsuarioRepository(IIdentificaUsuarioRestRepository identificaUsuarioRestRepository, 
            IGestionExpedienteRestRepository crearExpedienteRepository, IInformacionClienteRestRepository informacionClienteRestRepository)
        {
            _identificaUsuarioRestRepository = identificaUsuarioRestRepository;
            _expedienteRepository = crearExpedienteRepository;
            _informacionClienteRestRepository = informacionClienteRestRepository;

        }

        public async Task<IdentificaUsuarioResponse> IdentificarUsuario(string identificacion, string codDactilar,  string tipoId, bool obfuscate, bool vieneSolicitud)
        { 
            IdentificaUsuarioResponse responseUsuario = new();
            Int64 ValAux = 0;
            if (Int64.TryParse(identificacion, out ValAux) == false)
            {
                throw new IdentificarUsuarioException("Identificación debe ser numérica", "Identificación debe ser numérica", 2);
            }

            if (identificacion.Length < 10 || identificacion.Length > 10)
            {
                throw new IdentificarUsuarioException("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }           

            responseUsuario = await _identificaUsuarioRestRepository.IdentificarUsuario(identificacion, codDactilar, tipoId, obfuscate);
            RetomarSolicitudRequest request = new();
            request.numIdentificacion = identificacion;
            request.opcion = "12";
            VerificaSolicitudesResponseMicro responseMicro = await _expedienteRepository.RetomarSolicitud(request);            
            InformacionClienteDniResponse datosRCResponse = new InformacionClienteDniResponse();
            datosRCResponse = await _informacionClienteRestRepository.InformacionCliente(identificacion, codDactilar);
            responseUsuario.codigoDactilar = (!string.IsNullOrEmpty(datosRCResponse.codigoDactilar)
                                                    ? (datosRCResponse.codigoDactilar).Length >= 6 && (datosRCResponse.codigoDactilar).Length <= 10 ? datosRCResponse.codigoDactilar                                                    
                                                    : null
                                                    : null);

            if (responseMicro.SolicitudesProcesoContratacion.Any() || vieneSolicitud)
            {
                responseUsuario.tieneProducto = true;
                if (responseMicro.SolicitudesProcesoContratacion.Any())
                {
                    responseUsuario.idEtapa = responseMicro.SolicitudesProcesoContratacion[0].IdEtapa;
                    responseUsuario.estadoExpediente = responseMicro.SolicitudesProcesoContratacion[0].EstadoExpediente;
                    responseUsuario.etapaExpediente = responseMicro.SolicitudesProcesoContratacion[0].EtapaExpediente;

                    if (responseUsuario.idEtapa.Equals("3118") && responseMicro.SolicitudesProcesoContratacion[0].IdEstado.Equals("5603"))
                    {
                        responseUsuario.envioBiometria = true;
                    }
                }
            
            }

            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest()
            {
                identificacion = identificacion
            };

            ConsultaOficialResponseMicroServ responseDataOficial = await _informacionClienteRestRepository.ConsultarDatosOficial(oficialRequest);

            if(string.IsNullOrEmpty(responseDataOficial.idAgenciaOficial)) throw new IdentificarUsuarioException("Cliente no tiene asignado agencia", "Cliente no tiene asignado agencia", 9);

            responseUsuario.idAgenciaOficial = responseDataOficial.idAgenciaOficial;

            if (responseDataOficial.oficialExiste && !string.IsNullOrEmpty(responseDataOficial.usuarioOficial))
            {
                responseUsuario.UsuarioGestor = responseDataOficial.usuarioOficial;
                responseUsuario.OpidGestor = responseDataOficial.opidOficial;
            }
            else
            {
                responseUsuario.UsuarioGestor = responseDataOficial.usuarioJefeAencia;
                responseUsuario.OpidGestor = responseDataOficial.opidJefeAgencia;
            }

            return responseUsuario;
        }
    }
}
