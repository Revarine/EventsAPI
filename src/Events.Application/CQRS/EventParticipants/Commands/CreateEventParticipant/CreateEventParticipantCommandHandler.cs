using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.CreateEventParticipant;

public class CreateEventParticipantCommandHandler : IRequestHandler<CreateEventParticipantCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;

    public CreateEventParticipantCommandHandler(IUserRepository userRepository, IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(CreateEventParticipantCommand request, CancellationToken cancellationToken = default)
    {
        if(await _userRepository.Get(request.userId) == null) return false; //TODO: throw non
        if(await _eventRepository.Get(request.eventId) == null) return false;
        
        var eventParticipations = await _eventParticipantRepository.GetByUserId(request.userId);
        if(eventParticipations.Any(e => e.EventId == request.eventId)) return false; //TODO: throw Already Participating

        await _eventParticipantRepository.Create(_mapper.Map<EventParticipant>(request), cancellationToken);
        return true;
    }
}