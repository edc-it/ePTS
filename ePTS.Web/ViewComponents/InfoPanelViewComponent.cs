using ePTS.Data;
using ePTS.Entities.Identity;
using ePTS.Models.ViewModels;
using ePTS.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ePTS.Web.ViewComponents
{
    public class InfoPanelViewComponent : ViewComponent
    {
        // A reference to the application's DbContext is stored in a protected field so that derived classes can access it.
        protected readonly ApplicationDbContext _context;

        // A reference to the application's UserManager is stored in a protected field so that derived classes can access it.
        protected readonly UserManager<ApplicationUser> _userManager;

        // A reference to the application's ILogger is stored in a protected field so that derived classes can write logs.
        protected readonly ILogger _logger;

        public InfoPanelViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<InfoPanelViewComponent> logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(InfoPanelViewModel model)
        {
            var items = await GetItemsAsync(model);
            return View(items);
        }

        private async Task<InfoPanelViewModel?> GetItemsAsync(InfoPanelViewModel model)
        {

            return model;
        }

        #region Helpers

        // GetLocationHierarchyAsync is an asynchronous method that queries the database for location information,
        // organizes that information into a hierarchical structure, and returns it.
        public async Task<IEnumerable> GetLocationHierarchyAsync(string? refLocationId)
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
                .Include(x => x.OrganizationTypes)
                .Include(x => x.ParentOrganizations)
                .Where(x =>
                    x.IsOrganizationUnit == true ||
                    //x.RefOrganizationTypeId != 6 ||
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

        #endregion
    }
}
