using Melanin.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Melanin.Domain.Entities;

public class Member
{
    // Propriétés
    public int Id { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public MemberRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Relations
    public ICollection<Address> Addresses { get; private set; } = default!;
    public ICollection<Order> Orders { get; private set; } = default!;
    public ICollection<CartItem> CartItems { get; private set; } = default!;

    // Constructeur EF Core
    private Member() { }

    // Constructeur principal
    public Member(string firstName, string lastName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Le prénom est obligatoire", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Le nom est obligatoire", nameof(lastName));

        if (string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email, out _))
            throw new ArgumentException("L'adresse email n'est pas valide", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Le mot de passe est obligatoire", nameof(passwordHash));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = MemberRole.User;
        CreatedAt = DateTime.UtcNow;
        Addresses = new List<Address>();
        Orders = new List<Order>();
        CartItems = new List<CartItem>();
    }

    // Méthode de mise à jour
    public void Update(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Le prénom est obligatoire", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Le nom est obligatoire", nameof(lastName));
        if (string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email, out _))
            throw new ArgumentException("L'adresse email n'est pas valide", nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

}