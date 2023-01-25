namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class ConsultarCatalogoException : BaseCustomException
    {
        public ConsultarCatalogoException(string message = "Consulta Catalogo Exception", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }
    }
}
