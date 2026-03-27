using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class Category
    {
        // Propriétés
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;

        // Relations
        public ICollection<Product> Products { get; private set; } = default!;

        // Constructeur EF Core
        private Category() { }

        // Constructeur principal
        public Category(string name, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Le nom est obligatoire", nameof(name));

            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Le slug est obligatoire", nameof(slug));

            Name = name;
            Slug = slug;
            Products = new List<Product>();
        }

        // Méthode Update
        public void Update(string name, string slug)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Le nom est obligatoire", nameof(name));

            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Le slug est obligatoire", nameof(slug));

            Name = name;
            Slug = slug;
        }
    }
}