
using System;
using Ardalis.GuardClauses;
using Core.Domain;
using Core.Guards;
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


    private Order() : base(Guid.NewGuid()) { }

    private Order(Guid id, Buyer buyer) : base(id)
    {
        Status = OrderStatus.DRAFT;
        DateCreated = DateTime.Now;
        Buyer = buyer;
        AddOrderStartedEvent();
    }

    public static Order Start(Buyer buyer) => new(Guid.NewGuid(), buyer);


    public void AddItem(Guid productId, string prodcutName, double unitPrice, int quantity)
    {
        int index = _items.FindIndex(item => item.ProductId.Equals(productId));
        if (index!=-1)
        {
            _items[index].Add(quantity);
            AddOrderItemChangedEvent(_items[index]);
        }
        else
        {
            OrderItem orderItem = new(productId, prodcutName, unitPrice, quantity);
            _items.Add(orderItem);
            AddOrderItemAddedEvent(orderItem);
        }
        
    }

    public void RemoveItem(Guid productId)
    {
        int index = _items.FindIndex(item => item.ProductId.Equals(productId));
        if (index != -1)
        {
            OrderItem temp = _items[index];
            _items.RemoveAt(index);
            AddOrderItemRemovedEvent(temp);
        }
    }

    public void SetQuantity(Guid productId, int quantity)
    {
        int index = _items.FindIndex(item => item.ProductId.Equals(productId));
        if (index!=-1)
        {
            _items[index].SetNewQuantity(quantity);
            AddOrderItemChangedEvent(_items[index]);
        }
    }

    
    public void Place()
    {
        Guard.Against.EmptyItemList(_items);
        DatePlaced = DateTime.Now;
        Status = OrderStatus.PLACED;

        AddOrderPlacedEvent();
    }

    public double CalculatedTotal { get => _items.Sum(item => item.UnitPrice * item.Quantity); }

    private void AddOrderStartedEvent()
    {
        AddDomainEvent(new OrderStartedEvent(Id, Buyer.Id));
    }

    private void AddOrderItemAddedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemAddedEvent(Id, orderItem.ProductId, orderItem.ProductName, orderItem.UnitPrice, orderItem.Quantity));
    }

    private void AddOrderItemRemovedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemRemovedEvent(Id, orderItem.ProductId));
    }

    private void AddOrderItemChangedEvent(OrderItem orderItem)
    {
        AddDomainEvent(new OrderItemChangedEvent(Id, orderItem.ProductId, orderItem.Quantity));
    }

    private void AddOrderPlacedEvent()
    {
        AddDomainEvent(new OrderPlacedEvent(Id, Buyer.Id, CalculatedTotal));
    }



}

