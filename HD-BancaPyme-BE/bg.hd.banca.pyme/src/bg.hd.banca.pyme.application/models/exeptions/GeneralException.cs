namespace bg.hd.banca.pyme.application.models.exeptions
{
    public class GeneralException : BaseCustomException
    {
        public GeneralException(string message = "General Exception", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }
    }
}
