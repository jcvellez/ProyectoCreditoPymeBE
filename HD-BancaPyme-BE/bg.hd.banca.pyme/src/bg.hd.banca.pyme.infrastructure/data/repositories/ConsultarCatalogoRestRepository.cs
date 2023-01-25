using AutoMapper;
using bg.hd.banca.pyme.infrastructure.utils;
using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.models.dtos;
using bg.hd.banca.pyme.application.models.exeptions;
using bg.hd.banca.pyme.domain.entities;
using bg.hd.banca.pyme.domain.entities.catalogo;
using bg.hd.banca.pyme.infrastructure.utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using bg.hd.banca.pyme.application.interfaces.services;

namespace bg.hd.banca.pyme.infrastructure.data.repositories
{
    internal class ConsultarCatalogoRestRepository : IConsultarCatalogoRestRepository
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationServiceRepository _authentication;

        public ConsultarCatalogoRestRepository(IConfiguration Configuration, IMapper Mapper, IAuthenticationServiceRepository Authentication)
        {
            _configuration = Configuration;
            _mapper = Mapper;
            _authentication = Authentication;
        }

        public async Task<ConsultarCatalogoResponse> ConsultarCatalogo(ConsultarCatalogoRequest request)
        {            

            ConsultarCatalogoResponse consultarCatalogoResponse = new ConsultarCatalogoResponse();

            Producto producto = PrimitiveDataUtils.obtenerProducto(request.producto, _configuration);

            if (String.IsNullOrEmpty(producto.idProducto))
            {
                throw new ConsultarCatalogoException("Producto ingresado no es correcto", "Producto ingresado no es correcto", 4);
            }


            int[] opcionesValidas = producto.opcionesValidas;
            string[] catalogosPermitidos = producto.catalogosPermitidos;
            if (opcionesValidas.Contains(request.opcion))
            {
                if (catalogosPermitidos.Contains(request.idCatalogo))
                {
                    ConsultarCatalogoRequestMicroServ requestMicro = new ConsultarCatalogoRequestMicroServ()
                    {
                        opcion = request.opcion,
                        idCatalogo = request.idCatalogo,
                        idCatalogoPadre = request.idCatalogoPadre,
                        Filtro = request.Filtro,
                        valorFiltro = request.valorFiltro
                    };


                    string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/catalogos/catalogo/detalle";
                    string auth = string.Format(_configuration["AzureAd:tokenName"]);
                    HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), requestMicro);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MsDtoResponse<ConsultarCatalogoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ConsultarCatalogoResponse>>(responseBody, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.None
                    });
                    if (request.idCatalogo == "57")
                    {
                        List<CatalogoDetalle> nuevalista = new List<CatalogoDetalle>();
                        List<CatalogoDetalle> lista = responseJson.data.listaCatalogoDetalle.catalogoDetalle;
                        foreach (var item in lista)
                        {
                            if (item.strCodigoHost != "TC")
                            {
                                nuevalista.Add(item);
                            }

                        }
                        responseJson.data.listaCatalogoDetalle.catalogoDetalle = nuevalista;
                    }
                    else if (request.idCatalogo == "308" || request.idCatalogo == "306")
                    {
                        responseJson.data.listaCatalogoDetalle.catalogoDetalle = responseJson.data.listaCatalogoDetalle.catalogoDetalle.OrderBy(x => x.idCodigo).ToList();
                    }
                    
                    consultarCatalogoResponse = responseJson.data;

                    PrimitiveDataUtils.saveLogsInformation(uri, "", request, consultarCatalogoResponse);

                    return consultarCatalogoResponse;
                      
                }
                else
                {
                    throw new ConsultarCatalogoException("Catálogo no permitido para consulta", "Catálogo no permitido para consulta", 3);
                }

            }
            else
            {
                throw new ConsultarCatalogoException("Opción de consulta no es correcta", "Opción de consulta no es correcta", 2);
            }
        }
        public async Task<ConsultarCatalogoResponse> ConsultarCatalogoFiltrado(ConsultarCatalogoRequestMicroServ request)
        {

            ConsultarCatalogoResponse consultarCatalogoResponse = new ConsultarCatalogoResponse();

            int[] opcionesValidas = { 4 };
            string[] catalogosPermitidos = { "257" , "195", "299", "83", "53" };

            if (!catalogosPermitidos.Contains(request.idCatalogo) & !opcionesValidas.Contains(request.opcion) & string.IsNullOrEmpty(request.Filtro) & string.IsNullOrEmpty(request.valorFiltro))
            {
                throw new ConsultarCatalogoException("Request no valido para consulta por Filtro", "Request no valido para consulta por Filtro", 3);

            }
            
            if (catalogosPermitidos.Contains(request.idCatalogo) & opcionesValidas.Contains(request.opcion))
            {
                string uri = _configuration["InfraConfig:MicroCompositeNeo:urlService"] + "v2/catalogos/catalogo/detalle";
                string auth = string.Format(_configuration["AzureAd:tokenName"]);
                HttpResponseMessage response = await HTTPRequest.PostAsync(uri, auth, await _authentication.GetAccessToken(), request);
                string responseBody = await response.Content.ReadAsStringAsync();
                MsDtoResponse<ConsultarCatalogoResponse> responseJson = JsonConvert.DeserializeObject<MsDtoResponse<ConsultarCatalogoResponse>>(responseBody, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                });
                consultarCatalogoResponse = responseJson.data;

                PrimitiveDataUtils.saveLogsInformation(uri, "", request, consultarCatalogoResponse);

                return consultarCatalogoResponse;
            }
            else
            {
                throw new ConsultarCatalogoException("Catálogo no permitido para consulta por Filtro", "Catálogo no permitido para consulta", 3);
            }

        }

    }
}
