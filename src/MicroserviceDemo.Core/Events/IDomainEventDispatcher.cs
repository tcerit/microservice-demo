using System;
namespace Core.Events
{
	public interface IDomainEventDispatcher
	{
		Task Dispatch(IDomainEvent devent);
	}
}

