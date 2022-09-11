using System;
using Core.Settings;

namespace Core.Messaging
{
	public interface IMessageBroker
	{
		void Init();
		void Publish(byte[] message, string messageType);
	}
}

