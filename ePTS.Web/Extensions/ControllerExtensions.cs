using ePTS.Data;
using ePTS.Entities.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ePTS.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<List<Organization>?> LoadOrganizations(this Controller controller, ApplicationDbContext context)
        {
            //Guid? userId2 = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            //get userid from Claims
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? userIdString = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return null;
            }
            Guid userId = Guid.Parse(userIdString);
            //var userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userOrganizations = await context.ApplicationUserOrganizations
                .Where(uo => uo.UserId == userId)
                .Select(uo => uo.OrganizationId)
                .ToListAsync();

            return  context.Organizations
                .Where(o => o.ParentOrganizationId == null && userOrganizations.Contains(o.OrganizationId))
                .ToList();
        }
    }
}
