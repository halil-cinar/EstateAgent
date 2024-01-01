using EnumsNET;
using EstateAgent.Business.Abstract;
using EstateAgent.Dto.Dtos;
using EstateAgent.Dto.Result;
using EstateAgent.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace EstateAgent.WebApp.Controllers
{
    [Route("/Admin/Portfoy/Agent/")]
    public class AdminAgentController : Controller
    {
        private IAgentService _agentService;
        private IUserRoleService _userRoleService;
        private IAccountService _accountService;
        private List<MethodTypes> authMethod = new List<MethodTypes>();
        private readonly IToastNotification _toastNotification;

        public AdminAgentController(IAccountService accountService, IHttpContextAccessor httpContextAccessor, IUserRoleService userRoleService, IAgentService agentService, IToastNotification toastNotification)
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
            _userRoleService = userRoleService;
            _agentService = agentService;
            _toastNotification = toastNotification;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!authMethod.Contains(MethodTypes.AgentLoadMoreFilter))
            {
                return Redirect("/");
            }

            var page = Convert.ToInt32(string.IsNullOrEmpty(Request.Query["page"]) ? "0" : Request.Query["page"]);


            var result = await _agentService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AgentFilter>
            {
                ContentCount = 10,
                PageCount = page,
            });

            if(result.ResultStatus==Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.AgentUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.AgentRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.AgentGet));


                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return View();
        }

        [HttpGet("Detail/{id:long}")]
        public async Task<IActionResult> Detail(long id)
        {
            if (!authMethod.Contains(MethodTypes.AgentGet))
            {
                return Redirect("/");
            }
            var result = await _agentService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.CanUpdate = (authMethod.Contains(MethodTypes.AgentUpdate));
                ViewBag.CanDelete = (authMethod.Contains(MethodTypes.AgentRemove));
                ViewBag.CanDetail = (authMethod.Contains(MethodTypes.AgentGet));

                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpPost("AgentAdd/")]
        public async Task<IActionResult> AgentAdd([FromForm] AgentDto agent)
        {
            if (!authMethod.Contains(MethodTypes.AgentAdd))
            {
                return Redirect("/");
            }
            var result = await _agentService.Add(agent);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("AgentAdd");
        }


        [HttpGet("AgentAdd/")]
        public async Task<IActionResult> AgentAdd()
        {
            if (!authMethod.Contains(MethodTypes.AgentAdd))
            {
                return Redirect("/Index");
            }
                var result = await _userRoleService.GetOptionList(new UserRoleFilter
            {
                Role = Entities.Enums.RoleTypes.Agent
            });
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.UserOptionList = result.Result;
                return View(result.Result);
            }

            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpPost("Update/{id:long}")]
        public async Task<IActionResult> Update([FromForm] AgentDto agent)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }
            var result = await _agentService.Update(agent);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Update",agent.Id);
        }


        [HttpGet("Update/{id:long}")]
        public async Task<IActionResult> Update(long id)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }

            var result = await _agentService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpPost("ChangePhoto/")]
        public async Task<IActionResult> ChangePhoto([FromForm] AgentDto agent)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }
            var result = await _agentService.ChangePhoto(agent);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("ChangePhoto",agent.Id);
        }


        [HttpGet("ChangePhoto/{id:long}")]
        public async Task<IActionResult> ChangePhoto(long id)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }
            var result = await _agentService.Get(id);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return View(result.Result);
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }

        [HttpPost("ChangeUser/{id:long}")]
        public async Task<IActionResult> ChangeUser([FromForm] AgentDto agent)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }
            var result = await _agentService.ChangeUser(agent.Id,agent.UserId);
            if (result.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                return RedirectToAction("Index");
            }
            var message = string.Join(Environment.NewLine, result.ErrorMessages.Select(x => x.Message).ToList());
            _toastNotification.AddErrorToastMessage(message);
            return RedirectToAction("Index");
        }


        [HttpGet("ChangeUser/{id:long}")]
        public async Task<IActionResult> ChangeUser(long id)
        {
            if (!authMethod.Contains(MethodTypes.AgentUpdate))
            {
                return Redirect("/");
            }
            var Opresult = await _userRoleService.GetOptionList(new UserRoleFilter
            {
                Role = Entities.Enums.RoleTypes.Agent
            });
            if (Opresult.ResultStatus == Dto.Enums.ResultStatus.Success)
            {
                ViewBag.UserOptionList = Opresult.Result;
            }
            var result = await _agentService.Get(id);
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
            if (!authMethod.Contains(MethodTypes.AgentRemove))
            {
                return Redirect("/");
            }
            var result = await _agentService.Remove(id);
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
