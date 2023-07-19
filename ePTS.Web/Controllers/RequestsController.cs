using ePTS.Data;
using ePTS.Entities.Identity;
using ePTS.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ePTS.Web.Controllers
{
    public class RequestsController : BaseController
    {
        public RequestsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<BaseController> logger) : base(context, logger, userManager)
        {
        }

        [AcceptVerbs("Post")]
        public IActionResult GetLocations(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model =
                from location in _context.Locations
                where location.ParentLocationId == id
                select new
                {
                    location.RefLocationId,
                    location.LocationName
                };

            if (model == null)
            {
                return NotFound();
            }

            return Json(model);
        }

        [AcceptVerbs("Get")]
        public IActionResult GetOrganizationTree(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            // Select ALL Organizations
            var AllOrganizations = _context.Organizations
                .Select(x => new
                {
                    Id = x.OrganizationId,
                    Text = x.OrganizationName + " (" + x.Code + ")",
                    Parent = x.ParentOrganizationId,
                    Icon = x.OrganizationTypes!.OrganizationType == "School" ? "bi bi-building" : "bi bi-buildings"

                }).ToList();

            //Select the OrganizationParentId for the id parameter (OrganizationId) - this includes the Parent in the hierachical tree
            //var parent = AllOrganizations.Where(x => x.Id == id).Select(x => x.Parent).FirstOrDefault();
            var parent = AllOrganizations.Where(x => x.Id == id).Select(x => x.Id).FirstOrDefault();

            //Select parent organization object
            var top = AllOrganizations.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.Text,
                Parent = "#",
                x.Icon
            }).ToList();

            //Creates a generic Lookup<TKey,TElement>
            var lookup = AllOrganizations.ToLookup(x => x.Parent);

            //Flattens (the lookup) filtering all the children from the selected organization
            var model = lookup[parent].SelectRecursive(x => lookup[x.Id])
                .Select(x => new
                {
                    x.Id,
                    x.Text,
                    Parent = x.Parent == null ? "#" : x.Id == id ? "#" : x.Parent.ToString(),
                    x.Icon
                })
                .ToList();

            if (top == null)
            {
                return NotFound();
            }

            //Add parent organization object to model
            model.AddRange(top!);

            if (model == null)
            {
                return NotFound();
            }

            return Json(model);
        }
    }
}
