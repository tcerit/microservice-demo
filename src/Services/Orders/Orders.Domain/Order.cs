using Ardalis.GuardClauses;
using Core.Domain;
using Orders.Domain.Events;

namespace Orders.Domain;

public class Order : Entity
{


    public Buyer Buyer { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime DatePlaced { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;

    public decimal OrderTotal { get => _items.Sum(item => item.Product.Price * item.Quantity); }

    private Order() { }

    private Order(Guid id, Buyer buyer) : base(id)
    {
        Status = OrderStatus.UNSET;
        Buyer = buyer;
    }

    public static Order Start(Buyer buyer) {
        Order order = new(Guid.NewGuid(), buyer);
        order.Status = OrderStatus.DRAFT;
        order.DateCreated = DateTime.Now;
        order.AddOrderStartedEvent();
        return order;
    } 


    public void AddItem(OrderProduct product, int quantity)
    {
        int index = FindItemIndex(product);
        if (index!=-1)
        {
            _items[index].Add(quantity);
            AddOrderItemChangedEvent(_items[index]);
        }
        else
        {
            OrderItem orderItem = new(product, quantity);
            _items.Add(orderItem);
            AddOrderItemAddedEvent(orderItem);
        }
        
    }

    public void RemoveItem(OrderProduct product)
    {
        int index = FindItemIndex(product);
        if (index != -1)
        {
            OrderItem temp = _items[index];
            _items.RemoveAt(index);
            AddOrderItemRemovedEvent(temp);
        }
    }

    public void SetQuantity(OrderProduct product, int quantity)
    {
        int index = FindItemIndex(product);
        if (index!=-1)
        {
            _items[index].SetNewQuantity(quantity);
            AddOrderItemChangedEvent(_items[index]);
        }
    }

    private int FindItemIndex(OrderProduct product)
    {
        return _items.FindIndex(item => item.Product.Equals(product));
        
    }

    public void Place()
    {
        Guard.Against.EmptyItemList(_items);
        DatePlaced = DateTime.Now;
        Status = OrderStatus.PLACED;

        AddOrderPlacedEvent();
    }


    private void AddOrderStartedEvent()
    {
        AddDomainEvent(new OrderStartedEvent(Id, Buyer.Id));
    }

    private void AddOrderItemAddedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemAddedEvent(Id, orderItem.Product.Id, orderItem.Product.Name, orderItem.Product.Price, orderItem.Quantity));
    }

    private void AddOrderItemRemovedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemRemovedEvent(Id, orderItem.Product.Id));
    }

    private void AddOrderItemChangedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemChangedEvent(Id, orderItem.Product.Id, orderItem.Quantity));
    }

    private void AddOrderPlacedEvent()
    {
        AddDomainEvent(new OrderPlacedEvent(Id, Buyer.Id, OrderTotal));
    }
}

