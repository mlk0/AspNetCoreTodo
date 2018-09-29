using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Services {
    public class TodoItemService : ITodoItemService {
        private readonly ApplicationDbContext _applicationDbContext;

        public TodoItemService (ApplicationDbContext context, ApplicationDbContext applicationDbContext) {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AddNewTodoItem(TodoItem todoItem)
        {
             todoItem.Id = Guid.NewGuid();             

            _applicationDbContext.Items.Add(todoItem);
            var result = await _applicationDbContext.SaveChangesAsync();
            return result == 1;
        }

        public async Task<bool> AddNewTodoItemForUser(TodoItem todoItem, IdentityUser currentUser)
        {
            todoItem.Id = Guid.NewGuid();
            todoItem.UserId = currentUser.Id;

            this._applicationDbContext.Items.Add(todoItem);
            var rowsAffected = await this._applicationDbContext.SaveChangesAsync();
            return rowsAffected == 1;
        }

        public Task<TodoItem[]> GetIncompleteItemsAsync () {

            //the original response is IQueriable<T> and one option of going in async mode would be just to evaluate it and return an array
            var incompleteItems =_applicationDbContext.Items.Where(i=>i.IsDone==false);

            //however, even better option would be to respond with an awaitable result
            //each of the array, list, enumerable and dictionary have an async corresponding version
            // incompleteItems.ToArrayAsync();
            // incompleteItems.ToAsyncEnumerable();
            // incompleteItems.ToDictionaryAsync();
            // incompleteItems.ToListAsync();

            // the async versions are also triggering evaluation of the queriable
            // so when executing they will actually execute the sql statement agains the database
            // but instead of blocking the thread, the async versions since they are awaitable, 
            // they enable signaling the caller when they are ready to hand over the final result so the caller can do other things in meantime

            var result = incompleteItems.ToArrayAsync();
            return result;
        }

        public Task<TodoItem[]> GetIncompleteItemsForUserAsync(IdentityUser currentUser)
        {
            var incompleteTodItemsForUser = this._applicationDbContext.Items.Where(c=>c.IsDone==false && currentUser.Id == c.UserId);
            return incompleteTodItemsForUser.ToArrayAsync();
        }

        public async Task<bool> MarkTodoDone(string id)
        {
            var todoItem = await _applicationDbContext.Items.FirstOrDefaultAsync(c=>c.Id == new Guid(id));
            todoItem.IsDone = true;
            var saveCompletedTodoItem = await _applicationDbContext.SaveChangesAsync();
            return saveCompletedTodoItem == 1;
        }
    }
}