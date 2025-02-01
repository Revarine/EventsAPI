using System.Reflection;
using AutoMapper;

namespace Events.Application.Common.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
        .Where(t => t.GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
        .ToList();

        foreach (var type in types)
        {
            var sourceType = type.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)).GetGenericArguments()[0];

            CreateMap(sourceType, type).ReverseMap();
        }
    }
}