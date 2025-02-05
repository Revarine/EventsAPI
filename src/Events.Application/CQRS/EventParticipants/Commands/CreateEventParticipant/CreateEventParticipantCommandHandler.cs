using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;
using Events.Domain.Exceptions;
using FluentValidation;

namespace Events.Application.CQRS.EventParticipants.Commands.CreateEventParticipant;

public class CreateEventParticipantCommandHandler : IRequestHandler<CreateEventParticipantCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IEventParticipantRepository _eventParticipantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEventParticipantCommand> _validator;
    private readonly IMapper _mapper;

    public CreateEventParticipantCommandHandler(IUserRepository userRepository, IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository, IUnitOfWork unitOfWork,IValidator<CreateEventParticipantCommand> validator, IMapper mapper)
    {
        _userRepository = userRepository;
        _eventRepository = eventRepository;
        _eventParticipantRepository = eventParticipantRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<bool> Handle(CreateEventParticipantCommand request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if(!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        if(await _userRepository.Get(request.userId) == null) 
            throw new NotFoundException("User", request.userId);
        
        if(await _eventRepository.Get(request.eventId) == null) 
            throw new NotFoundException("Event", request.eventId);
        
        var eventParticipations = await _eventParticipantRepository.GetByUserId(request.userId);
        if(eventParticipations.Any(e => e.EventId == request.eventId)) 
            throw new AlreadyExistsException("EventParticipant", $"User {request.userId} is already participating in event {request.eventId}");

        await _eventParticipantRepository.Create(_mapper.Map<EventParticipant>(request), cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
}