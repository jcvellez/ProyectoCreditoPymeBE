using bg.hd.banca.pyme.domain.entities.config;
using bg.hd.banca.pyme.domain.entities.expediente;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.catalogo;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Globalization;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.ExcepcionAnalisis;
using bg.hd.banca.pyme.domain.entities.biometria;
using static bg.hd.banca.pyme.domain.entities.expediente.ConsultarSectorPymesResponse;
using bg.hd.banca.pyme.domain.entities.BancaControl;
using bg.hd.banca.pyme.domain.entities.PreCalificador;

namespace bg.hd.banca.pyme.application.services
{
    public class GestionExpedienteRepository : IGestionExpedienteRepository
    {
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRepository;
        private readonly IInformacionClienteRepository _informacionRepository;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IInformacionClienteRestRepository _informacionClienteRestRepository;
        private readonly IAnalisisRatingRestRepository _analisisRatingRestRepository;
        private readonly IBiometriaRestRepository _biometriaRestRepository;
        private readonly IPreCalificadorRestRepository _preCalificadorRestRepository;

        public GestionExpedienteRepository(IConfiguration Configuration, IGestionExpedienteRestRepository crearExpedienteRepository,
            IInformacionClienteRestRepository informacionClienteRepository, IInformacionClienteRepository informacionRepository, IConsultarCatalogoRestRepository _consultarCatalogoRestRepository,
            IInformacionClienteRestRepository informacionClienteRestRepository, IAnalisisRatingRestRepository analisisRatingRestRepository,
            IBiometriaRestRepository biometriaRestRepository, IPreCalificadorRestRepository _preCalificadorRestRepository)
        {
            _expedienteRepository = crearExpedienteRepository;
            _informacionClienteRepository = informacionClienteRepository;
            _informacionRepository = informacionRepository;
            _configuration = Configuration;
            this._consultarCatalogoRestRepository = _consultarCatalogoRestRepository;
            _informacionClienteRestRepository = informacionClienteRestRepository;
            _analisisRatingRestRepository = analisisRatingRestRepository;
            this._biometriaRestRepository = biometriaRestRepository;
            this._preCalificadorRestRepository = _preCalificadorRestRepository;

        }

        public async Task<CrearExpedienteResponse> CrearExpediente(CrearExpedienteRequest request)
        {
            CrearExpedienteResponse response = new CrearExpedienteResponse();
            Expediente dataExpediente = new Expediente();
            bool continuar = true;
            bool saveConyugue = false;
            Transaccion errorTransaccion = new Transaccion();
            ValidarPoliticasResponse validarPoliticasResponse = new ValidarPoliticasResponse();
            ConsultarConfiguracionResponse ConfiguracionResponse = new ConsultarConfiguracionResponse();
            ModificarExpedienteRequest expedienteRequestModificar = new ModificarExpedienteRequest();
            InformacionClienteDniResponse _datosRConyugue = new InformacionClienteDniResponse();
            InformacionClienteDniResponse _datosRCTitular = new InformacionClienteDniResponse();
            InformacionClienteDniResponse _datosPersona = new InformacionClienteDniResponse();
            ConsultaDatosRUCResponse _dataSri = new ConsultaDatosRUCResponse();
            ConsultarCatalogoResponse _catalogo = new();
            GrabarDatosPersonaResponse GrabarPersona = new GrabarDatosPersonaResponse();
            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;

            #region consultar datos del oficial
            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
            oficialRequest.identificacion = request.Identificacion;

            #region Consulta Datos SRI
            _dataSri = await _informacionClienteRepository.ConsultaDatosRUC(request.Identificacion);
            #endregion          

            dataExpediente.IdOficina = int.Parse(!string.IsNullOrEmpty(request.idAgenciaOficial) ? request.idAgenciaOficial : "0");

            dataExpediente.UsuarioGestor = request.UsuarioGestor;
            dataExpediente.OpidGestor = request.OpidGestor;

            #endregion


            if (request.IdExpediente != null && request.IdExpediente != 0)
            {
                ConsultarExpProductoActRequest consultaExpRequest = new ConsultarExpProductoActRequest()
                {
                    idExpediente = request.IdExpediente,
                    descriptionProducto = request.Producto,
                    usuario = dataExpediente.UsuarioGestor
                };


                ConsultarExpProductoActResponse consultaExpediente = await _expedienteRepository.consultaExpedientesId(consultaExpRequest);

                if (consultaExpediente.codigoRetorno==0)
                {
                    Producto productoDeta = await _expedienteRepository.ObtenerProducto(request.Producto);


                    if (productoDeta.etapasAprobado.Contains(consultaExpediente.infoExpediente.idEtapa) ||
                        productoDeta.etapasLiquidacion.Contains(consultaExpediente.infoExpediente.idEtapa)
                        )
                    {
                        response.CodigoRetorno = 0;
                        response.Mensaje = "PROCESO OK";
                        response.idExpediente = consultaExpediente.infoExpediente.idExpediente;
                        return response;
                    }
                }
            }

            #region Consultar datos de la solicitud
            SolicitudRequest solicitudRequest = new SolicitudRequest();
            solicitudRequest.IdSolicitud = request.IdSolicitud;
            solicitudRequest.Identificacion = request.Identificacion;
            Solicitud solitudResponse = await _expedienteRepository.ConsultarSolicitud(solicitudRequest);
            string _dataSolicitud = JsonConvert.SerializeObject(solitudResponse.DataSolicitud);
            dataExpediente.DatosActivos = JsonConvert.DeserializeObject<Activos>(_dataSolicitud);
            #endregion

           

            //consulta servicio de ARQ para traer datos de HOST-CLTE
            identificaUsuarioDataResponse = await _informacionClienteRestRepository.InformacionClienteData(request.Identificacion);

            #region Consultar datos persona NEO
            ConsultaPersonaResponse personaResponse = await _informacionClienteRepository.InformacionClientePersona(request.Identificacion);
            dataExpediente.DatosCliente = personaResponse.DatosPersona;
            dataExpediente.DatosConyuge = personaResponse.DatosConyugue;
            #endregion

            #region Graba datos personas

            if (_dataSri != null)
            {
                List<string> Arreglo_codigoCIUU = new List<string>();
                var respuesta = _dataSri.data
                    .Where(elem => elem.estadoContribuyente.Trim().Equals("ACTIVO"))
                                           .Select(elem => elem).ToList();
                respuesta.ForEach(elem => Arreglo_codigoCIUU.Add(elem.codigoCIUU));


                if (respuesta.Count() > 0)
                {

                    ConsultarCatalogoRequestMicroServ requestCatalogo = new ConsultarCatalogoRequestMicroServ()
                    {
                        opcion = 4,
                        idCatalogo = "257",
                        idCatalogoPadre = "0",
                        Filtro = "strCodigoHost",
                        valorFiltro = Arreglo_codigoCIUU.First()
                    };

                    _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();
                }
                else
                {
                    throw new GestionExpedienteException("Cliente no contiene actividad economica en estado activo ", "Cliente no contiene actividad economica en estado activo", 9);
                }

                string CodNeoCIIU = "0";
                if (_catalogo.listaCatalogoDetalle != null)
                {
                     CodNeoCIIU = "" + _catalogo.listaCatalogoDetalle.catalogoDetalle[0].idCodigo;
         
                }
                else
                {
                    if (respuesta[0].codigoCIUUSB == null) throw new GeneralException("Cliente no contiene actividad economica en neo ", "Cliente no contiene actividad economica en neo", 9);

                    ConsultarCatalogoRequestMicroServ requestCatalogo = new ConsultarCatalogoRequestMicroServ()
                    {
                        opcion = 4,
                        idCatalogo = "257",
                        idCatalogoPadre = "0",
                        Filtro = "strCodigoHost",
                        valorFiltro = respuesta[0].codigoCIUUSB
                    };

                    _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();

                    CodNeoCIIU = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();                    
                }

                _datosPersona = await _informacionRepository.ConsultarDatosRC(request.Identificacion, request.Producto);

                CultureInfo culture = new CultureInfo("en-US");
                string format = "dd/MM/yyyy";
                DateTime result;
                result = DateTime.ParseExact(_datosPersona.fechaNacimiento, format, culture);
                var fechaNacimiento = result.ToString("yyyy/MM/dd");

                GrabarDatosPersonaRequest dataPersonaGrabar = new GrabarDatosPersonaRequest()
                {
                    nombre = _datosPersona.nombres, //registro civil
                    identificacion = _datosPersona.identificacion,
                    estadoCivil = _datosPersona.idEstadoCivil, //registro civil
                    situacionLaboral = string.Format(_configuration["GeneralConfig:idRelacionDepenendecia"]),  //1950  dejar en appsetings                     
                    origenIngresos = string.Format(_configuration["GeneralConfig:idOrigenIngresos"]),  //4621  dejar en appsetings
                    idActividadCiiu = CodNeoCIIU, // consulta del sri
                    fechaNacimiento = fechaNacimiento,
                    nacionalidad = _datosPersona.idNacionalidad, //registro civil
                    idGenero = _datosPersona.idGenero, //registro civil
                    tipoPersona = string.Format(_configuration["GeneralConfig:tipoPersonaNatural"]),

                };
                // setea datos de microservicio core  CLTE
                dataPersonaGrabar.direccionDomicilio.celular = identificaUsuarioDataResponse.telefonoCelular;
                dataPersonaGrabar.direccionDomicilio.correoElectronico = identificaUsuarioDataResponse.correoElectronico;

                GrabarPersona = await _informacionClienteRepository.GrabarDatosPersona(dataPersonaGrabar);

            }
            #endregion

            #region Ingreso expediente
            IngresoExpedienteRequest expedienteRequest = new IngresoExpedienteRequest()
            {
                identificacion = request.Identificacion,
                idPersona = dataExpediente.DatosCliente.idPersonaNeo,
                idCatOficina = "94",
                idOficina = dataExpediente.IdOficina.ToString(),
                idFormulario = "0",
                idModulo = "298",
                usuarioGestor = dataExpediente.UsuarioGestor,
                usuarioEtapa = dataExpediente.UsuarioGestor,
                opidGestor = dataExpediente.OpidGestor,
                strUsuario = dataExpediente.UsuarioGestor,
                descriptionProducto = request.Producto,
                idExpediente = request.IdExpediente.ToString()
            };



            response = await _expedienteRepository.IngresoExpediente(expedienteRequest);
            dataExpediente.IdExpediente = int.Parse(string.IsNullOrEmpty(response.idExpediente) ? "0" : response.idExpediente);

            response.usuario = dataExpediente.UsuarioGestor;
            #endregion


            #region Modificar solicitud
            ConsultarCatalogoRequestMicroServ catalogorequest = new ConsultarCatalogoRequestMicroServ();
            catalogorequest.opcion = 4;
            catalogorequest.idCatalogo = "195";
            catalogorequest.idCatalogoPadre = "0";
            catalogorequest.Filtro = "strValor";
            catalogorequest.valorFiltro = request.TipoTabla;
            catalogorequest.valorFiltro =
            (catalogorequest.valorFiltro.Equals("fija") ? "francesa" :
                        (catalogorequest.valorFiltro.Equals("variable") ? "alemana" : ""));
            ConsultarCatalogoResponse catalogoresponseTabala = await _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(catalogorequest);

            Producto productoDetalle = await _expedienteRepository.ObtenerProducto(request.Producto);
            var idCodigoCat = String.Empty;
            if(catalogoresponseTabala.listaCatalogoDetalle != null) idCodigoCat = catalogoresponseTabala.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();

            if (!request.ClienteTokenizado)
            {
                ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
                solicitudModiRequest.Producto = request.Producto;
                solicitudModiRequest.IdSolicitud = request.IdSolicitud;
                solicitudModiRequest.IdExpediente = response.idExpediente;
                solicitudModiRequest.Nombre = dataExpediente.DatosCliente.nombreCompleto;
                solicitudModiRequest.CorreoCore = dataExpediente.DatosCliente.emailDomicilio;
                solicitudModiRequest.CelularCore = dataExpediente.DatosCliente.celular;
                solicitudModiRequest.MontoSolicitado = request.MontoSolicitado;
                solicitudModiRequest.PlazoSolicitado = request.PlazoSolicitado;
                solicitudModiRequest.idTipoTabla = request.Producto == "alVencimiento" ? null : idCodigoCat;
                solicitudModiRequest.TasaProducto = request.TasaProducto;
                solicitudModiRequest.ValorDividendo = request.ValorDividendo;
                solicitudModiRequest.DiaPago = request.DiaPago;
                solicitudModiRequest.idTipoPeriodicidad = productoDetalle.tipoPeriodicidad;
                solicitudModiRequest.IdSegmentoEstrategicoCore = dataExpediente.DatosCliente.idSegmentoEstrategico;
                solicitudModiRequest.SubProductoCore = productoDetalle.subProductoCore;
                solicitudModiRequest.GenerarException = false;
                solicitudModiRequest.Identificacion = request.Identificacion;
                ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);
                if (solicitudResponse.DataTransaccion.codigoResponse != 0)
                {
                    continuar = false;
                    errorTransaccion = solicitudResponse.DataTransaccion;
                    errorTransaccion.MessageResponse = "Error al modificar la solicitud del cliente";
                }
            }
            #endregion

            #region Validar Politicas
            if (continuar)
            {

                ValidarPoliticasRequest validarPoliticasRequest = new ValidarPoliticasRequest();
                validarPoliticasRequest.TipoIdentificacionTitular = dataExpediente.DatosCliente.tipoIdentificacion;
                validarPoliticasRequest.IdentificacionTitular = request.Identificacion;
                validarPoliticasRequest.NombresCompletoTitular = dataExpediente.DatosCliente.nombreCompleto;
                validarPoliticasRequest.NombreTitular = dataExpediente.DatosCliente.primerNombre + " " + dataExpediente.DatosCliente.segundoNombre;
                validarPoliticasRequest.ApellidoTitular = dataExpediente.DatosCliente.primerApellido + " " + dataExpediente.DatosCliente.segundoApellido;
                validarPoliticasRequest.FechaNacimientoTitular = dataExpediente.DatosCliente.fechaNacimiento;
                validarPoliticasRequest.IngresosTitular = dataExpediente.DatosCliente.ingresoMensual;
                validarPoliticasRequest.IdNacionalidadTitular = dataExpediente.DatosCliente.idCatDetNacionalidad;
                validarPoliticasRequest.IdEstadoCivilTitular = dataExpediente.DatosCliente.idCatDetEstadoCivil;
                validarPoliticasRequest.IdRegimenMatrimonialTitular = dataExpediente.DatosCliente.regimenMatrimonial;
                validarPoliticasRequest.IdRelacionDependenciaTitular = dataExpediente.DatosCliente.relacionDependenciaNeo;
                validarPoliticasRequest.AntiguedadTitular = dataExpediente.DatosCliente.antiguedad;
                validarPoliticasRequest.EdadTitular = dataExpediente.DatosCliente.edad;
                validarPoliticasRequest.IdProducto = request.Producto;
                validarPoliticasRequest.IdExpediente = response.idExpediente;
                validarPoliticasRequest.Usuario = response.usuario;
                validarPoliticasRequest.PlazoSolicitado = request.PlazoSolicitado;
                validarPoliticasRequest.PeriodicidadSolicitado = _configuration["GeneralConfig:catalogos:nacionalidad"];
                validarPoliticasRequest.MontoSolicitado = request.MontoSolicitado;
                validarPoliticasRequest.GenerarException = false;



                _datosRCTitular = await _informacionRepository.ConsultarDatosRC(request.Identificacion, request.Producto);

                if (!string.IsNullOrEmpty(_datosRCTitular.identificacionConyuge))
                {
                    Int64 ValAux = 0;
                    if (Int64.TryParse(_datosRCTitular.identificacionConyuge, out ValAux))
                    {
                        if (ValAux != 0)
                        {
                            saveConyugue = true;
                            _datosRConyugue = await _informacionRepository.ConsultarDatosRC(_datosRCTitular.identificacionConyuge, request.Producto);
                            validarPoliticasRequest.TipoIdentificacionAdicional = _configuration["GeneralConfig:tipoIdentificacionDescripcion"];
                            validarPoliticasRequest.IdentificacionAdicional = _datosRConyugue.identificacion;
                            validarPoliticasRequest.NombresCompletoAdicional = _datosRConyugue.nombres;
                            validarPoliticasRequest.NombreAdicional = _datosRConyugue.persona.primerNombre + _datosRConyugue.persona.segundoNombre;
                            validarPoliticasRequest.ApellidoAdicional = _datosRConyugue.persona.primerApellido + _datosRConyugue.persona.SegundoApellido;
                            validarPoliticasRequest.FechaNacimientoAdicional = _datosRConyugue.fechaNacimiento;
                            validarPoliticasRequest.IdNacionalidadAdicional = _datosRConyugue.idNacionalidad;
                            validarPoliticasRequest.IdRegimenMatrimonialAdicional = _datosRConyugue.idRegimenMatrimonial;
                            validarPoliticasRequest.EdadAdicional = _datosRConyugue.edad;
                        }
                    }
                }


                validarPoliticasResponse = await _expedienteRepository.ValidarPoliticas(validarPoliticasRequest);

                if (validarPoliticasResponse.DataTransaccion.codigoResponse != 0)
                {
                    continuar = false;
                    errorTransaccion = validarPoliticasResponse.DataTransaccion;
                    errorTransaccion.MessageResponse = "Error al validar Politicas";
                }
                // comentar cuando funcione host
                //validarPoliticasResponse.idDictamen = "292";
                //continuar = true;


            }
            #endregion

            #region Obtener configuracion
            if (continuar)
            {
                ConsultarConfiguracionRequest consultarConfiguracionRequest = new ConsultarConfiguracionRequest();
                consultarConfiguracionRequest.Identificacion = request.Identificacion;
                consultarConfiguracionRequest.idProducto = request.Producto;
                consultarConfiguracionRequest.strUsuario = response.usuario;
                consultarConfiguracionRequest.idDictamen = validarPoliticasResponse.idDictamen;
                consultarConfiguracionRequest.idTipoConfiguracionExpediente = "4738";
                consultarConfiguracionRequest.idEtapa = "3113";
                consultarConfiguracionRequest.estado = "3076";
                consultarConfiguracionRequest.idModulo = "0";
                consultarConfiguracionRequest.idFormulario = "0";
                consultarConfiguracionRequest.GenerarException = false;

                ConfiguracionResponse = await _expedienteRepository.ConsultarConfiguracion(consultarConfiguracionRequest);

                if (ConfiguracionResponse.DataTransaccion.codigoResponse != 0)
                {
                    continuar = false;
                    errorTransaccion = ConfiguracionResponse.DataTransaccion;
                    errorTransaccion.MessageResponse = "Error al obtener la configuración del expediente";
                }
            }
            #endregion

            if (continuar)
            {
                #region actualizar expediente
                expedienteRequestModificar.Identificacion = request.Identificacion;
                expedienteRequestModificar.idProducto = request.Producto;
                expedienteRequestModificar.idExpediente = response.idExpediente;
                expedienteRequestModificar.idCatDictamen = "11";
                expedienteRequestModificar.idDictamen = validarPoliticasResponse.idDictamen;
                expedienteRequestModificar.motivoDictamen = validarPoliticasResponse.motivoDictamen;
                expedienteRequestModificar.usuarioModifica = response.usuario;
                expedienteRequestModificar.usuarioGestor = response.usuario;
                expedienteRequestModificar.idEtapa = "3113";
                expedienteRequestModificar.idCatEstado = ConfiguracionResponse.idCatEstadoActualiza;
                expedienteRequestModificar.idEstado = ConfiguracionResponse.codEstadoActualiza;
                expedienteRequestModificar.idModulo = "0";
                expedienteRequestModificar.idFormulario = "0";
                expedienteRequestModificar.strUsuario = response.usuario;

                await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);
                #endregion

                #region validar Politica 292
                if (validarPoliticasResponse.idDictamen != "292") { throw new GestionExpedienteException("Validar Politicas", "Cliente no pasa la validacion de politicas", 8); }
                #endregion
            }

            #region Grabar datos normativos
            if (continuar)
            {
                ActualizaInformacionNormativaRequest normativoRequest = new ActualizaInformacionNormativaRequest();
                normativoRequest.GenerarException = false;
                normativoRequest.idExpediente = response.idExpediente;
                normativoRequest.identificacionCliente = request.Identificacion;
                normativoRequest.usuario = response.usuario;
                normativoRequest.opcion = "1";
                normativoRequest.opcionActualizar = "3";
                normativoRequest.preguntaEstasCasadoUnionHecho = saveConyugue ? "S" : "N";
                if (saveConyugue)
                {
                    normativoRequest.idTipoIdentificacionConyuge = "525";
                    normativoRequest.identificacionConyuge = _datosRConyugue.identificacion;
                    normativoRequest.primerNombreConyuge = _datosRConyugue.persona.primerNombre;
                    normativoRequest.segundoNombreConyuge = _datosRConyugue.persona.segundoNombre;
                    normativoRequest.apellidoPaternoConyuge = _datosRConyugue.persona.primerApellido;
                    normativoRequest.apellidoMaternoConyuge = _datosRConyugue.persona.SegundoApellido;
                }
                Transaccion normativoResponse = await _informacionClienteRepository.GrabarDatosNormativos(normativoRequest);
                if (normativoResponse.codigoResponse != 0)
                {
                    continuar = false;
                    errorTransaccion = normativoResponse;
                    errorTransaccion.MessageResponse = "Error al modificar los datos normativos del conyugue";
                }
            }
            #endregion

            #region Actualizar el etapa y estado del expediente
            expedienteRequestModificar.Identificacion = request.Identificacion;
            expedienteRequestModificar.idProducto = request.Producto;
            expedienteRequestModificar.idExpediente = response.idExpediente;
            expedienteRequestModificar.usuarioModifica = response.usuario;
            expedienteRequestModificar.strUsuario = response.usuario;
            expedienteRequestModificar.idModulo = "0";
            expedienteRequestModificar.idFormulario = "0";
            expedienteRequestModificar.idEtapa = continuar ? "3114" : "";

            expedienteRequestModificar.strUsuario = response.usuario;

            if (!continuar)
            {
                expedienteRequestModificar.idCatEstado = "100";
                expedienteRequestModificar.idCatMotivoRechazo = "111";
                expedienteRequestModificar.idMotivoRechazo = "3136";
                expedienteRequestModificar.comentarioRechazo = errorTransaccion.codigoResponse.ToString() + " - " + errorTransaccion.MessageResponse;
            }

            await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);

            if (!continuar) { throw new GestionExpedienteException(errorTransaccion.MessageResponse, errorTransaccion.DescriptionResponse, errorTransaccion.codigoResponse); }

            #endregion

            return response;
        }

        public async Task<Solicitud> ConsultarSolicitud(string identificacion, string idSolicitud)
        {
            SolicitudRequest request = new SolicitudRequest();
            request.Identificacion = identificacion;
            request.IdSolicitud = idSolicitud;
            return await _expedienteRepository.ConsultarSolicitud(request);
        }

        public async Task<CrearSolicitudResponse> CrearSolicitud(CrearSolicitudRequest request)
        {
            ConsultaDatosRUCResponse _dataSri = new ConsultaDatosRUCResponse();
            CrearSolicitudResponse solicitudResponse = new();
            ConsultarSectorPymesResponse sectoresVetados = new();

            if (request.IdSolicitud == 0)
            {
                InformacionClienteDniResponse informacionClienteDniResponse = await _informacionClienteRepository.InformacionCliente(request.Identificacion,"");
                request.Nombre = informacionClienteDniResponse.nombres.Replace("'", "");
                solicitudResponse = await _expedienteRepository.CrearSolicitud(request);
            }
            else
            {
                solicitudResponse.Mensaje = "ok - proceso";
                solicitudResponse.IdSolicitud = request.IdSolicitud.ToString();
            }

            #region Consulta Datos SRI
            _dataSri = await _informacionClienteRepository.ConsultaDatosRUC(request.Identificacion);
            List<string> Arreglo_codigoCIUU = new List<string>();
            var respuesta = _dataSri.data
            .Where(elem => elem.estadoContribuyente.Trim().Equals("ACTIVO"))
                                    .Select(elem => elem).ToList();
            respuesta.ForEach(elem => Arreglo_codigoCIUU.Add(elem.codigoCIUU));

            #endregion

            if (respuesta.Count() == 0)
            {
                throw new GestionExpedienteException("Cliente no contiene actividad economica en estado activo ", "Cliente no contiene actividad economica en estado activo", 9);
            }

            sectoresVetados = await _expedienteRepository.SectoresVetados(request.Identificacion);
            Sectores resp = new Sectores();
            resp = sectoresVetados.ListaSectores[0];
            if (resp.SectorVetado == "1")
            {
                throw new GestionExpedienteException("Cliente Vetado no aplica", "Cliente Vetado no aplica", 9);
            }

            ConsultarCatalogoResponse _catalogo = new();
            ConsultarCatalogoRequestMicroServ requestCatalogo = new ConsultarCatalogoRequestMicroServ()
            {
                opcion = 4,
                idCatalogo = "83",
                idCatalogoPadre = "0",
                Filtro = "strValor4",
                valorFiltro = "D"
            };

            _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();
            //B051000
            var actividaVetada = _catalogo.listaCatalogoDetalle.catalogoDetalle.Find(c => c.strCodigoHost.Trim() == respuesta[0].codigoCIUU.Trim());

            if (actividaVetada != null)
            {
                throw new GestionExpedienteException("Cliente no contiene actividad economica en estado activo ", "Cliente no contiene actividad economica en estado activo", 9);
            }


            return solicitudResponse;
        }

        public async Task<ActualizarExpedienteResponse> ActualizarExpediente(ActualizarExpedienteRequest request)
        {
            CrearExpedienteResponse responseExpedienteCab = new CrearExpedienteResponse();
            ActualizarExpedienteResponse response = new ActualizarExpedienteResponse();
            #region Modificar solicitud
            bool continuar = true;
            Transaccion errorTransaccion = new Transaccion();
            /******* ConsultarCatalogoFiltrado**************/
            ConsultarCatalogoRequestMicroServ catalogorequest = new ConsultarCatalogoRequestMicroServ();
            catalogorequest.opcion = 4;
            catalogorequest.idCatalogo = "195";
            catalogorequest.idCatalogoPadre = "0";
            catalogorequest.Filtro = "strValor";
            catalogorequest.valorFiltro = request.idTipoAmortizacion;
            catalogorequest.valorFiltro =
            (catalogorequest.valorFiltro.Equals("fija") ? "francesa" :
                        (catalogorequest.valorFiltro.Equals("variable") ? "alemana" :
                        throw new ActualizarExpedienteException("valorFiltro no existe", "valorFiltro no existe", 2)
                        ));


            ConsultarCatalogoResponse catalogoresponse = await _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(catalogorequest);

            Producto productoDetalle = await _expedienteRepository.ObtenerProducto(request.idProducto);


            ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
            solicitudModiRequest.Producto = request.idProducto;
            solicitudModiRequest.IdSolicitud = request.idSolicitud;
            solicitudModiRequest.IdExpediente = request.idExpediente;
            solicitudModiRequest.MontoSolicitado = request.montoFinanciar;
            solicitudModiRequest.PlazoSolicitado = request.plazo;
            solicitudModiRequest.idTipoTabla = request.idProducto == "alVencimiento" ? null : catalogoresponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString(); // catalogoresponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            solicitudModiRequest.TasaProducto = request.tasaInteresProducto;
            solicitudModiRequest.ValorDividendo = request.cuota;
            solicitudModiRequest.DiaPago = request.diaPago;
            solicitudModiRequest.idTipoPeriodicidad = productoDetalle.tipoPeriodicidad;
            solicitudModiRequest.SubProductoCore = productoDetalle.subProductoCore;
            solicitudModiRequest.GenerarException = false;
            ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);
            if (solicitudResponse.DataTransaccion.codigoResponse != 0)
            {
                continuar = false;
                errorTransaccion = solicitudResponse.DataTransaccion;
                errorTransaccion.MessageResponse = "Error al modificar la solicitud del cliente";
            }

            request.bancoCtaCreditoDebito = request.Ctctipo != null ? _configuration["GeneralConfig:bancoCtaCreditoDebito"] : null;

            #endregion
            if (continuar)
            {
                /********* datos del oficial **********/
                ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
                oficialRequest.identificacion = request.identificacion;
                string strUsuario = string.Empty, idOpidUsuario = string.Empty;

                    request.usuarioEtapa = request.UsuarioGestor;
                    request.strUsuario = request.UsuarioGestor;
                    idOpidUsuario = request.OpidGestor;
                    strUsuario = request.UsuarioGestor;
                /********************************/

                /**** DatosCliente *****************/
                ConsultaPersonaResponse personaResponse = await _informacionClienteRepository.InformacionClientePersona(request.identificacion);

                /////////
                ///
                IngresoExpedienteRequest expedienteRequest = new IngresoExpedienteRequest()
                {
                    identificacion = request.identificacion,
                    idPersona = personaResponse.DatosPersona.idPersonaNeo,
                    idCatOficina = "94",
                    idOficina = request.idAgenciaOficial.ToString(),
                    idFormulario = "0",
                    idModulo = "298",
                    usuarioGestor = strUsuario,
                    usuarioEtapa = strUsuario,
                    opidGestor = idOpidUsuario,
                    strUsuario = strUsuario,
                    descriptionProducto = request.idProducto,
                    idExpediente = request.idExpediente.ToString()
                };
                responseExpedienteCab = await _expedienteRepository.IngresoExpediente(expedienteRequest);

                ///
                if (personaResponse.DatosPersona.idCatPaisTrabajo != "0" && personaResponse.DatosPersona.idCatProvinciaTrabajo != "0" &&
                    personaResponse.DatosPersona.idCatCuidadTrabajo != "0" && personaResponse.DatosPersona.idParroquiaTrabajo != "0" &&
                        personaResponse.DatosPersona.idDireTrabajo != "0")
                {
                    request.idPaisDestinoFondos = personaResponse.DatosPersona.idCatPaisTrabajo;
                    request.idProvinciaDestinoFondos = personaResponse.DatosPersona.idCatProvinciaTrabajo;
                    request.idCiudadDestinoFondos = personaResponse.DatosPersona.idCatCuidadTrabajo;
                    request.idParroquiaDestinoFondos = personaResponse.DatosPersona.idParroquiaTrabajo;
                    request.idDireccion = personaResponse.DatosPersona.idDireTrabajo;
                }
                else
                {
                    request.idPaisDestinoFondos = personaResponse.DatosPersona.idCatPaisDomicilio;
                    request.idProvinciaDestinoFondos = personaResponse.DatosPersona.idCatProvinciaDomicilio;
                    request.idCiudadDestinoFondos = personaResponse.DatosPersona.idCatCuidadDomicilio;
                    request.idParroquiaDestinoFondos = personaResponse.DatosPersona.idParroquiaDomicilio;
                    request.idDireccion = personaResponse.DatosPersona.idDireDomicilio;
                }

                /////////

                request.idPersona = personaResponse.DatosPersona.idPersonaNeo;
                /******* fin DatosCliente**************/

                /******* idModulo y idFormulario**************/
                request.idModulo = _configuration["GeneralConfig:idModulo"];
                request.idFormulario = _configuration["GeneralConfig:idFormulario"];
                /******* fin **************/


                request.idTipoAmortizacion = catalogoresponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
                /******* fin **************/


                if (request.tieneCuenta != null && request.tieneCuenta == "0")
                {
                    request.nombreEnTarjeta = personaResponse.DatosPersona.primerNombre + " " + personaResponse.DatosPersona.primerApellido;
                    request.nombreEnLibretaChequera = personaResponse.DatosPersona.nombreCompleto;
                    request.TipoCuentaDebito = "4593";
                    request.Ctctipo = "4593";

                    #region consulta tarjeta virtual Banca Control 
                    BancaControlRequest _bancaControlresquest = new BancaControlRequest();
                    _bancaControlresquest.usuario = request.identificacion;
                    _bancaControlresquest.usuariowindows = request.usuarioEtapa;
                    _bancaControlresquest.opid = idOpidUsuario;
                    BancaControlResponse _bancaControlresponse = await _informacionClienteRestRepository.ConsultarEstadoTarjetaVirtual(_bancaControlresquest, false);
                    if (_bancaControlresponse.CodigoRetorno == 0)
                    {
                        request.requiereBancontrol = _bancaControlresponse.requiereTarjetaBancontrol;
                    }
                    #endregion
                }

                ModificarExpedienteRequest expedienteRequestModificar = new ModificarExpedienteRequest();

                expedienteRequestModificar.Identificacion = request.identificacion;
                expedienteRequestModificar.idProducto = request.idProducto;
                expedienteRequestModificar.idExpediente = request.idExpediente.ToString();
                expedienteRequestModificar.usuarioModifica = strUsuario;
                expedienteRequestModificar.strUsuario = strUsuario;
                expedienteRequestModificar.idModulo = "0";
                expedienteRequestModificar.idFormulario = "0";
                expedienteRequestModificar.idEtapa = request.tieneCuenta != null ? "11605": "3114"; // Personalizacion
                expedienteRequestModificar.idEstado = request.tieneFirmaElectronica == true ? "43072" : "2846";

                await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);

                response = await _expedienteRepository.ActualizarExpediente(request);


            }
            return response;
        }
        public async Task<ParametrosGeneralesResponse> ParametrosGenerales(ParametrosGeneralesRequest request)
        {
            ParametrosGeneralesResponse response = new();
            response = await _expedienteRepository.ParametrosGenerales(request);
            return response;
        }

        public async Task<ModificarSolicitudResponse> ModificarSolicitud(ModificarSolicitudRequest request)
        {
            return await _expedienteRepository.ModificarSolicitud(request);
        }

        public async Task<RetomarSolicitudResponse> RetomarSolicitud(RetomarSolicitudRequest request)
        {
            ConsultaGuidPersonaResponse consultaGUID = new();
            RetomarSolicitudResponse response = new RetomarSolicitudResponse();
            response.codigoRetorno = "0";
            response.mensaje = "OK";
            request.opcion = "12";
            VerificaSolicitudesResponseMicro responseMicro = await _expedienteRepository.RetomarSolicitud(request);
            consultaGUID = await _expedienteRepository.ConsultaGuidPersona(request.numIdentificacion);
            response.guid_persona = consultaGUID.guid_persona;
            if (responseMicro.ContratarProducto.puedeContratarProducto.ToUpper() == "TRUE")
            {
                if (responseMicro.SolicitudesProcesoContratacion.Any())
                {
                    Producto productoDetalle = await _expedienteRepository.ObtenerProducto(responseMicro.DtosProducto.nombreProducto);

                    Flujo expediente = new Flujo();
                    SolicitudesProcesoContratacion solicitudProceso = new SolicitudesProcesoContratacion();
                    solicitudProceso = responseMicro.SolicitudesProcesoContratacion[0];

                    response.nombreProducto = responseMicro.DtosProducto.nombreProducto;
                    response.idProducto = responseMicro.DtosProducto.idProducto;
                    ValidaFirmaElectronicaRequest firmaRequest = new();
                    ValidaFirmaElectronicaResponse firmaResponse = new();

                    firmaRequest.identificacion = request.numIdentificacion;
                    firmaResponse = await _expedienteRepository.ValidaCreaSolicitudesProducto(firmaRequest);

                    expediente = responseMicro.DtosProducto.flujo.FirstOrDefault(x => x.idEtapa == solicitudProceso.IdEtapa && x.idEstado == solicitudProceso.IdEstado);

                    if (expediente != null)
                    {
                        #region Validar Biometria Bloqueado
                        ValidaBiometriaRequest biometriaRequest = new ValidaBiometriaRequest();
                        biometriaRequest.Producto = responseMicro.DtosProducto.nombreProducto;
                        biometriaRequest.Identificacion = request.numIdentificacion;
                        ValidaBiometriaResponse responseBiometria = await _biometriaRestRepository.ValidaBiometria(biometriaRequest);
                        #endregion

                        #region escenario 1 Cliente tiene expediente en proceso de contratacion
                        response.tieneSolicitudesEnProceso = true;
                        response.idExpediente = solicitudProceso.IdExpediente;
                        response.idSolicitud = solicitudProceso.IdSolicitud;
                        response.usuario = solicitudProceso.UsuarioGestor;
                        response.opidUsuario = solicitudProceso.OpidGestor;
                        response.redireccionar = solicitudProceso.Pantalla;
                        response.autenticacion = expediente.autenticacion;
                        response.montoSolicitado = solicitudProceso.MontoCupoSolicitado;

                        #endregion

                    }
                    else
                    {
                        #region Escenario 2 Cliente tiene expediente en proceso de contratacion - Expediente en Analisis de Crédito
                        //NO APLICA
                        //if (responseMicro.DtosProducto.etapasProceso.Contains(solicitudProceso.IdEtapa)) { throw new RetomarSolicitudException("Cliente en proceso", "Cliente en etapa de analisis del crédito", 3); }
                        #endregion

                        #region Escenario 3 Cliente tiene expediente en proceso de contratacion - Expediente está en proceso de Revision legal, documental y liquidación
                        if (responseMicro.DtosProducto.etapasLiquidacion.Contains(solicitudProceso.IdEtapa)) { throw new RetomarSolicitudException("Cliente liquidado", "Cliente en etapa de Liquidación", 4); }
                        #endregion

                        #region Escenario 4 -TEMPORAL- Cliente tiene expediente en proceso de contratacion - Expediente está en etapa personalizar o formalización
                        if (responseMicro.DtosProducto.etapasAprobado.Contains(solicitudProceso.IdEtapa))
                        {
                            response.tieneSolicitudesEnOficina = false;
                            response.idExpediente = solicitudProceso.IdExpediente;
                            response.montoSolicitado = solicitudProceso.MontoCupoSolicitado;
                            response.idSolicitud = solicitudProceso.IdSolicitud;
                            response.redireccionar = solicitudProceso.Pantalla;
                            if (solicitudProceso.IdProductoPadre == "56")
                            {                                
                                if (solicitudProceso.IdEtapa == "11605")
                                    response.redireccionar = "acreditacion-oficina";
                                if (solicitudProceso.IdEtapa == "3118")
                                    response.tieneSolicitudesEnOficina = true;
                            }
                            DateTime fechaUno = Convert.ToDateTime(solicitudProceso.FechaExpediente);
                            DateTime fechaDos = DateTime.Now;
                            TimeSpan difFechas = fechaDos - fechaUno;
                            int dias = difFechas.Days;
                            int diasMax = Convert.ToInt32(productoDetalle.diasVigencia);
                            dias = diasMax - dias;
                            response.diasVigencia = dias.ToString();
                            //if (solicitudProceso.IdProductoPadre == "57")
                            //{
                            //    response.tieneSolicitudesEnOficina = false;
                            //    response.tieneSolicitudesEnProceso = true;
                            //}
                            
                            //throw new RetomarSolicitudException("Cliente debe ir agencia", "Cliente debe ir a firmar sus documentos", 5); 
                        }
                        if ((solicitudProceso.IdEtapa.Equals("5602") && (solicitudProceso.IdEstado.Equals("5604") || solicitudProceso.IdEstado.Equals("5606") || solicitudProceso.IdEstado.Equals("5605"))) || (solicitudProceso.IdEtapa.Equals("3118") && solicitudProceso.IdEstado.Equals("6368")))
                        {
                            response.tieneSolicitudesEnOficina = true;

                        }

                        #endregion
                    }
                    if (firmaResponse.codError == "00")
                    {
                        if (firmaResponse.excedeMaximoPermitido == true)
                        {
                            throw new RetomarSolicitudException("Firma electrónica no disponible", "Superaste la cantidad de intentos para firmar en línea tus documentos", 7);
                        }
                    }
                    else
                    {
                        throw new RetomarSolicitudException("Error en consulta", "Error al validar certificados no utilizados para firma electrónica", 7);
                    }

                }
                else { response.autenticacion = "OTP"; }
            }
            else { throw new RetomarSolicitudException("Cliente no puede contratar producto", "debe ir a una agencia", 400); }

            if (request.ClienteTokenizado && (response.montoSolicitado == "0" || response.montoSolicitado == "0.00" || String.IsNullOrEmpty(response.montoSolicitado)))
            {
                response.idProducto = "";
                response.nombreProducto = "";
            }

            return response;
        }

        public async Task<CerrarExpedienteResponse> CerrarExpediente(CerrarExpedienteRequest request)
        {
            CerrarExpedienteResponse cerrarExpedienteResponse = new();
            ModificarExpedienteRequest expedienteRequestModificar = new ModificarExpedienteRequest();
            Transaccion modificacionResponse = new Transaccion();
            ExcepcionAnalisisResponse excepcionAnalisisResponse = new ExcepcionAnalisisResponse();
            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
            oficialRequest.identificacion = request.Identificacion;
                expedienteRequestModificar.strUsuario = request.UsuarioGestor;
          



            if (request.Opcion == 1)
            {
                expedienteRequestModificar.Identificacion = request.Identificacion;
                expedienteRequestModificar.idProducto = request.Producto;
                expedienteRequestModificar.idExpediente = request.IdExpediente;
                expedienteRequestModificar.idModulo = "0";
                expedienteRequestModificar.idFormulario = "0";
                expedienteRequestModificar.idEtapa = "3114";
                expedienteRequestModificar.idEstado = "2851";
                expedienteRequestModificar.idCatEstado = "100";
                expedienteRequestModificar.idDictamen = "294";
                expedienteRequestModificar.motivoDictamen = "Cliente No Cumple Perfil";
                expedienteRequestModificar.observaciones = "Cliente no aplica a precalificador - declaraciones de impuesto a la renta sólo presentan ingresos y gastos";
                modificacionResponse = await _expedienteRepository.ModificarExpediente(expedienteRequestModificar);


                PreCalificadorRequest preCalificadorRequest = new PreCalificadorRequest()
                {
                    idCliente = request.IdCliente,
                    idProceso = request.IdProceso,
                    usuario = string.Format(_configuration["GeneralConfig:usuarioWeb"])
                };

                PreCalificadorResponse _responseInformeFinalSBS = await _preCalificadorRestRepository.InformeFinalSBS(preCalificadorRequest);

                //ExcepcionAnalisisRequest requestAnalisis = new ExcepcionAnalisisRequest()
                //{
                //    Opcion = string.Format(_configuration["GeneralConfig:OpcionExcepcionAnalisis"]),
                //    IdClienteRating = request.IdCliente,
                //    IdProceso = request.IdProceso,
                //    Comentario = string.Format(_configuration["GeneralConfig:ComentarioExcepcionAnalisis"]),
                //    Usuario = string.Format(_configuration["GeneralConfig:usuarioWeb"]) //usuario_web
                //};
                //excepcionAnalisisResponse = await _analisisRatingRestRepository.GenerarExcepcionAnalisis(requestAnalisis);
            }
            else
            {
                PreCalificadorRequest preCalificadorRequest = new PreCalificadorRequest()
                {
                    idCliente = request.IdCliente,
                    idProceso = request.IdProceso,
                    usuario = string.Format(_configuration["GeneralConfig:usuarioWeb"])
                };

                PreCalificadorResponse _responseInformeFinalSBS = await _preCalificadorRestRepository.InformeFinalSBS(preCalificadorRequest);

            }

           if (modificacionResponse.codigoResponse == 0)
            {
                cerrarExpedienteResponse.CodigoRetorno = modificacionResponse.codigoResponse;
                cerrarExpedienteResponse.Mensaje = modificacionResponse.MessageResponse;
            }

            return cerrarExpedienteResponse;
        }
        public async Task<ActualizarSolicitudPantallaResponse> ActualizarSolicitudPantalla(ActualizarSolicitudPantallaRequest request)
        {
            ActualizarSolicitudPantallaResponse response = new();
            SolicitudRequest solicitudRequest = new SolicitudRequest();
            solicitudRequest.IdSolicitud = request.IdSolicitud;
            solicitudRequest.Identificacion = request.Identificacion;
            Solicitud solitudResponse = await _expedienteRepository.ConsultarSolicitud(solicitudRequest);

            ConsultarCatalogoResponse _catalogo = new();
            ConsultarCatalogoRequestMicroServ requestCatalogo = new ConsultarCatalogoRequestMicroServ()
            {
                opcion = 4,
                idCatalogo = "299",
                idCatalogoPadre = "0",
                Filtro = "strValor2",
                valorFiltro = request.UrlVista
            };

            _catalogo = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();
            var idEstadoSolicitud = "";

            if (_catalogo.listaCatalogoDetalle != null)
            {
                idEstadoSolicitud = _catalogo.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            }
            else
            {
                throw new GestionExpedienteException(_catalogo.Mensaje, _catalogo.Mensaje, 3);
            }

            ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
            solicitudModiRequest.Producto = request.Producto;
            solicitudModiRequest.IdSolicitud = request.IdSolicitud;
            solicitudModiRequest.idEstadoActualSolicitud = idEstadoSolicitud;
            solicitudModiRequest.idTipoTabla = solitudResponse.DataSolicitud.IdTipoTabla;
            ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);

            response.Mensaje = solicitudResponse.Mensaje;
            response.CodigoRetorno = solicitudResponse.CodigoRetorno;

            return response;
        }
        public async Task<ConsultaGuidPersonaResponse> ConsultaGuidPersona(string identificacion)
        {
            return await _expedienteRepository.ConsultaGuidPersona(identificacion);
        }

        public async Task<ValidaFirmaElectronicaResponse> ValidaCreaSolicitudesProducto(ValidaFirmaElectronicaRequest request)
        {
            return await _expedienteRepository.ValidaCreaSolicitudesProducto(request);
        }

        public async Task<ConsultarExpProductoActResponse> consultaExpedientesId(ConsultarExpProductoActRequest request)
        {
            return await _expedienteRepository.consultaExpedientesId(request);
        }
        public async Task<EncolarExpProcesoNeoBatchResponse> EncolarProcesoNeoBatch(EncolarExpProcesoNeoBatchRequest request)
        {
            return await _expedienteRepository.EncolarProcesoNeoBatch(request);
        }
        public async Task<ActualizaSolicitudResponse> ActualizaSolicitud(ActualizarSolicitudRequest request)
        {
            ActualizaSolicitudResponse response = new();
            bool continuar = true;
            Transaccion errorTransaccion = new Transaccion();
            /******* ConsultarCatalogoFiltrado**************/
            ConsultarCatalogoRequestMicroServ catalogorequest = new ConsultarCatalogoRequestMicroServ();
            catalogorequest.opcion = 4;
            catalogorequest.idCatalogo = "195";
            catalogorequest.idCatalogoPadre = "0";
            catalogorequest.Filtro = "strValor";
            catalogorequest.valorFiltro = request.idTipoAmortizacion;
            catalogorequest.valorFiltro =
            (catalogorequest.valorFiltro.Equals("fija") ? "francesa" :
                        (catalogorequest.valorFiltro.Equals("variable") ? "alemana" :
                        throw new ActualizarExpedienteException("valorFiltro no existe", "valorFiltro no existe", 2)
                        ));


            ConsultarCatalogoResponse catalogoresponse = await _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(catalogorequest);

            Producto productoDetalle = await _expedienteRepository.ObtenerProducto(request.idProducto);


            ModificarSolicitudRequest ModiReqSol = new ModificarSolicitudRequest();
            ModiReqSol.Producto = request.idProducto;
            ModiReqSol.IdSolicitud = request.idSolicitud;
            ModiReqSol.IdExpediente = request.idExpediente;
            ModiReqSol.MontoSolicitado = request.montoFinanciar;
            ModiReqSol.PlazoSolicitado = request.plazo;
            ModiReqSol.idTipoTabla = request.idProducto == "alVencimiento" ? null : catalogoresponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString(); // catalogoresponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            ModiReqSol.TasaProducto = request.tasaInteresProducto;
            ModiReqSol.ValorDividendo = request.cuota;
            ModiReqSol.DiaPago = request.diaPago;
            ModiReqSol.idTipoPeriodicidad = productoDetalle.tipoPeriodicidad;
            ModiReqSol.SubProductoCore = productoDetalle.subProductoCore;
            ModiReqSol.GenerarException = false;

            ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(ModiReqSol);
            if (solicitudResponse.DataTransaccion.codigoResponse != 0)
            {
                continuar = false;
                errorTransaccion = solicitudResponse.DataTransaccion;
                errorTransaccion.MessageResponse = "Error al modificar la solicitud del cliente";
            }
            if (!continuar) { throw new GestionExpedienteException(errorTransaccion.MessageResponse, errorTransaccion.DescriptionResponse, errorTransaccion.codigoResponse); }

            response.Mensaje = solicitudResponse.Mensaje;
            response.CodigoRetorno = solicitudResponse.CodigoRetorno;   

            return response;
        }
    }
}