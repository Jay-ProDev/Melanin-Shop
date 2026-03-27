# Projet Melanin - Résumé complet (v3)

## Objectif
Créer un site e-commerce semi-premium pour vendre :
- Wigs / Perruques
- Mèches de qualité (Bundles)
- Produits de soin pour cheveux (huiles, crèmes)

### Expérience utilisateur :
- Catalogue produits avec catégories et filtres
- Barre de recherche
- Gestion du panier et des commandes
- Paiement sécurisé via Stripe
- Suivi du stock et des commandes
- Espace admin (ajout/suppression produits, gestion stock, activate/deactivate)
- Switch dark/light mode

---

## Stack technique
- **Frontend** : React + Tailwind CSS v4 + Jotai (state management) + Axios + React Router
- **Backend** : C# ASP.NET Core Web API + EF Core
- **Base de données** : SQL Server (potentiellement PostgreSQL en production)
- **Authentification** : JWT Token (HmacSha512)
- **Hashage** : Argon2 via Soenneker.Hashing.Argon2
- **Documentation API** : Scalar (au lieu de Swagger)
- **Build tool frontend** : Vite

---

## Profil du développeur
- Développeur junior sérieux et structuré
- Comprend : UML, EF Core, Clean Architecture, séparation Front/Back, base de données relationnelle
- Développe en C#, React et SQL Server
- Veut faire les choses proprement dès le départ
- Approche **vertical slicing** : une feature complète (Domain → API) avant de passer à la suivante, puis frontend
- V1 maintenant, fonctionnalités supplémentaires plus tard
- Veut être guidé comme par un professeur : une étape à la fois, avec explications

---

## Architecture Backend : Clean Architecture
```
Melanin.Domain          → Entités, Enums, Exceptions
Melanin.Application     → Interfaces, Services
Melanin.Infrastructure  → EF Core, DbContext, Repositories, Configurations
Melanin.API             → Controllers, DTOs, Mappers, Token, Configs
```

### Références entre projets :
- `API` → référence `Application` + `Infrastructure`
- `Application` → référence `Domain`
- `Infrastructure` → référence `Application` + `Domain`

### Namespaces importants :
- `Melanin.Infrastructure.Database.Configs` → configurations Fluent API
- `Melanin.Infrastructure.Database.Repositories` → repositories
- `Melanin.Application.Interfaces.Repositories` → interfaces repositories
- `Melanin.Application.Interfaces.Services` → interfaces services
- `Melanin.Application.Services` → services
- `Melanin.Domain.BusinessExecptions` → ⚠️ typo volontaire (comme le prof)

---

## Architecture Frontend
```
melanin-front/
├── public/
├── src/
│   ├── assets/          → images, logos
│   ├── components/      → composants réutilisables (Navbar, Footer)
│   ├── pages/           → les pages (Home, Login, Register, NotFound)
│   ├── services/        → appels API avec axios
│   ├── store.js         → atoms Jotai (tokenAtom, roleAtom)
│   ├── App.jsx          → routes + layout
│   ├── main.jsx         → point d'entrée (BrowserRouter, Provider Jotai)
│   └── index.css        → import Tailwind + couleurs custom
├── .env                 → VITE_API_URL
├── index.html
└── package.json
```

---

## Design / Thème
- **Mode clair** (défaut) : Beige (#F5F0EB), marron (#5C3D2E), doré (#B8860B) — style chaleureux
- **Mode sombre** : Noir (#0A0A0A), rose gold (#E8C4B0) — style luxe
- **Switch** : bouton ☀/☾ dans la Navbar, persiste via localStorage
- **Style** : Minimaliste et élégant, typographie serif pour les titres, tracking large, lettres majuscules
- **Dark mode Tailwind** : via classe `dark` sur `<html>` + `@custom-variant dark` dans index.css
- **Inspiration** : diouda.fr pour le footer e-commerce

### Couleurs custom Tailwind (définies dans index.css @theme) :
- `rose-gold` / `rose-gold-dark` / `rose-gold-light`
- `gold` / `gold-dark` / `gold-light`
- `brown` / `brown-dark` / `brown-light`
- `beige` / `beige-dark` / `beige-light`

---

## Règles et conventions importantes

### Règle nullable Repository vs Service :
- **Repository** → retourne `T?` (peut ne pas trouver en base → null)
- **Service** → retourne `T` (gère le null lui-même → exception ou résultat garanti)
- **Controller** → reçoit soit `T` ✅ soit une exception ❌ (ne voit jamais de null)

### Règle double vérification Delete :
- Le **Service** vérifie l'existence → lance exception si null
- Le **Repository** supprime directement sans re-vérifier (`Remove(entity!)`)
- ⚠️ MemberRepository du prof a une double vérification — c'est une erreur, on ne la reproduit pas

### Règle relations Fluent API :
- On définit une relation dans **un seul sens** — pas des deux côtés
- Exemple : relation Category↔Product définie dans `CategoryConfiguration` uniquement

### Style des entités :
- `private set` sur toutes les propriétés
- Constructeur EF Core `private` sans paramètres
- Constructeur principal avec paramètres et validations métier (`ArgumentException`)
- Collections en `ICollection<T>` initialisées avec `new List<T>()`
- `default!` pour les strings non-nullable
- Méthodes `Update(...)`, `Activate()`, `Deactivate()`, `IncreaseStock()`, `DecreaseStock()` dans l'entité

### Style des DTOs :
- `required` + DataAnnotations (`[Required]`, `[MaxLength]`, `[Range]`, `[EmailAddress]`)
- Séparation Request/Response dans fichiers séparés
- ResponseDTO : `= default!` sur les strings, pas de `required`
- Pas de `IsActive` ni `CreatedAt` dans les DTOs de création (gérés par l'entité)

### Style des Controllers :
- `[FromBody]` explicite sur les DTOs
- `[EndpointSummary("...")]` sur chaque endpoint
- Pas de suffixe `Async` dans les noms de méthodes
- `Ok(new { Message = "..." })` pour Update et Delete (pas `NoContent`)
- Catch ciblé par type d'exception

### JsonStringEnumConverter :
- Ajouté dans `Program.cs` → `AddControllers().AddJsonOptions(...)` 
- Permet d'envoyer les enums en string dans les requêtes JSON (`"Inches18"` au lieu de `4`)

---

## Script SQL de référence
```sql
CREATE TABLE Member_(Id_Member INT, FirstName VARCHAR(50) NOT NULL, LastName VARCHAR(50) NOT NULL, Email VARCHAR(50) NOT NULL, PasswordHash VARCHAR(255) NOT NULL, Role VARCHAR(50), CreatedAt DATETIME, PRIMARY KEY(Id_Member), UNIQUE(Email));
CREATE TABLE Address(Id_Address INT, City VARCHAR(50) NOT NULL, PostalCode INT NOT NULL, Country VARCHAR(50) NOT NULL, Phone VARCHAR(20) NOT NULL, Street VARCHAR(50) NOT NULL, FullName VARCHAR(100), Id_Member INT NOT NULL, PRIMARY KEY(Id_Address), FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member));
CREATE TABLE Category(Id_Category INT, Name VARCHAR(50) NOT NULL, Slug VARCHAR(50) NOT NULL, PRIMARY KEY(Id_Category));
CREATE TABLE Product(Id_Product INT, Name VARCHAR(50) NOT NULL, Description VARCHAR(500) NOT NULL, UnitPrice DECIMAL(10,2) NOT NULL, StockQuantity INT NOT NULL, CreatedAt DATETIME, IsActive BIT NOT NULL, HairLength VARCHAR(20), HairTexture VARCHAR(20), HairColor VARCHAR(50), CapSize VARCHAR(20), Id_Category INT NOT NULL, PRIMARY KEY(Id_Product), FOREIGN KEY(Id_Category) REFERENCES Category(Id_Category));
CREATE TABLE CartItem(Id_CartItem INT, Quantity INT NOT NULL, UnitPrice DECIMAL(10,2) NOT NULL, Id_Product INT NOT NULL, Id_Member INT NOT NULL, PRIMARY KEY(Id_CartItem), FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product), FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member));
CREATE TABLE Order_(Id_Order INT, TotalPrice DECIMAL(10,2) NOT NULL, CreatedAt DATETIME, Status VARCHAR(50) NOT NULL, ShippingAddressId INT NOT NULL, BillingAddressId INT NULL, CouponCode VARCHAR(50) NULL, Id_Member INT NOT NULL, PRIMARY KEY(Id_Order), FOREIGN KEY(ShippingAddressId) REFERENCES Address(Id_Address), FOREIGN KEY(BillingAddressId) REFERENCES Address(Id_Address), FOREIGN KEY(Id_Member) REFERENCES Member_(Id_Member));
CREATE TABLE OrderItem(Id_OrderItem INT, Quantity INT NOT NULL, UnitPrice DECIMAL(10,2) NOT NULL, Id_Order INT NOT NULL, Id_Product INT NOT NULL, PRIMARY KEY(Id_OrderItem), FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order), FOREIGN KEY(Id_Product) REFERENCES Product(Id_Product));
CREATE TABLE Payment(Id_Payment VARCHAR(50), Amount DECIMAL(10,2) NOT NULL, Status VARCHAR(50) NOT NULL, PaidAt DATETIME, StripeSessionId VARCHAR(255), Id_Order INT NOT NULL, PRIMARY KEY(Id_Payment), UNIQUE(Id_Order), FOREIGN KEY(Id_Order) REFERENCES Order_(Id_Order));
```

---

## CE QUI EST FAIT ✅

---

### Melanin.Domain ✅

#### Entités (dossier Entities/)

**Member** → Id, FirstName, LastName, Email, PasswordHash, Role (MemberRole), CreatedAt
- Relations : Addresses, Orders, CartItems
- Méthode `Update(firstName, lastName, email)`

**Address** → Id, City, PostalCode, Country, Phone, Street, FullName?
- Relations : Member / FK : MemberId
- ⚠️ Warning CS8618 sur Phone (à corriger dans la feature Address)

**Category** → Id, Name, Slug
- Relations : Products
- Méthode `Update(name, slug)`

**Product** → Id, Name, Description, UnitPrice, StockQuantity, IsActive, HairColor? (nullable depuis v3), HairLength?, HairTexture?, CapSize?, CreatedAt
- Relations : Category / FK : CategoryId
- `IsActive = true` et `CreatedAt = DateTime.UtcNow` dans le constructeur
- Méthodes : `Update(...)`, `Activate()`, `Deactivate()`, `IncreaseStock(quantity)`, `DecreaseStock(quantity)`
- `DecreaseStock` lance `ProductOutOfStockException` si stock insuffisant

**CartItem** → Id, Quantity, UnitPrice
- Relations : Product, Member / FK : ProductId, MemberId

**Order** → Id, TotalPrice, CreatedAt, Status (OrderStatus), CouponCode?
- Relations : Member, ShippingAddress, BillingAddress?, OrderItems, Payment?
- FK : MemberId, ShippingAddressId, BillingAddressId?

**OrderItem** → Id, Quantity, UnitPrice
- Relations : Order, Product / FK : OrderId, ProductId

**Payment** → Id (string), Amount, Status (PaymentStatus), PaidAt?, StripeSessionId?
- Relations : Order / FK : OrderId

#### Enums (dossier Enums/)
- `MemberRole` → User, Admin
- `OrderStatus` → Pending, Confirmed, Shipped, Delivered, Cancelled
- `PaymentStatus` → Pending, Completed, Failed, Refunded
- `HairLength` → Inches8, Inches10, Inches12, Inches14, Inches16, Inches18, Inches20, Inches22, Inches24, Inches26, Inches28, Inches30
- `HairTexture` → Straight, Wavy, Curly, Kinky, DeepWave, BodyWave, LooseWave
- `CapSize` → Small, Medium, Large, XLarge

#### Exceptions (dossier BusinessExecptions/) ⚠️ typo volontaire
Style : un fichier par feature, toutes les classes dans le même fichier, `string? message`.

**MemberException.cs** : MemberException, MemberNotFoundException (sans param + avec id), MemberAlreadyExistsException, MemberBadCredentialException

**CategoryException.cs** : CategoryException, CategoryNotFoundException (sans param + avec id), CategoryAlreadyExistsException(slug)

**ProductException.cs** : ProductException, ProductNotFoundException (sans param + avec id), ProductOutOfStockException(name)

---

### Melanin.Application ✅ (Member + Category + Product)

#### Interfaces Repositories (dossier Interfaces/Repositories/)

**IMemberRepository** (dans `Interfaces/` directement — style prof) :
```csharp
Task<Member?> GetByIdAsync(int id);
Task<Member?> GetByEmailAsync(string email);
Task<string?> GetHashPwdAsync(string email);
Task<IEnumerable<Member>> GetAllAsync();
Task<Member> AddAsync(Member member);
Task UpdateAsync(Member member);
Task DeleteAsync(int id);
```

**ICategoryRepository** :
```csharp
Task<Category?> GetByIdAsync(int id);
Task<Category?> GetBySlugAsync(string slug);
Task<IEnumerable<Category>> GetAllAsync();
Task<Category> AddAsync(Category category);
Task UpdateAsync(Category category);
Task DeleteAsync(int id);
```

**IProductRepository** :
```csharp
Task<Product?> GetByIdAsync(int id);
Task<IEnumerable<Product>> GetAllAsync();
Task<IEnumerable<Product>> GetAllActiveAsync();
Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
Task<IEnumerable<Product>> SearchAsync(string keyword);
Task<Product> AddAsync(Product product);
Task UpdateAsync(Product product);
Task DeleteAsync(int id);
```

#### Interfaces Services (dossier Interfaces/Services/)

**IMemberService** (dans `Interfaces/` directement — style prof) :
```csharp
Task<Member> RegisterAsync(Member member);
Task<Member> LoginAsync(string email, string password);
Task<Member> GetByIdAsync(int id);
Task<IEnumerable<Member>> GetAllAsync();
Task UpdateAsync(Member member);
Task DeleteAsync(int id);
```

**ICategoryService** :
```csharp
Task<Category> GetByIdAsync(int id);
Task<Category> GetBySlugAsync(string slug);
Task<IEnumerable<Category>> GetAllAsync();
Task<Category> CreateAsync(Category category);
Task UpdateAsync(Category category);
Task DeleteAsync(int id);
```

**IProductService** :
```csharp
Task<Product> GetByIdAsync(int id);
Task<IEnumerable<Product>> GetAllAsync();
Task<IEnumerable<Product>> GetAllActiveAsync();
Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
Task<IEnumerable<Product>> SearchAsync(string keyword);
Task<Product> CreateAsync(Product product);
Task UpdateAsync(Product product);
Task DeleteAsync(int id);
```

#### Services (dossier Services/)

**MemberService** : RegisterAsync (vérifie email + Argon2 hash), LoginAsync (Argon2 verify), GetByIdAsync, GetAllAsync, UpdateAsync, DeleteAsync

**CategoryService** : CreateAsync (vérifie slug unique), GetByIdAsync, GetBySlugAsync, GetAllAsync, UpdateAsync (vérifie existence + slug unique sur autre catégorie), DeleteAsync

**ProductService** : CreateAsync (**vérifie que la categoryId existe** via ICategoryRepository), GetByIdAsync, GetAllAsync, GetAllActiveAsync, GetByCategoryIdAsync, SearchAsync, UpdateAsync, DeleteAsync
- ⚠️ `ProductService` injecte aussi `ICategoryRepository` pour vérifier l'existence de la catégorie

#### Package NuGet :
- `Soenneker.Hashing.Argon2`

---

### Melanin.Infrastructure ✅ (Member + Category + Product)

#### Configurations Fluent API (dossier Database/Configs/)
Style : `internal class`, `ToTable`, `HasName`, `IsClustered`, `ValueGeneratedOnAdd`, `HasColumnName`, `HasConversion<string>()` pour enums, `HasSentinel(0)` sur enums, `HasPrecision(10,2)` sur decimals, noms explicites FK/Index.

**MemberConfiguration** : Table `Members`, colonnes avec noms SQL, Email unique (IDX_Member__email), Role en string avec default User + Sentinel, Relations Addresses/CartItems (Cascade) + Orders (Restrict)

**CategoryConfiguration** : Table `Category`, Slug unique (IDX_Category__slug), Relation Products (Restrict)

**ProductConfiguration** : Table `Product`, UnitPrice HasPrecision(10,2), HairLength/HairTexture/CapSize en string + HasSentinel(0), **pas de relation définie ici** (déjà dans CategoryConfiguration)

#### MelaninDbContext (dossier Database/)
- DbSets : Members, Addresses, Categories, Products, CartItems, Orders, OrderItems, Payments
- `ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())`

#### Repositories (dossier Database/Repositories/)

**MemberRepository** : FindAsync pour GetById, FirstOrDefaultAsync pour GetByEmail, Select pour GetHashPwd, SaveChangesAsync après chaque écriture, ⚠️ double vérification dans DeleteAsync (erreur du prof — à ne pas reproduire)

**CategoryRepository** : FindAsync pour GetById, FirstOrDefaultAsync pour GetBySlug, pas de double vérification dans Delete (`Remove(category!)`)

**ProductRepository** :
- `GetByIdAsync` → `FirstOrDefaultAsync` + `.Include(p => p.Category)` (FindAsync ne supporte pas Include)
- `GetAllAsync` → `.Include(p => p.Category)`
- `GetAllActiveAsync` → `.Where(p => p.IsActive).Include(...)`
- `GetByCategoryIdAsync` → `.Where(p => p.CategoryId == id && p.IsActive).Include(...)`
- `SearchAsync` → `.Where(p => p.IsActive && (p.Name.Contains(kw) || p.Description.Contains(kw))).Include(...)`
- `AddAsync` → sauvegarde puis **recharge avec Include** (`FirstAsync` après SaveChanges) pour éviter NullReferenceException dans le Mapper
- `DeleteAsync` → `Remove(product!)` sans double vérification

#### Packages NuGet :
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`

#### Migrations :
- `InitialCreate` → base MelaninDB créée
- `AddCategory` → table Category ajoutée
- `AddProduct` → table Product configurée (rename, colonnes, précision)
- `UpdateProductHairColorNullable` → HairColor devient nullable

---

### Melanin.API ✅ (Member + Category + Product)

#### DTOs (dossier DTO/)

**Request/MemberRequestDTOs.cs** : RegisterDTO, LoginDTO, UpdateMemberDTO

**Request/CategoryDTOs.cs** : CreateCategoryDTO, UpdateCategoryDTO

**Request/ProductDTOs.cs** :
- `CreateProductDTO` → Name, Description, UnitPrice ([Range(0.01,...)]), StockQuantity, CategoryId, HairColor?, HairLength?, HairTexture?, CapSize?
- `UpdateProductDTO` → même sans StockQuantity (stock géré séparément)
- `UpdateStockDTO` → Quantity ([Range(1,...)])

**Response/MemberResponseDTO.cs** : Id, FirstName, LastName, Email, CreatedAt (pas de Role)

**Response/CategoryResponseDTO.cs** : Id, Name, Slug

**Response/ProductResponseDTO.cs** : Id, Name, Description, UnitPrice, StockQuantity, IsActive, HairColor?, HairLength?, HairTexture?, CapSize?, CategoryId, **CategoryName**, CreatedAt

#### Mappers (dossier DTO/Mappers/)
Style : `internal static class`, méthodes d'extension.

**MemberMapper** : `ToDto(Member)`, `ToEntity(RegisterDTO)`

**CategoryMapper** : `ToDto(Category)`, `ToEntity(CreateCategoryDTO)`

**ProductMapper** :
- `ToDto(Product)` → inclut `CategoryName = product.Category?.Name ?? string.Empty`
- `ToEntity(CreateProductDTO)` → pas de IsActive ni CreatedAt (gérés par l'entité)

#### Token (dossier Token/)
**TokenTool** : HmacSha512, Claims (NameIdentifier + Role), config depuis appsettings.json, classe interne `Data { required int MemberId, required string Role }`

#### Controllers (dossier Controllers/)

**MemberController** : register, login (retourne JWT), GetById, GetAll, Update, Delete

**CategoryController** :
- POST / → Create
- GET /{id} → GetById
- GET /slug/{slug} → GetBySlug
- GET / → GetAll
- PUT /{id} → Update
- DELETE /{id} → Delete

**ProductController** :
- POST / → Create (catch CategoryNotFoundException)
- GET /{id} → GetById
- GET / → GetAll (Admin)
- GET /active → GetAllActive (Shop public)
- GET /category/{categoryId} → GetByCategory
- GET /search?keyword=xxx → Search ([FromQuery])
- PUT /{id} → Update
- PUT /{id}/activate → Activate
- PUT /{id}/deactivate → Deactivate
- PUT /{id}/stock/increase → IncreaseStock
- PUT /{id}/stock/decrease → DecreaseStock (catch ProductOutOfStockException → 400)
- DELETE /{id} → Delete

#### Configs (dossier Configs/)
**BearerSecuritySchemeTransformer** : ajoute le bouton Bearer dans Scalar

#### Program.cs :
- DbContext SqlServer
- `AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))` ← permet d'envoyer les enums en string
- Injections Scoped : IMemberRepository→MemberRepository, IMemberService→MemberService, ICategoryRepository→CategoryRepository, ICategoryService→CategoryService, IProductRepository→ProductRepository, IProductService→ProductService
- TokenTool Singleton
- JWT Authentication (Issuer, Audience, SigningKey HmacSha512, Lifetime)
- CORS : `WithOrigins("http://localhost:5173")`
- Scalar avec BearerSecuritySchemeTransformer, titre "Melanin API" v1
- Middleware : UseCors → UseHttpsRedirection → UseAuthentication → UseAuthorization → MapControllers

#### appsettings.json :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MelaninDB;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "Token": {
    "Key": "[clé secrète 64+ caractères]",
    "Issuer": "MelaninAPI",
    "Audience": "MelaninClient",
    "Expire": 120
  }
}
```

#### Packages NuGet API :
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Scalar.AspNetCore`

---

### Frontend React ✅ (Member + Design)

#### Packages installés :
- `react-router`, `axios`, `jotai`, `jwt-decode`, `tailwindcss`, `@tailwindcss/vite`

#### Store (src/store.js) :
- `store` → `createStore()` Jotai
- `tokenAtom` → `atomWithStorage("token", null)`
- `roleAtom` → atom dérivé, décode le rôle depuis JWT avec `jwtDecode`

#### Services (src/services/authService.js) :
Style du prof : retourne `{ success: true/false, data/error }` avec try/catch dans le service.
- `authRegister`, `authLogin`, `getMemberById`
- BaseURL via `import.meta.env.VITE_API_URL`
- Token Bearer via `store.get(tokenAtom)`

#### Pages :
- **Home.jsx** → vitrine "Collection 2026", boutons Découvrir + Nos Soins
- **Login.jsx** → `form action={}` + `formData.get()` (style React 19), `useAtom(tokenAtom)`, `<Navigate>` si déjà connecté
- **Register.jsx** → formulaire inscription, redirige vers /login
- **NotFound.jsx** → page 404

#### Composants :
- **Navbar.jsx** → logo MELANIN, liens, switch ☀/☾ (localStorage + classList), Connexion/Inscription ou Déconnexion selon token, lien Dashboard si Admin
- **Footer.jsx** → réassurance, colonnes, icônes réseaux sociaux + paiement

#### App.jsx : Layout min-h-screen, routes /, /login, /register, *

---

## Décisions importantes prises

| Sujet | Décision |
|---|---|
| Approche | Vertical slicing : backend complet puis frontend par feature |
| Panier | Pas de table Cart, CartItem lié directement à Member |
| Panier invité | Géré en localStorage côté React |
| Adresse facturation | BillingAddressId nullable dans Order |
| Couleur cheveux | String libre, **nullable** (pas tous les produits ont une couleur) |
| Attributs cheveux | Nullable dans Product pour supporter huiles et crèmes |
| DTOs | Uniquement dans Melanin.API, séparation Request/Response |
| Application | Travaille directement avec les entités Domain |
| Hashage | Argon2 via Soenneker.Hashing.Argon2 |
| Collections | ICollection<T> dans les entités |
| Constructeurs | EF Core private, principal avec paramètres et validations |
| Suppression Member | Restrict sur Orders (protéger historique comptabilité) |
| Rôle dans DTO | NON — le rôle est dans le JWT Token uniquement |
| JWT | HmacSha512, config appsettings.json, TokenTool Singleton |
| State management | Jotai (atoms) — store accessible hors React |
| Dark mode | Tailwind dark: classes + classe `dark` sur HTML + localStorage |
| CSS | Full Tailwind, pas de CSS Modules |
| Documentation API | Scalar |
| CORS | WithOrigins strict (localhost:5173) |
| Formulaires React | `form action={}` + `formData.get()` (style React 19) |
| Enums JSON | JsonStringEnumConverter → enums en string dans les requêtes |
| Relations Fluent API | Définies dans un seul sens uniquement |
| Include EF Core | Dans tous les GET de ProductRepository (charge la Category) |
| AddAsync Product | Recharge avec Include après SaveChanges (évite NullRef dans Mapper) |
| DecreaseStock | Pas d'endpoint dédié — appelé automatiquement dans OrderService |
| V2 entités | Hiérarchie ProductBase → HairProduct / CareProduct (TPH ou TPT) |

---

## CE QUI RESTE À FAIRE ⏳

### Backend (vertical slicing)

#### Feature Address
1. `AddressConfiguration` (Fluent API) — corriger warning Phone CS8618
2. `IAddressRepository`, `IAddressService` (Application)
3. `AddressService` (Application)
4. `AddressRepository` (Infrastructure)
5. `AddressDTOs`, `AddressMapper`, `AddressController` (API)
6. Migration
7. Frontend → formulaire adresse dans le checkout

#### Feature Cart
1. `CartItemConfiguration` — ajouter `HasPrecision(10,2)` sur UnitPrice ⚠️
2. `ICartItemRepository`, `ICartItemService`
3. `CartItemService`
4. `CartItemRepository`
5. `CartDTOs`, `CartMapper`, `CartController`
6. Migration
7. Frontend → page Panier, composant CartItem, icône panier dans Navbar

#### Feature Order
1. `OrderConfiguration` + `OrderItemConfiguration` — `HasPrecision(10,2)` ⚠️
2. `IOrderRepository`, `IOrderService`
3. `OrderService` — appelle `product.DecreaseStock()` automatiquement
4. `OrderRepository`
5. `OrderDTOs`, `OrderMapper`, `OrderController`
6. Migration
7. Frontend → page Commande, historique commandes

#### Feature Payment (Stripe)
1. `PaymentConfiguration` — `HasPrecision(10,2)` ⚠️
2. `IPaymentService`
3. `StripeService`
4. `PaymentController` (webhooks Stripe)
5. Frontend → page Paiement, redirection Stripe

### Frontend React (à faire maintenant)

#### Feature Category + Product
- `categoryService.js` → getAll (pour le dropdown du formulaire Admin)
- `productService.js` → getAll, getAllActive, getByCategory, search, create, update, delete, increaseStock, decreaseStock, activate, deactivate
- **Page Shop** → catalogue avec filtres par catégorie (cases à cocher), barre de recherche
- **Page ProductDetail** → détail d'un produit
- **Composant ProductCard** → carte produit réutilisable

#### Améliorations frontend prévues
- Panier avec icône dans Navbar (localStorage pour invités)
- Checkout (adresse + paiement)
- **Dashboard Admin** → ajout/modification produits (dropdown catégories dynamique), gestion stock (boutons +/-), activate/deactivate
- Responsive design (mobile)
- Favicon / logo Melanin
- Pages légales (CGV, Confidentialité, Livraison, Retours)

### Améliorations backend prévues
- `[Authorize]` et `[Authorize(Roles = "Admin")]` sur les endpoints sensibles
- ExceptionHandler global (pattern du prof)
- Seed data pour les catégories
- Gestion des images produits
- Pagination sur GetAll
- Soft delete sur Member (IsActive au lieu de supprimer)

---

## Commandes utiles

### Migrations EF Core
```bash
# Créer une migration
dotnet ef migrations add NomMigration --project Melanin.Infrastructure --startup-project Melanin.API

# Appliquer en base
dotnet ef database update --project Melanin.Infrastructure --startup-project Melanin.API

# Annuler la dernière migration
dotnet ef migrations remove --project Melanin.Infrastructure --startup-project Melanin.API
```

### Lancer le projet
```bash
# Backend
cd Melanin.API && dotnet run

# Frontend
cd melanin-front && npm run dev
```

### URLs
- API : `https://localhost:7138/api`
- Scalar : `https://localhost:7138/scalar`
- Frontend : `http://localhost:5173`
