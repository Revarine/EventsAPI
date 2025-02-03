using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetEventParticipantsByUserId;

public class GetEventParticipantsByUserIdQueryHandler : IRequestHandler<GetEventParticipantsByUserIdQuery, IEnumerable<EventParticipantDTO>>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;
    public GetEventParticipantsByUserIdQueryHandler(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<EventParticipantDTO>> Handle(GetEventParticipantsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventParticipantRepository.GetByUserId(request.userId, cancellationToken);
        return result.Select(s => _mapper.Map<EventParticipantDTO>(s));
    }
}