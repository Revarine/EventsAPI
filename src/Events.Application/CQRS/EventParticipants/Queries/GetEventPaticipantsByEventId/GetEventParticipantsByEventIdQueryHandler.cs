using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventPaticipantsByEventId;

public class GetEventParticipantsByEventIdQueryHandler : IRequestHandler<GetEventParticipantsByEventIdQuery, IEnumerable<EventParticipantDTO>>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;
    public GetEventParticipantsByEventIdQueryHandler(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EventParticipantDTO>> Handle(GetEventParticipantsByEventIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventParticipantRepository.GetByEventId(request.eventId, cancellationToken);
        return result.Select(s => _mapper.Map<EventParticipantDTO>(s));
    }
}