using Todo.Domain.Models;
using Todo.Domain.RepositoryTodo.Domain.Repositories;
using Todo.Infrastructure.Contexts;

namespace Todo.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public void Add(TodoModel todoItem)
        {
            _context.Todos.Add(todoItem);
            _context.SaveChanges();
        }

        public void Update(TodoModel todoItem)
        {
            _context.Entry(todoItem).CurrentValues.SetValues(todoItem);
            _context.SaveChanges();
        }

        public void Delete(TodoModel todoItem)
        {
            _context.Todos.Remove(todoItem);
            _context.SaveChanges();
        }

        public TodoModel GetById(int id)
        {
            return _context.Todos.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<TodoModel> GetAll()
        {
            var todos = _context.Todos;

            return todos;
        }
    }
}
