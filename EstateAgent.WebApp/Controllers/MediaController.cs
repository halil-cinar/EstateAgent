using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Media/")]
    public class MediaController : Controller
    {
        private IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("{id:long}")]
        public async Task<FileResult> GetMedia(long id)
        {
            var result= await _mediaService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return File(result.Result.Media, result.Result.MediaType, result.Result.MediaName);
            }
            return null;
        }

    }
}
