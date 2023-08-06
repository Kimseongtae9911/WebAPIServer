using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services;

namespace WebAPIServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IAccountDB m_accountDB;

        public LoginController(IAccountDB accountDB)
        {
            m_accountDB = accountDB;
        }

        [HttpPost]
        public async Task<PkLoginRes> Post(PkLoginReq request)
        {
            var response = new PkLoginRes();

            var errorCode = await m_accountDB.Login(request.ID, request.Password);
            if(errorCode != ErrorCode.None)
            {
                response.Result = errorCode;
                return response;
            }

            return response;
        }
    }
}
