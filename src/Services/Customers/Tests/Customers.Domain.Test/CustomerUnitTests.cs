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
    public void Create_ShouldRaise_CustomerCreatedEvent()
    {
        Customer customer = Customer.Create("name", "lastname");
       
        var domainEvent = (CustomerCreatedEvent) customer.DomainEvents.First(e => e.GetType().Equals(typeof(CustomerCreatedEvent)));

        Assert.Equal(customer.Id, domainEvent.CustomerId);
    }

}
