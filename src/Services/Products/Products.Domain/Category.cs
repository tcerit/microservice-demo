using Ardalis.GuardClauses;
using Core.Domain;

namespace Products.Domain
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products;


        private Category()
        {
        }

        private Category(Guid id, string name) : base(id)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(Name));
            Name = name;
        }

        public static Category Create(string name) => new(Guid.NewGuid(), name);

    }
}