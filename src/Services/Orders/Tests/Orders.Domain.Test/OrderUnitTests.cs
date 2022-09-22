using Orders.Domain.Events;

namespace Orders.Domain.Test;

public class OrderUnitTests
{
    [Fact]
    public void Start_ShouldRaiseOrderStartedEvent()
    {
        Buyer buyer = new BuyerBuilder().BuildDefault();

        Order order = Order.Start(buyer);

        var domainEvent = (OrderStartedEvent)order.DomainEvents.First(e => e.GetType().Equals(typeof(OrderStartedEvent)));
        Assert.Equal(order.Id, domainEvent.OrderId);
    }

    [Fact]
    public void AddItem_ShouldRaiseOrderItemAddedEvent_WhenDifferentItemAdded()
    {
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        Order order = new OrderBuilder().BuildWithItem(product, 1);
        OrderProduct product2 = new OrderProductBuilder().BuildDefault();

        order.AddItem(product2, 1);

        var domainEvent = (OrderItemAddedEvent)order.DomainEvents.Last(e => e.GetType().Equals(typeof(OrderItemAddedEvent)));
        Assert.Equal(order.Id, domainEvent.OrderId);
        Assert.Equal(product2.Id, domainEvent.ProductId);
    }

    [Fact]
    public void AddItem_ShouldRaiseOrderItemChangedEvent_WhenExistingItemAdded()
    {
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        Order order = new OrderBuilder().BuildWithItem(product, 1);

        order.AddItem(product, 1);

        var domainEvent = (OrderItemChangedEvent)order.DomainEvents.First(e => e.GetType().Equals(typeof(OrderItemChangedEvent)));
        Assert.Equal(order.Id, domainEvent.OrderId);
        Assert.Equal(product.Id, domainEvent.ProductId);
    }

    [Fact]
    public void AddItem_ShouldAddAsNewItem_WhenDifferentItemAdded()
    {
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        Order order = new OrderBuilder().BuildWithItem(product, 1);
        OrderProduct product2 = new OrderProductBuilder().BuildDefault();

        order.AddItem(product2, 1);

        Assert.Equal(2, order.Items.Count());
    }

    [Fact]
    public void AddItem_ShouldAddToQuantity_WhenExistingItemAdded()
    {
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        int initialQuantity = 1;
        Order order = new OrderBuilder().BuildWithItem(product, initialQuantity);
        int additionalQuantity = 1;

        order.AddItem(product, additionalQuantity);

        int finalQuantity = order.Items.Where(item => item.Product.Id.Equals(product.Id)).Single().Quantity;
        Assert.Equal(initialQuantity + additionalQuantity, finalQuantity);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveProduct()
    {
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        Order order = new OrderBuilder().BuildWithItem(product, 1);

        order.RemoveItem(product);

        Assert.Equal(0, order.Items.Count);
    }

    [Fact]
    public void OrderTotal_IsZero_WhenNoItems()
    {
        Buyer buyer = new BuyerBuilder().BuildDefault();

        Order order = Order.Start(buyer);

        Assert.Equal(0, order.OrderTotal);
    }

    [Fact]
    public void OrderTotal_IsCorrect_WhenOneItem()
    {
        Order order = new OrderBuilder().BuildWithOrderStart();
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        int quantity = 2;

        order.AddItem(product, quantity);

        Assert.Equal(quantity*product.Price, order.OrderTotal);
    }

    [Fact]
    public void OrderTotal_IsCorrect_WhenMultipleItems()
    {
        Order order = new OrderBuilder().BuildWithOrderStart();
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        int quantity = 2;
        OrderProduct product2 = new OrderProductBuilder().BuildDifferent();
        int quantity2 = 5;

        order.AddItem(product, quantity);
        order.AddItem(product2, quantity2);

        Assert.Equal(quantity * product.Price + quantity2 * product2.Price, order.OrderTotal);
    }

    [Fact]
    public void OrderTotal_IsCorrect_WhenItemsUpdated()
    {
        Order order = new OrderBuilder().BuildWithOrderStart();
        OrderProduct product = new OrderProductBuilder().BuildDefault();
        int quantity = 2;
        int quantity2 = 5;

        order.AddItem(product, quantity);
        order.AddItem(product, quantity2);

        Assert.Equal(product.Price * (quantity + quantity2), order.OrderTotal);
    }
}
