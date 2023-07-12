using AutoMapper;
using Todo.Domain.Commons;
using Todo.Domain.Requests;
using Todo.Domain.Responses;

namespace Todo.Domain.Models.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<InsertTodoRequest, TodoModel>()
                .ForMember(dest => dest.Title, map => map.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, map => map.MapFrom(src => src.Description))
                .ForMember(dest => dest.DaysToFinish, map => map.MapFrom(src => src.DaysToFinish))
                .ForMember(dest => dest.Status, map => map.MapFrom(src => TodoStatus.Todo.Status))
                .ForMember(dest => dest.CreatedDate, map => map.Ignore())
                .ForMember(dest => dest.Id, map => map.Ignore())
                .ForMember(dest => dest.DueDate, map => map.Ignore() )
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<TodoModel, TodoResponse>()
                .ForMember(dest => dest.Id, map => map.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, map => map.MapFrom(src => src.Title))
                .ForMember(dest => dest.TodoStatus, map => map.MapFrom(src => TodoStatus.GetTodoStatus(src.Status).StatusDescription))
                .ForMember(dest => dest.Description, map => map.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedDate, map => map.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.DueDate, map => map.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.IsCompleted, map => map.Ignore())
                .ForMember(dest => dest.Observation, map => map.Ignore())
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}