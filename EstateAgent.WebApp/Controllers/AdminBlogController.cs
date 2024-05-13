using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/Blog")]
    public class AdminBlogController : Controller
    {
        private IBlogService _blogService;

        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private long loginUserId;
        private readonly IToastNotification _toastNotification;

        public AdminBlogController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IToastNotification toastNotification, IBlogService blogService)
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
            _toastNotification = toastNotification;
            _blogService = blogService;
            
        }

        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.BlogLoadMoreFilter))
            {
                return Redirect("/");
            }
            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"]) ? "0" : Request.Query["page"]);

            var result = await _blogService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.BlogFilter>
            {
                ContentCount = 10,
                PageCount = page
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.BlogUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.BlogRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.BlogGet));
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("BlogAdd")]
        public IActionResult Add()
        {
            if (!authMethod.Contains(MethodTypes.BlogAdd))
            {
                return Redirect("/");
            }
            ViewBag.LoginUserId = loginUserId;
            return View();
        }

        [HttpPost("BlogAdd")]
        public async Task<IActionResult> Add(BlogDto blog)
        {
            if (!authMethod.Contains(MethodTypes.BlogAdd))
            {
                return Redirect("/");
            }
            blog.UserId = loginUserId;
            var result = await _blogService.Add(blog);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpGet("Detail/{id:long}")]
        public async Task<IActionResult> Detail(long id)
        {
            if (!authMethod.Contains(MethodTypes.BlogGet))
            {
                return Redirect("/");
            }
            var result = await _blogService.Get(id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.BlogUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.BlogRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.BlogGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpPost("Update/{id:long}")]
        public async Task<IActionResult> Update([FromForm] BlogDto blog)
        {
            if (!authMethod.Contains(MethodTypes.BlogUpdate))
            {
                return Redirect("/");
            }
            var result = await _blogService.Update(blog);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Update",blog.Id);
        }


        [HttpGet("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            if (!authMethod.Contains(MethodTypes.BlogUpdate))
            {
                return Redirect("/");
            }

            var result = await _blogService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpPost("ChangePhoto/")]
        public async Task<IActionResult> ChangePhoto([FromForm] BlogDto blog)
        {
            if (!authMethod.Contains(MethodTypes.BlogUpdate))
            {
                return Redirect("/");
            }
            var result = await _blogService.ChangePhoto(blog);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("ChangePhoto",blog.Id);
        }


        [HttpGet("ChangePhoto/{id:long}")]
        public async Task<IActionResult> ChangePhoto(long id)
        {
            if (!authMethod.Contains(MethodTypes.BlogUpdate))
            {
                return Redirect("/");
            }
            var result = await _blogService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!authMethod.Contains(MethodTypes.BlogRemove))
            {
                return Redirect("/");
            }
            var result = await _blogService.Remove(id);
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
