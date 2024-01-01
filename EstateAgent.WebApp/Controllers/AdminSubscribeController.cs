using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/Subscribe")]
    public class AdminSubscribeController : Controller
    {
        private ISubscribeService _subscribeService;

        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private readonly IToastNotification _toastNotification;

        public AdminSubscribeController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, ISubscribeService subscribeService, IToastNotification toastNotification)
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
            _subscribeService = subscribeService;
            _toastNotification = toastNotification;
        }


        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.SubscribeLoadMoreFilter))
            {
                return Redirect("/");
            }
            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"]) ? "0" : Request.Query["page"]);

            var result = await _subscribeService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.SubscribeFilter>
            {
                ContentCount = 10,
                PageCount = page
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.SubscribeUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.SubscribeRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.SubscribeGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("Add")]
        public IActionResult Add()
        {
            if (!authMethod.Contains(MethodTypes.SubscribeLoadMoreFilter))
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(SubscribeDto subscribe)
        {
            if (!authMethod.Contains(MethodTypes.SubscribeAdd))
            {
                return Redirect("/");
            }
            var result = await _subscribeService.Add(subscribe);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Add");
        }


       

        [HttpPost("Update/{id:long}")]
        public async Task<IActionResult> Update([FromForm] SubscribeDto subscribe)
        {
            if (!authMethod.Contains(MethodTypes.SubscribeUpdate))
            {
                return Redirect("/");
            }
            var result = await _subscribeService.Update(subscribe);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Update",subscribe.Id);
        }


        [HttpGet("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            if (!authMethod.Contains(MethodTypes.SubscribeUpdate))
            {
                return Redirect("/");
            }

            var result = await _subscribeService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }


        
        [HttpGet("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!authMethod.Contains(MethodTypes.SubscribeRemove))
            {
                return Redirect("/");
            }
            var result = await _subscribeService.Remove(id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");

        }



    }
}
