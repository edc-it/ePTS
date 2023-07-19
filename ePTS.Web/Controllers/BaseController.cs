using ePTS.Data;
using ePTS.Entities.Core;
using ePTS.Entities.Identity;
using ePTS.Models.ViewModels;
using ePTS.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace ePTS.Web.Controllers
{
    // BaseController inherits from the built-in Controller class in ASP.NET Core and serves as a base class for your own controllers.
    // By putting shared logic here, you can reuse that logic across multiple controllers.
    public class BaseController : Controller
    {
        // A reference to the application's DbContext is stored in a protected field so that derived classes can access it.
        protected readonly ApplicationDbContext _context;

        // A reference to the application's ILogger is stored in a protected field so that derived classes can write logs.
        protected readonly ILogger _logger;

        // The UserManager is used to retrieve information about the current user.
        protected readonly UserManager<ApplicationUser> _userManager;

        // The BaseController constructor takes an ApplicationDbContext as a parameter, which is supplied by dependency injection.
        public BaseController(ApplicationDbContext context, ILogger<BaseController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // new refactored: 
        protected async Task<Guid?> GetCurrentUserIdAsync()
        {
            return (await _userManager.GetUserAsync(HttpContext.User))?.Id;
        }

        protected async Task<List<Guid>> GetUserOrganizationIdsAsync(Guid? userId)
        {
            var organizationIds = await _context.ApplicationUserOrganizations
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationId)
                .ToListAsync();

            return organizationIds.Where(x => x.HasValue).Select(x => x!.Value).ToList();
        }

        protected async Task<List<Guid>> GetChildOrganizationIdsAsync(List<Guid> userOrganizationIds, Guid? id)
        {
            var allOrganizations = await _context.Organizations.ToListAsync();
            var lookup = allOrganizations.ToLookup(x => x.ParentOrganizationId);
            return userOrganizationIds
                .SelectMany(id => lookup[id].SelectRecursive(x => lookup[x.OrganizationId]))
                .Select(x => x.OrganizationId)
                .ToList();
        }

        protected async Task<List<Organization>> GetBreadcrumbAsync(Guid organizationId)
        {
            var selectedOrganizationId = HttpContext.Session.GetString("SelectedOrganizationId");
            Guid? userOrganizationId = null;

            if (!string.IsNullOrEmpty(selectedOrganizationId))
            {
                userOrganizationId = Guid.Parse(selectedOrganizationId);
            }
            else
            {
                return new List<Organization>();
            }

            var organization = await _context.Organizations
                .Where(x => x.OrganizationId == organizationId)
                .Include(x => x.OrganizationTypes)
                .FirstOrDefaultAsync();

            if (organization == null)
                return new List<Organization>();

            var hierarchy = new List<Organization>();

            // Keep adding parents to breadcrumb until we reach the selected organization
            while (organization.OrganizationId != userOrganizationId)
            {
                hierarchy.Insert(0, organization);

                if (organization.ParentOrganizationId == null)
                    break;

                organization = await _context.Organizations
                    .Where(x => x.OrganizationId == organization.ParentOrganizationId)
                    .Include(x => x.OrganizationTypes)
                    .FirstOrDefaultAsync();

                if (organization == null)
                    break;
            }

            // After the loop, if we still haven't added the selected organization to the breadcrumb,
            // add it now.
            if (organization != null && organization.OrganizationId == userOrganizationId && (hierarchy.Count == 0 || hierarchy[0].OrganizationId != userOrganizationId))
            {
                hierarchy.Insert(0, organization);
            }

            return hierarchy;
        }

        public async Task<Guid?> GetSelectedOrganization()
        {
            //var userId = (await _userManager.GetUserAsync(User))?.Id;
            var userId = await GetCurrentUserIdAsync();

            // Get the user's organizations
            var userOrganizations = await _context.ApplicationUserOrganizations
                .Where(a => a.UserId == userId)
                .ToListAsync();

            Guid? selectedOrganizationId = null;

            if (userOrganizations.Count == 1)
            {
                // If the user only has one organization, set that as the selected organization
                selectedOrganizationId = userOrganizations.First().OrganizationId;
                if (selectedOrganizationId != null)
                {
                    HttpContext.Session.SetString("SelectedOrganizationId", selectedOrganizationId.Value.ToString());
                }
            }
            else
            {
                var selectedOrganizationIdString = HttpContext.Session.GetString("SelectedOrganizationId");
                if (string.IsNullOrEmpty(selectedOrganizationIdString))
                {
                    // If the user has multiple organizations and none has been selected yet,
                    // redirect to the selection page
                    Response.Redirect(Url.Action("SelectOrganization", "Organizations") ?? "");
                }
                else
                {
                    selectedOrganizationId = Guid.Parse(selectedOrganizationIdString);
                }
            }

            return selectedOrganizationId;
        }

        protected async Task<InfoPanelViewModel> GenerateInfoPanel(Organization organization, Guid? userId)
        {
            IEnumerable<OrganizationParentViewModel> organizationParents = new List<OrganizationParentViewModel>();
            IEnumerable<LocationParentViewModel> organizationLocations = new List<LocationParentViewModel>();

            // Get Parent Location hierarchy
            if (organization.RefLocationId != null)
            {
                organizationLocations = (IEnumerable<LocationParentViewModel>)await GetLocationHierarchyAsync(organization.RefLocationId);
            }
            else
            {
                organizationLocations = null;
            }

            if (organization.OrganizationId != Guid.Empty)
            {
                organizationParents = (IEnumerable<OrganizationParentViewModel>)await GetParentHierarchyAsync(organization.OrganizationId);
            }
            else
            {
                organizationParents = null;
            }

            var organizationHierarchy = organization.OrganizationId != Guid.Empty
                ? await GetBreadcrumbAsync(organization.OrganizationId)
                : new List<Organization>();

            return new InfoPanelViewModel
            {
                CurrentLevel = 0,
                CurrentController = "Organizations",
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationType = organization.OrganizationTypes!.OrganizationType,
                Code = organization.Code,
                IsOrganizationUnit = organization.IsOrganizationUnit,
                OrganizationParent = organizationHierarchy,
                LocationParent = organizationLocations,
                UserOrganizationId = userId.HasValue && userId.Value == organization.OrganizationId ? organization.OrganizationId : null,
            };
        }


        // GetLocationHierarchyAsync is an asynchronous method that queries the database for location information,
        // organizes that information into a hierarchical structure, and returns it.
        public async Task<IEnumerable> GetLocationHierarchyAsync(string refLocationId)
        {
            // The method starts by querying all locations from the database, including their types and parent locations.
            var allLocations = await _context.Locations
                .Include(x => x.LocationTypes)
                .Include(x => x.ParentLocations)
                .Select(x => new LocationParentViewModel
                {
                    Index = 1,
                    RefLocationId = x.RefLocationId,
                    LocationName = x.LocationName,
                    ParentLocationId = x.ParentLocationId,
                    ParentName = x.ParentLocations!.LocationName,
                    RefLocationTypeId = x.RefLocationTypeId,
                    LocationType = x.LocationTypes!.LocationType,
                    LocationLevel = x.LocationTypes.LocationLevel
                })
                .ToListAsync();

            // It then uses the ListLocations helper method to arrange these locations into a hierarchy based on the refLocationId parameter.
            var locations = ListLocations(allLocations, refLocationId);

            // Finally, it re-indexes these locations and sorts them in descending order.
            IEnumerable<LocationParentViewModel> sortedLocations = locations
                .Select((x, index) => new LocationParentViewModel
                {
                    Index = index,
                    RefLocationId = x.RefLocationId,
                    LocationName = x.LocationName,
                    ParentLocationId = x.ParentLocationId,
                    ParentName = x.ParentName,
                    RefLocationTypeId = x.RefLocationTypeId,
                    LocationType = x.LocationType,
                    LocationLevel = x.LocationLevel
                })
                .OrderByDescending(x => x.Index)
                .ToList();

            return sortedLocations.ToList();
        }

        // ListLocations is a recursive helper method that creates a hierarchical list of locations based on a given location ID.
        public static IEnumerable<LocationParentViewModel> ListLocations(IEnumerable<LocationParentViewModel> list, string? id)
        {
            // It starts by finding the location in the list with the given ID.
            var current = list.Where(n => n.RefLocationId == id).FirstOrDefault();

            // If no such location exists, it returns an empty list.
            if (current == null) return Enumerable.Empty<LocationParentViewModel>();

            // Otherwise, it returns a new list that starts with the current location followed by the result of calling ListLocations with the current location's parent ID.
            return Enumerable.Concat(new[] { current }, ListLocations(list, current.ParentLocationId));
        }

        public async Task<IEnumerable> GetParentHierarchyAsync(Guid modelGuidId)
        {
            // Get Organization Hierarchy
            // List ALL Administrator Organizations and include current OrganizationId
            var adminOrganizations = await _context.Organizations
                .Include(x => x.ParentOrganizations)
                .Include(x => x.OrganizationTypes)
                .Where(x =>
                    x.IsOrganizationUnit == true ||
                    x.OrganizationId == modelGuidId
                    )
                .Select(x => new OrganizationParentViewModel
                {
                    Index = 1,
                    OrganizationId = x.OrganizationId,
                    OrganizationName = x.OrganizationName,
                    OrganizationType = x.OrganizationTypes!.OrganizationType,
                    ParentOrganizationId = x.ParentOrganizationId,
                    Parent = x.ParentOrganizations!.OrganizationName,
                    IsOrganizationUnit = x.IsOrganizationUnit
                })
                .ToListAsync();

            //Get Organization Hierarchy of current OrganizationId
            var parents = ListParents(adminOrganizations, modelGuidId);

            //Instatiate for adding index to "parents" and Sort Descending
            IEnumerable<OrganizationParentViewModel> sortedParents = new List<OrganizationParentViewModel>();

            //Add index and Sort descending to "parents"
            sortedParents = parents
                .Select((x, index) => new OrganizationParentViewModel
                {
                    Index = index,
                    OrganizationId = x.OrganizationId,
                    OrganizationName = x.OrganizationName,
                    OrganizationType = x.OrganizationType,
                    ParentOrganizationId = x.ParentOrganizationId,
                    Parent = x.Parent,
                    IsOrganizationUnit = x.IsOrganizationUnit
                })
                .OrderByDescending(x => x.Index)
                .ToList();

            return sortedParents.ToList();

        }

        public static IEnumerable<OrganizationParentViewModel> ListParents(IEnumerable<OrganizationParentViewModel> list, Guid? ID)
        {
            var current = list.Where(n => n.OrganizationId == ID).FirstOrDefault();

            if (current == null)
                return Enumerable.Empty<OrganizationParentViewModel>();

            return Enumerable.Concat(new[] { current }, ListParents(list, current.ParentOrganizationId));
        }
    }
}
