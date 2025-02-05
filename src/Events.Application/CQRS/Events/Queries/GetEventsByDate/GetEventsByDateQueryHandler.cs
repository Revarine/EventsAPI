using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventsByDate;

public class GetEventsByDateQueryHandler : IRequestHandler<GetEventsByDateQuery, IEnumerable<EventDTO>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventsByDateQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDTO>> Handle(GetEventsByDateQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetEventsByDate(request.Date, cancellationToken);
        return result.Select(e => _mapper.Map<EventDTO>(e));
    }
}