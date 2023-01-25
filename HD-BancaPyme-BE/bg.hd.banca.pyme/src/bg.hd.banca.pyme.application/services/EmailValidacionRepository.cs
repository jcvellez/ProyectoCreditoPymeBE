using bg.hd.banca.pyme.application.interfaces.repositories;
using bg.hd.banca.pyme.application.interfaces.services;
using bg.hd.banca.pyme.domain.entities.email;
using bg.hd.banca.pyme.application.models.exeptions;


namespace bg.hd.banca.pyme.application.services
{
    public class EmailValidacionRepository: IEmailValidacionRepository
    {
        private readonly IEmailValidacionRestRepository _emailValidacionRestRepository;

        public EmailValidacionRepository(IEmailValidacionRestRepository emailValidacionRestRepository)
        {
            _emailValidacionRestRepository = emailValidacionRestRepository;
        }

        public async Task<EmailValidacionResponse> ValidarEmail(EmailValidacionRequest request)
        {
            if (request.identificacion is null)
            {
                throw new EmailValidacionExecption("Identificación no valida", "Identificación no valida", 2);
            }

            if (request.correo is null)
            {
                throw new EmailValidacionExecption("Correo no valido", "Correo no valido", 5);
            }

            return await _emailValidacionRestRepository.ValidarEmail(request);
        
            
        }
    }
}
