using EstateAgent.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EstateAgent.WebApp.Controllers
{
    public class AgentController : Controller
    {
        private IAgentService  _agentService;

        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _agentService.LoadMoreFilter(new Dto.Filter.LoadMoreFilter<Dto.Dtos.AgentFilter>
            {
                ContentCount = 20,
                PageCount = 0,
            });
            if(result.ResultStatus==Dto.Enums.ResultStatus.Success )
            {
                return View(result.Result);
            }
            return View();
        }
    }
}
