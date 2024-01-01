using EnumsNET;
using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Enums;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/Portfoy/Property")]
    public class AdminPropertyController : Controller
    {
        private IPropertyService _propertyService;
        private IAgentService _agentService;
        private IPropertyMediaService _propertyMediaService;
        private readonly IToastNotification _toastNotification;


        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        public AdminPropertyController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IPropertyService propertyService, IAgentService agentService, IPropertyMediaService propertyMediaService, IToastNotification toastNotification)
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
            _propertyService = propertyService;
            _agentService = agentService;
            _propertyMediaService = propertyMediaService;
            _toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.PropertyLoadMoreFilter))
            {
                return Redirect("/");
            }
            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"]) ? "0" : Request.Query["page"]);

            var result = await _propertyService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.PropertyFilter>
            {
                ContentCount = 10,
                PageCount = page
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.PropertyUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.PropertyRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.PropertyGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }
        [HttpGet("Add")]
        public async Task<IActionResult> Add()
        {
            if (!authMethod.Contains(MethodTypes.PropertyAdd))
            {
                return Redirect("/");
            }
            var result = await _agentService.GetOptionList( null);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.AgentOptionList = result.Result;
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

            return View();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(PropertyDto property)
        {
            if (!authMethod.Contains(MethodTypes.PropertyAdd))
            {
                return Redirect("/");
            }
            var result = await _propertyService.Add(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Add");
        }


        [HttpGet("Detail/{id:long}")]
        public async Task<IActionResult> Detail(long id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyGet))
            {
                return Redirect("/");
            }
            var result = await _propertyService.Get(id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.PropertyUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.PropertyRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.PropertyGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpPost("Update/{id:long}")]
        public async Task<IActionResult> Update([FromForm] PropertyDto property)
        {
            if (!authMethod.Contains(MethodTypes.PropertyUpdate))
            {
                return Redirect("/");
            }
            var result = await _propertyService.Update(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Update",property.Id);
        }


        [HttpGet("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyUpdate))
            {
                return Redirect("/");
            }
            var result2 = await _agentService.GetOptionList(null);
            if (result2.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.AgentOptionList = result2.Result;
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
            var result = await _propertyService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpPost("ChangePhoto/{propertyId:long}/{Id:long}")]
        public async Task<IActionResult> ChangePhoto([FromForm] PropertyMediaDto property, long Id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaUpdate))
            {
                return Redirect("/");
            }
            var result = await _propertyMediaService.Update(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"../../{property.PropertyId}");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"../../{property.PropertyId}");
        }

        [HttpPost("ChangePhoto/Add")]
        public async Task<IActionResult> ChangePhoto([FromForm] PropertyMediaDto property)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaUpdate))
            {
                return Redirect("/");
            }
            var result = await _propertyMediaService.Add(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"./{property.PropertyId}");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"./{property.PropertyId}");
        }



        [HttpPost("ChangePhoto/Swap/{propertyId:long}/{Id:long}")]
        public async Task<IActionResult> ChangePhotoSwap([FromForm] PropertyMediaDto property, long Id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaLoadMoreFilter))
            {
                return Redirect("/");
            }
            var result = await _propertyMediaService.Swap(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"../../{property.PropertyId}");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"../../{property.PropertyId}");
        }


        [HttpPost("ChangePhoto/Photo/{propertyId:long}/{Id:long}")]
        public async Task<IActionResult> ChangePhotoPhoto([FromForm] PropertyMediaDto property, long Id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaUpdate))
            {
                return Redirect("/");
            }
            var result = await _propertyMediaService.ChangePhoto(property);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return Redirect($"../../{property.PropertyId}");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return Redirect($"../../{property.PropertyId}");
        }


        [HttpGet("ChangePhoto/{propertyId:long}")]
        public async Task<IActionResult> ChangePhoto(long propertyId)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaUpdate))
            {
                return Redirect("/");
            }
            ViewBag.PropertyId = propertyId;
            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"]) ? "0" : Request.Query["page"]);

            var result = await _propertyMediaService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<PropertyMediaFilter>
            {
                ContentCount= 20,
                PageCount= page,
                Filter=new PropertyMediaFilter
                {
                    PropertyId=propertyId,
                }
            });

            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                if (result.Result?.Values.Count > 0)
                {
                    ViewBag.MaxSlideIndex=result.Result?.Values[result.Result.Values.Count-1].SlideIndex;
                }

                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.PropertyMediaUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.PropertyMediaRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.PropertyMediaGet));


                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyRemove))
            {
                return Redirect("/");
            }
            var result = await _propertyService.Remove(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");

        }

        [HttpGet("ChangePhoto/Delete/{id:long}")]
        public async Task<IActionResult> ChangePhotoDelete(long id)
        {
            if (!authMethod.Contains(MethodTypes.PropertyMediaRemove))
            {
                return Redirect("/");
            }
            var result = await _propertyMediaService.Remove(id);
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
