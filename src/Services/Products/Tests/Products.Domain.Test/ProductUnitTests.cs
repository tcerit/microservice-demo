using System;
using Products.Domain.Events;
using Products.Domain.Exceptions;

namespace Products.Domain.Test;

public class UnitTest1
{
    [Fact]
    public void Create_ShouldRaiseProductCreatedEvent()
    {
        Product product = Product.Create("Product");

        var domainEvent = (ProductCreatedEvent)product.DomainEvents.First(e => e.GetType().Equals(typeof(ProductCreatedEvent)));
        Assert.Equal(product.Id, domainEvent.ProductId);
    }

    [Fact]
    public void List_ShouldRaiseProductListedEvent()
    {
        Product product = Product.Create("Product");

        product.List();

        var domainEvent = (ProductListedEvent)product.DomainEvents.First(e => e.GetType().Equals(typeof(ProductListedEvent)));
        Assert.Equal(product.Id, domainEvent.ProductId);
    }

    [Fact]
    public void Delist_ShouldRaiseProductListedEvent()
    {
        Product product = Product.Create("Product");

        product.Delist();

        var domainEvent = (ProductDelistedEvent)product.DomainEvents.First(e => e.GetType().Equals(typeof(ProductDelistedEvent)));
        Assert.Equal(product.Id, domainEvent.ProductId);
    }

    [Fact]
    public void SetPicture_ShouldSet_ForUrlStrings()
    {
        Product product = Product.Create("Product");
        string url = "http://www.example.com";

        product.SetPicture(url);

        Assert.Equal(url, product.Picture.Url);
    }

    [Fact]
    public void SetPicture_ShouldThrowException_ForNOnUrlStrings()
    {
        Product product = Product.Create("Product");
        string url = "notanurl";

        Action act = () => product.SetPicture(url);

        Assert.Throws<InvalidUrlException>(act);
    }
}
