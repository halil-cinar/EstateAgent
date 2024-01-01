using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/ContactInfo")]
    public class AdminContactInfoController : Controller
    {
        private IContactInfoService _contactInfoService;
        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();

        private readonly IToastNotification _toastNotification;


        public AdminContactInfoController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IContactInfoService contactInfoService, IToastNotification toastNotification)
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
            _contactInfoService = contactInfoService;
            _toastNotification = toastNotification;
        }


        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.ContactInfoLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _contactInfoService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.ContactInfoFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.ContactInfoUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.ContactInfoRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.ContactInfoGet));

                return View(result.Result.Values[0]);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }


        [HttpPost("Update/")]
        public async Task<IActionResult> Update([FromForm] ContactInfoDto contactInfo)
        {
            if (!authMethod.Contains(MethodTypes.ContactInfoUpdate))
            {
                return Redirect("/");
            }
            var result = await _contactInfoService.Update(contactInfo);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpGet("Update/")]
        public async Task<IActionResult> Update()
        {
            if (!authMethod.Contains(MethodTypes.ContactInfoUpdate))
            {
                return Redirect("/");
            }
            var result = await _contactInfoService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<ContactInfoFilter>
            {
                ContentCount= 1,
                PageCount=0

            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result.Values[0]);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }


        


    }
}
