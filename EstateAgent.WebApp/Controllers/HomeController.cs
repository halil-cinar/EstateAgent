using EnumsNET;
using EstateAgent.Business.Abstract;
using EstateAgent.Dto.ListDtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using EstateAgent.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics;

namespace EstateAgent.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private ISystemSettingService _systemSettingService;
        private IPropertyService _propertyService;
        private IAboutService _aboutService;
        private IAccountService _accountService;
        
        private long? loginUserId=null;
        private string logoId = "";
        private SessionListDto session=null;
        
        public HomeController(IPropertyService propertyService, IAboutService aboutService, IHttpContextAccessor httpContextAccessor, IAccountService accountService, ISystemSettingService systemSettingService)
        {
            _propertyService = propertyService;
            _aboutService = aboutService;
            _accountService = accountService;
            var sessionResult = _accountService.GetSession();
            sessionResult.Wait();
            if (sessionResult.Result.ResultStatus == Dto.Enums.ResultStatus.Success && sessionResult.Result?.Result?.UserId != null)
            {
                loginUserId = sessionResult.Result?.Result?.UserId;
                session = sessionResult.Result?.Result;

            }
            _systemSettingService = systemSettingService;
            var logoResult = _systemSettingService.Get("logo");
            logoResult.Wait();
            if(logoResult.Result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {   
                logoId=logoResult.Result.Result.Value;
            }
        }

        public async Task<IActionResult> Index()
        {
            TempData["LoginUserId"] = loginUserId?.ToString();
            //ViewBag.LogoId= logoId;
            //ViewData["LogoId"]=logoId;

            var result = await _propertyService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.PropertyFilter>
            {
                ContentCount = 15,
                PageCount = 0
               
            });

            var aboutResult = await _aboutService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AboutFilter>
            {
                ContentCount = 1,
                PageCount = 0
            });
            if(aboutResult.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                ViewBag.About=aboutResult.Result.Values[0].Content.Substring(0,Math.Min(300, aboutResult.Result.Values[0].Content.Length-1))+"...";
            }
            var statuses = Enum.GetValues(typeof(PropertySaleStatus));
            var statusList = new List<OptionDto<int>>();
            foreach (var role in statuses)
            {
                statusList.Add(new OptionDto<int>
                {
                    Value = (int)role,
                    Text = ((PropertySaleStatus)role).AsString(EnumFormat.Description)
                });
            }
            ViewBag.StatusOptionList = statusList;
            ViewBag.Session = session;
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            
            return View();
        }

       







        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}