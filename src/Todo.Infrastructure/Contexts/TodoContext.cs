using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;

namespace Todo.Infrastructure.Contexts
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }
        public DbSet<TodoModel> Todos { get; set; }
    }
}
