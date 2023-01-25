using static bg.hd.banca.pyme.domain.entities.persona.GrabarDatosPersonaRequest;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabarPersonaRequestMicroServ
    {
        public string numIdentificacion { get; set; }
        public string tipoIdentificacion { get; set; }
        public string nombre { get; set; }
        public string estadoCivil { get; set; }
        public string primerNombre { get; set; }
        public string segundoNombre { get; set; }
        public string primerApellido { get; set; }
        public string segundoApellido { get; set; }
        public string idRelacionDepenendecia { get; set; }
        public string idOrigenIngresos { get; set; }
        public string idActividadEconomica { get; set; }
        public string idActividadCiiu { get; set; }
        public string tiempoLabora { get; set; }
        public string ingreso { get; set; }
        public string sueldoMensual { get; set; }
        public string ventaMensual { get; set; } = "0";
        public string fechaNacimiento { get; set; }
        public string nacionalidad { get; set; }
        public string idGenero { get; set; }
        public string tipoPersona { get; set; }
        public string origenIngresos { get; set; }
        public string situacionLaboral { get; set; }
        public Direccion direccionDomicilio { get; set; }
        /// <summary>
        /// Fecha de Venta CRM
        /// </summary>
        /// <example>24-09-2022</example>
        public string? fechaVenta { get; set; } = "";
        public DireccionTrabajo direccionTrabajo { get; set; }

        public GrabarPersonaRequestMicroServ()
        {
            direccionDomicilio = new Direccion();
            direccionTrabajo = new DireccionTrabajo();
        }

        //public class DireccionTrabajo
        //{
        //    //tipoDireccion            
        //    public string? callePrincipal { get; set; }
        //    public string? calleIntercepta { get; set; }
        //    //manzana
        //    //villa            
        //    public string? provincia { get; set; }
        //    public string? ciudad { get; set; }
        //    //parroquia
        //    //pais            
        //    public string? telefono { get; set; }
        //    public string? referenciaDireccion { get; set; }
        //    //barrioSector
           
        //}



    }
}
