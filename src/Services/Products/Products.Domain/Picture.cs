using System;
using Ardalis.GuardClauses;
using Core.Domain;
using Products.Domain.Exceptions;

namespace Products.Domain
{
    public class Picture : ValueObject
    {
        public string? Url  { get; private set; }
        public DateTime DateSet { get; private set; } = DateTime.MinValue;

        private Picture() { }

        internal Picture(string url)
        {
            Guard.Against.Null(url, nameof(Url));
            Guard.Against.InvalidUrl(url);
            Url = url;
            DateSet = DateTime.Now;
        }


    }
}

