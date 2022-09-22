using Customers.Domain.Events;

namespace Customers.Domain.Test;

public class CustomerUnitTests
{

    [Fact]
    public void Create_ShouldThrowException_WhenNameIsNull()
    {
        Customer customer;

        Action act = () => customer = Customer.Create(null, "lastname");

        Assert.Throws<ArgumentNullException>(act);
    }


    [Fact]
    public void Create_ShouldThrowlException_WhenLastNameIsNull()
    {
        Customer customer;

        Action act = () => customer = Customer.Create("name", null);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void EarnPointsFromOrder_ShouldNotChangeLoyaltyPoints_WhenOrderIsFree()
    {
        CustomerBuilder customerBuilder = new();
        Customer customer = customerBuilder.BuildHavingSomeLoyaltyPoints();
        decimal existingPoints = customer.LoyaltyPoints;

        customer.EarnPointsFromOrder(0);

        Assert.Equal(existingPoints, customer.LoyaltyPoints);
    }

    [Fact]
    public void EarnPointsFromOrder_ShouldIncreaseLoyaltyPoints_WhenOrderIsNotFree()
    {
        CustomerBuilder customerBuilder = new();
        Customer customer = customerBuilder.BuildHavingSomeLoyaltyPoints();
        decimal existingPoints = customer.LoyaltyPoints;

        customer.EarnPointsFromOrder(1);

        Assert.True(customer.LoyaltyPoints > existingPoints);
    }

    [Fact]
    public void Create_ShouldRaise_CustomerCreatedEvent()
    {
        Customer customer = Customer.Create("name", "lastname");
       
        var domainEvent = (CustomerCreatedEvent) customer.DomainEvents.First(
            e => e.GetType().Equals(typeof(CustomerCreatedEvent)));

        Assert.Equal(customer.Id, domainEvent.CustomerId);
    }

    [Fact]
    public void EarnPointsFromOrder_ShouldRaise_LoyaltyPointsEarnedEvent()
    {
        CustomerBuilder customerBuilder = new();
        Customer customer = customerBuilder.BuildHavingSomeLoyaltyPoints();
        decimal existingPoints = customer.LoyaltyPoints;
        customer.EarnPointsFromOrder(100m);

        var domainEvent = (LoyaltyPointsEarnedEvent)customer.DomainEvents.First(
            e => e.GetType().Equals(typeof(LoyaltyPointsEarnedEvent)));

        Assert.Equal(customer.Id, domainEvent.CustomerId);
        Assert.Equal(customer.LoyaltyPoints - existingPoints, domainEvent.LoyaltyPoints);
    }

}
