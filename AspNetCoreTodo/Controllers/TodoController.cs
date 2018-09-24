using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers {
    public class TodoController : Controller {
        public IActionResult Index () {
            // TODO: Your code here
            // Get to-do items from database

            // Put items into a model

            // Render view using the model
            var todoViewModel = new TodoViewModel () {
                Items = new TodoItem[] { new TodoItem { Id = Guid.NewGuid (), IsDone = false, Title = "Do something", DueAt = DateTimeOffset.Now } }
            };
            return View (todoViewModel);
        }

    }
}