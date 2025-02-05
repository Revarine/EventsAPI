using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByLocation;

public class GetEventsByLocationQueryHandler : IRequestHandler<GetEventsByLocationQuery, IEnumerable<EventDTO>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    public GetEventsByLocationQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDTO>> Handle(GetEventsByLocationQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetEventsByLocation(request.Location, cancellationToken);
        return result.Select(e => _mapper.Map<EventDTO>(e));
    }
}