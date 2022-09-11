using System;
using System.Threading.Tasks;

namespace Core.Data.Outbox
{
	public interface IOutboxManager
	{
        Task<List<OutboxItem>> GetUnsentItemsAsync();
		Task SendPendingItemsAsync();

		
	}
}

