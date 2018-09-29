using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspNetCoreTodo.Controllers {
    [Authorize]
    public class TodoController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        public ILogger<TodoController> _logger { get; }

        public TodoController (ITodoItemService todoItemService, UserManager<IdentityUser> userManager, ILogger<TodoController> logger) {
            this._logger = logger;
            TodoItemService = todoItemService;
            this._userManager = userManager;

        }

        public ITodoItemService TodoItemService { get; }

        public async Task<IActionResult> Index () {

            //TODO: even when the user is not logged in the User object will always be not null
            if (User == null) {
                _logger.LogWarning ($"User is null");
            }

            var currentUser = await _userManager.GetUserAsync (User);
            if (currentUser == null) {
                _logger.LogError ($"currentUser is null");
                return Challenge ();
            }

            _logger.LogDebug ($"currentUser : {JsonConvert.SerializeObject(currentUser, Newtonsoft.Json.Formatting.Indented)}");

            // Get to-do items from database
            //  var incompleteTodoItems = await this.TodoItemService.GetIncompleteItemsAsync();
            TodoItem[] incompleteTodoItems = await this.TodoItemService.GetIncompleteItemsForUserAsync(currentUser);
            
            var todoViewModel = new TodoViewModel { Items = incompleteTodoItems };

            return View (todoViewModel);
        }

        //other option is to not use async controller method but to simply use the original synchronos that is blocking until the Result is not being materialized
        //before resuming to build the todoViewModel
        public IActionResult IncompleteTodoItems () {
            var result = this.TodoItemService.GetIncompleteItemsAsync ().Result;
            var todoViewModel = new TodoViewModel { Items = result };

            return View ("Index", todoViewModel); //View(result);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem (TodoItem todoItem) {

            if (!ModelState.IsValid) {
                return RedirectToAction ("Index");
            }

            var currentUser = await this._userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();


            // var addNewItemResult = await this.TodoItemService.AddNewTodoItem (todoItem);
            bool addNewItemResult = await this.TodoItemService.AddNewTodoItemForUser (todoItem, currentUser);

            if (!addNewItemResult) {
                return BadRequest ("unable to add item");
            }

            return RedirectToAction ("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone (string id) {
            if (String.IsNullOrWhiteSpace (id)) {
                return RedirectToAction ("Index");
            }

            var currentUser = await this._userManager.GetUserAsync(User);
            if(currentUser == null) return Challenge();

            // bool markTodoCompletedSuccessfully = await this.TodoItemService.MarkTodoDone (id);
            bool markTodoCompletedSuccessfully = await this.TodoItemService.MarkTodoDoneByUser(id, currentUser);

            if (!markTodoCompletedSuccessfully) {
                return BadRequest ("Failed Marking todo item completed");

            }

            return RedirectToAction ("Index");
        }

    }
}