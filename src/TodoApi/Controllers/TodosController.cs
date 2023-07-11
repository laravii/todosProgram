using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo.Domain.Models;
using Todo.Domain.Models.Requests;
using Todo.Domain.Requests;
using Todo.Domain.Responses;
using Todo.Domain.Services;
using Todo.Domain.Validators;

namespace ToDo.Api.Controllers.Controllers
{
    [ApiController]
    [Route("v1/api/todos")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IMapper _mapper;

        public TodosController(ITodoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<TodoModel> todos = _todoService.GetAllTodos();
            var result = new List<TodoResponse>();

            if (!todos.Any()) { return Ok(result); }

            foreach (var todo in todos)
            {
                var item = _mapper.Map<TodoResponse>(todo);
                result.Add(item);
            }
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(InsertTodoRequest todoItem)
        {
            var validator = new InsertTodoRequestValidator();
            var validatorResult = validator.Validate(todoItem);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }

            if (_todoService.TodoExists(todoItem.Title))
                return Conflict("Item já cadastrado");

            var createdItem = _todoService.AddTodoItem(todoItem);
            var result = _mapper.Map<TodoResponse>(createdItem);

            return CreatedAtAction(nameof(Post), result);
        }

        [HttpPut("{id}/update")]
        public IActionResult Put(int id, UpdateTodoRequest request)
        {
            var validator = new UpdateTodoRequestValidator();
            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }

            if (!_todoService.TodoExists(id))
                return NotFound();

            var result = _todoService.UpdateTodoItem(id, request);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPut("{id}/update/status")]
        public IActionResult Put(int id, UpdateStatusRequest request)
        {
            var validator = new UpdateStatusRequestValidator();
            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }

            if (!_todoService.TodoExists(id))
                return NotFound();

            var result = _todoService.UpdateTodoStatus(id, request);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _todoService.DeleteTodoItem(id);
            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}
