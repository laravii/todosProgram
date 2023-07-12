using Todo.Domain.Models;

namespace Todo.Domain.RepositoryTodo.Domain.Repositories
{
    public interface ITodoRepository
    {
        void Add(TodoModel todoItem);
        void Update(TodoModel todoItem);
        void Delete(TodoModel todoItem);
        TodoModel GetById(int id);
        IEnumerable<TodoModel> GetAll();
    }
}
