using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.ArchivosImpuesto;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.application.models.exeptions;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.informacionCliente;

namespace bg.hd.banca.pyme.application.services
{

    public class ArchivosImpuestoRepository : IArchivosImpuestoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IArchivosImpuestoRestRepository _archivosImpuestoRestRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRestRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IInformacionClienteRepository _informacionRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;

        public ArchivosImpuestoRepository(IInformacionClienteRestRepository informacionClienteRepository, IInformacionClienteRepository informacionRepository, IGestionExpedienteRestRepository crearExpedienteRepository, IArchivosImpuestoRestRepository _archivosImpuestoRestRepository, IInformacionClienteRestRepository _informacionClienteRestRepository, IConfiguration _configuration)
        {
            _expedienteRepository = crearExpedienteRepository;
            _informacionRepository = informacionRepository;
            _informacionClienteRepository = informacionClienteRepository;

            this._archivosImpuestoRestRepository = _archivosImpuestoRestRepository;
            this._informacionClienteRestRepository = _informacionClienteRestRepository;
            this._configuration = _configuration;
        }
        public async Task<ArchivosImpuestoResponse> ValidarArchivoImpuesto(ArchivosImpuestoRequest request)
        {
            ArchivosImpuestoResponse responsePDf = new ArchivosImpuestoResponse();
            ActualizaVentasClienteResponse responseActualizaVentas = new ActualizaVentasClienteResponse();
            double montoMinimoRedondeo = double.Parse( _configuration["GeneralConfig:montoMinimoVtaRendondeo"]);
            double montoMaximoRedondeo = double.Parse(_configuration["GeneralConfig:montoMaximaVtaRendondeo"]);
            if (request.identificacion is null)
            {
                throw new ArchivosImpuestoExeption("Identificacion es requerido", "Identificacion es requerido", 2);
            }
            Int64 ValAux = 0;
            if (Int64.TryParse(request.identificacion, out ValAux) == false)
            {
                throw new ArchivosImpuestoExeption("Identificación debe ser numérica ", "Identificación debe ser numérica", 2);
            }
            if (request.identificacion.Length < 10 || request.identificacion.Length > 10)
            {
                throw new ArchivosImpuestoExeption("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }
            if (request.idProceso is null)
            {
                throw new ArchivosImpuestoExeption("idProceso es requerida", "idProceso es requerida", 2);
            }

            if (request.idProceso <= 0)
            {
                throw new ArchivosImpuestoExeption("idProceso no puede ser cero ", "idProceso no puede ser cero", 2);
            }



            //if (request.file is null)
            //{
            //    throw new ArchivosImpuestoExeption("Archivo no valido", "Archivo no valido", 5);
            //}

            responsePDf = await _archivosImpuestoRestRepository.ValidarArchivoImpuesto(request);

            if (responsePDf.BanActVentas == "1")
            {
                ActualizaVentasClienteMicroServ requestVentas = new ActualizaVentasClienteMicroServ();
                requestVentas.ID = request.identificacion;
                requestVentas.TIPOID = _configuration["GeneralConfig:tipoIdentificacionDescripcion"];
                requestVentas.UPCLVTAS = "S";
                requestVentas.UPCLFVTANUAL = "S";
                requestVentas.CLFVTANUAL = request.anyo + "1231";
                double ventas = 0;
                double.TryParse(responsePDf.VentasAnuales, out ventas);
                ventas = Math.Round(ventas, 0);
                if (ventas>=montoMinimoRedondeo && ventas<=montoMaximoRedondeo )
                     ventas = Math.Truncate(ventas / 1000)+1;
                else
                    ventas = Math.Truncate(ventas / 1000);
                requestVentas.CLVTAS = ventas.ToString() + "00";// se agregan ceros de decimales 
                responseActualizaVentas = await _informacionClienteRestRepository.ActualizarVentasCliente(requestVentas);
                responsePDf.CodigoRetorno = responseActualizaVentas.CodigoRetorno;
                responsePDf.mensaje = responseActualizaVentas.Mensaje;

                ActualizaPatrimonioRequest requestCrm = new ActualizaPatrimonioRequest()
                {
                    numIdentificacion = request.identificacion,
                    usuario = _configuration["GeneralConfig:usuario"],
                    activos = responsePDf.SaldoActivos,
                    pasivos = responsePDf.SaldoPasivos,
                    patrimonio = responsePDf.SaldoPatrimonio,                    
                    tipoTransaccion = "ACT"

                };

                ActualizaPatrimonioResponse responseCrm = await _informacionClienteRestRepository.ActualizarDataCrm(requestCrm);

                if (responseActualizaVentas.CodigoRetorno == 0)
                {
                    ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
                    solicitudModiRequest.Producto = request.producto;
                    solicitudModiRequest.IdSolicitud = request.idSolicitud;
                    solicitudModiRequest.tieneActVenta = "S";
                    solicitudModiRequest.fuenteIngreso = "PDF SRI";
                    ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);

                    GrabarDatosPersonaResponse GrabarPersona = new GrabarDatosPersonaResponse();
                    InformacionClienteDniResponse _datosPersona = new InformacionClienteDniResponse();
                    _datosPersona = await _informacionRepository.ConsultarDatosRC(request.identificacion, request.producto);

                    GrabarDatosPersonaRequest dataPersonaGrabar = new GrabarDatosPersonaRequest()
                    {
                        nombre = _datosPersona.nombres, //registro civil
                        identificacion = _datosPersona.identificacion,
                        estadoCivil = _datosPersona.idEstadoCivil, //registro civil
                        situacionLaboral = string.Format(_configuration["GeneralConfig:idRelacionDepenendecia"]),  //1950  dejar en appsetings                     
                        origenIngresos = string.Format(_configuration["GeneralConfig:idOrigenIngresos"]),  //4621  dejar en appsetings
                        nacionalidad = _datosPersona.idNacionalidad, //registro civil
                        idGenero = _datosPersona.idGenero, //registro civil
                        tipoPersona = string.Format(_configuration["GeneralConfig:tipoPersonaNatural"]),
                        ventaMensual = (Convert.ToDecimal(responsePDf.VentasAnuales) / 12).ToString(),
                        fechaVenta = "31-12-" + request.anyo
                    };

                    GrabarPersona = await _informacionClienteRepository.GrabarDatosPersona(dataPersonaGrabar);


                }

            }


            return responsePDf;
        }
    }
}
