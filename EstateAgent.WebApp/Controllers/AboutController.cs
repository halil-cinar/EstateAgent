using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class AboutController : Controller
    {
        private IAboutService _aboutService;
        private IAboutMediaService _aboutMediaService;

        public AboutController(IAboutService aboutService, IAboutMediaService aboutMediaService)
        {
            _aboutService = aboutService;
            _aboutMediaService = aboutMediaService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _aboutService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });
            var mediaResult = await _aboutMediaService.GetAll(null);
            if(mediaResult.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                ViewBag.AboutMediaLists = mediaResult.Result;
            }
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }

            return View();
        }
    }
}
