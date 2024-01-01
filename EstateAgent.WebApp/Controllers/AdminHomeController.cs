using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin")]
    [Route("/Admin/Home")]
    public class AdminHomeController : Controller
    {
        private IAccountService _accountService;
        private IHomeService _homeService;
        private List<MethodTypes> authMethod=new List<MethodTypes>();
        private readonly IToastNotification _toastNotification;

        public AdminHomeController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IHomeService homeService, IToastNotification toastNotification)
        {

            _accountService = accountService;
            var result = _accountService.GetSession();
            result.Wait();
            if (result.Result.ResultStatus == Dto.Enums.ResultStatus.Error || result.Result.Result == null)
            {
                httpContextAccessor.HttpContext.Response.Redirect("/");
            }
            else
            {
                var authResult = _accountService.AuthorizationControl(result.Result.Result.Id);
                authResult.Wait();
                if (authResult.Result.ResultStatus == Dto.Enums.ResultStatus.Error)
                {
                    httpContextAccessor.HttpContext.Response.Redirect("/Error");
                }

                authMethod = authResult.Result.Result;

            }
            _homeService = homeService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _homeService.Get();
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewData["Error"] = "Error";
               
               
                return View(result.Result);
            }
            var message=string.Join(Environment.NewLine, result.ErrorMessages.Select(x=>x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
    }
}
