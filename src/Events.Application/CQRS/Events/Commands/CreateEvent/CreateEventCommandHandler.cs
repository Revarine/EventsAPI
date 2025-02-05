using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;
using Events.Domain.Exceptions;
using FluentValidation;

namespace Events.Application.CQRS.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateEventCommand> _validator;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IValidator<CreateEventCommand> validator, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        if (await _userRepository.Get(request.OrganizerId) == null)
            throw new NotFoundException("User", request.OrganizerId);
        
        var eventEntity = _mapper.Map<Event>(request);
        await _eventRepository.Create(eventEntity, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
} 