using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<TodoItem[]> GetIncompleteItemsAsync();
        Task<bool> AddNewTodoItem(TodoItem todoItem);
        Task<bool> MarkTodoDone(string id);
        Task<TodoItem[]> GetIncompleteItemsForUserAsync(IdentityUser currentUser);
        Task<bool> AddNewTodoItemForUser(TodoItem todoItem, IdentityUser currentUser);
    }
}