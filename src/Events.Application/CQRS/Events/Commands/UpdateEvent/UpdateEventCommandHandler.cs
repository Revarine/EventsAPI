using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateEventCommand> _validator;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, IValidator<UpdateEventCommand> validator, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);
        
        var existingEvent = await _eventRepository.Get(request.Id);
        if (existingEvent == null) throw new NotFoundException("Event", request.Id);
        
        var updatedEvent = _mapper.Map<Event>(request);
        await _eventRepository.Update(request.Id, updatedEvent, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
} 