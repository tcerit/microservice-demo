using Ardalis.GuardClauses;
using Core.Domain;
using Products.Domain.Events;

namespace Products.Domain;

public class Product : Entity
{

    public string Name { get; private set; }
    public Category? Category { get; private set; }
    public decimal Price { get; private set; }
    public Picture Picture { get; private set; }
    public bool IsListed { get; private set; }

    private Product()
    {
    }

    private Product(Guid id, string name, decimal price) : base(id)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        Guard.Against.Negative(price, nameof(Price));
        Name = name;
        Price = price;
        IsListed = false;
    }

    public static Product Create(string name, decimal price = 0)
    {
        Product product = new(Guid.NewGuid(), name, price);
        product.AddProductCreatedEvent();
        return product;
    }

    public void AddToCategory(Category category)
    {
        Guard.Against.Null(category);
        Category = category;
    }

    public void SetPicture(string url)
    {
        Picture = new Picture(url);
    }

    public void UdpatePrice(decimal newPrice)
    {
        Price = newPrice;
    }

    public void List()
    {
        IsListed = true;
        AddProductListedEvent();
        
    }

    public void Delist()
    {
        IsListed = false;
        AddProductDelistedEvent();
    }

    private void AddProductCreatedEvent()
    {
        AddDomainEvent(new ProductCreatedEvent(Id, Name, Price));
    }

    private void AddProductListedEvent()
    {
        AddDomainEvent(new ProductListedEvent(Id));
    }

    private void AddProductDelistedEvent()
    {
        AddDomainEvent(new ProductDelistedEvent(Id));
    }

}

