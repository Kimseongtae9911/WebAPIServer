using Microsoft.AspNetCore.Mvc;
using WebAPIServer.ModelReqRes;
using WebAPIServer.Services;

namespace WebAPIServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientSignUpController : ControllerBase
    {
        private readonly IAccountDB m_accountDB;

        public ClientSignUpController(IAccountDB accountDB)
        {
            m_accountDB = accountDB;
        }

        [HttpPost]
        public async Task<PkClientSignUpRes> Post(PkClientSignUpReq request)
        {
            var response = new PkClientSignUpRes();

            var errorCode = await m_accountDB.ClientSignUp(request.ID, request.Password);
            if (errorCode != ErrorCode.None)
            {
                response.Result = errorCode;
                return response;
            }


            return response;
        }
    }
}
