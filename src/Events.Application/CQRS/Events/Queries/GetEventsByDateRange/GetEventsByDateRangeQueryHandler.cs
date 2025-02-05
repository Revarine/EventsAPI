using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByDateRange;

public class GetEventsByDateRangeQueryHandler : IRequestHandler<GetEventsByDateRangeQuery, IEnumerable<EventDTO>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    public GetEventsByDateRangeQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<EventDTO>> Handle(GetEventsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetEventsByDateRange(request.StartDate, request.EndDate, cancellationToken);
        return result.Select(e => _mapper.Map<EventDTO>(e));
    }
}