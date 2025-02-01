using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventParticipant;

public class GetEventParticipantQueryHandler : IRequestHandler<GetEventParticipantQuery, EventParticipantDTO>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;
    public GetEventParticipantQueryHandler(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }

    public async Task<EventParticipantDTO> Handle(GetEventParticipantQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventParticipantRepository.Get(request.Id, cancellationToken);
        return _mapper.Map<EventParticipantDTO>(result);
    }
}