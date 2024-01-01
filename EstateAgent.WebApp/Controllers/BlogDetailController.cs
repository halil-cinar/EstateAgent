using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/blogdetail")]
    public class BlogDetailController : Controller
    {
        private IBlogService _blogService;

        public BlogDetailController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Index(long id)
        {
            var result = await _blogService.Get(id);
            var recentBlogsResult = await _blogService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.BlogFilter>
            {
                ContentCount = 3,
                PageCount = 0,
                Filter = new Dto.Dtos.BlogFilter
                {
                    Order = new Dto.Dtos.Order
                    {
                        PostedDateDirection = true
                    }
                }
            });
            ViewBag.RecentBlogs = recentBlogsResult.Result;
            if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            return NoContent();
        }
    }
}
