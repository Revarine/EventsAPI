using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByCategory;

public class GetEventsByCategoryQueryHandler : IRequestHandler<GetEventsByCategoryQuery, IEnumerable<EventDTO>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    
    public GetEventsByCategoryQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<EventDTO>> Handle(GetEventsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetEventsByCategory(request.Category, cancellationToken);
        return result.Select(e => _mapper.Map<EventDTO>(e));
    }
}