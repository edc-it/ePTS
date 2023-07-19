using ePTS.Data;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ePTS.Web.Controllers
{
    public class UserOrganizationsController : BaseController
    {
        public UserOrganizationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrganizationsController> logger) : base(context, logger, userManager)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
