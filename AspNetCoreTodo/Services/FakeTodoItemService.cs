using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
    public class FakeTodoItemService : ITodoItemService
    {
        public Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var incompleteTodoItems = new TodoItem[]{
                new TodoItem{
                    Title = "Learn ASP.NET Core",
                DueAt = DateTimeOffset.Now.AddDays(1)
                },
                new TodoItem{
                    Title = "Build awesome apps",
                DueAt = DateTimeOffset.Now.AddDays(2)
                }
            };

            var taskResponse = Task.FromResult(incompleteTodoItems);

            return taskResponse;
        }
    }
}