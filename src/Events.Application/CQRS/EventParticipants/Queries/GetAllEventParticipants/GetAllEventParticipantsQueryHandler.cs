using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Queries.GetAllEventParticipants;

public class GetAllEventParticipantsQueryHandler : IRequestHandler<GetAllEventParticipantsQuery, IEnumerable<EventParticipantDTO>>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;
    public GetAllEventParticipantsQueryHandler(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<EventParticipantDTO>> Handle(GetAllEventParticipantsQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventParticipantRepository.GetAll(request.page, request.pageSize, cancellationToken);
        return result.Select(s => _mapper.Map<EventParticipantDTO>(s));
    }
}