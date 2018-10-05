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

    [Authorize (Roles="Admin")]
    public class ManageUsersController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ManageUsersController> _logger;

        public ManageUsersController (UserManager<IdentityUser> userManager,
            ILogger<ManageUsersController> logger
        ) {
            this._userManager = userManager;
            this._logger = logger;
        }

        //[Authorize(Roles="manager")]
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