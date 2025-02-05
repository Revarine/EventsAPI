using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Exceptions;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.Get(request.Id);
        if (@event == null) throw new NotFoundException("Event", request.Id);

        await _eventRepository.Delete(request.Id, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
} 