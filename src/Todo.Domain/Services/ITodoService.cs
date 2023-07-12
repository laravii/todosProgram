using Todo.Domain.Models;
using Todo.Domain.Models.Requests;
using Todo.Domain.Models.Results;
using Todo.Domain.Requests;
using Todo.Domain.Responses;

namespace Todo.Domain.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoModel> GetAllTodos();
        TodoModel AddTodoItem(InsertTodoRequest request);
        RepositoryResult UpdateTodoItem(int id, UpdateTodoRequest request);
        RepositoryResult UpdateTodoStatus(int id, UpdateStatusRequest request);
        RepositoryResult DeleteTodoItem(int id);
        bool TodoExists(int id);
        bool TodoExists(string id);
    }
}
