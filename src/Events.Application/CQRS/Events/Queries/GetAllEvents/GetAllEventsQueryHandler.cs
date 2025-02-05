using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetAllEvents;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, IEnumerable<EventDTO>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventDTO>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var results = await _eventRepository.GetAll(request.page, request.pageSize, cancellationToken);
        return results.Select(e => _mapper.Map<EventDTO>(e));
    }
} 