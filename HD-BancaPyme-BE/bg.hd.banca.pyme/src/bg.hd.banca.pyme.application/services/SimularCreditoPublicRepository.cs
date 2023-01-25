using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities.SimularCreditoPublic;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bg.hd.banca.pyme.domain.entities;

namespace bg.hd.banca.pyme.application.services
{
    public class SimularCreditoPublicRepository : ISimularCreditoPublicRepository
    {
        private readonly ISimularCreditoPublicRestRepository _simularcreditopublicRestRepository;
        private readonly IConfiguration _configuration;
        private readonly IGestionExpedienteRestRepository _expedienteRepository;

        public SimularCreditoPublicRepository(IConfiguration Configuration,ISimularCreditoPublicRestRepository _simularcreditopublicRestRepository, IGestionExpedienteRestRepository modificarExpedienteRepository)
        {
            this._simularcreditopublicRestRepository = _simularcreditopublicRestRepository;
            this._configuration = Configuration;
            _expedienteRepository = modificarExpedienteRepository;
        }
            

        public async Task<SimularCreditoPublicResponse> SimularCreditoPublic(SimularCreditoPublicRequest request)
        {
            SimularCreditoPublicResponse simularCredito = new SimularCreditoPublicResponse();
            ConsultaTasaInteresResponse tasaInteresResponse = new ConsultaTasaInteresResponse();
            #region validaMonto
            if (request.monto <= 0 || request.monto is null)
            {
                throw new SimularCreditoPublicException("monto es requerido", "monto es requerido", 2);
            }
            #endregion

            #region validaPlazo
            if (request.plazo <= 0 || request.plazo is null)
            {
                throw new SimularCreditoPublicException("plazo es requerido", "plazo es requerido", 2);
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
                simularCredito = await _simularcreditopublicRestRepository.SimularCreditoPublic_CuotaMensual(request);
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
                tasaInteresResponse = await _simularcreditopublicRestRepository.ConsultaTasaInteres(TasaInteresRequest);
                double tasa = (double) tasaInteresResponse.tasaNominal;
                simularCredito = await _simularcreditopublicRestRepository.SimularCreditoPublic_AlVencimiento(request, tasa);
            }
            
            
            return simularCredito;
        }
    }
}
