using Events.Application.Common.ResponseDTO;
using MediatR;

namespace Events.Application.CQRS.Events.Queries.GetEventByTitle;

public record GetEventByTitleQuery(string Title) : IRequest<EventDTO>;