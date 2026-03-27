# Projet Melanin - Résumé complet

## Objectif
Créer un site e-commerce semi-premium pour vendre :
- Wigs / Perruques
- Mèches de qualité (Bundles)
- Produits de soin pour cheveux (huiles, crèmes)

### Expérience utilisateur :
- Catalogue produits
- Gestion du panier et des commandes
- Paiement sécurisé via Stripe
- Suivi du stock et des commandes

---

## Stack technique
- **Frontend** : React + Tailwind
- **Backend** : C# ASP.NET Core Web API + EF Core
- **Base de données** : SQL Server (potentiellement PostgreSQL en production)

---

## Profil du développeur
- Développeur junior sérieux et structuré
- Comprend : UML, EF Core, Clean Architecture, séparation Front/Back, base de données relationnelle
- Développe en C#, React et SQL Server
- Veut faire les choses proprement dès le départ
- V1 maintenant, fonctionnalités supplémentaires plus tard
- Veut être guidé comme par un professeur : une étape à la fois, avec explications

---

## Architecture : Clean Architecture
```
Melanin.Domain          → Entités, Enums, Exceptions
Melanin.Application     → Interfaces, Services
Melanin.Infrastructure  → EF Core, Repositories, Configurations
Melanin.API             → Controllers, DTOs
```

### Références entre projets :
- `API` → référence `Application` + `Infrastructure`
- `Application` → référence `Domain`
- `Infrastructure` → référence `Application` + `Domain`

---

## Script SQL final
```sql
CREATE TABLE Member_(
   Id_Member INT,
   FirstName VARCHAR(50) NOT NULL,
   LastName VARCHAR(50) NOT NULL,
   Email VARCHAR(50) NOT NULL,
   PasswordHash VARCHAR(255) NOT NULL,
   Role VARCHAR(50),
   CreatedAt DATETIME,
   PRIMARY KEY(Id_Member),
   UNIQUE(Email)
);

CREATE TABLE Address(
   Id_Address INT,
   City VARCHAR(50) NOT NULL,
   PostalCode INT NOT NULL,
   Country VARCHAR(50) NOT NULL,
   Phone VARCHAR(20) NOT NULL,
   Street VARCHAR(50) NOT NULL,
   FullName VARCHAR(100),
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_Address),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE Category(
   Id_Category INT,
   Name VARCHAR(50) NOT NULL,
   Slug VARCHAR(50) NOT NULL,
   PRIMARY KEY(Id_Category)
);

CREATE TABLE Product(
   Id_Product INT,
   Name VARCHAR(50) NOT NULL,
   Description VARCHAR(500) NOT NULL,
   UnitPrice DECIMAL(10,2) NOT NULL,
   StockQuantity INT NOT NULL,
   CreatedAt DATETIME,
   IsActive BIT NOT NULL,
   HairLength VARCHAR(20),
   HairTexture VARCHAR(20),
   HairColor VARCHAR(50) NOT NULL,
   CapSize VARCHAR(20),
   Id_Category INT NOT NULL,
   PRIMARY KEY(Id_Product),
   FOREIGN KEY(Id_Category) REFERENCES Category(Id_Category)
);

CREATE TABLE CartItem(
   Id_CartItem INT,
   Quantity INT NOT NULL,
   UnitPrice DECIMAL(10,2) NOT NULL,
   Id_Product INT NOT NULL,
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_CartItem),
   FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE Order_(
   Id_Order INT,
   TotalPrice DECIMAL(10,2) NOT NULL,
   CreatedAt DATETIME,
   Status VARCHAR(50) NOT NULL,
   ShippingAddressId INT NOT NULL,
   BillingAddressId INT NULL,
   CouponCode VARCHAR(50) NULL,
   Id_Member INT NOT NULL,
   PRIMARY KEY(Id_Order),
   FOREIGN KEY(ShippingAddressId) REFERENCES Address(Id_Address),
   FOREIGN KEY(BillingAddressId) REFERENCES Address(Id_Address),
   FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member)
);

CREATE TABLE OrderItem(
   Id_OrderItem INT,
   Quantity INT NOT NULL,
   UnitPrice DECIMAL(10,2) NOT NULL,
   Id_Order INT NOT NULL,
   Id_Product INT NOT NULL,
   PRIMARY KEY(Id_OrderItem),
   FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order),
   FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product)
);

CREATE TABLE Payment(
   Id_Payment VARCHAR(50),
   Amount DECIMAL(10,2) NOT NULL,
   Status VARCHAR(50) NOT NULL,
   PaidAt DATETIME,
   StripeSessionId VARCHAR(255),
   Id_Order INT NOT NULL,
   PRIMARY KEY(Id_Payment),
   UNIQUE(Id_Order),
   FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order)
);
```

---

## Ce qui est fait

### Melanin.Domain

#### Entités
Style : `private set`, constructeur EF Core `private`, constructeur principal avec paramètres et validations métier, collections en `ICollection<T>`, navigation properties puis clés étrangères.

- **Member** → Id, FirstName, LastName, Email, PasswordHash, Role (MemberRole), CreatedAt | Relations : Addresses, Orders, CartItems
- **Address** → Id, City, PostalCode, Country, Phone, Street, FullName? | Relations : Member / FK : MemberId
- **Category** → Id, Name, Slug | Relations : Products
- **Product** → Id, Name, Description, UnitPrice, StockQuantity, IsActive, HairLength?, HairTexture?, HairColor, CapSize?, CreatedAt | Relations : Category / FK : CategoryId
- **CartItem** → Id, Quantity, UnitPrice | Relations : Product, Member / FK : ProductId, MemberId
- **Order** → Id, TotalPrice, CreatedAt, Status (OrderStatus), CouponCode? | Relations : Member, ShippingAddress, BillingAddress?, OrderItems, Payment? / FK : MemberId, ShippingAddressId, BillingAddressId?
- **OrderItem** → Id, Quantity, UnitPrice | Relations : Order, Product / FK : OrderId, ProductId
- **Payment** → Id (string), Amount, Status (PaymentStatus), PaidAt?, StripeSessionId? | Relations : Order / FK : OrderId

#### Enums (dossier Enums/)
- `MemberRole` → User, Admin
- `OrderStatus` → Pending, Confirmed, Shipped, Delivered, Cancelled
- `PaymentStatus` → Pending, Completed, Failed, Refunded
- `HairLength` → Inches8 → Inches30
- `HairTexture` → Straight, Wavy, Curly, Kinky, DeepWave, BodyWave, LooseWave
- `CapSize` → Small, Medium, Large, XLarge

#### Exceptions (dossier Exceptions/)
Style : héritage, classe parent générique puis classes spécifiques.
- `MemberException` → classe parent
- `MemberNotFoundException`
- `MemberAlreadyExistsException`
- `MemberBadCredentialException`

---

### Melanin.Application

#### Interfaces (dossier Interfaces/)
**IMemberRepository :**
```csharp
Task<Member?> GetByIdAsync(int id);
Task<Member?> GetByEmailAsync(string email);
Task<string?> GetHashPwdAsync(string email);
Task<IEnumerable<Member>> GetAllAsync();
Task<Member> AddAsync(Member member);
Task UpdateAsync(Member member);
Task DeleteAsync(int id);
```

**IMemberService :**
```csharp
Task<Member> RegisterAsync(Member member);
Task<Member> LoginAsync(string email, string password);
Task<Member> GetByIdAsync(int id);
Task UpdateAsync(Member member);
Task DeleteAsync(int id);
```

#### Services (dossier Services/)
**MemberService** → logique métier complète :
- `RegisterAsync` → vérifie email existant, hashe le mot de passe avec Argon2, sauvegarde
- `LoginAsync` → récupère hash, vérifie avec Argon2, retourne le membre
- `GetByIdAsync` → récupère par Id, lance exception si null
- `UpdateAsync` → vérifie existence, met à jour
- `DeleteAsync` → vérifie existence, supprime

#### Package NuGet installé :
- `Soenneker.Hashing.Argon2` → hashage des mots de passe

---

## Décisions importantes prises

| Sujet | Décision |
|---|---|
| Panier | Pas de table Cart, CartItem lié directement à Member |
| Panier invité | Géré en localStorage côté React |
| Adresse facturation | BillingAddressId nullable dans Order |
| Couleur cheveux | String libre, pas d'enum |
| Attributs cheveux | Nullable dans Product pour supporter huiles et crèmes |
| DTOs | Uniquement dans Melanin.API avec validations [Required] etc. |
| Application | Travaille directement avec les entités Domain |
| Hashage | Argon2 via Soenneker.Hashing.Argon2 |
| Collections | ICollection<T> dans les entités |
| Constructeurs | EF Core private, principal avec paramètres et validations |

---

## Où on en est

| Couche | Statut |
|---|---|
| Melanin.Domain | ✅ Terminé |
| Melanin.Application (Member) | ✅ Terminé |
| Melanin.Infrastructure | ⏳ Prochaine étape |
| Melanin.API | ⏳ Après Infrastructure |

## Prochaine étape → Melanin.Infrastructure
1. Configurations Fluent API pour chaque entité (dossier Persistence/Configurations/)
2. MelaninDbContext (dossier Persistence/)
3. Implémentation de MemberRepository (dossier Persistence/Repositories/)
4. Connexion SQL Server dans appsettings.json
5. Première migration EF Core

### Packages NuGet à installer dans Infrastructure :
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`

### Package NuGet à installer dans API :
- `Microsoft.EntityFrameworkCore.Tools`
