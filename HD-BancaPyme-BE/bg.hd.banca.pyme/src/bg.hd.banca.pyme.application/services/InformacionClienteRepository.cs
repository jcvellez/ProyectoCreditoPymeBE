using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.persona;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.domain.entities.catalogo;
using bg.hd.banca.pyme.domain.entities.expediente;
using System.Globalization;
using bg.hd.banca.pyme.domain.entities.BancaControl;
using bg.hd.banca.pyme.domain.entities.config;
using Newtonsoft.Json;
using bg.hd.banca.pyme.domain.entities.crmCasos;

namespace bg.hd.banca.pyme.application.services
{

    public class InformacionClienteRepository : IInformacionClienteRepository
    {

        private readonly IInformacionClienteRestRepository _informacionClienteRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IConsultarCatalogoRestRepository _consultarCatalogoRestRepository;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;

        public InformacionClienteRepository(IConfiguration Configuration, IInformacionClienteRestRepository informacionClienteRestRepository,
            IConsultarCatalogoRestRepository consultarCatalogoRestRepository, IGestionExpedienteRestRepository crearExpedienteRepository
        )
        {
            _configuration = Configuration;
            _informacionClienteRestRepository = informacionClienteRestRepository;
            _consultarCatalogoRestRepository = consultarCatalogoRestRepository;
            _expedienteRepository = crearExpedienteRepository;



        }

        public async Task<InformacionClienteResponse> InformacionCliente(string identificacion)
        {
            if (identificacion.Length < 10 || identificacion.Length > 10)
            {
                throw new InformacionClienteExeption("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }

            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;
            InformacionClienteDniResponse informacionClienteDniResponse = null;
            ConsultaPersonaResponse consultaDatosPersona = null;
            InformacionClienteResponse informacionClienteResponse = new();

            IdentificaNombres obj = new();

            informacionClienteDniResponse = await _informacionClienteRestRepository.InformacionCliente(identificacion,"");
            identificaUsuarioDataResponse = await _informacionClienteRestRepository.InformacionClienteData(identificacion);
            consultaDatosPersona = await _informacionClienteRestRepository.InformacionClientePersona(identificacion);

            obj = await _informacionClienteRestRepository.ObtenerNombres(informacionClienteDniResponse.nombres);

            informacionClienteResponse.telefonoCelular = identificaUsuarioDataResponse.telefonoCelular;
            informacionClienteResponse.correoElectronico = identificaUsuarioDataResponse.correoElectronico;
            informacionClienteResponse.fechaNacimiento = identificaUsuarioDataResponse.fechaNacimiento;
            //// Datos Quemados
            informacionClienteResponse.provincia = consultaDatosPersona.DatosPersona.provinciaDomicilioDesc;
            informacionClienteResponse.ciudad = consultaDatosPersona.DatosPersona.ciudadDomicilioDesc;
            informacionClienteResponse.direccion = consultaDatosPersona.DatosPersona.callePrincipalDomicilio;
            informacionClienteResponse.referencia = consultaDatosPersona.DatosPersona.referencia;
            ///
            informacionClienteResponse.Identificacion = informacionClienteDniResponse.identificacion;
            informacionClienteResponse.nombres = informacionClienteDniResponse.nombres;
            informacionClienteResponse.nacionalidad = informacionClienteDniResponse.nacionalidad;
            informacionClienteResponse.estadoCivil = informacionClienteDniResponse.estadoCivil;
            ///
            informacionClienteResponse.primerNombre = obj.primerNombre;
            informacionClienteResponse.segundoNombre = obj.segundoNombre;
            informacionClienteResponse.primerApellido = obj.primerApellido;
            informacionClienteResponse.SegundoApellido = obj.SegundoApellido;

            ///guid persona           
            return informacionClienteResponse;
        }

        public async Task<InformacionClienteDniResponse> ConsultarDatosRC(string identificacion, string producto)
        {
            InformacionClienteDniResponse _response = new InformacionClienteDniResponse();
            ConsultarCatalogoResponse _catalogoResponse = new ConsultarCatalogoResponse();
            string catalogo = string.Empty;


            _response = await _informacionClienteRestRepository.InformacionCliente(identificacion,"");

            catalogo = _configuration["GeneralConfig:catalogos:estadoCivil"];
            _catalogoResponse = await ConsultarCatalogoFiltrado(catalogo, "strValor4", _response.estadoCivil, producto);

            if (_catalogoResponse.listaCatalogoDetalle.catalogoDetalle.Count() > 0)
            {
                _response.idEstadoCivil = _catalogoResponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            }

            //Para obtener código de nacionalidad - Catálogo 135
            catalogo = _configuration["GeneralConfig:catalogos:nacionalidad"];
            _catalogoResponse = await ConsultarCatalogoFiltrado(catalogo, "strValor4", _response.nacionalidad, producto);
            if (_catalogoResponse.listaCatalogoDetalle.catalogoDetalle.Count() > 0)
            {
                _response.idNacionalidad = _catalogoResponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            }

            //Para obtener código de género - Catálogo 31
            catalogo = _configuration["GeneralConfig:catalogos:genero"];
            _catalogoResponse = await ConsultarCatalogoFiltrado(catalogo, "strValor4", _response.genero, producto);
            if (_catalogoResponse.listaCatalogoDetalle.catalogoDetalle.Count() > 0)
            {
                _response.idGenero = _catalogoResponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            }

            //regimen matrimonial
            if (_response.idEstadoCivil == "2452" || _response.idEstadoCivil == "2456") { _response.idRegimenMatrimonial = "2159"; }


            return _response;
        }

        private async Task<ConsultarCatalogoResponse> ConsultarCatalogoFiltrado(string idCatalogo, string filtro, string valorFiltro, string producto)
        {
            ConsultarCatalogoResponse _response = new();
            ConsultarCatalogoRequest _request = new();

            _request.producto = producto;
            _request.opcion = 4;
            _request.idCatalogo = idCatalogo;
            _request.idCatalogoPadre = "0";
            _request.Filtro = filtro;
            _request.valorFiltro = valorFiltro;

            _response = await _consultarCatalogoRestRepository.ConsultarCatalogo(_request);


            return _response;

        }


        ///////////////////////////ConsultarDatosNegocio
        public async Task<ConsultaDatosNegocioResponse> ConsultarDatosNegocio(string identificacion)
        {
            //GrabarDatosPersonaResponse GrabarPersona = new GrabarDatosPersonaResponse();
            ConsultaDatosNegocioResponse response = new();
            ConsultaDatosRUCResponse _dataSri = new ConsultaDatosRUCResponse();
            ConsultaInstalacionesSegurosResponse segurosResponse = new();
            //Datos de SRI

            segurosResponse = await _informacionClienteRestRepository.ConsultaInstalacionSeguros(identificacion);

            _dataSri = await _informacionClienteRestRepository.ConsultaDatosRUC(identificacion);
            ConsultaPersonaResponse consultaDatosPersona = null;
            //Datos de la persona
            consultaDatosPersona = await _informacionClienteRestRepository.InformacionClientePersona(identificacion);
            //lista de actividades comerciales
            var respuesta = _dataSri.data.Where(elem => elem.estadoContribuyente.Trim().Equals("ACTIVO"))
                                        .Select(elem => elem).ToList();
            //llamado a Catalogo Filtrado
            ConsultarCatalogoResponse catalogoResponse = new();
            ConsultarCatalogoRequestMicroServ requestCatalogo = new ConsultarCatalogoRequestMicroServ()
            {
                opcion = 4,
                idCatalogo = "257",
                idCatalogoPadre = "0",
                Filtro = "strCodigoHost",
                valorFiltro = respuesta[0].codigoCIUU
            };

            catalogoResponse = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();

            string CodigoNeoCiuu = "";

            if (catalogoResponse.listaCatalogoDetalle != null)
            {
                CodigoNeoCiuu = catalogoResponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString();
            }
            else
            {
                if (respuesta[0].codigoCIUUSB == null) throw new GeneralException("Cliente no contiene actividad economica en neo ", "Cliente no contiene actividad economica en neo", 9);
                requestCatalogo.valorFiltro = respuesta[0].codigoCIUUSB.ToString();
                catalogoResponse = _consultarCatalogoRestRepository.ConsultarCatalogoFiltrado(requestCatalogo).GetAwaiter().GetResult();
                CodigoNeoCiuu = catalogoResponse.listaCatalogoDetalle.catalogoDetalle[0].idCodigo.ToString(); ;
            }
            //------------------------------
            CultureInfo culture = new CultureInfo("en-US");
            string format = "dd/MM/yyyy";
            DateTime result;
            result = DateTime.ParseExact(consultaDatosPersona.DatosPersona.fechaNacimiento, format, culture);
            var fechaNacimiento = result.ToString("yyyy/MM/dd");

            response.telefono = consultaDatosPersona.DatosPersona.telefonoTrabajo;
            response.idProvincia = consultaDatosPersona.DatosPersona.idCatProvinciaTrabajo;
            response.Provincia = consultaDatosPersona.DatosPersona.provinciaTrabajoDesc;
            response.idCiudad = consultaDatosPersona.DatosPersona.idCatCuidadTrabajo;
            response.Ciudad = consultaDatosPersona.DatosPersona.ciudadTrabajoDesc;
            response.Direccion = consultaDatosPersona.DatosPersona.dirTrabajo;
            response.Referencia = consultaDatosPersona.DatosPersona.referenciaTrabajo;
            response.idPersona = consultaDatosPersona.DatosPersona.idPersonaNeo;
            response.actividadEconomica = respuesta[0].actividadEconomica;
            response.codigoCIUU = respuesta[0].codigoCIUU;
            response.idCodigoNeoCIUU = CodigoNeoCiuu;
            response.idParroquia = consultaDatosPersona.DatosPersona.idParroquiaTrabajo;
            response.Parroquia = consultaDatosPersona.DatosPersona.parroquiaTrabajoDesc;

            response.inmueble_propio = segurosResponse.propio;
            response.compania_aseguradora = segurosResponse.companiaAseguradora;
            response.NumeroEmpleados = segurosResponse.NumeroEmpleados;

            if (response.compania_aseguradora != null && response.compania_aseguradora != "0")
            {
                response.tieneSeguro = true;
            }


            if (response.CodigoRetorno == 0)
            {
                return response;
            }
            else { throw new RetomarSolicitudException("Error al consultar", "Error al consultar", 400); }
        }

        ///////////////////////////GuardarDatosNegocio 
        public async Task<GuardarDatosNegocioResponse> GuardarDatosNegocio(GuardarDatosNegocioRequest request)
        {
            GrabarDatosPersonaResponse GrabarPersona = new GrabarDatosPersonaResponse();
            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = null;
            ConsultaPersonaResponse consultaDatosPersona = null;
            GrabaInstalacionesSegurosResponse grabasegurosResponse = new();
            GrabaInstalacionesSegurosRequest objgraba = new GrabaInstalacionesSegurosRequest();
            objgraba.Identificacion = request.identificacion;
            objgraba.Propio = request.inmueble_propio;
            objgraba.CompaniaAseguradora = request.compania_aseguradora;
            objgraba.NumeroEmpleados = request.num_empleados;

            grabasegurosResponse = await _informacionClienteRestRepository.GrabaInstalacionSeguros(objgraba);

            identificaUsuarioDataResponse = await _informacionClienteRestRepository.InformacionClienteData(request.identificacion);
            consultaDatosPersona = await _informacionClienteRestRepository.InformacionClientePersona(request.identificacion);

            GrabarDatosPersonaRequest dataPersonaGrabar = new GrabarDatosPersonaRequest()
            {
                identificacion = request.identificacion,
                idActividadCiiu = request.idCodigoNeoCIUU
            };

            dataPersonaGrabar.direccionDomicilio.celular = identificaUsuarioDataResponse.telefonoCelular;
            dataPersonaGrabar.direccionDomicilio.correoElectronico = identificaUsuarioDataResponse.correoElectronico;
            dataPersonaGrabar.direccionDomicilio.telDomicilio = consultaDatosPersona.DatosPersona.telDomicilio;

            dataPersonaGrabar.direccionTrabajo.telefono = request.telefono;
            dataPersonaGrabar.direccionTrabajo.provincia = request.idProvincia;
            dataPersonaGrabar.direccionTrabajo.ciudad = request.idCiudad;
            dataPersonaGrabar.direccionTrabajo.callePrincipal = request.direccion;
            dataPersonaGrabar.direccionTrabajo.referenciaDireccion = request.Referencia;
            dataPersonaGrabar.direccionTrabajo.parroquia = Convert.ToString(request.idParroquia);

            GrabarPersona = await _informacionClienteRestRepository.GrabarDatosPersona(dataPersonaGrabar);

            GuardarDatosNegocioResponse obj = new();
            obj.CodigoRetorno = GrabarPersona.CodigoRetorno;
            obj.Mensaje = GrabarPersona.Mensaje;
            obj.idPersona = GrabarPersona.idPersona;

            if (!String.IsNullOrEmpty(request.tieneActDirNegocio))
            {
                ModificarSolicitudRequest solicitudModiRequest = new ModificarSolicitudRequest();
                solicitudModiRequest.Producto = request.Producto;
                solicitudModiRequest.IdSolicitud = request.idSolicitud;
                solicitudModiRequest.tieneActDirNegocio = request.tieneActDirNegocio;
                ModificarSolicitudResponse solicitudResponse = await _expedienteRepository.ModificarSolicitud(solicitudModiRequest);
            }

            if (obj.CodigoRetorno != 0)
            {
                throw new RetomarSolicitudException("Error al grabar datos", "Error al grabar datos", 400);
            }
            return obj;
        }

        public async Task<IngresarDetalleVentasResponse> IngresarDetalleVentas(IngresarDetalleVentasRequest request)
        {
            return await _informacionClienteRestRepository.IngresarDetalleVentas(request);
        }
        public async Task<ConsultarDetalleVentasResponse> ConsultarDetalleVentas(string identificacion)
        {
            return await _informacionClienteRestRepository.ConsultarDetalleVentas(identificacion);
        }

        public async Task<GrabaClientesProveedoresResponse> GrabaProveedorCliente(GrabaClientesProveedoresRequest request)
        {
            return await _informacionClienteRestRepository.GrabaProveedorCliente(request);
        }

        public async Task<ConsultaClientesProveedoresResponse> ConsultarClientesProveedores(string identificacion, int tipoClienteProveedor)
        {
            return await _informacionClienteRestRepository.ConsultarClientesProveedores(identificacion, tipoClienteProveedor);
        }

        public async Task<GrabaReferenciaBancariaResponse> GrabaReferenciasBancarias(GrabaReferenciaBancariaRequest request)
        {
            return await _informacionClienteRestRepository.GrabaReferenciasBancarias(request);
        }

        public async Task<ConsultaReferenciaBancariaResponse> ConsultaReferenciasBancarias(string identificacion)
        {
            return await _informacionClienteRestRepository.ConsultaReferenciasBancarias(identificacion);
        }

        public async Task<ConsultarCuentaPorIdResponse> ConsultarCuentaPorId(string identificacion)
        {
            return await _informacionClienteRestRepository.ConsultarCuentaPorId(identificacion);
        }

        public async Task<ConsultarDetalleCertificadosResponse> ConsultarCertificacionesAmbientales(string identificacion)
        {
            return await _informacionClienteRestRepository.ConsultarCertificacionesAmbientales(identificacion);
        }
        public async Task<GrabaCertificacionAmbientalResponse> GrabaCertificacionesAmbientales(GrabaCertificacionAmbientalRequest request)
        {
            return await _informacionClienteRestRepository.GrabaCertificacionesAmbientales(request);
        }

        public async Task<ConsultaInstalacionesSegurosResponse> ConsultaInstalacionSeguros(string identificacion)
        {
            return await _informacionClienteRestRepository.ConsultaInstalacionSeguros(identificacion);
        }

        public async Task<GrabaInstalacionesSegurosResponse> GrabaInstalacionSeguros(GrabaInstalacionesSegurosRequest request)
        {
            return await _informacionClienteRestRepository.GrabaInstalacionSeguros(request);
        }
        public async Task<BancaControlResponse> ConsultarEstadoTarjetaVirtual(BancaControlRequest request, bool controlarException)
        {
            return await _informacionClienteRestRepository.ConsultarEstadoTarjetaVirtual(request, controlarException);

        }

        public async Task<GestionarDatosNormativosReponse> GestionarDatosNormativos(GestionarDatosNormativosRequest request)
        {
            bool error = true;
            GestionarDatosNormativosReponse reponse = new();
            ActualizaInformacionNormativaRequest normativoRequest = new ActualizaInformacionNormativaRequest();
            normativoRequest.GenerarException = false;
            normativoRequest.idExpediente = request.idExpediente;
            normativoRequest.identificacionCliente = request.identificacionCliente;
            normativoRequest.usuario = string.Format(_configuration["GeneralConfig:usuarioWeb"]);
            normativoRequest.opcion = "2";
            normativoRequest.opcionActualizar = "2";
            normativoRequest.preguntaPaisNacimientoDiferenteEcuador = request.preguntaPaisNacimientoDiferenteEcuador;
            normativoRequest.paisNacimiento = request.preguntaPaisNacimientoDiferenteEcuador != "N" ? request.paisNacimiento : "";
            normativoRequest.preguntaTienesRucActivo = "S";
            normativoRequest.numeroRUC = request.identificacionCliente + "001";

            Transaccion normativoResponse = await _informacionClienteRestRepository.GrabarDatosNormativos(normativoRequest);
            if (normativoResponse.codigoResponse != 0)
            {
                error = true;
                reponse.CodigoRetorno = normativoResponse.codigoResponse;
                reponse.Mensaje = "Error al modificar los datos normativos";
            }
            else
            {
                error = false;
                GestionResidenciaFiscalRequest requestResidenciaFiscal = new GestionResidenciaFiscalRequest();
                requestResidenciaFiscal.controlarExcepcion = false;
                requestResidenciaFiscal.opcion = "";
                requestResidenciaFiscal.producto = request.producto;
                requestResidenciaFiscal.numIdentificacion = request.identificacionCliente;
                requestResidenciaFiscal.usuario = request.usuario;
                requestResidenciaFiscal.tieneResidenciaFiscalExterior = request.tieneResidenciaFiscalExterior;
                requestResidenciaFiscal.ResidenciaFiscalExterior.residenciaFiscal = request.residenciaFiscalExterior;
                GestionResidenciaFiscalResponse responseResidenciaFiscal = await _informacionClienteRestRepository.GestionarResidenciaFiscal(requestResidenciaFiscal);

                reponse.CodigoRetorno = normativoResponse.codigoResponse;
                reponse.Mensaje = normativoResponse.MessageResponse;

                if (responseResidenciaFiscal.CodigoRetorno != 0)
                {
                    error = true;
                    reponse.CodigoRetorno = normativoResponse.codigoResponse;
                    reponse.Mensaje = "Error al modificar los datos de Residencia Fiscal";
                }

                ActualizaRFCrmRequest requestResidenciaFiscalCrm = new();
                ConsultarCatalogoRequest requestCatalogo = new();
                ConsultarCatalogoResponse responseCatalogo = new();
                ActualizaRFCrmResponse responseResidenciaFiscalCrm = new();

                requestResidenciaFiscalCrm.tipoIdentificacion = _configuration["GeneralConfig:tipoIdentificacionDescripcion"];
                requestResidenciaFiscalCrm.identificacion = request.identificacionCliente;
                requestResidenciaFiscalCrm.canal = _configuration["GeneralConfig:usuario"];
                requestResidenciaFiscalCrm.usuario = request.usuario;
                List<DireccionFiscal> Direcciones = new List<DireccionFiscal>();
                DireccionFiscal Direccion = new DireccionFiscal();
                foreach (var item in request.residenciaFiscalExterior)
                {
                    Direccion.OtroPais = "1";
                    requestCatalogo.opcion = 5;
                    requestCatalogo.idCatalogo = "53";
                    requestCatalogo.producto = request.producto;
                    requestCatalogo.idCatalogoPadre = "0";
                    requestCatalogo.valorFiltro = item.codigoPaisR;
                    Direccion.Direccion = item.DireccionR;
                    Direccion.NumeroNIF = item.nifR;
                    responseCatalogo = await _consultarCatalogoRestRepository.ConsultarCatalogo(requestCatalogo);
                    Direccion.Pais = responseCatalogo.listaCatalogoDetalle.catalogoDetalle[0].strCodigoHost;
                    Direcciones.Add(Direccion);
                }
                if (Direcciones.Count > 0)
                {
                    requestResidenciaFiscalCrm.direcciones.direccionFiscal = Direcciones;
                    requestResidenciaFiscalCrm.controlExcepcion = true;
                    responseResidenciaFiscalCrm = await _informacionClienteRestRepository.ActualizaResidenciaFiscalCrm(requestResidenciaFiscalCrm);
                }

            }

            if (error)
            {
                throw new GeneralException("Error Aplicativo", reponse.Mensaje, reponse.CodigoRetorno);
            }

            return reponse;
        }

        public async Task<ConsultaResidenciaFiscalResponse> ConsultaResidenciaFiscal(ConsultaResidenciaFiscalRequest request)
        {
            return await _informacionClienteRestRepository.ConsultaResidenciaFiscal(request);
        }

        public async Task<GestionValidarPoliticasResponse> GestionValidarPoliticas(GestionValidarPoliticasRequest request)
        {
            GestionValidarPoliticasResponse response = new();
            ValidarPoliticasRequest validarPoliticasRequest = new ValidarPoliticasRequest();
            Expediente dataExpediente = new();
            bool saveConyugue = false;
            bool error = false;
            ConsultarOficialRequest oficialRequest = new ConsultarOficialRequest();
            oficialRequest.identificacion = request.identificacion;
            dataExpediente.IdOficina = int.Parse(request.idAgenciaOficial);

            dataExpediente.UsuarioGestor = request.UsuarioGestor;
            dataExpediente.OpidGestor = request.OpidGestor;


            ConsultaPersonaResponse personaResponse = await _informacionClienteRestRepository.InformacionClientePersona(request.identificacion);
            dataExpediente.DatosCliente = personaResponse.DatosPersona;
            dataExpediente.DatosConyuge = personaResponse.DatosConyugue;

            validarPoliticasRequest.TipoIdentificacionTitular = dataExpediente.DatosCliente.tipoIdentificacion;
            validarPoliticasRequest.IdentificacionTitular = request.identificacion;
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
            validarPoliticasRequest.IdProducto = request.producto;
            validarPoliticasRequest.IdExpediente = request.idExpediente;
            validarPoliticasRequest.Usuario = dataExpediente.UsuarioGestor;
            validarPoliticasRequest.PlazoSolicitado = request.PlazoSolicitado;
            validarPoliticasRequest.PeriodicidadSolicitado = _configuration["GeneralConfig:catalogos:nacionalidad"];
            validarPoliticasRequest.MontoSolicitado = request.MontoSolicitado;
            validarPoliticasRequest.GenerarException = false;

            InformacionClienteDniResponse _datosRCTitular = await ConsultarDatosRC(request.identificacion, request.producto);

            if (!string.IsNullOrEmpty(_datosRCTitular.identificacionConyuge))
            {
                Int64 ValAux = 0;
                if (Int64.TryParse(_datosRCTitular.identificacionConyuge, out ValAux))
                {
                    if (ValAux != 0)
                    {
                        saveConyugue = true;
                        InformacionClienteDniResponse _datosRConyugue = await ConsultarDatosRC(_datosRCTitular.identificacionConyuge, request.producto);
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

            ValidarPoliticasResponse validarPoliticasResponse = await _expedienteRepository.ValidarPoliticas(validarPoliticasRequest);


            if (validarPoliticasResponse.idDictamen != "292")
            {

                throw new GeneralException(validarPoliticasResponse.dictamen, validarPoliticasResponse.motivoDictamen, 2);

            }

            response.Codigo = validarPoliticasResponse.DataTransaccion.codigoResponse;
            response.Mensaje = validarPoliticasResponse.DataTransaccion.MessageResponse;

            if (validarPoliticasResponse.DataTransaccion.codigoResponse != 0)
            {
                error = true;
                response.Codigo = validarPoliticasResponse.DataTransaccion.codigoResponse;
                response.Mensaje = "Error al validar politica";
            }

            if (error)
            {
                throw new GeneralException("Error Aplicativo", response.Mensaje, response.Codigo);
            }

            return response;
        }

        public async Task<List<ConsultaContratoPorCanalResponse>> ConsultaContratoPorCanal(string identificacion, string canal)
        {
            return await _informacionClienteRestRepository.ConsultaContratoPorCanal(identificacion,canal);
        }

        public async Task<RegistroContratosResponse> RegistroContratos(RegistroContratosRequest request)
        {
            return await _informacionClienteRestRepository.RegistroContratos(request);
        }
    }
}