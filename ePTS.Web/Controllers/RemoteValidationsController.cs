using ePTS.Data;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ePTS.Web.Controllers
{
    public class RemoteValidationsController : BaseController
    {
        public RemoteValidationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RemoteValidationsController> logger) : base(context, logger, userManager)
        {
            //_context = context;
        }

        [AcceptVerbs("Post")]
        public IActionResult VerifyOrganizationCode(string? Code, string? OrganizationCodeInitialValue)
        {
            if (Code == OrganizationCodeInitialValue)
            {
                return Json(true);
            }

            if (_context.Organizations.Any(e => e.Code == Code))
            {
                return Json(false);
            }

            return Json(true);
        }

        [AcceptVerbs("Post")]
        public IActionResult VerifySchoolCode(string Code, string SchoolCodeInitialValue)
        {
            if (Code == SchoolCodeInitialValue)
            {
                return Json(true);
            }

            if (_context.Schools.Any(e => e.SchoolCode == Code))
            {
                return Json(false);
            }

            return Json(true);
        }

        [AcceptVerbs("Post")]
        public IActionResult VerifyLocationCode(string RefLocationId, string LocationCodeInitialValue)
        {
            if (RefLocationId == LocationCodeInitialValue)
            {
                return Json(true);
            }

            if (_context.Locations.Any(e => e.RefLocationId == RefLocationId))
            {
                return Json(false);
            }

            return Json(true);
        }

    }
}
