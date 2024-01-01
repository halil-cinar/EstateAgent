using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Account/")]
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(IdentityDto identity)
        {
            var result = await _accountService.Login(identity);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                //TempData["Session"] = result.Result;
                return Redirect("/");
            }
            return Redirect("/");
        }

        [HttpPost("/Account/Logout")]

        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.Logout();
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                //TempData["Session"] = result.Result;
                return Redirect("/");
            }
            return Redirect("/");
        }



    }
}
