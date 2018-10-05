using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspNetCoreTodo.Controllers {

    [Authorize (Roles = "Admin")]
    public class ManageUsersController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ManageUsersController> _logger;

        public ManageUsersController (UserManager<IdentityUser> userManager,
            ILogger<ManageUsersController> logger
        ) {
            this._userManager = userManager;
            this._logger = logger;
        }

        public async Task<IActionResult> DeleteUser (string userId) {

            this._logger.LogDebug ($"DeleteUser - userId : {userId}");
            var user = await this._userManager.Users.FirstOrDefaultAsync (u => u.Id == userId);
            if (user != null) {
                var deleteUserResult = await this._userManager.DeleteAsync (user);
                if (!deleteUserResult.Succeeded) {
                    foreach (var error in deleteUserResult.Errors) {
                        ModelState.TryAddModelError (error.Code, error.Description);
                    }
                }
            }
            else{
                ModelState.TryAddModelError ("USER_NOT_FOUND", "User referenced by id NOT FOUND");
            }

            return RedirectToAction ("GetUsers");
        }

        public async Task<IActionResult> GetUsers () {

            var currentUser = await _userManager.GetUserAsync (User);
            var isAdmin = await _userManager.IsInRoleAsync (currentUser, Constants.AdministratorRole);
            this._logger.LogDebug ($"currentUser : {JsonConvert.SerializeObject(currentUser)}, isAdmin : {isAdmin}");

            var isManager = await _userManager.IsInRoleAsync (currentUser, Constants.ManagerRole);
            this._logger.LogDebug ($"currentUser : {JsonConvert.SerializeObject(currentUser)}, isManager : {isManager}");

            var admins = await this._userManager.GetUsersInRoleAsync (Constants.AdministratorRole);
            var allUsers = await this._userManager.Users.ToArrayAsync ();

            var viewModel = new ManageUsersViewModel {
                Administrators = admins.ToArray (),
                AllUsers = allUsers
            };

            this._logger.LogDebug (JsonConvert.SerializeObject (viewModel));

            return View ("GetUsers", viewModel);
        }

    }
}