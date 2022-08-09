using System;
using MediatR;

namespace Core.Events
{
	public class DomainEventDispatcher : IDomainEventDispatcher
	{
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Dispatch(IDomainEvent devent)
        {
            if (devent != null)
            {
                var domainEventNotification = CreateDomainEventNotification(devent);
                await _mediator.Publish(domainEventNotification);
            }
        }

        private INotification CreateDomainEventNotification(IDomainEvent domainEvent)
        {
            var genericDispatcherType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(genericDispatcherType, domainEvent) ?? new DefaultNotification();

            return (INotification) notification;
        }
    }

    class DefaultNotification : INotification
    {

    }
}

