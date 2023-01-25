using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using bg.hd.banca.pyme.domain.entities.expediente;
using Microsoft.Extensions.Configuration;
using bg.hd.banca.pyme.domain.entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.services
{
    public class SimularCreditoRepository : ISimularCreditoRepository
    {
        private readonly ISimularCreditoRestRepository _simularcreditoRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;
        public SimularCreditoRepository(IConfiguration Configuration, ISimularCreditoRestRepository _simularcreditoRestRepository, IGestionExpedienteRestRepository modificarExpedienteRepository)
        {
            this._simularcreditoRestRepository = _simularcreditoRestRepository;
            _configuration = Configuration;
            _expedienteRepository = modificarExpedienteRepository;
        }


        public async Task<SimularCreditoResponse> SimularCredito(SimularCreditoRequest request)
        {
            SimularCreditoResponse simularCredito = new SimularCreditoResponse();
            ConsultaTasaInteresResponse tasaInteresResponse = new ConsultaTasaInteresResponse();
            ModificarSolicitudRequestMicroServ dataSolicitud = new ModificarSolicitudRequestMicroServ();
            double tasaAnterior = 0; // dia de pago por default
            if (request.tipoCalculo == "R")
            {

                #region Consultar datos de la solicitud
                SolicitudRequest solicitudRequest = new SolicitudRequest();
                solicitudRequest.IdSolicitud = request.idSolicitud;
                solicitudRequest.Identificacion = request.cedula;
                Solicitud solitudResponse = await _expedienteRepository.ConsultarSolicitud(solicitudRequest);
                string _dataSolicitud = JsonConvert.SerializeObject(solitudResponse.DataSolicitud);
                dataSolicitud = JsonConvert.DeserializeObject<ModificarSolicitudRequestMicroServ>(_dataSolicitud);
                #endregion
                int monto = 0;
                int.TryParse(dataSolicitud.MontoSolicitado.Substring(0, dataSolicitud.MontoSolicitado.IndexOf(".")), out monto);
                request.monto = monto;
                int diaPago = 1; // dia de pago por default
                int.TryParse(dataSolicitud.DiaPago, out diaPago);
                
                request.diaPago = diaPago==0 ? 1 : diaPago;

                request.tipoCuota = dataSolicitud.IdTipoTabla == string.Format(_configuration["GeneralConfig:tablaAmortizacion:alemana"]) ? "variable" :
                                    dataSolicitud.IdTipoTabla == string.Format(_configuration["GeneralConfig:tablaAmortizacion:francesa"]) ? "fija" :
                                    "fija";// para elk vencimiento se debe seterar un tipo de tb amortizacion

                Producto productoDetalle = await _expedienteRepository.ObtenerIdProducto(dataSolicitud.IdProducto);
                request.tipoProducto = productoDetalle.nombreProducto;
                int plazoSolicitado = 0; // dia de pago por default
                int.TryParse(dataSolicitud.PlazoSolicitado, out plazoSolicitado);
                request.plazo = plazoSolicitado;
               
                double.TryParse(dataSolicitud.TasaProducto, out tasaAnterior);
                //request.tipoCuota = request.tipoCuota == null: "fija" ? request.tipoCuota ;
            }

            #region validaCampoIdentificacion
            Int64 ValAux = 0;
            if (Int64.TryParse(request.cedula, out ValAux) == false)
            {
                throw new SimularCreditoException("Identificación debe ser numérica ", "Identificación debe ser numérica", 2);
            }
            if (request.cedula.Length < 10 || request.cedula.Length > 10)
            {
                throw new SimularCreditoException("Identificación no tiene formato solicitado de 10 caracteres", "Identificación no tiene formato solicitado de 10 caracteres", 2);
            }
            #endregion

            #region validaMonto
           
            if (request.monto <= 0 || request.monto is null)
            {
                throw new SimularCreditoException("monto es requerido", "monto es requerido", 2);
            }
            #endregion

            #region validaPlazo
            if (request.plazo <= 0 || request.plazo is null)
            {
                throw new SimularCreditoException("plazo es requerido", "plazo es requerido", 2);
            }
            #endregion

            if (!request.tipoProducto.Equals("cuotaMensual") && !request.tipoProducto.Equals("alVencimiento"))
            {
                throw new SimularCreditoPublicException("El tipo de producto no existe", "El tipo de producto no existe", 2);
            }

            if (!request.tipoCuota.Equals("fija") && !request.tipoCuota.Equals("variable"))
            {
                throw new SimularCreditoPublicException("El tipo de cuota no existe", "El tipo de cuota no existe", 2);
            }

            if (request.diaPago < 1 || request.diaPago > 30)
            {
                throw new SimularCreditoPublicException("El dia de pago debe estar entre 1 y 30", "El dia de pago debe estar entre 1 y 30", 2);

            }
            

            if (request.tipoProducto.Equals("cuotaMensual"))
            {
                simularCredito = await _simularcreditoRestRepository.SimularCredito_CuotaMensual(request);
            }
            else if (request.tipoProducto.Equals("alVencimiento"))
            {
                Producto productoDetalle = await _expedienteRepository.ObtenerProducto(request.tipoProducto);

                ConsultaTasaInteresRequest TasaInteresRequest = new ConsultaTasaInteresRequest()
                {
                    idCanal = Convert.ToInt32(_configuration["GeneralConfig:canal"]),
                    idProducto = Convert.ToInt32(productoDetalle.idProducto),
                    PeriodicidadDias = Convert.ToInt32(_configuration["GeneralConfig:PeriodicidadDias"])
                };
                tasaInteresResponse = await _simularcreditoRestRepository.ConsultaTasaInteres(TasaInteresRequest);
                double tasa = (double)tasaInteresResponse.tasaNominal;


                simularCredito = await _simularcreditoRestRepository.SimularCredito_AlVencimiento(request, tasa);
            }

            simularCredito.tasaInteresSolicitada = tasaAnterior;
            simularCredito.diaPago = request.diaPago;
            simularCredito.tipoProducto = request.tipoProducto;
            simularCredito.tipoCuota = request.tipoCuota;
            simularCredito.monto = request.monto;


            return simularCredito;
            // return _simularcreditoRestRepository.SimularCredito_CuotaMensual(request, token);
        }
    }
}