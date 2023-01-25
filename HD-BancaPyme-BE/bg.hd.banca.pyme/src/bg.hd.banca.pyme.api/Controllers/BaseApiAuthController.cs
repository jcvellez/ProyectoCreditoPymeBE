using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace bg.hd.banca.pyme.api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseApiAuthController: ControllerBase
    {
    }
}
