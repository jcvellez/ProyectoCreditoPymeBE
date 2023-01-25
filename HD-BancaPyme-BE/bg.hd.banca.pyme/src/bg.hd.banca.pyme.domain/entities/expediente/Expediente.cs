using bg.hd.banca.pyme.domain.entities.informacionCliente;
using bg.hd.banca.pyme.domain.entities.persona;

namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class Expediente
    {

       public int? IdExpediente{ get; set; }
       public int? IdSolicitud{ get; set; }
       public int? IdProductoPadre{ get; set; }
       public int? IdProducto{ get; set; }
       public int? IdCanal{ get; set; }
       public int? IdOficina{ get; set; } 
       public int? IdEtapa{ get; set; }
       public int? IdEstado{ get; set; }
       public int? IdDictamen{ get; set; }
       public string? Identificador{ get; set; }
       public string? UsuarioGestor{ get; set; }
       public string? OpidGestor{ get; set; }
       public string? UsuarioEtapa{ get; set; }       
       public string? Aplicacion{ get; set; }
       public string? MotivoDictamen{ get; set; }
       public string? StrIp{ get; set; }
       public string? NumeroCuenta{ get; set; }
       public string? FechaAceptacionContrato{ get; set; }
       public DatosPersonaResponse? DatosCliente{ get; set; }
       public DatosPersonaResponse? DatosConyuge{ get; set; }
       public Activos? DatosActivos{ get; set; }
        
    }

    


}
