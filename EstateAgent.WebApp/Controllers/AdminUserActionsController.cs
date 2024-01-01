using EnumsNET;
using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Linq;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/UserActions")]
    public class AdminUserActionsController : Controller
    {
        private IUserService _userService;
        private IUserRoleService _userRoleService;


        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private readonly IToastNotification _toastNotification;

        public AdminUserActionsController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IUserService userService, IToastNotification toastNotification, IUserRoleService userRoleService)
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
            _userService = userService;
            _toastNotification = toastNotification;
            _userRoleService = userRoleService;
        }


        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.UserLoadMoreFilter))
            {
                return Redirect("/");
            }
            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"])?"0": Request.Query["page"]);
            var result = await _userService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<UserFilter>
            {
                ContentCount = 10,
                PageCount = page,

            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.UserUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.UserRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.UserGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("UserAdd")]
        public async Task<IActionResult> UserAdd()
        {
            if (!authMethod.Contains(MethodTypes.UserAdd))
            {
                return Redirect("/");
            }
            var roles = Enum.GetValues(typeof(RoleTypes));
            var roleOptionList = new List<OptionDto<int>>();
            foreach (var role in roles)
            {
                roleOptionList.Add(new OptionDto<int>
                {
                    Value = (int)role,
                    Text = ((RoleTypes)role).AsString(EnumFormat.Description)
                });
            }
            ViewBag.Roles = roleOptionList;
            
            return View();
        }

        [HttpPost("UserAdd")]
        public async Task<IActionResult> UserAdd(UserDto user)
        {
            if (!authMethod.Contains(MethodTypes.UserAdd))
            {
                return Redirect("/");
            }
            var result= await _userService.Add(user);
            if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("UserAdd");
        }

        [HttpGet("Detail/{id:long}")]
        public async Task<IActionResult> Detail(long id)
        {
            if (!authMethod.Contains(MethodTypes.UserGet))
            {
                return Redirect("/");
            }
            var result = await _userService.Get(id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.UserUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.UserRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.UserGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("Update/{id:long}")]
        public async Task<IActionResult> Update([FromForm] UserDto user)
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
            return RedirectToAction("Update",user.Id);
        }


        [HttpGet("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            if (!authMethod.Contains(MethodTypes.UserUpdate))
            {
                return Redirect("/");
            }

            var result = await _userService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpPost("ChangePhoto/")]
        public async Task<IActionResult> ChangePhoto([FromForm] UserDto user)
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
            return RedirectToAction("ChangePhoto",user.Id);
        }


        [HttpGet("ChangePhoto/{id:long}")]
        public async Task<IActionResult> ChangePhoto(long id)
        {
            if (!authMethod.Contains(MethodTypes.UserUpdate))
            {
                return Redirect("/");
            }
            var result = await _userService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpGet("ChangeRole/{userId:long}")]
        public async Task<IActionResult> ChangeRole(long userId)
        {
            var result = await _userRoleService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<UserRoleFilter>
            {
                ContentCount = 1
                ,PageCount=0
            });
            var roles = Enum.GetValues(typeof(RoleTypes));
            var roleOptionList = new List<OptionDto<int>>();
            foreach (var role in roles)
            {
                roleOptionList.Add(new OptionDto<int>
                {
                    Value = (int)role,
                    Text = ((RoleTypes)role).AsString(EnumFormat.Description)
                });
            }
            ViewBag.Roles = roleOptionList;
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(result.Result.Values[0]);
            }
            var message= string.Join(Environment.NewLine, result.ErrorMessages.Select(x=>x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");

        }

        [HttpPost("ChangeRole/{userId:long}")]
        public async Task<IActionResult> ChangeRole(UserRoleDto userRole)
        {
            var result = await _userRoleService.Add(userRole);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("ChangeRole",userRole.Id);

        }


        [HttpGet("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!authMethod.Contains(MethodTypes.UserRemove))
            {
                return Redirect("/");
            }
            var result = await _userService.Remove(id);
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");

        }

        [HttpGet("ResetPassword/{id:long}")]
        public async Task<IActionResult> ResetPassword(long id)
        {
            if (!authMethod.Contains(MethodTypes.UserResetPassword))
            {
                return Redirect("/");
            }
            var result = await _userService.ResetPassword(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");

        }







    }
}
