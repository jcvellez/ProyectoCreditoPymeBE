using bg.hd.banca.pyme.domain.entities.email;

namespace bg.hd.banca.pyme.application.interfaces.services
{
    public interface IEmailValidacionRepository
    {
        Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request);
    }
}
