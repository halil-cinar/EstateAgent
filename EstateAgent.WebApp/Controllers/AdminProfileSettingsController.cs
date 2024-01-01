using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/ProfileSetting")]
    public class AdminProfileSettingsController : Controller
    {
        private IUserService _userService;

        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private long loginUserId;
        private readonly IToastNotification _toastNotification;

        public AdminProfileSettingsController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IUserService userService, IToastNotification toastNotification)
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
                loginUserId = result.Result.Result.UserId;
            }
            _userService = userService;
            _toastNotification = toastNotification;
        }


        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.UserGet))
            {
                return Redirect("/Admin");
            }
            var result = await _userService.Get(loginUserId);
            if (result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.UserUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.UserRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.UserGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return NoContent();
        }

        [HttpPost("")]
        public async Task<IActionResult> Index([FromForm]UserDto user)
        {
            if (!authMethod.Contains(MethodTypes.UserUpdate))
            {
                return Redirect("/");
            }
            var result = await _userService.Update(user);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {

                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return NoContent();
        }


        [HttpPost("ChangePassword")]
        public async  Task<IActionResult> ChangePassword([FromForm]UserDto user)
        {
            if (!authMethod.Contains(MethodTypes.UserUpdate))
            {
                return Redirect("/");
            }
            var result = await _userService.ChangeIdentity(user);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {

                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return NoContent();
        }

        [HttpPost("ChangePhoto")]
        public async  Task<IActionResult> ChangePhoto([FromForm]UserDto user)
        {
            if (!authMethod.Contains(MethodTypes.UserUpdate))
            {
                return Redirect("/");
            }
            var result = await _userService.ChangePhoto(user);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {

                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return NoContent();
        }


    }
}
