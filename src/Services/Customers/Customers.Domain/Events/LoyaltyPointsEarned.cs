using System;
using Core.Events;

namespace Customers.Domain.Events
{
	public class LoyaltyPointsEarnedEvent : DomainEvent
	{
		public Guid CustomerId { get; private set; }
        public decimal EarnedPoints { get; private set; }
        public decimal LoyaltyPoints { get; private set; }

        public LoyaltyPointsEarnedEvent(Guid customerId, decimal earnedPoints, decimal loyaltyPoints)
        {
            CustomerId = customerId;
            EarnedPoints = earnedPoints;
            LoyaltyPoints = loyaltyPoints;
        }
    }
}

