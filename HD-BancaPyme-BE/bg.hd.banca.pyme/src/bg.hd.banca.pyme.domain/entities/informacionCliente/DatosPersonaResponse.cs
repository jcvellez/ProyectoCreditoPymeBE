using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.informacionCliente
{
    public class DatosPersonaResponse

    {

        public string tipoIdentificacion { get; set; }

        public string idCatDetTipoIdentificacion { get; set; }

        public string identificacion { get; set; }

        public string nombreCompleto { get; set; }

        public string primerNombre { get; set; }

        public string segundoNombre { get; set; }

        public string primerApellido { get; set; }

        public string segundoApellido { get; set; }

        public string estudios { get; set; }

        public string idCatCorrespondencia { get; set; }

        public string idCatDetCorrespondencia { get; set; }

        public string genero { get; set; }

        public string descGenero { get; set; }

        public string generoHost { get; set; }

        public string tiempoUnionLibre { get; set; }

        public string nacionalidad { get; set; }

        public string idCatDetNacionalidad { get; set; }

        public string estadoCivil { get; set; }

        public string CodigoClte { get; set; }

        public string fechaNacimiento { get; set; }

        public string Profesion { get; set; }

        public string profesionDesc { get; set; }

        public string idCatDetProfesion { get; set; }

        public string idCatPaisDomicilio { get; set; }

        public string idCatProvinciaDomicilio { get; set; }

        public string provinciaDomicilioDesc { get; set; }
        public string provinciaTrabajoDesc { get; set; }
        
        public string idCatCuidadDomicilio { get; set; }

        public string ciudadDomicilioDesc { get; set; }

        public string sectorDomicilio { get; set; }

        public string callePrincipalDomicilio { get; set; }

        public string calleInterseccionDomicilio { get; set; }

        public string manzanaDomicilio { get; set; }

        public string villaSolarDomicilio { get; set; }

        public string celular { get; set; }

        public string telefonoDomicilio { get; set; }

        public string idCatPaisTrabajo { get; set; }

        public string idCatProvinciaTrabajo { get; set; }

        public string idCatCuidadTrabajo { get; set; }

        public string sectorTrabajo { get; set; }

        public string telefonoTrabajo { get; set; }

        public string telefonoTrabajo2 { get; set; }

        public string emailTrabajo { get; set; }

        public string callePrincipalTrabajo { get; set; }

        public string calleInterseccionTrabajo { get; set; }

        public string manzanaTrabajo { get; set; }

        public string villaSolarTrabajo { get; set; }

        public string numCasaLocal { get; set; }

        public string tipoBanca { get; set; }

        public string tipoPersona { get; set; }

        public string actividadEconmica { get; set; }

        public string idCatDetActividadEconomica { get; set; }

        public string responsable { get; set; }

        public string separacionBienes { get; set; }

        public string separacionBienesHost { get; set; }

        public string regimenMatrimonialHost { get; set; }

        public string dependientes { get; set; }

        public string antiguedad { get; set; }

        public string cargoActual { get; set; }

        public string nombreEmpresa { get; set; }

        public string sueldoMensual { get; set; }

        public string ingresoMensual { get; set; }

        public string costoVenta { get; set; }

        public string otrosIngresos { get; set; }

        public string origenIngreso { get; set; }

        public string gastoFamiliar { get; set; }

        public string arriendoCreditoc { get; set; }

        public string otrosGastos { get; set; }

        public string ventasMensuales { get; set; }

        public string dirDomicilio { get; set; }

        public string telDomicilio { get; set; }

        public string telopcDomicilio { get; set; }

        public string celular1 { get; set; }

        public string emailDomicilio { get; set; }

        public string ciudadDomicilio { get; set; }

        public string tipoVivienda { get; set; }

        public string nombreArrendador { get; set; }

        public string ciudadNacCodigo { get; set; }

        public string ciudadNacimiento { get; set; }

        public string profesion1 { get; set; }

        public string profesionCod { get; set; }

        public string tiempoResidencia { get; set; }

        public string actividadCodigo { get; set; }

        public string actividad { get; set; }

        public string actividadDesc { get; set; }

        public string dirTrabajo { get; set; }

        public string ciudadTrabajo { get; set; }

        public string idDireDomicilio { get; set; }

        public string DireDomiHost { get; set; }

        public string idDireTrabajo { get; set; }

        public string DireTrabHost { get; set; }

        public string RucPersona { get; set; }

        public string nombreArrendador1 { get; set; }

        public string telefonoArrendador { get; set; }

        public string valorArriendo { get; set; }

        public string otrosIngresos1 { get; set; }

        public string ventasMensuales1 { get; set; }

        public string segmentoRiesgoDesc { get; set; }

        public string segmentoRiesgoCore { get; set; }

        public string sinergia { get; set; }

        public string idCatDetSituacionLaboral { get; set; }

        public string relacionDependenciaNeo { get; set; }

        public string relacionDependenciaCRM { get; set; }

        public string idCatDetEstadoCivil { get; set; }

        public string idCatDetOrigenIngresos { get; set; }

        public string idCatDetTipoVivienda { get; set; }

        public string tipoIdentificacionDesc { get; set; }

        public string FechaIngresoCLTE { get; set; }

        public string antiguedadCliente { get; set; }

        public string idCatDetTipoBanca { get; set; }

        public string idCatDetSegmentoEstrategico { get; set; }

        public string idTipoBancaNeo { get; set; }

        public string tipoBancaHost { get; set; }

        public string idSegmentoEstrategico { get; set; }

        public string segmentoEstrategicoClienteHost { get; set; }

        public string fechaVentas { get; set; }

        public string ventasAnuales { get; set; }

        public string idPersonaNeo { get; set; }

        public string regimenMatrimonial { get; set; }

        public string edadPersona { get; set; }

        public string codPersona { get; set; }

        public string referencia { get; set; }

        public string direccionTrabajo { get; set; }

        public string envioEstadoCuenta { get; set; }

        public string idTipoPersona { get; set; }

        public string edad { get; set; }

        public string idParroquiaDomicilio { get; set; }

        public string idParroquiaTrabajo { get; set; }

        public string correspondencia { get; set; }

        public string actividadCIIU { get; set; }

        public string idactividadCIIU { get; set; }

        public string idPaisResidencia { get; set; }

        public string codigoTelefonicoPais { get; set; }

        public string telefonoEnExterior { get; set; }

        public string ciudadTrabajoDesc { get; set; } = "";
        public string referenciaTrabajo { get; set; } = "";
        public string parroquiaTrabajoDesc { get; set; } = "";
    }
}