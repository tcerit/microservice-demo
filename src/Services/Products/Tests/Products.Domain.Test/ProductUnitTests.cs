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
    public void Create_ShouldThrowException_WhenPriceIsNegative()
    {
        Product product;

        Action act = () => product = Product.Create("Product", -1m);

        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Price_IsCorrect_WhenUpdatedPrice()
    {
        Product product = Product.Create("Product", 10m);
        decimal newPrice = 20m;

        product.UdpatePrice(newPrice);

        Assert.Equal(newPrice, product.Price);
    }


    [Fact]
    public void UpdatePrice_ShouldThrowException_WhenPriceIsNegative()
    {
        Product product = Product.Create("Product");
        

        Action act = () => product.UdpatePrice(-1m);

        Assert.Throws<ArgumentException>(act);
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
