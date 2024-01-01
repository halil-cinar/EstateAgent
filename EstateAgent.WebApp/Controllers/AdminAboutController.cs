using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/About")]
    public class AdminAboutController : Controller
    {
        private IAboutService _aboutService;
        private IAboutMediaService _aboutMediaService;
        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private readonly IToastNotification _toastNotification;

        public AdminAboutController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IAboutService aboutService, IAboutMediaService aboutMediaService, IToastNotification toastNotification)
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
            _aboutService = aboutService;
            _aboutMediaService = aboutMediaService;
            _toastNotification = toastNotification;
        }


        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.AboutLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _aboutService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });

            ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.AboutUpdate));

            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("Update")]
        public async Task<IActionResult> Update()
        {
            if (!authMethod.Contains(MethodTypes.AboutUpdate))
            {
                return Redirect("/");
            }
            var result = await _aboutService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AboutDto about)
        {
            if (!authMethod.Contains(MethodTypes.AboutUpdate))
            {
                return Redirect("/");
            }
            var result = await _aboutService.Update(about);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index", "AdminAbout");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("ChangePhoto/")]
        public async Task<IActionResult> ChangePhoto()
        {
            if (!authMethod.Contains(MethodTypes.AboutMediaUpdate))
            {
                return Redirect("/");
            }

            var result = await _aboutMediaService.GetAll(null);

            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                if (result.Result.Count > 0)
                {
                    ViewBag.MaxSlideIndex = result.Result?[result.Result.Count - 1].SlideIndex;
                    ViewBag.MinSlideIndex = result.Result?[0].SlideIndex;

                    if (ViewBag.MaxSlideIndex == null)
                    {
                        ViewBag.MaxSlideIndex = 0;
                    }
                }

                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.AboutMediaUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.AboutMediaRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.AboutMediaGet));


                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("ChangePhoto/Swap/{Id:long}")]
        public async Task<IActionResult> ChangePhotoSwap([FromForm] AboutMediaDto aboutMedia, long Id)
        {
            if (!authMethod.Contains(MethodTypes.AboutMediaLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _aboutMediaService.Swap(aboutMedia);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"/Admin/About/ChangePhoto");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"/Admin/About/ChangePhoto");
        }


        [HttpPost("ChangePhoto/Photo/{Id:long}")]
        public async Task<IActionResult> ChangePhotoPhoto([FromForm] AboutMediaDto aboutMedia, long Id)
        {
            if (!authMethod.Contains(MethodTypes.AboutMediaUpdate))
            {
                return Redirect("/");
            }
            var result = await _aboutMediaService.ChangePhoto(aboutMedia);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"/Admin/About/ChangePhoto");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"/Admin/About/ChangePhoto");
        }

        [HttpGet("ChangePhoto/Delete/{id:long}")]
        public async Task<IActionResult> ChangePhotoDelete(long id)
        {
            if (!authMethod.Contains(MethodTypes.AboutMediaRemove))
            {
                return Redirect("/");
            }
            var result = await _aboutMediaService.Remove(id);

            return RedirectToAction("Index");
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
        }

        [HttpPost("ChangePhoto/Add")]
        public async Task<IActionResult> ChangePhotoAdd([FromForm] AboutMediaDto aboutMedia)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaUpdate))
            {
                return Redirect("/");
            }
            var result = await _aboutMediaService.Add(aboutMedia);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"/Admin/About/ChangePhoto");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"/Admin/About/ChangePhoto");
        }


    }
}
