using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.PreCalificador;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.expediente;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.domain.entities.FichaPreCalificador;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.domain.entities.ArchivoImpuestoIva;
using bg.hd.banca.pyme.domain.entities.informacionCliente;

namespace bg.hd.banca.pyme.application.services
{
    public class PreCalificadorRepository : IPreCalificadorRepository
    {
        private readonly IPreCalificadorRestRepository _preCalificadorRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;
        private readonly IInformacionClienteRepository _informacionRepository;

        public PreCalificadorRepository(IPreCalificadorRestRepository _preCalificadorRestRepository, IConfiguration _configuration,
            IGestionExpedienteRestRepository _expedienteRepository, IInformacionClienteRestRepository _informacionClienteRepository,
            IInformacionClienteRepository informacionRepository)
        {
            this._preCalificadorRestRepository = _preCalificadorRestRepository;
            this._configuration = _configuration;
            this._expedienteRepository = _expedienteRepository;
            this._informacionClienteRepository = _informacionClienteRepository;
            _informacionRepository = informacionRepository;

        }

        public async Task<PreCalificadorResponse> PreCalificar(PreCalificadorRequest request)
        {
            ConsultarConfiguracionResponse ConfiguracionResponse = new ConsultarConfiguracionResponse();
            PreCalificadorResponse _responseHostRiesgo = new();
            PreCalificadorResponse _responseCualitativo = new();
            PreCalificadorResponse _responseGenerarSBS = new();
            GenerarFichaPreCalificadorResponse _responseFichaPreCalificador = new();
            PreCalificadorResponse _responseInformeFinalSBS = new();
            PreCalificadorResponse _responseGuardarFichaPre = new();
            ProcDeclaracionIVAResponse _responseProcesoDeclaracionIva = new();
            ModificarExpedienteRequest expedienteRequestModificar = new ModificarExpedienteRequest();

            // Se Ejecuta Servicios para la Pre Calificacion
            // 1
            _responseHostRiesgo = await _preCalificadorRestRepository.ConsultaHostRiesgos(request);

            // 2
            _responseCualitativo = await _preCalificadorRestRepository.GenerarAnalisisCualitativo(request);

            // 3
            _responseGenerarSBS = await _preCalificadorRestRepository.GenerarCalificacionSBS(request);

            // 4
            _responseFichaPreCalificador = await _preCalificadorRestRepository.GenerarFichaPreCalificador(request);

            //_responseGuardarFichaPre = await _preCalificadorRestRepository.GuardarFichaPrecalificador(request, token);





            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
            oficialRequest.identificacion = request.identificacion;

            var usuarioAgencia = "";
            var OpidGestor = "";

            usuarioAgencia = request.UsuarioGestor;
            OpidGestor = request.OpidGestor;

            ///////////////
            //_responseInformeFinalSBS = await _preCalificadorRestRepository.InformeFinalSBS(request, token);
            //////////   
            _responseProcesoDeclaracionIva = await _preCalificadorRestRepository.ProcesoDeclaracionIva(request);
            var idEtapa = "3114"; // Solicitud
            var dictamen = "294";

            if (_responseProcesoDeclaracionIva.Dictamen.Equals("292") && _responseFichaPreCalificador.Dictamen.Equals("292"))
            {
                // idEtapa = "11605"; // Personalizacion
                dictamen = "292";
            }


            ConsultarConfiguracionRequest consultarConfiguracionRequest = new ConsultarConfiguracionRequest();
            consultarConfiguracionRequest.Identificacion = request.identificacion;
            consultarConfiguracionRequest.idProducto = request.Producto;
            consultarConfiguracionRequest.strUsuario = string.Format(_configuration["GeneralConfig:usuarioWeb"]);
            consultarConfiguracionRequest.idDictamen = dictamen;
            consultarConfiguracionRequest.idTipoConfiguracionExpediente = "4738";
            consultarConfiguracionRequest.idEtapa = "3113";
            consultarConfiguracionRequest.estado = "3076";
            consultarConfiguracionRequest.idModulo = "0";
            consultarConfiguracionRequest.idFormulario = "0";
            consultarConfiguracionRequest.GenerarException = false;

            ConfiguracionResponse = await _expedienteRepository.ConsultarConfiguracion(consultarConfiguracionRequest);


            expedienteRequestModificar.idCatEstado = ConfiguracionResponse.idCatEstadoActualiza;
            expedienteRequestModificar.idEstado = ConfiguracionResponse.codEstadoActualiza;
            expedienteRequestModificar.idProducto = request.Producto;
            expedienteRequestModificar.idModulo = "0";
            expedienteRequestModificar.strUsuario = usuarioAgencia;
            expedienteRequestModificar.idExpediente = request.idExpediente;
            expedienteRequestModificar.idFormulario = "0";
            expedienteRequestModificar.idEtapa = idEtapa;
            expedienteRequestModificar.idDictamen = ConfiguracionResponse.codDictamen;

            expedienteRequestModificar.idCatDictamen = "11";
            expedienteRequestModificar.opidGestor = OpidGestor;

            var mensajeError = "";
            if (_responseProcesoDeclaracionIva.Dictamen != "292")
            {
                mensajeError = _responseProcesoDeclaracionIva.Mensaje;
                expedienteRequestModificar.motivoDictamen = "Cliente No Cumple Perfil";
            }
            if (_responseFichaPreCalificador.Dictamen != "292")
            {
                mensajeError = _responseFichaPreCalificador.Mensaje;
                expedienteRequestModificar.motivoDictamen = "Cliente No Cumple Perfil";
            }

            expedienteRequestModificar.observaciones = mensajeError != "" ? "Precalificador Rating: " + mensajeError : "";
            //if (request.version != 2)
            //{
            await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);
            //}


            if (_responseProcesoDeclaracionIva.Dictamen.Equals("292") && _responseFichaPreCalificador.Dictamen.Equals("292"))
            {
                var montoCredito = System.Convert.ToDecimal(_configuration["GeneralConfig:MontoCredito:maximo"]);
                PreCalificadorResponse response = new();
                response.Mensaje = "Ok - Proceso";
                response.CodigoRetorno = 0;
                response.MontoAprobado = System.Convert.ToDecimal(_responseFichaPreCalificador.montoAprobado) > montoCredito ? montoCredito : System.Convert.ToDecimal(_responseFichaPreCalificador.montoAprobado);
                response.tieneAprobacion = System.Convert.ToDecimal(_responseFichaPreCalificador.montoAprobado) != request.montoSolicitado ? true : false;
                response.montoSolicitado = request.montoSolicitado;

                return response;
            }
            else
            {
                throw new PreCalificadorException(mensajeError, mensajeError, 3);
            }

        }

        public async Task<CuentasContablesResponse> IngresoCuentasContables(CuentasContablesRequest request)
        {
            ActualizaVentasClienteResponse responseActualizaVentas = new ActualizaVentasClienteResponse();

            if (request.act_caja.Equals(""))
            {
                request.act_caja = "0";
            }
            if (request.act_cuentasCobrar.Equals(""))
            {
                request.act_cuentasCobrar = "0";
            }
            if (request.act_totalActfijos.Equals(""))
            {
                request.act_totalActfijos = "0";
            }
            if (request.pas_obligacionBancariaCP.Equals(""))
            {
                request.pas_obligacionBancariaCP = "0";
            }
            if (request.pas_pasivosNoCtes.Equals(""))
            {
                request.pas_pasivosNoCtes = "0";
            }
            if (request.act_totalInventario.Equals(""))
            {
                request.act_totalInventario = "0";
            }
            if (request.pas_otrosPasivos.Equals(""))
            {
                request.pas_otrosPasivos = "0";
            }
            if (request.pas_obligacionBancariaLP.Equals(""))
            {
                request.pas_obligacionBancariaLP = "0";
            }

            CuentasContablesResponse response = await _preCalificadorRestRepository.IngresoCuentasContables(request);
            double montoMinimoRedondeo = double.Parse(_configuration["GeneralConfig:montoMinimoVtaRendondeo"]);
            double montoMaximoRedondeo = double.Parse(_configuration["GeneralConfig:montoMaximaVtaRendondeo"]);
            if (response.CodigoRetorno == 0)
            {

                ActualizaVentasClienteMicroServ requestVentas = new ActualizaVentasClienteMicroServ();
                requestVentas.ID = request.identificacion;
                requestVentas.TIPOID = _configuration["GeneralConfig:tipoIdentificacionDescripcion"];
                requestVentas.UPCLVTAS = "S";
                requestVentas.UPCLFVTANUAL = "S";
                requestVentas.CLFVTANUAL = response.Anyo3 + "1231";
                double ventas = 0;
                double.TryParse(response.SaldoVentaAnyo3, out ventas);
                ventas = Math.Round(ventas, 0);
                if (ventas >= montoMinimoRedondeo && ventas <= montoMaximoRedondeo)
                    ventas = Math.Truncate(ventas / 1000) + 1;
                else
                    ventas = Math.Truncate(ventas / 1000);

                requestVentas.CLVTAS = ventas.ToString() + "00";// se agregan ceros de decimales 
                responseActualizaVentas = await _informacionClienteRepository.ActualizarVentasCliente(requestVentas);

                ActualizaPatrimonioRequest requestCrm = new ActualizaPatrimonioRequest()
                {
                    numIdentificacion = request.identificacion,
                    usuario = _configuration["GeneralConfig:usuario"],
                    activos = response.SaldoActivos,
                    pasivos = response.SaldoPasivos,
                    patrimonio = response.SaldoPatrimonio,
                    tipoTransaccion = "ACT"

                };

                ActualizaPatrimonioResponse responseCrm = await _informacionClienteRepository.ActualizarDataCrm(requestCrm);

                ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
                solicitudModiRequest.Producto = request.Producto;
                solicitudModiRequest.IdSolicitud = request.IdSolicitud;
                solicitudModiRequest.tieneActVenta = "S";
                solicitudModiRequest.fuenteIngreso = "PDF SRI";
                ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);

                GrabarDatosPersonaResponse GrabarPersona = new GrabarDatosPersonaResponse();
                InformacionClienteDniResponse _datosPersona = new InformacionClienteDniResponse();
                _datosPersona = await _informacionRepository.ConsultarDatosRC(request.identificacion, request.Producto);

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
                    ventaMensual = (Convert.ToDecimal(response.SaldoVentaAnyo3) / 12).ToString(),
                    fechaVenta = "31-12-" + response.Anyo3
                };

                GrabarPersona = await _informacionClienteRepository.GrabarDatosPersona(dataPersonaGrabar);
            }

            return response;
        }
    }
}