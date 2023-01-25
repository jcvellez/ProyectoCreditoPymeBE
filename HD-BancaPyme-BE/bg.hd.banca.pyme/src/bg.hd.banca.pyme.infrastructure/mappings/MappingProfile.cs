using AutoMapper;
using bg.hd.banca.pyme.domain.entities.otp;
using bg.hd.banca.pyme.domain.entities.SimularCredito;
using bg.hd.banca.pyme.domain.entities.SimularCreditoPublic;
using bg.hd.banca.pyme.infrastructure.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using bg.hd.banca.pyme.domain.entities.persona;
using bg.hd.banca.pyme.application.models.ms;

namespace bg.hd.banca.pyme.infrastructure.mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            CreateMap<XmlElement, OtpGenerarResponse>()

                .ForMember(dest =>
                dest.CodigoRetorno,
                opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "cretorno")))

                .ForMember(dest =>
                dest.Mensaje,
                opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "err")))


                 .ReverseMap();

            CreateMap<XmlElement, OtpValidarResponse>()
            

            .ForMember(dest =>
            dest.CodigoRetorno,
            opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "cretorno")))

            .ForMember(dest =>
            dest.Mensaje,
            opt => opt.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "err")))


             .ReverseMap();


            CreateMap<Task<SimularCreditoResponse>, SimularCreditoResponse>();

            CreateMap<XmlElement, SimularCreditoResponse>()
           .ForMember(dest =>
           dest.codigoRetorno,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "CodigoRetorno")))


           .ForMember(dest =>
           dest.mensaje,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "MensajeRetorno")))


           .ForMember(dest =>
           dest.cuota,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "dividendo")))


           .ForMember(dest =>
           dest.totalPagar,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "totalcredito")))


           .ForMember(dest =>
           dest.tasaInteres,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "tasaInteres")))
                  
           
           .ReverseMap();

            ////////////////////////
            CreateMap<XmlElement, SimularCreditoPublicResponse>()
           .ForMember(dest =>
           dest.codigoRetorno,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "CodigoRetorno")))


           .ForMember(dest =>
           dest.mensaje,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "MensajeRetorno")))


           .ForMember(dest =>
           dest.cuota,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "dividendo")))


           .ForMember(dest =>
           dest.totalPagar,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "totalcredito")))


           .ForMember(dest =>
           dest.tasaInteres,
           cre => cre.MapFrom(src => PrimitiveDataUtils.GetDataStringXmlNode(src, "tasaInteres")))


           .ReverseMap();

            CreateMap<MsClienteInformacion, IdentificaUsuarioResponse>()
              .ForMember(dest => dest.PrimerNombre, source => source.MapFrom(src => src.nombreCliente.TrimEnd()))
              .ForMember(dest => dest.CelularOfuscado, source => source.MapFrom(src => src.telefonoCelular.TrimEnd()))
              .ReverseMap();
        }

    }
}
