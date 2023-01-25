using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.documento;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.persona;
using Microsoft.Extensions.Configuration;

namespace bg.hd.banca.pyme.application.services
{
    public class DocumentoRepository : IDocumentoRepository
    {
        private readonly IDocumentoRestRepository _documentoRestRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;
        private readonly IConfiguration _configuration;

        public DocumentoRepository(IDocumentoRestRepository _documentoRestRepository, IGestionExpedienteRestRepository _expedienteRepository,
            IInformacionClienteRestRepository informacionClienteRepository,IConfiguration Configuration)
        {
            this._documentoRestRepository = _documentoRestRepository;
            this._expedienteRepository = _expedienteRepository;
            this._informacionClienteRepository = informacionClienteRepository;
            _configuration = Configuration;

        }

        public async Task<DigitalizarDocumentosResponse> DigitalizarDocumentos(DigitalizarDocumentosRequest request)
        {
            foreach (var tipoArchivo in request.ListaDocumentos.ListadoDocumentoDigitalizar )
            {
                if(tipoArchivo.ExtDocumento.ToUpper() != "PDF")
                {
                    throw new GeneralException("Tipo de Archivo no permitido", "Tipo de Archivo no permitido", 2);
                } 
            }

            return await _documentoRestRepository.DigitalizarDocumentos(request);

        }

        public async Task<GenerarDocumentosCreditoResponse> GenerarDocumentosContratoscredito(GenerarDocumentosCreditoRequest request)
        {
            ModificarExpedienteRequest expedienteRequestModificar = new ModificarExpedienteRequest();

            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
            oficialRequest.identificacion = request.Identificacion;


            ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
            solicitudModiRequest.Producto = request.producto;
            solicitudModiRequest.IdSolicitud = request.IdSolicitud;
            solicitudModiRequest.SubProductoCore = _configuration["GeneralConfig:subProductoCoreFirmaElect"];
            ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);

            expedienteRequestModificar.Identificacion = request.Identificacion;
            expedienteRequestModificar.idProducto = request.producto;
            expedienteRequestModificar.idExpediente = request.IdExpediente.ToString();
            expedienteRequestModificar.usuarioModifica = request.UsuarioGestor;
            expedienteRequestModificar.strUsuario = request.UsuarioGestor;
            expedienteRequestModificar.idModulo = "0";
            expedienteRequestModificar.idFormulario = "0";
            expedienteRequestModificar.idEtapa = "11605";
            expedienteRequestModificar.idEstado = "2846";
            expedienteRequestModificar.aplicaFirmaElect = "S";

            await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);            


            return await _documentoRestRepository.GenerarDocumentosContratoscredito(request);

        }
    }
}
