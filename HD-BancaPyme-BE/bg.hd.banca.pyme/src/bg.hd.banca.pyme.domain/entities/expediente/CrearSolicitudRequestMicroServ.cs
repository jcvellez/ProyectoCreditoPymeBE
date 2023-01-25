namespace bg.hd.banca.pyme.domain.entities.expediente
{
    public class CrearSolicitudRequestMicroServ
    {
        public string Opcion { get; set; } // Se envia 1 para crear solicitud 
        public string IdCanal { get; set; }
        public string IdProducto { get; set; }
        public string IdTipoIdentificacion { get; set; }
        public string NumIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string AutorizaConsultaBuro { get; set; }// Asumir con S
        public string CorreoElectronico { get; set; }
        public string IdSolicitud { get; set; }
        public string IdExpediente { get; set; }
    }
}
