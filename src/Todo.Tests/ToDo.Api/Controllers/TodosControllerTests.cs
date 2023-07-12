using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Todo.Domain.Commons;
using Todo.Domain.Models;
using Todo.Domain.Models.Profiles;
using Todo.Domain.Models.Requests;
using Todo.Domain.Models.Results;
using Todo.Domain.Requests;
using Todo.Domain.Responses;
using Todo.Domain.Services;
using Todo.Tests.Builders;
using ToDo.Api.Controllers.Controllers;

namespace Todo.Tests.ToDo.Api.Controllers
{
    public class TodosControllerTests
    {
        private readonly Mock<ITodoService> _todoServiceMock;
        private readonly IMapper _mapper;

        public TodosControllerTests()
        {
            _todoServiceMock = new();

            var mockMaper = new MapperConfiguration(
                cfg =>
                    {
                        cfg.AddProfile(new TodoProfile());
                    });

            _mapper = mockMaper.CreateMapper();
        }

        [Fact]
        public void Should_Get()
        {
            var todoItems = new List<TodoModel>()
            {
                new TodoModelBuilder().Build(),
            };

            _todoServiceMock.Setup(x => x.GetAllTodos()).Returns(todoItems);

            var sut = GetSut();
            var result = sut.Get() as OkObjectResult;
            var items = Assert.IsType<List<TodoResponse>>(result?.Value);

            Assert.Single(items);
        }

        [Fact]
        public void Should_Get_Later()
        {
            var todoItems = new List<TodoModel>()
            {
                new TodoModelBuilder().WithDaysToFinish(-3).Build()
            };

            _todoServiceMock.Setup(x => x.GetAllTodos()).Returns(todoItems);

            var sut = GetSut();
            var result = sut.Get() as OkObjectResult;
            var items = Assert.IsType<List<TodoResponse>>(result?.Value);

            Assert.Single(items);
            Assert.NotEmpty(items[0].Observation);
        }

        [Fact]
        public void Should_Get_When_No_Data()
        {
            var todoItems = new List<TodoModel>();

            _todoServiceMock.Setup(x => x.GetAllTodos()).Returns(todoItems);

            var sut = GetSut();
            var result = sut.Get() as OkObjectResult;
            var items = Assert.IsType<List<TodoResponse>>(result?.Value);

            Assert.False(items.Any());
        }

        [Fact]
        public void Should_Post()
        {
            var request = new InsertTodoRequestBuilder().Build();

            _todoServiceMock.Setup(x => x.AddTodoItem(It.IsAny<InsertTodoRequest>()))
                .Returns(new TodoModelBuilder()
                        .WithTitle(request.Title)
                        .WithStatus(TodoStatus.Todo)
                        .Build());

            var sut = GetSut();
            var result = sut.Post(request) as CreatedAtActionResult;
            var items = Assert.IsType<TodoResponse>(result?.Value);

            Assert.NotNull(items);
            _todoServiceMock.Verify(x => x.AddTodoItem(It.IsAny<InsertTodoRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldNot_Post_When_Duplicated_Item()
        {
            var request = new InsertTodoRequestBuilder().Build();

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<string>()))
                .Returns(true);

            var sut = GetSut();
            var result = sut.Post(request) as ConflictObjectResult;

            Assert.Equal(409, result?.StatusCode);
            _todoServiceMock.Verify(x => x.AddTodoItem(It.IsAny<InsertTodoRequest>()), Times.Never);
        }

        [Fact]
        public void ShouldNot_Post_When_Validate_Error()
        {
            var request = new InsertTodoRequestBuilder()
                .WithTitle("a").Build();

            var sut = GetSut();
            var result = sut.Post(request) as BadRequestObjectResult;

            Assert.Equal(400, result?.StatusCode);
            _todoServiceMock.Verify(x => x.AddTodoItem(It.IsAny<InsertTodoRequest>()), Times.Never);
        }

        [Fact]
        public void Should_Put_Item()
        {
            var id = 1;

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(true);

            _todoServiceMock.Setup(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()))
                .Returns(RepositoryResult.Sucess);

            var sut = GetSut();
            sut.Put(id, new UpdateTodoRequestBuilder().Build());

            _todoServiceMock.Verify(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldNot_Put_Item_When_Done()
        {
            var id = 1;

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(true);

            _todoServiceMock.Setup(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()))
                .Returns(RepositoryResult.AlreadyDone);

            var sut = GetSut();
            var result = sut.Put(id, new UpdateTodoRequestBuilder().Build())
                as BadRequestObjectResult;
            var message = Assert.IsType<string>(result?.Value);

            Assert.Equal(400, result?.StatusCode);
            Assert.Equal(RepositoryResult.AlreadyDone.Message, message);
            _todoServiceMock.Verify(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldNot_Put_Item_NotExists()
        {
            var id = 1;

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(false);

            var sut = GetSut();
            var result = sut.Put(id, new UpdateTodoRequestBuilder().Build())
                as NotFoundResult;

            Assert.Equal(404, result?.StatusCode);
            _todoServiceMock.Verify(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()), Times.Never);
        }
        
        [Fact]
        public void ShouldNot_Put_Item_When_Validate_Error()
        {
            var id = 1;

            var sut = GetSut();
            var result = sut.Put(id, new UpdateTodoRequestBuilder().WithTitle("a").Build())
                         as BadRequestObjectResult;

            Assert.Equal(400, result?.StatusCode);            
            _todoServiceMock.Verify(x => x.UpdateTodoItem(It.IsAny<int>(), It.IsAny<UpdateTodoRequest>()), Times.Never);
        }

        [Fact]
        public void Should_Put_Status()
        {
            var id = 1;
            var request = new UpdateStatusRequest()
            {
                Status = "doing"
            };

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(true);

            _todoServiceMock.Setup(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()))
                .Returns(RepositoryResult.Sucess);

            var sut = GetSut();
            sut.Put(id, request);

            _todoServiceMock.Verify(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldNot_Put_Status_When_Done()
        {
            var id = 1;
            var request = new UpdateStatusRequest()
            {
                Status = "doing"
            };

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(true);

            _todoServiceMock.Setup(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()))
                .Returns(RepositoryResult.AlreadyDone);

            var sut = GetSut();
            var result = sut.Put(id, request) as BadRequestObjectResult;
            var message = Assert.IsType<string>(result?.Value);

            Assert.Equal(400, result?.StatusCode);
            Assert.Equal(RepositoryResult.AlreadyDone.Message, message);
            _todoServiceMock.Verify(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()), Times.Once);
        }

        [Fact]
        public void ShouldNot_Put_Status_NotExists()
        {
            var id = 1;
            var request = new UpdateStatusRequest()
            {
                Status = "doing"
            };

            _todoServiceMock.Setup(x => x.TodoExists(It.IsAny<int>()))
                .Returns(false);


            var sut = GetSut();
            var result = sut.Put(id, request) as NotFoundResult;

            Assert.Equal(404, result?.StatusCode);
            _todoServiceMock.Verify(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()), Times.Never);
        }
        
        [Fact]
        public void ShouldNot_Put_Status_When_Validate_Error()
        {
            var id = 1;
            var request = new UpdateStatusRequest()
            {
                Status = "test"
            };

            var sut = GetSut();
            var result = sut.Put(id, request) as BadRequestObjectResult;

            Assert.Equal(400, result?.StatusCode);
            _todoServiceMock.Verify(x => x.UpdateTodoStatus(It.IsAny<int>(), It.IsAny<UpdateStatusRequest>()), Times.Never);
        }

        [Fact]
        public void Should_Delete()
        {
            _todoServiceMock.Setup(x => x.DeleteTodoItem(It.IsAny<int>()))
                .Returns(RepositoryResult.Sucess);

            var sut = GetSut();
            var result = sut.Delete(1) as NoContentResult;

            Assert.Equal(204, result?.StatusCode);
        }

        [Fact]
        public void Should_Delete_When_Not_Found()
        {
            _todoServiceMock.Setup(x => x.DeleteTodoItem(It.IsAny<int>()))
                .Returns(RepositoryResult.NotFound);

            var sut = GetSut();
            var result = sut.Delete(1) as NotFoundObjectResult;
            var message = Assert.IsType<string>(result?.Value);

            Assert.Equal(RepositoryResult.NotFound.Message, message);
            Assert.Equal(404, result?.StatusCode);
        }

        private TodosController GetSut() => new(_todoServiceMock.Object, _mapper);
    }
}
