using System.ComponentModel.DataAnnotations;

namespace bg.hd.banca.pyme.domain.entities.token.acceso
{
    public class ValidaTokenAccesoCanalApliRequest
    {
        [Required] public string CanalId { get; set; } = "";
        [Required] public string AplicacionId { get; set; } = "";
        [Required] public string TkValue { get; set; } = "";
    }
}
