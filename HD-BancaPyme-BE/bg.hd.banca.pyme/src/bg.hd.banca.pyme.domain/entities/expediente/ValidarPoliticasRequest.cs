using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class ValidarPoliticasRequest
    {
        string _TipoIdentificacionTitular = string.Empty;
        string _IdentificacionTitular = string.Empty;
        string _NombresCompletoTitular = string.Empty;
        string _NombreTitular = string.Empty;
        string _ApellidoTitular = string.Empty;
        string _FechaNacimientoTitular = string.Empty;
        string _IngresosTitular = string.Empty;
        string _IdNacionalidadTitular = string.Empty;
        string _IdEstadoCivilTitular = string.Empty;
        string _IdRegimenMatrimonialTitular = string.Empty;
        string _IdRelacionDependenciaTitular = string.Empty;
        string _AntiguedadTitular = string.Empty;
        string _TipoIdentificacionAdicional = string.Empty;
        string _IdentificacionAdicional = string.Empty;
        string _NombresCompletoAdicional = string.Empty;
        string _NombreAdicional = string.Empty;
        string _ApellidoAdicional = string.Empty;
        string _FechaNacimientoAdicional = string.Empty;
        string _IdNacionalidadAdicional = string.Empty;
        string _IdRegimenMatrimonialAdicional = string.Empty;
        string _IdProducto = string.Empty;
        string _IdExpediente = string.Empty;
        string _Usuario = string.Empty;
        string _PlazoSolicitado = string.Empty;
        string _PeriodicidadSolicitado = string.Empty;
        string _MontoSolicitado = string.Empty;
        string _EdadTitular = string.Empty;
        string _EdadAdicional = string.Empty;
        bool _generarException = true;

        public string TipoIdentificacionTitular { get => _TipoIdentificacionTitular; set => _TipoIdentificacionTitular = value; }
        public string IdentificacionTitular { get => _IdentificacionTitular; set => _IdentificacionTitular = value; }
        public string NombresCompletoTitular { get => _NombresCompletoTitular; set => _NombresCompletoTitular = value; }
        public string NombreTitular { get => _NombreTitular; set => _NombreTitular = value; }
        public string ApellidoTitular { get => _ApellidoTitular; set => _ApellidoTitular = value; }
        public string FechaNacimientoTitular { get => _FechaNacimientoTitular; set => _FechaNacimientoTitular = value; }
        public string IngresosTitular { get => _IngresosTitular; set => _IngresosTitular = value; }
        public string IdNacionalidadTitular { get => _IdNacionalidadTitular; set => _IdNacionalidadTitular = value; }
        public string IdEstadoCivilTitular { get => _IdEstadoCivilTitular; set => _IdEstadoCivilTitular = value; }
        public string IdRegimenMatrimonialTitular { get => _IdRegimenMatrimonialTitular; set => _IdRegimenMatrimonialTitular = value; }
        public string IdRelacionDependenciaTitular { get => _IdRelacionDependenciaTitular; set => _IdRelacionDependenciaTitular = value; }
        public string AntiguedadTitular { get => _AntiguedadTitular; set => _AntiguedadTitular = value; }
        public string TipoIdentificacionAdicional { get => _TipoIdentificacionAdicional; set => _TipoIdentificacionAdicional = value; }
        public string IdentificacionAdicional { get => _IdentificacionAdicional; set => _IdentificacionAdicional = value; }
        public string NombresCompletoAdicional { get => _NombresCompletoAdicional; set => _NombresCompletoAdicional = value; }
        public string NombreAdicional { get => _NombreAdicional; set => _NombreAdicional = value; }
        public string ApellidoAdicional { get => _ApellidoAdicional; set => _ApellidoAdicional = value; }
        public string FechaNacimientoAdicional { get => _FechaNacimientoAdicional; set => _FechaNacimientoAdicional = value; }
        public string IdNacionalidadAdicional { get => _IdNacionalidadAdicional; set => _IdNacionalidadAdicional = value; }
        public string IdRegimenMatrimonialAdicional { get => _IdRegimenMatrimonialAdicional; set => _IdRegimenMatrimonialAdicional = value; }
        public string IdProducto { get => _IdProducto; set => _IdProducto = value; }
        public string IdExpediente { get => _IdExpediente; set => _IdExpediente = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public string PlazoSolicitado { get => _PlazoSolicitado; set => _PlazoSolicitado = value; }
        public string PeriodicidadSolicitado { get => _PeriodicidadSolicitado; set => _PeriodicidadSolicitado = value; }
        public string MontoSolicitado { get => _MontoSolicitado; set => _MontoSolicitado = value; }
        public string EdadTitular { get => _EdadTitular; set => _EdadTitular = value; }
        public string EdadAdicional { get => _EdadAdicional; set => _EdadAdicional = value; }
        public bool GenerarException { get => _generarException; set => _generarException = value; }

    }
}
