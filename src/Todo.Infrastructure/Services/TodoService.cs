using AutoMapper;
using Todo.Domain.Commons;
using Todo.Domain.Extensions;
using Todo.Domain.Models;
using Todo.Domain.Models.Requests;
using Todo.Domain.Models.Results;
using Todo.Domain.RepositoryTodo.Domain.Repositories;
using Todo.Domain.Requests;
using Todo.Domain.Responses;
using Todo.Domain.Services;

namespace Todo.Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodoService(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public IEnumerable<TodoModel> GetAllTodos()
        {
            var todos = _todoRepository.GetAll();
            
            return todos;
        }

        public TodoModel AddTodoItem(InsertTodoRequest request)
        {
            var model = _mapper.Map<TodoModel>(request);
            model.CreatedDate = DateTime.UtcNow;
            model.DueDate = model.CreatedDate.AddDays(request.DaysToFinish);
            _todoRepository.Add(model);

            return model;
        }

        public RepositoryResult UpdateTodoItem(int id, UpdateTodoRequest request)
        {
            TodoModel existingTodoItem = _todoRepository.GetById(id);

            if (existingTodoItem == null) { return RepositoryResult.NotFound; }

            if (TodoStatus.GetTodoStatus(existingTodoItem.Status).Equals(TodoStatus.Done)) { return RepositoryResult.AlreadyDone; }

            existingTodoItem.Title = request.Title.UpdateValidateString(existingTodoItem.Title);
            existingTodoItem.Description = request.Description.UpdateValidateString(existingTodoItem.Description);
            existingTodoItem.DaysToFinish = request.DaysToFinish.UpdateValidateInt(existingTodoItem.DaysToFinish);
            existingTodoItem.DueDate = existingTodoItem.CreatedDate.AddDays(existingTodoItem.DaysToFinish);

            _todoRepository.Update(existingTodoItem);

            return RepositoryResult.Sucess;
        }

        public RepositoryResult DeleteTodoItem(int id)
        {
            var todo = _todoRepository.GetById(id);

            if (todo == null)
            {
                return RepositoryResult.NotFound;
            }

            _todoRepository.Delete(todo);
            return RepositoryResult.Sucess;
        }

        public bool TodoExists(int id)
        {
            var todo = _todoRepository.GetById(id);
            return todo != null;
        }

        public bool TodoExists(string title)
        {
            var todos = _todoRepository.GetAll();
            if (!todos.Any()) { return false; }

            var todo = todos.FirstOrDefault(x => x.Title == title);
            return todo != null;
        }

        public RepositoryResult UpdateTodoStatus(int id, UpdateStatusRequest request)
        {
            TodoModel existingTodoItem = _todoRepository.GetById(id);

            if (existingTodoItem == null) { return RepositoryResult.NotFound; }

            if (TodoStatus.GetTodoStatus(existingTodoItem.Status).Equals(TodoStatus.Done)) { return RepositoryResult.AlreadyDone; }

            existingTodoItem.Status = TodoStatus.UpdateStatus(existingTodoItem.Status, request.Status).Status;

            _todoRepository.Update(existingTodoItem);

            return RepositoryResult.Sucess;
        }
    }
}