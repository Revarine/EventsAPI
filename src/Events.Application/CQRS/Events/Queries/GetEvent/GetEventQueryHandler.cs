using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEvent;

public class GetEventQueryHandler : IRequestHandler<GetEventQuery, EventDTO>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDTO> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<EventDTO>(result);
    }
} 