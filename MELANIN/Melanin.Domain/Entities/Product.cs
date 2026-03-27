using Melanin.Domain.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class Product
    {
        // Propriétés
        public int Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public decimal UnitPrice { get; private set; }
        public int StockQuantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }

        // Attributs cheveux
        public string? HairColor { get; private set; }
        public HairLength? HairLength { get; private set; }
        public HairTexture? HairTexture { get; private set; }
        public CapSize? CapSize { get; private set; }

        // Relation
        public Category Category { get; private set; } = default!;

        // Clé étrangère
        public int CategoryId { get; private set; }

        // Constructeur EF Core
        private Product() { }

        // Constructeur principal
        public Product(string name, string description, decimal unitPrice, int stockQuantity, int categoryId, string? hairColor = null, HairLength? hairLength = null, HairTexture? hairTexture = null, CapSize? capSize = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Le nom est obligatoire", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La description est obligatoire", nameof(description));

            if (unitPrice <= 0)
                throw new ArgumentException("Le prix doit être supérieur à 0", nameof(unitPrice));

            if (stockQuantity < 0)
                throw new ArgumentException("Le stock ne peut pas être négatif", nameof(stockQuantity));

            if (categoryId <= 0)
                throw new ArgumentException("La catégorie est obligatoire", nameof(categoryId));


            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            StockQuantity = stockQuantity;
            CategoryId = categoryId;
            HairColor = hairColor;
            HairLength = hairLength;
            HairTexture = hairTexture;
            CapSize = capSize;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        // Méthode Update
        public void Update(string name, string description, decimal unitPrice, int categoryId, string? hairColor = null, HairLength? hairLength = null, HairTexture? hairTexture = null, CapSize? capSize = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Le nom est obligatoire", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La description est obligatoire", nameof(description));

            if (unitPrice <= 0)
                throw new ArgumentException("Le prix doit être supérieur à 0", nameof(unitPrice));

            if (categoryId <= 0)
                throw new ArgumentException("La catégorie est obligatoire", nameof(categoryId));

            Name = name;
            Description = description;
            UnitPrice = unitPrice;
            CategoryId = categoryId;
            HairColor = hairColor;
            HairLength = hairLength;
            HairTexture = hairTexture;
            CapSize = capSize;
        }

        // Gestion du stock
        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La quantité doit être positive", nameof(quantity));

            if (StockQuantity - quantity < 0)
                throw new ArgumentException("Stock insuffisant", nameof(quantity));

            StockQuantity -= quantity;
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("La quantité doit être positive", nameof(quantity));

            StockQuantity += quantity;
        }

        // Gestion IsActive
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}