namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class GestionExpedienteException : BaseCustomException
    {

        public GestionExpedienteException(string message = "Gestion Expediente Exception", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }

    }
}
