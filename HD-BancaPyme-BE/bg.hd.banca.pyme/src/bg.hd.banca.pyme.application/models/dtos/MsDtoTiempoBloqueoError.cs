namespace bg.hd.banca.pyme.application.models.dtos
{
    public class MsDtoTiempoBloqueoError
    {
        public int code { get; set; }
        public string ? traceid { get; set; }
        public string ? message { get; set; }
        public List<MsDtoErrorBiometria> ?errors { get; set; }
    }

    public class MsDtoErrorBiometriaPor
    {
        public int code { get; set; }
        public string? traceid { get; set; }
        public string? message { get; set; }
        public List<MsDtoErrorBiometriaPorcent>? errors { get; set; }
    }
    public class MsDtoErrorBiometriaIntento
    {
        public int code { get; set; }
        public string? traceid { get; set; }
        public string? message { get; set; }
        public List<MsDtoErrorBiometriaIntero>? errors { get; set; }
    }
}
