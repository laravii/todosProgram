using Bogus;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Models;
using Todo.Domain.RepositoryTodo.Domain.Repositories;
using Todo.Infrastructure.Contexts;
using Todo.Infrastructure.Repositories;
using Todo.Tests.Builders;

namespace Todo.Tests.Infrastructure.Repositories
{
    public class TodoRepositoryTests
    {
        private readonly Faker _faker;
        private readonly TodoContext _context;

        public TodoRepositoryTests()
        {
            _faker = new Faker();
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TodosDB")
                .Options;
            _context = new (options);
        }

        [Fact]
        public void Add_ValidTodoItem_CallsSaveChanges()
        {
            // Arrange
            var todoItem = new TodoModelBuilder().Build();

            // Act
            var sut = GetSut();
            sut.Add(todoItem);

            // Assert
            Assert.Equal(1, _context.Todos.Count());
        }

        [Fact]
        public void GetById_ExistingId_ReturnsTodoItem()
        {
            // Arrange
            var existingId = 1;
            var todoItem = new TodoModelBuilder().WithId(existingId).Build();
            todoItem.Id = existingId;
            _context.Todos.Add(todoItem);
            _context.SaveChanges();

            // Act
            var sut = GetSut();
            var result = sut.GetById(existingId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingId, result.Id);

            Dispose();
        }

        [Fact]
        public void GetById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var existingId = 320;
            var todoItem = new TodoModelBuilder().WithId(existingId).Build();
            todoItem.Id = existingId;
            _context.Todos.Add(todoItem);
            _context.SaveChanges();

            // Act
            var sut = GetSut();
            sut.Delete(todoItem);

            var result = sut.GetById(existingId);
            // Assert
            Assert.Null(result);
            Dispose();
        }


        [Fact]
        public void Delete_With_Success()
        {
            // Arrange
            var nonExistingId = 99;

            // Act
            var sut = GetSut();
            var result = sut.GetById(nonExistingId);

            // Assert
            Assert.Null(result);
            Dispose();
        }


        [Fact]
        public void GetAll_ReturnsAllTodoItems()
        {
            // Arrange
            var todoItems = new List<TodoModel>() {
            new TodoModelBuilder().Build(),
            new TodoModelBuilder().Build()
            };

            _context.Todos.AddRange(todoItems);
            _context.SaveChanges();

            // Act
            var sut = GetSut();
            var result = sut.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todoItems.Count, result.Count());
            Dispose();
        }

        [Fact]
        public void GetAll_Without_Data()
        {
            // Act
            var sut = GetSut();
            var result = sut.GetAll();

            // Assert
            Assert.Empty(result);
            Dispose();
        }

        // Helper methods
        private ITodoRepository GetSut() =>
            new TodoRepository(_context);

        private void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
