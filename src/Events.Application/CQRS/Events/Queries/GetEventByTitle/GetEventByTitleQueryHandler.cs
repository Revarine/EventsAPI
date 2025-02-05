using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventByTitle;

public class GetEventByTitleQueryHandler : IRequestHandler<GetEventByTitleQuery, EventDTO>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    public GetEventByTitleQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }
    public async Task<EventDTO> Handle(GetEventByTitleQuery request, CancellationToken cancellationToken)
    {
        var result = await _eventRepository.GetEventByTitle(request.Title, cancellationToken);
        return _mapper.Map<EventDTO>(result);
    }
}