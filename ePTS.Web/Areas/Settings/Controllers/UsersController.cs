using ePTS.Data;
using ePTS.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Models.ViewModels;
using ePTS.Entities.Identity;
using ePTS.Entities.Core;
using System.Data;
using ePTS.Entities.Gradebooks;

namespace ePTS.Web.Areas.Settings.Controllers
{
    [Area("Settings")]
    [Authorize(Policy = "RequireAdministratorRole")]
    public class UsersController : BaseController
    {
        //private readonly ApplicationDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        
        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<OrganizationsController> logger,
            RoleManager<ApplicationRole> roleManager
            ) : base(context, logger, userManager)
        {
            //_context = context;
            //_userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations", new { area = ""});
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);

            var users = await (from u in _context.ApplicationUsers
                               join r in _context.UserRoles on u.Id equals r.UserId
                               join ur in _context.Roles on r.RoleId equals ur.Id
                               join uo in _context.ApplicationUserOrganizations on u.Id equals uo.UserId
                               join o in _context.Organizations on uo.OrganizationId equals o.OrganizationId
                               join p1 in _context.Organizations on o.ParentOrganizationId equals p1.OrganizationId into p1joined
                               from p1subquery in p1joined.DefaultIfEmpty()
                               join p2 in _context.Organizations on p1subquery.ParentOrganizationId equals p2.OrganizationId into p2joined
                               from p2subquery in p2joined.DefaultIfEmpty()
                               join p3 in _context.Organizations on p2subquery.ParentOrganizationId equals p3.OrganizationId into p3joined
                               from p3subquery in p3joined.DefaultIfEmpty()
                               join p4 in _context.Organizations on p3subquery.ParentOrganizationId equals p4.OrganizationId into p4joined
                               from p4subquery in p4joined.DefaultIfEmpty()
                               where (o.OrganizationId == selectedOrganizationId || childOrganizationIds.Contains(o.OrganizationId))
                                     // Don't show the current user in the list
                                     && userId != u.Id
                               select new ApplicationUsersViewModel
                               {
                                   MoGE = p1subquery.RefOrganizationTypeId == 2 ? p1subquery.OrganizationName : p2subquery.RefOrganizationTypeId == 2 ? p2subquery.OrganizationName : p3subquery.RefOrganizationTypeId == 2 ? p3subquery.OrganizationName : p4subquery.RefOrganizationTypeId == 2 ? p4subquery.OrganizationName : null,
                                   Province = p1subquery.RefOrganizationTypeId == 3 ? p1subquery.OrganizationName : p2subquery.RefOrganizationTypeId == 3 ? p2subquery.OrganizationName : p3subquery.RefOrganizationTypeId == 3 ? p3subquery.OrganizationName : p4subquery.RefOrganizationTypeId == 3 ? p4subquery.OrganizationName : null,
                                   District = p1subquery.RefOrganizationTypeId == 4 ? p1subquery.OrganizationName : p2subquery.RefOrganizationTypeId == 4 ? p2subquery.OrganizationName : p3subquery.RefOrganizationTypeId == 4 ? p3subquery.OrganizationName : p4subquery.RefOrganizationTypeId == 4 ? p4subquery.OrganizationName : null,
                                   Zone = p1subquery.RefOrganizationTypeId == 5 ? p1subquery.OrganizationName : p2subquery.RefOrganizationTypeId == 5 ? p2subquery.OrganizationName : p3subquery.RefOrganizationTypeId == 5 ? p3subquery.OrganizationName : p4subquery.RefOrganizationTypeId == 5 ? p4subquery.OrganizationName : null,
                                   Organization = o.OrganizationName,
                                   OrganizationTypeId = o.RefOrganizationTypeId,
                                   OrganizationType = o.OrganizationTypes.OrganizationType,
                                   Id = u.Id,
                                   UserName = u.UserName,
                                   Email = u.Email,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   LockoutEnabled = u.LockoutEnabled,
                                   LockoutEnd = u.LockoutEnd,
                                   AccessFailedCount = u.AccessFailedCount,
                                   Role = ur.Name
                               }
                      )
                      
                      .OrderBy(x => x.MoGE).ThenBy(x => x.Province).ThenBy(x => x.District).ThenBy(x => x.Zone).ThenBy(x => x.Organization)
                      .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return base.View(model: new ApplicationUsersViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Organization = string.Join(", ", user!.UserOrganizations!.Select(u => u.Organizations!.OrganizationName)) ?? null,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                ModifiedBy = user.ModifiedBy,
                ModifiedDate = user.ModifiedDate,
                CreatedBy = user.CreatedBy,
                CreatedDate = user.CreatedDate,
                AccessFailedCount = user.AccessFailedCount,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd

            });
        }

        public async Task<IActionResult> Create()
        {
            ViewData["RoleId"] = new SelectList(_roleManager.Roles
                .Where(x => x.Name != "Administrator")
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }), "Id", "Name");

            ViewData["OrganizationId"] = new SelectList(_context.Organizations
                .Where(x => x.IsOrganizationUnit == true), "OrganizationId", "OrganizationName");

            

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations", new { area = ""});
            }


            // *** For JSTree ***
            //Logged User OrganizationId
            //Guid? userOrganizacionId = (await _userManager.GetUserAsync(HttpContext.User))?.OrganizationId;

            

            //Get Logged User Organization
            var organization = await _context.Organizations
                    .SingleOrDefaultAsync(o => o.OrganizationId == selectedOrganizationId);

            if (organization == null)
            {
                return NotFound();
            }

            //Returns the top hiearchy organization for JSTree
            ViewData["ParentOrganizationId"] = organization.OrganizationId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUsersRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    
                    
                };

                var createUser = await _userManager.CreateAsync(user, model.Password);

                if (createUser.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, code);
                    

                    ApplicationRole role = await _roleManager.FindByIdAsync(model.RoleId.ToString());

                    if (role != null)
                    {
                        var addRoles = await _userManager.AddToRoleAsync(user, role.Name);

                        ApplicationUserOrganization userOrganization = new()
                        {
                            OrganizationId = model.OrganizationId,
                            UserId = user.Id,
                            //
                            //CreatedBy = ,
                            CreatedDate = DateTime.Now
                            
                        };

                        _context.Add(userOrganization);
                        await _context.SaveChangesAsync();

                        if (!addRoles.Succeeded)
                        {

                            AddErrors(addRoles);

                            ViewData["RoleId"] = new SelectList(_roleManager.Roles
                                .Where(x => x.Name != "Administrator")
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Name
                                }), "Id", "Name");

                            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");

                            return RedirectToAction(nameof(Index));
                        }
                    }

                    TempData["messageType"] = "success";
                    TempData["messageTitle"] = "RECORD CREATED";
                    TempData["message"] = "New record created successfully";

                    return RedirectToAction(nameof(Index));
                }

                AddErrors(createUser);
            }

            ViewData["RoleId"] = new SelectList(_roleManager.Roles
                .Where(x => x.Name != "Administrador")
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }), "Id", "Name");

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);
//            Guid? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            if (user == null)
            {
                return NotFound();
            }

            ApplicationUsersEditViewModel model = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                RoleId = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id.ToString(),
                //RoleId = _userManager.GetRolesAsync(user).Result.FirstOrDefault() != null ? _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.FirstOrDefault()).Id.ToString() : null,
                LockoutEnabled = user.LockoutEnabled
            };

            ViewData["RoleId"] = new SelectList(_roleManager.Roles
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }), "Id", "Name", model.RoleId);

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations", new { area = "" });
            }
            var organization = await _context.Organizations
                    .SingleOrDefaultAsync(o => o.OrganizationId == selectedOrganizationId);

            if (organization == null)
            {
                return NotFound();
            }

            //Returns the top hiearchy organization for JSTree
            ViewData["ParentOrganizationId"] = organization.OrganizationId;

            ViewData["OrganizationId"] = _context.ApplicationUserOrganizations.Where(x => x.UserId == Guid.Parse(id)).Select(x => x.OrganizationId).FirstOrDefault();
                //new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUsersEditViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(id);
                
                if (user == null)
                {
                    return NotFound();
                }

                var userOrganization = _context.ApplicationUserOrganizations
                    .Where(x => x.UserId == Guid.Parse(id) && x.OrganizationId == model.OrganizationIdInitialValue)
                    .FirstOrDefault();

                if (userOrganization == null)
                {
                    return NotFound();
                }

                if (model.OrganizationId != model.OrganizationIdInitialValue)
                {
                    //userOrganization.OrganizationId = model.OrganizationId;
                    //_context.Update(userOrganization);
                    _context.Remove(userOrganization);
                    await _context.SaveChangesAsync();

                    // Create a new record with the updated OrganizationId
                    var newUserOrganization = new ApplicationUserOrganization
                    {
                        UserId = user.Id,
                        OrganizationId = model.OrganizationId
                    };

                    _context.Add(newUserOrganization);
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.LockoutEnabled = model.LockoutEnabled;

                string? existingRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? null;
                string? existingRoleId = existingRole != null ? _roleManager.Roles.Single(r => r.Name == existingRole).Id.ToString() : null;
                IdentityResult result = await _userManager.UpdateAsync(user);
                result = await _userManager.SetLockoutEnabledAsync(user, false);
                if (model.LockoutEnabled)
                {
                    result = await _userManager.SetLockoutEnabledAsync(user, true);
                }
                else
                {
                    result = await _userManager.SetLockoutEnabledAsync(user, false);
                }

                if (result.Succeeded)
                {
                    if (existingRoleId != model.RoleId)
                    {
                        if (existingRoleId == null)
                        {

                            ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                            if (applicationRole != null)
                            {
                                IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                if (newRoleResult.Succeeded)
                                {
                                    //ToDo
                                }
                            }
                        }
                        else
                        {
                            IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                //ToString
                                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        //ToDo
                                    }
                                }
                            }
                        }

                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoleId"] = new SelectList(_roleManager.Roles
                .Select(x => new
                {
                    x.Id,
                    x.Name
                }), "Id", "Name", model.RoleId);

            ViewData["OrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "OrganizationName");

            return View(model);
        }

        public async Task<IActionResult> Reset(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ApplicationUsersResetPasswordViewModel model = new ApplicationUsersResetPasswordViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = Guid.Parse(id)
            };

            return View(model);

        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset(string id, ApplicationUsersResetPasswordViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (result.Succeeded)
                {
                    TempData["messageType"] = "success";
                    TempData["messageTitle"] = "PASSWORD RESET";
                    TempData["message"] = "Password reset was successful";
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }
            return View(model);
        }


        public async Task<IActionResult> Unlock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.LockoutEnd == null)
            {
                TempData["messageType"] = "warning";
                TempData["messageTitle"] = "USER IS NOT LOCKED!";
                TempData["message"] = "No updates where made to the user";
                return RedirectToAction(nameof(Details), new { id });
            }

            ApplicationUsersViewModel model = new ApplicationUsersViewModel
            {
                Id = Guid.Parse(id),
                UserName = user.UserName,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                AccessFailedCount = user.AccessFailedCount
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unlock(string id, ApplicationUsersViewModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.LockoutEnd == null)
            {
                TempData["messageType"] = "warning";
                TempData["messageTitle"] = "USER IS NOT LOCKED!";
                TempData["message"] = "No updates where made to the user";
                return RedirectToAction(nameof(Details), new { id });
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null); ;

            if (result.Succeeded)
            {
                TempData["messageType"] = "success";
                TempData["messageTitle"] = "USER ACCOUNT UNLOCKED";
                TempData["message"] = "User account unlocked successfully";
                return RedirectToAction(nameof(Index));
            }
            AddErrors(result);

            return View(model);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return base.View(model: new ApplicationUsersViewModel
            {

                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,
                AccessFailedCount = user.AccessFailedCount

            });

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["messageType"] = "danger";
                TempData["messageTitle"] = "RECORD DELETED";
                TempData["message"] = "Record deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            return base.View(model: new ApplicationUsersViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //Organization = user.OrganizationId == null ? null : _context.Organizations.Single(o => o.OrganizationId == user.OrganizationId).OrganizationName,
                Role = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Name ?? null,

            });
        }

        private bool IdentityUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == Guid.Parse(id));
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
