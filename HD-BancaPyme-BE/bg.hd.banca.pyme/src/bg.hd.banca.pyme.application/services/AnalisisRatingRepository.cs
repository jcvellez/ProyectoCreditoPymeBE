using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.ClientesRatingNeo;
using bg.hd.banca.pyme.domain.entities.persona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.pyme.application.services
{
    public class AnalisisRatingRepository : IAnalisisRatingRepository
    {
        private readonly IAnalisisRatingRestRepository _clientesratingneoRestRepository;
        private readonly IInformacionClienteRestRepository _informacionClienteRestRepository;

        public AnalisisRatingRepository(IAnalisisRatingRestRepository _clientesratingneoRestRepository, IInformacionClienteRestRepository informacionClienteRestRepository)
        {
            this._clientesratingneoRestRepository = _clientesratingneoRestRepository;
            this._informacionClienteRestRepository= informacionClienteRestRepository;
        }
        public async Task<ClientesRatingNeoResponse> CrearAnalisis(ClientesRatingNeoRequest request)
        {
            IdentificaUsuarioDataResponse identificaUsuarioDataResponse = new IdentificaUsuarioDataResponse();
            ClientesRatingNeoResponse  analisis = new ClientesRatingNeoResponse();
            AnyosBalanceResponse anyosBalanceResponse = new AnyosBalanceResponse();

            identificaUsuarioDataResponse = await _informacionClienteRestRepository.InformacionClienteData(request.identificacion);
            request.direccion = identificaUsuarioDataResponse.direccion1;
            request.nombreCliente = identificaUsuarioDataResponse.nombres;

            analisis = await _clientesratingneoRestRepository.CrearAnalisis(request);

            AnyosWebRequest anyosBalance = new AnyosWebRequest();
            anyosBalance.identificacion = request.identificacion;
            anyosBalance.idProceso = analisis.IdProcesoRating;
            anyosBalance.fechaRevision = analisis.fechaRevision;

            anyosBalanceResponse = await _clientesratingneoRestRepository.ConsultarAnyosBalance(anyosBalance);
            analisis.Informar = anyosBalanceResponse.Informar;
            analisis.anyos= anyosBalanceResponse.anyos;

            return analisis;
            
        }
    }
}
