using Customers.Domain.Events;
using Core.Domain;
using Ardalis.GuardClauses;

namespace Customers.Domain;

public class Customer : Entity
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public decimal LoyaltyPoints { get; private set; }
    public DateTime DateCreated { get; private set; }

    private Customer() { }

    private Customer(Guid id, string name, string lastName) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        Guard.Against.NullOrWhiteSpace(lastName, nameof(LastName));
        Name = name;
        LastName = lastName;
        DateCreated = DateTime.Now;
    }

    public static Customer Create(string name, string lastName)
    {
        Customer customer = new(Guid.NewGuid(), name, lastName);
        customer.AddUserCreatedEvent();
        return customer;
    }

    public void EarnPointsFromOrder(decimal ordarTotal)
    {
        Guard.Against.Negative(ordarTotal);
        decimal earnedPoints = ordarTotal / 10;
        LoyaltyPoints += earnedPoints;
        AddLoyaltyPointsEarnedEvent(earnedPoints);
    }

    private void AddUserCreatedEvent()
    {
        AddDomainEvent(new CustomerCreatedEvent(Id, $"{Name} {LastName}"));
    }

    private void AddLoyaltyPointsEarnedEvent(decimal earnedPoints)
    {
        AddDomainEvent(new LoyaltyPointsEarnedEvent(Id, earnedPoints, LoyaltyPoints));
    }
}

