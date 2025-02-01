using AutoMapper;
using Events.Application.Common.Interfaces;
using Events.Domain.Entities;
using MediatR;

namespace Events.Application.CQRS.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _eventRepository.Get(request.Id);
        if (existingEvent == null) return false;

        await _eventRepository.Update(request.Id, _mapper.Map<Event>(request), cancellationToken);
        return true;
    }
} 