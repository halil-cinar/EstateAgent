using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _blogService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.BlogFilter>
            {
                ContentCount = 20,
                PageCount = 0,
            });
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
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            return View();
        }
    }
}
