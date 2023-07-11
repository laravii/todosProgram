using AutoMapper;
using Moq;
using Todo.Domain.Commons;
using Todo.Domain.Models;
using Todo.Domain.Models.Profiles;
using Todo.Domain.Models.Requests;
using Todo.Domain.Models.Results;
using Todo.Domain.RepositoryTodo.Domain.Repositories;
using Todo.Domain.Services;
using Todo.Infrastructure.Services;
using Todo.Tests.Builders;

namespace Todo.Tests.Todo.Infrastructure.Services
{
    public class TodoServiceTests
    {
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly IMapper _mapper;

        public TodoServiceTests()
        {
            _todoRepositoryMock = new();

            var mockMaper = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile(new TodoProfile());
                });

            _mapper = mockMaper.CreateMapper();
        }

        [Fact]
        public void GetAllTodos_Return_Items()
        {
            var todoItems = new List<TodoModel>() {
            new TodoModelBuilder().Build(),
            new TodoModelBuilder().Build()
            };

            _todoRepositoryMock.Setup(x => x.GetAll())
                .Returns(todoItems);

            var sut = GetSut();
            var result = sut.GetAllTodos();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void AddTodoItem_Sucess()
        {
            var request = new InsertTodoRequestBuilder().Build();

            var sut = GetSut();
            var result = sut.AddTodoItem(request);

            Assert.NotNull(result);
            _todoRepositoryMock.Verify(x => x.Add(It.IsAny<TodoModel>()), Times.Once);
        }


        [Fact]
        public void UpdateItem_Sucess()
        {
            var id = 1;
            _todoRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new TodoModelBuilder().WithId(id).Build());

            var request = new UpdateTodoRequestBuilder().Build();

            var sut = GetSut();
            var result = sut.UpdateTodoItem(id, request);

            Assert.True(result.Success);
            _todoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
        }

        [Fact]
        public void Shuold_Not_UpdateItem_NotFound()
        {
            var id = 1;
            var request = new UpdateTodoRequestBuilder().Build();

            var sut = GetSut();
            var result = sut.UpdateTodoItem(id, request);

            Assert.False(result.Success);
            Assert.Equal(RepositoryResult.NotFound, result);
            _todoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Never);
        }

        [Fact]
        public void Shold_Not_UpdateItem_When_Done()
        {
            var id = 1;
            _todoRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new TodoModelBuilder().WithId(id).WithStatus(TodoStatus.Done).Build());

            var request = new UpdateTodoRequestBuilder().Build();

            var sut = GetSut();
            var result = sut.UpdateTodoItem(id, request);

            Assert.False(result.Success);
            Assert.Equal(RepositoryResult.AlreadyDone, result);
            _todoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Never);
        }

        [Fact]
        public void Shold_DeleteTodoItem_Success()
        {
            var id = 1;
            _todoRepositoryMock
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new TodoModelBuilder().WithId(id).Build());
           
            var sut = GetSut();
            sut.DeleteTodoItem(id);

            _todoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _todoRepositoryMock.Verify(x => x.Delete(It.IsAny<TodoModel>()), Times.Once);
        }

        [Fact]
        public void Shold_Not_DeleteTodoItem_When_Not_Exists_Item()
        {
            var id = 1;
            var sut = GetSut();
            sut.DeleteTodoItem(id);

            _todoRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _todoRepositoryMock.Verify(x => x.Delete(It.IsAny<TodoModel>()), Times.Never);
        }

        [Fact]
        public void TodoExists_Return_True()
        {
            var id = 1;
            _todoRepositoryMock
                           .Setup(x => x.GetById(It.IsAny<int>()))
                           .Returns(new TodoModelBuilder().WithId(id).Build());

            var sut = GetSut();
            var result = sut.TodoExists(id);

            Assert.True(result);
        }

        [Fact]
        public void TodoExists_Return_False()
        {
            var id = 1;

            var sut = GetSut();
            var result = sut.TodoExists(id);

            Assert.False(result);
        }

        [Fact]
        public void TodoExists_String_Return_True()
        {
            var title = "Test Title";
            var todoItems = new List<TodoModel>() {
            new TodoModelBuilder().Build(),
            new TodoModelBuilder()
                           .WithTitle("Test Title")
                           .Build()
            };

            _todoRepositoryMock
                           .Setup(x => x.GetAll())
                           .Returns(todoItems);

            var sut = GetSut();
            var result = sut.TodoExists(title);

            Assert.True(result);
        }

        [Fact]
        public void TodoExists_String_Return_False_When_Not_Exists()
        {
            var title = "Test Title";
            var todoItems = new List<TodoModel>() 
            {
                new TodoModelBuilder().Build()
            };

            _todoRepositoryMock
                           .Setup(x => x.GetAll())
                           .Returns(todoItems);

            var sut = GetSut();
            var result = sut.TodoExists(title);

            Assert.False(result);
        }

        [Fact]
        public void TodoExists_String_Return_False_When_Empty_DataBase()
        {
            var title = "Test Title";
            var todoItems = new List<TodoModel>();

            _todoRepositoryMock
                           .Setup(x => x.GetAll())
                           .Returns(todoItems);

            var sut = GetSut();
            var result = sut.TodoExists(title);

            Assert.False(result);
        }

        [Fact]
        public void Shold_Update_Status()
        {
            var id = 1;
            var request = new UpdateStatusRequest() { Status = "doing"};

            _todoRepositoryMock
                           .Setup(x => x.GetById(It.IsAny<int>()))
                           .Returns(new TodoModelBuilder().WithId(id).Build());
            
            var sut = GetSut();
            var result = sut.UpdateTodoStatus(id, request);

            Assert.True(result.Success);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Once);
        }

        [Fact]
        public void SholdNot_Update_Status_When_Done()
        {
            var id = 1;
            var request = new UpdateStatusRequest() { Status = "doing" };

            _todoRepositoryMock
                           .Setup(x => x.GetById(It.IsAny<int>()))
                           .Returns(new TodoModelBuilder().WithId(id).WithStatus(TodoStatus.Done).Build());

            var sut = GetSut();
            var result = sut.UpdateTodoStatus(id, request);

            Assert.False(result.Success);
            Assert.Equal(result, RepositoryResult.AlreadyDone);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Never);
        }
        
        [Fact]
        public void SholdNot_Update_Status_Not_Exists()
        {
            var id = 1;
            var request = new UpdateStatusRequest() { Status = "doing" };

            var sut = GetSut();
            var result = sut.UpdateTodoStatus(id, request);

            Assert.False(result.Success);
            Assert.Equal(result, RepositoryResult.NotFound);
            _todoRepositoryMock.Verify(x => x.Update(It.IsAny<TodoModel>()), Times.Never);
        }

        private ITodoService GetSut() =>
            new TodoService(_todoRepositoryMock.Object, _mapper);
    }
}
