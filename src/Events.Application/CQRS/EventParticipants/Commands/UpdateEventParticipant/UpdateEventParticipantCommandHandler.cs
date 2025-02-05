using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Events.Application.CQRS.EventParticipants.Commands.UpdateEventParticipant;

public class UpdateEventParticipantCommandHandler : IRequestHandler<UpdateEventParticipantCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateEventParticipantCommand> _validator;
    private readonly IMapper _mapper;

    public UpdateEventParticipantCommandHandler(IUserRepository userRepository, IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IUnitOfWork unitOfWork, IValidator<UpdateEventParticipantCommand> validator, IMapper mapper)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _eventParticipantRepository = eventParticipantRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<bool> Handle(UpdateEventParticipantCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if(!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        if(await _userRepository.Get(request.userId) == null) throw new NotFoundException("User", request.userId);
        if(await _eventRepository.Get(request.eventId) == null) throw new NotFoundException("Event", request.eventId);

        var eventParticipation = await _eventParticipantRepository.Get(request.participationId, cancellationToken);
        if(eventParticipation == null) throw new NotFoundException("EventParticipant", request.participationId);
        await _eventParticipantRepository.Update(eventParticipation.Id, _mapper.Map<EventParticipant>(request), cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
}