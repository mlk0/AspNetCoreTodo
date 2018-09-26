using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers {
    public class TodoController : Controller {
        public TodoController (ITodoItemService todoItemService) {
            TodoItemService = todoItemService;
        }

        public ITodoItemService TodoItemService { get; }

        public async Task<IActionResult> Index () {
            // TODO: Your code here

            // return Ok();
            // Get to-do items from database
            var incomleteTodoItems = await this.TodoItemService.GetIncompleteItemsAsync ();
            var todoViewModel = new TodoViewModel { Items = incomleteTodoItems };

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

            if(!ModelState.IsValid){
                return RedirectToAction("Index");
            }

            var addNewItemResult = await this.TodoItemService.AddNewTodoItem(todoItem);
            if(!addNewItemResult){
                return BadRequest("unable to add item");
            }

            return RedirectToAction("Index");
        }

    }
}