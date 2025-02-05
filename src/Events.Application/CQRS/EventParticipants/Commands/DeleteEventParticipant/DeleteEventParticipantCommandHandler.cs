using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.DeleteEventParticipant;

public class DeleteEventParticipantCommandHandler : IRequestHandler<DeleteEventParticipantCommand, bool>
{
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteEventParticipantCommandHandler(IEventParticipantRepository eventParticipantRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _eventParticipantRepository = eventParticipantRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<bool> Handle(DeleteEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var eventParticipation = await _eventParticipantRepository.Get(request.id);
        if(eventParticipation == null) throw new NotFoundException("EventParticipant", request.id);

        await _eventParticipantRepository.Update(eventParticipation.Id, _mapper.Map<EventParticipant>(request));
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
}