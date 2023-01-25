using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.catalogo;
using bg.hd.banca.pyme.domain.entities.documento;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.domain.entities.expediente.detalle;
using bg.hd.banca.pyme.domain.entities.firmaElectronica;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Globalization;

namespace bg.hd.banca.pyme.application.services
{
    public class FirmaElectronicaRepository : IFirmaElectronicaRepository
    {
        private readonly IFirmaElectronicaRestRepository _firmaElectronicaRestRepository;
        private readonly IGestionExpedienteRepository _gestionExpedienteRepository;
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IConfiguration _configuration;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;

        public FirmaElectronicaRepository(IConsultarCatalogoRestRepository consultarCatalogoRestRepository,IInformacionClienteRestRepository informacionClienteRepository, IConfiguration Configuration, IFirmaElectronicaRestRepository _firmaElectronicaRestRepository, IGestionExpedienteRestRepository expedienteRepository)
        {
            this._configuration = Configuration;
            this._firmaElectronicaRestRepository = _firmaElectronicaRestRepository;
            this._expedienteRepository = expedienteRepository;
            this._informacionClienteRepository = informacionClienteRepository;
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;

        }
        public async Task<GenerarCertificadoFirmaElectronicaResponse> GenerarCertificadoFirmaElectronica(GenerarCertificadoFirmaElectronicaRequest request)
        {
            RegistrarAuditoriaResponse registrarAuditoria = new();
            RegistrarAuditoriaRequest requestAuditoria = new();
            GenerarCertificadoFirmaElectronicaResponse generaCert = new();
            ConsultarExpProductoActRequest expProductoActRequest = new ConsultarExpProductoActRequest();

            expProductoActRequest.descriptionProducto = request.producto;
            expProductoActRequest.idExpediente = Convert.ToInt32(request.IdExpediente);


            expProductoActRequest.usuario = request.usuario;

            requestAuditoria.ip = request.ip;
            requestAuditoria.opcion = "1";
            requestAuditoria.idExpediente = request.IdExpediente;
            requestAuditoria.identificacion = request.identificacion;
            requestAuditoria.idDocumento = "";
            requestAuditoria.accion = "crea-firmaelectronica";
            requestAuditoria.comentario = "Iniciando Proceso de Firma";
            requestAuditoria.usuario = request.usuario;

            registrarAuditoria = await _firmaElectronicaRestRepository.RegistrarAuditoriaFirmaElectronica(requestAuditoria);

            if (registrarAuditoria.CodigoRetorno != 0)
            {
                throw new GeneralException("Error Aplicativo - RegistrarAuditoriaFirmaElectronica", registrarAuditoria.Mensaje, 2);
            }

            ConsultarExpProductoActResponse expProductoActResponse = await _expedienteRepository.consultaExpedientesId(expProductoActRequest);
            request.productoNombre = request.producto;
            request.producto = expProductoActResponse.infoExpediente.subProductoCore + "F";
            ConsultaPersonaResponse personaResponse = await _informacionClienteRepository.InformacionClientePersona(request.identificacion);

            request.nombre = personaResponse.DatosPersona.primerNombre + " " + personaResponse.DatosPersona.segundoNombre;
            request.apellidoPaterno = personaResponse.DatosPersona.primerApellido;
            request.apellidoMaterno = personaResponse.DatosPersona.segundoApellido;
            request.direccion = personaResponse.DatosPersona.dirDomicilio;
            request.telefono = personaResponse.DatosPersona.celular;
            request.provincia = personaResponse.DatosPersona.provinciaDomicilioDesc;
            request.ciudad = personaResponse.DatosPersona.ciudadDomicilioDesc;
            request.email = personaResponse.DatosPersona.emailDomicilio;
            if (String.IsNullOrEmpty(request.ip))
            {
                request.ip = "192.162.0.1";
            };
            generaCert = await _firmaElectronicaRestRepository.GenerarCertificadoFirmaElectronica(request);

            if (generaCert.CodigoRetorno == 0)
            {
                ModificarExpedienteRequest expedienteRequestModificar = new();
                expedienteRequestModificar.Identificacion = request.identificacion;
                expedienteRequestModificar.idProducto = request.productoNombre;
                expedienteRequestModificar.idExpediente = request.IdExpediente;
                expedienteRequestModificar.usuarioModifica = request.usuario;
                expedienteRequestModificar.strUsuario = request.usuario;
                expedienteRequestModificar.idModulo = "0";
                expedienteRequestModificar.idFormulario = "0";
                expedienteRequestModificar.idEtapa = "3118";//Formalizar Producto
                expedienteRequestModificar.idEstado = "5603";//Genera Certificado Digital
                await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);
            }

            return generaCert;
        }

        public async Task<RegistrarAuditoriaResponse> RegistrarAuditoriaFirmaElectronica(RegistrarAuditoriaRequest request)
        {
            RegistrarAuditoriaResponse registro = new RegistrarAuditoriaResponse();
            char delimitador = ',';
            string[] valores = request.idDocumento.Split(delimitador);

            foreach (string iterator in valores)
            {
                
                request.idDocumento = iterator;
                request.opcion = _configuration["FirmaElectronica:opcion"];
                registro = await _firmaElectronicaRestRepository.RegistrarAuditoriaFirmaElectronica(request);
            }


            return registro;//await _firmaElectronicaRestRepository.RegistrarAuditoriaFirmaElectronica(request);
        }
        public async Task<ConfirmacionCertificadoResponse> ConfirmarCertificadoFirmaElectronica(ConfirmacionCertificadoRequest request)
        {
            ConfirmacionCertificadoResponse confirmarFirmaElectronicaResponse = new ConfirmacionCertificadoResponse();
            RegistrarAuditoriaResponse registrarAuditoria = new();
            RegistrarAuditoriaRequest requestAuditoria = new();
            ActualizaExpedientesDetalleResponse DetallExp = new();

            requestAuditoria.ip = request.ip;
            requestAuditoria.opcion = "1";
            requestAuditoria.idExpediente = request.IdExpediente;
            requestAuditoria.identificacion = request.identificacion;
            requestAuditoria.idDocumento = "";
            requestAuditoria.accion = "firma-documento";
            requestAuditoria.comentario = "Firmar Documentos";



            requestAuditoria.usuario = request.usuario;


            registrarAuditoria = await _firmaElectronicaRestRepository.RegistrarAuditoriaFirmaElectronica(requestAuditoria);

            if (registrarAuditoria.CodigoRetorno != 0)
            {
                throw new GeneralException("Error Aplicativo - RegistrarAuditoriaFirmaElectronica", registrarAuditoria.Mensaje, 2);
            }
            if (String.IsNullOrEmpty(request.ip))
            {
                request.ip = "192.162.0.1";
            };


            confirmarFirmaElectronicaResponse = await _firmaElectronicaRestRepository.ConfirmarCertificadoFirmaElectronica(request);


            EncolarExpProcesoNeoBatchRequest procesoNeoBatchRequest = new EncolarExpProcesoNeoBatchRequest();
            procesoNeoBatchRequest.opcion = "1";
            procesoNeoBatchRequest.idExpediente = request.IdExpediente;
            procesoNeoBatchRequest.idCodigo = "0";
            procesoNeoBatchRequest.idEstadoProceso = "1";
            procesoNeoBatchRequest.usuarioInvoca = request.usuario;
            procesoNeoBatchRequest.fechaGenerarAprobarSolHost = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


            ///ProcesoNeoBatch procesoNeoBatch = this._configuration.GetSection("ProcesoNeoBatch").Get<ProcesoNeoBatch>();

            List<string> procesoNeoBatch = new List<string>()
            {
                "13;Inicia Proceso de Indexacion documento FOTO",
                "14;Inicia Proceso de Indexacion documento IDENTIDAD"
            };

            foreach (string TipoProcesoTempReg in procesoNeoBatch)
            {
                string[] TipoProcesoTemp = TipoProcesoTempReg.Split(";");
                procesoNeoBatchRequest.idTipoProceso = TipoProcesoTemp[0];
                procesoNeoBatchRequest.comentarioEstadoProceso = TipoProcesoTemp[1];
                procesoNeoBatchRequest.controlExcepcion = false;
                EncolarExpProcesoNeoBatchResponse procesoNeoBatchResponse = await _expedienteRepository.EncolarProcesoNeoBatch(procesoNeoBatchRequest);
                if (procesoNeoBatchResponse.CodigoRetorno != 0)
                {
                    throw new GeneralException("Error API - EncolarProcesoNeoBatch", procesoNeoBatchResponse.Mensaje + " **** [" + $"{procesoNeoBatchRequest.idTipoProceso}: {procesoNeoBatchRequest.comentarioEstadoProceso }" + "] ****", 3);
                }
            }
            if (confirmarFirmaElectronicaResponse.CodigoRetorno == 0)
            {

                string CodigoRetorno = string.Empty;
                string MensajeRetorno = string.Empty;
                string strHoraActual = (DateTime.UtcNow.AddHours(-5)).ToString("HH:mm", CultureInfo.CurrentCulture);
                string HoraMinima = "";
                string HoraMaxima = "";
                bool banderaDentroDeHorario = ValidarHoraSeaValida(request.producto ,2, strHoraActual, out HoraMinima, out HoraMaxima, out CodigoRetorno, out MensajeRetorno);
                confirmarFirmaElectronicaResponse.HoraMinima = HoraMinima;
                confirmarFirmaElectronicaResponse.HoraMaxima = HoraMaxima;

                confirmarFirmaElectronicaResponse.fueraHorarioLaboral = !banderaDentroDeHorario;

                if (CodigoRetorno != "000")
                {
                    throw new GeneralException("Error API - ValidarHoraSeaValida", MensajeRetorno, 5);
                }

                ModificarExpedienteRequest expedienteRequestModificar = new();
                expedienteRequestModificar.Identificacion = request.identificacion;
                expedienteRequestModificar.idProducto = request.producto;
                expedienteRequestModificar.idExpediente = request.IdExpediente;
                expedienteRequestModificar.usuarioModifica = request.usuario;
                expedienteRequestModificar.strUsuario = request.usuario;
                expedienteRequestModificar.idModulo = "0";
                expedienteRequestModificar.idFormulario = "0";
                expedienteRequestModificar.idEtapa = "5602";
                expedienteRequestModificar.idEstado = "5604";
                await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);

                ActualizaExpedientesDetalleRequest DetalleExpRequest = new ActualizaExpedientesDetalleRequest()
                {
                    idExpediente = request.IdExpediente,
                    idProducto = request.producto,
                    idModulo = "298",
                    idFormulario = "1",
                    strUsuario = request.usuario,
                    fechaAceptacionContrato = DateTime.UtcNow.AddHours(-5).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                };

                DetallExp = await _expedienteRepository.ActualizaExpedientesDetalles(DetalleExpRequest);

            }
            return confirmarFirmaElectronicaResponse;
        }


        public bool ValidarHoraSeaValida(string producto, int opcion, string fecha, out string HoraMinima, out string HoraMaxima, out string codTrasn, out string mensajeTrans)
        {
            codTrasn = "000";
            mensajeTrans = "";
            bool bBandera = false;
            string HoraMaximaAux = "";
            HoraMaxima = HoraMaximaAux;
            string HoraMinimaAux = "";
            HoraMinima = HoraMinimaAux;
            DateTime utc = DateTime.UtcNow;
            DateTime now = DateTime.Now;
            string UtcNow = string.Format("{0} {1}", utc, utc.Kind);
            string Now = string.Format("{0} {1}", now, now.Kind);

            string fecha_Hora_Actual_serv_OPENSHIFT = "";
            try
            {
                DateTime saveNow = DateTime.Now;
                DateTime saveUtcNow = DateTime.UtcNow;
                DateTime dt = DateTime.ParseExact(fecha, "HH:mm", CultureInfo.CurrentCulture);
                fecha_Hora_Actual_serv_OPENSHIFT = string.Format("{0} {1}", dt, dt.Kind);
                ConsultarCatalogoRequest _requestCatalogo = new();

                _requestCatalogo.producto = producto;
                _requestCatalogo.opcion = 5;
                _requestCatalogo.idCatalogo = "194";
                _requestCatalogo.idCatalogoPadre = "0";
                _requestCatalogo.Filtro = "";
                _requestCatalogo.valorFiltro = "3069";
                (_consultarCatalogoRestRepository.ConsultarCatalogo(_requestCatalogo).Result).listaCatalogoDetalle.catalogoDetalle.ForEach(elem => HoraMaximaAux = elem.strValor2);
                HoraMaxima = HoraMaximaAux;
                _requestCatalogo.valorFiltro = "3070";
                (_consultarCatalogoRestRepository.ConsultarCatalogo(_requestCatalogo).Result).listaCatalogoDetalle.catalogoDetalle.ForEach(elem => HoraMinimaAux = elem.strValor2);
                HoraMinima = HoraMinimaAux;

                string[] sHoraMaxima = HoraMaxima.Split(':');
                string[] sHoraMinima = HoraMinima.Split(':');

                switch (opcion)
                {
                    case 1:

                        if (dt.Hour < int.Parse(sHoraMaxima[0]) || (int.Parse(sHoraMaxima[0]) == dt.Hour && int.Parse(sHoraMaxima[1]) > dt.Minute))
                        {
                            bBandera = true;
                        }
                        if (bBandera)
                        {
                            if (dt.Hour > int.Parse(sHoraMinima[0]) || (int.Parse(sHoraMinima[0]) == dt.Hour && int.Parse(sHoraMinima[1]) < dt.Minute))
                            {
                                bBandera = true;
                            }
                        }
                        break;

                    case 2:
                        if (dt.Hour < int.Parse(sHoraMaxima[0]) || (int.Parse(sHoraMaxima[0]) == dt.Hour && int.Parse(sHoraMaxima[1]) > dt.Minute))
                        {
                            bBandera = true;
                        }
                        if (bBandera)
                        {
                            if (dt.Hour > int.Parse(sHoraMinima[0]) || (int.Parse(sHoraMinima[0]) == dt.Hour && int.Parse(sHoraMinima[1]) < dt.Minute))
                            {
                                bBandera = true;
                            }
                            else
                            {
                                bBandera = false;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                codTrasn = "001";
                mensajeTrans = "Error al validar Horario: " + ex.Message;
            }
            #region graba logs
            string jsonRequest = @"{" +
                                       $"\n'Opcion': '{opcion}'," +
                                       $"\n'fecha_Hora_Actual_serv_OPENSHIFT': '{fecha_Hora_Actual_serv_OPENSHIFT}'," +
                                       $"\n'UtcNow_UniversalTime': '{UtcNow}'," +
                                       $"\n'Now_LocalTime': '{Now}'\n" +
                                    @"}";
            string jsonResponse = @"{" +
                                    $"\n'CodigoRetorno': '{codTrasn}'," +
                                    $"\n'MensajeRetorno': 'Metodo - ValidarHoraSeaValida: {mensajeTrans}'," +
                                    $"\n'dentroDeHorario': '{bBandera}'," +
                                    $"\n'HoraMinima': '{HoraMinima}'," +
                                    $"\n'HoraMaxima': '{HoraMaxima}'\n" +
                                  @"}";
            Log.Information("{ResourceRequestPath} {identificador} {requestData} {ResponseData}", "hdbancapersona/v2/firma-electronica/documento/firmar [ValidarHoraSeaValida]", "", jsonRequest, jsonResponse);
            #endregion
            return bBandera;
        }

    }
}
