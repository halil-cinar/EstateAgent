using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/Advanced/")]
    public class AdminAdvancedSettingsController : Controller
    {
        private ISystemSettingService _systemSettingService;
        private readonly IToastNotification _toastNotification;
        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();


        public AdminAdvancedSettingsController(ISystemSettingService systemSettingService, IToastNotification toastNotification, IAccountService accountService,IHttpContextAccessor httpContextAccessor)
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
            _systemSettingService = systemSettingService;
            _toastNotification = toastNotification;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.SystemSettingsLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _systemSettingService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<SystemSettingsFilter>
            {
                ContentCount = 10,
                PageCount = 0,
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("")]
        public async Task<IActionResult> Index(SystemSettingsDto systemSettings)
        {
            if (!authMethod.Contains(MethodTypes.SystemSettingsUpdate))
            {
                return Redirect("/");
            }
            var result = await _systemSettingService.Update(systemSettings);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }



        [HttpGet("Logo")]
        public async Task<IActionResult> LogoUpdate()
        {
            if (!authMethod.Contains(MethodTypes.SystemSettingsLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _systemSettingService.Get("logo");
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }
        [HttpPost("Logo")]
        public async Task<IActionResult> LogoUpdate(LogoDto logo)
        {
            if (!authMethod.Contains(MethodTypes.SystemSettingsUpdate))
            {
                return Redirect("/");
            }
            var result= await _systemSettingService.ChangeLogo(logo);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect("/Admin");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        


    }
}
