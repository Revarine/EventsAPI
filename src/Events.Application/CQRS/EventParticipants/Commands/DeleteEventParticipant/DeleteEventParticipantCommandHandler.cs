using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.DeleteEventParticipant;

public class DeleteEventParticipantCommandHandler : IRequestHandler<DeleteEventParticipantCommand, bool>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IMapper _mapper;

    public DeleteEventParticipantCommandHandler(IEventParticipantRepository eventParticipantRepository, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _mapper = mapper;
    }
    public async Task<bool> Handle(DeleteEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipation = await _eventParticipantRepository.Get(request.id);
        if(eventParticipation == null) return false; //TODO: throw non

        await _eventParticipantRepository.Update(eventParticipation.Id, _mapper.Map<EventParticipant>(request));
        return true;
    }
}