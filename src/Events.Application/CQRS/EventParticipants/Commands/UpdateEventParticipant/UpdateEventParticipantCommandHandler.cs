using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.UpdateEventParticipant;

public class UpdateEventParticipantCommandHandler : IRequestHandler<UpdateEventParticipantCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;

    public UpdateEventParticipantCommandHandler(IUserRepository userRepository, IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateEventParticipantCommand request, CancellationToken cancellationToken)
    {
        if(await _userRepository.Get(request.userId) == null) return false; //TODO: throw non
        if(await _eventRepository.Get(request.eventId) == null) return false;

        var userEventParticipations = await _eventParticipantRepository.GetByUserId(request.userId, cancellationToken);
        var eventParticipation = userEventParticipations.First(e => e.EventId == request.eventId);
        if(eventParticipation == null) return false; //TODO: throw non
        await _eventParticipantRepository.Update(eventParticipation.Id, _mapper.Map<EventParticipant>(request), cancellationToken);
        return true;
    }
}