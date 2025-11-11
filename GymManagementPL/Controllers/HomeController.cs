using GymManagementBLL.Service.InterFaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;
        // Actions
        // Base URL/Home/Index
        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();
            return View(Data);
        }
    }
}
