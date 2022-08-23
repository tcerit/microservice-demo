using System;
namespace Core.Events
{
	public interface IDomainEvent
	{
		 public DateTime DateOccured { get; set; }

		public string Serialize();
	}
}

