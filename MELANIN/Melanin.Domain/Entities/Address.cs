using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Domain.Entities
{
    public class Address
    {
        // Propriétés
        public int Id { get; private set; }
        public string City { get; private set; } = default!;
        public int PostalCode { get; private set; }
        public string Country { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string? FullName { get; private set; }

        // Relation
        public Member Member { get; private set; } = default!;

        // Clé étrangère
        public int MemberId { get; private set; }

        // Constructeur EF Core
        private Address() { }

        // Constructeur principal
        public Address(string city, int postalCode, string country, string street, int memberId, string phone, string? fullName = null)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("La ville est obligatoire", nameof(city));

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Le pays est obligatoire", nameof(country));

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("La rue est obligatoire", nameof(street));

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Le téléphone est obligatoire", nameof(phone));

            if (postalCode <= 0)
                throw new ArgumentException("Le code postal est invalide", nameof(postalCode));

            City = city;
            PostalCode = postalCode;
            Country = country;
            Street = street;
            MemberId = memberId;
            Phone = phone;
            FullName = fullName;
        }
    }
}