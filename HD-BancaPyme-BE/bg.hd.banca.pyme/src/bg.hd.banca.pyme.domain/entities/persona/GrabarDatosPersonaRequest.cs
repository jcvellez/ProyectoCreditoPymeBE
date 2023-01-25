using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bg.hd.banca.pyme.domain.entities.persona
{
    public class GrabarDatosPersonaRequest
    {
        /// <summary>
        /// Identificacion
        /// </summary>
        /// <example>0999999999</example>
        [Required]
        [StringLength(10, ErrorMessage = "La identificación debe tener 10 caracteres", MinimumLength = 10)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "La identificación debe ser númerica")]
        public string identificacion { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        /// <example></example>
        public string nombre { get; set; }
        /// <summary>
        /// Estado Civil
        /// </summary>
        /// <example></example>
        public string estadoCivil { get; set; }
        /// <summary>
        /// Primer Nombre
        /// </summary>
        /// <example></example>
        public string primerNombre { get; set; }
        /// <summary>
        /// Segundo Nombre
        /// </summary>
        /// <example></example>
        public string segundoNombre { get; set; }
        /// <summary>
        /// Primer Apellido
        /// </summary>
        /// <example></example>
        public string primerApellido { get; set; }
        /// <summary>
        /// Segundo Apellido
        /// </summary>
        /// <example></example>
        public string segundoApellido { get; set;}
        /// <summary>
        /// Id Relación Dependendia
        /// </summary>
        /// <example></example>
        public string idRelacionDepenendecia { get; set; }
        /// <summary>
        /// Id Origen Ingresos
        /// </summary>
        /// <example></example>
        public string idOrigenIngresos { get; set; }
        /// <summary>
        /// id Actividad Económica
        /// </summary>
        /// <example></example>
        public string idActividadEconomica { get; set; }
        /// <summary>
        /// Id Actividad Ciiu
        /// </summary>
        /// <example></example>
        public string idActividadCiiu { get; set; }
        /// <summary>
        /// Tiempo Labora
        /// </summary>
        /// <example>3</example>
        public string tiempoLabora { get; set; }
        /// <summary>
        /// Ingreso
        /// </summary>
        /// <example>1000</example>
        public string ingreso { get; set; }
        /// <summary>
        /// Sueldo Mensual
        /// </summary>
        /// <example>1500</example>
        public string sueldoMensual { get; set; }
        /// <summary>
        /// Venta Mensual
        /// </summary>
        /// <example></example>
        public string ventaMensual { get; set; }
        /// <summary>
        /// Fecha Nacimiento
        /// </summary>
        /// <example></example>
        public string fechaNacimiento { get; set; }
        /// <summary>
        /// Nacionalidad
        /// </summary>
        /// <example></example>
        public string nacionalidad { get; set; }
        /// <summary>
        /// Id Genero
        /// </summary>
        /// <example></example>
        public string idGenero { get; set; }

        public string tipoPersona { get; set; }

        public string origenIngresos { get; set; }
        public string situacionLaboral { get; set; }
        /// <summary>
        /// Fecha de Venta CRM
        /// </summary>
        /// <example>24-09-2022</example>
        public string? fechaVenta { get; set; } = "";
        [JsonIgnoreAttribute] public Direccion? direccionDomicilio { get; set; }
        [JsonIgnoreAttribute] public DireccionTrabajo? direccionTrabajo { get; set; }

        public GrabarDatosPersonaRequest()
        {
            direccionDomicilio = new Direccion();
            direccionTrabajo = new DireccionTrabajo();
        }

        public class Direccion
        {
            //tipoDireccion            
            //public string? callePrincipal { get; set; }
            //public string? calleIntercepta { get; set; }            
            ////manzana
            ////villa            
            //public string? provincia { get; set; }            
            //public string? ciudad { get; set; }
            ////parroquia
            ////pais            
            //public string? telefono { get; set; }            
            //public string? referenciaDireccion { get; set; }
            //barrioSector
            public string? celular { get; set; }
            public string? correoElectronico { get; set; }
            [JsonIgnore] public string? telDomicilio { get; set; }            
            //idPaisResidencia
            //codigoTelefonicoPais
            //telefonoEnExterior
        }
        public class DireccionTrabajo
        {
                   
            public string? callePrincipal { get; set; }
            //public string? calleIntercepta { get; set; }
                    
            public string? provincia { get; set; }
            public string? ciudad { get; set; }
            
            public string? telefono { get; set; }
            public string? referenciaDireccion { get; set; }
            [JsonIgnore] public string? sectorTrabajo { get; set; }
            [JsonIgnore] public string? parroquia { get; set; }
            [JsonIgnore] public string? manzanaTrabajo { get; set; }
            [JsonIgnore] public string? calleInterseccionTrabajo { get; set; }            
        }
    }    
}