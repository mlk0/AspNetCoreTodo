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

    }
}