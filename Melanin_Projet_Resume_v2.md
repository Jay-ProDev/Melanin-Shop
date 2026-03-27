# Projet Melanin - Résumé complet (v2)

## Objectif
Créer un site e-commerce semi-premium pour vendre :
- Wigs / Perruques
- Mèches de qualité (Bundles)
- Produits de soin pour cheveux (huiles, crèmes)

### Expérience utilisateur :
- Catalogue produits avec catégories
- Gestion du panier et des commandes
- Paiement sécurisé via Stripe
- Suivi du stock et des commandes
- Espace admin (ajout/suppression produits, gestion descriptions)
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
- Approche **vertical slicing** : une feature complète (Domain → API → React) avant de passer à la suivante
- V1 maintenant, fonctionnalités supplémentaires plus tard
- Veut être guidé comme par un professeur : une étape à la fois, avec explications
- Compare toujours avec les exemples de son prof pour adopter les bonnes pratiques

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

## Script SQL de référence (base pour le développement, EF Core crée la vraie base via migrations)
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

## CE QUI EST FAIT ✅

---

### Melanin.Domain ✅

#### Entités (dossier Entities/)
Style : `private set`, constructeur EF Core `private`, constructeur principal avec paramètres et validations métier, collections en `ICollection<T>`, `default!` pour les strings.

- **Member** → Id, FirstName, LastName, Email, PasswordHash, Role (MemberRole), CreatedAt | Relations : Addresses, Orders, CartItems
  - Méthode `Update(firstName, lastName, email)` avec validations (pour le PUT endpoint)
- **Address** → Id, City, PostalCode, Country, Phone, Street, FullName? | Relations : Member / FK : MemberId
  - ⚠️ Warning CS8618 sur Phone (pas bloquant, à corriger quand on fera la feature Address)
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

#### Exceptions (dossier BusinessExceptions/)
Style : héritage, classe parent générique puis classes spécifiques avec messages intégrés.
- `MemberException` → classe parent avec `(string? message)`
- `MemberNotFoundException` → "Membre introuvable" ou "Membre avec l'id {id} introuvable"
- `MemberAlreadyExistsException` → "Cet email est déjà utilisé"
- `MemberBadCredentialException` → "Email ou mot de passe incorrect"

---

### Melanin.Application ✅ (Member)

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
Task<IEnumerable<Member>> GetAllAsync();
Task UpdateAsync(Member member);
Task DeleteAsync(int id);
```

#### Services (dossier Services/)
**MemberService** → logique métier complète :
- `RegisterAsync` → vérifie email existant, hashe le mot de passe avec Argon2, sauvegarde
- `LoginAsync` → récupère hash, vérifie avec Argon2, retourne le membre
- `GetByIdAsync` → récupère par Id, lance exception si null
- `GetAllAsync` → récupère tous les membres
- `UpdateAsync` → vérifie existence, met à jour
- `DeleteAsync` → vérifie existence, supprime

#### Package NuGet installé :
- `Soenneker.Hashing.Argon2` → hashage des mots de passe

---

### Melanin.Infrastructure ✅ (Member)

#### Configuration Fluent API (dossier Database/Configs/)
Style du prof : `internal class`, noms explicites pour PK/FK/Index, `IsClustered()`, `ValueGeneratedOnAdd()`, `HasConversion<string>()` pour les enums, `HasDefaultValue`, `HasSentinel`.

**MemberConfiguration :**
- Table : `Members`
- PK : `PK_Member`, IsClustered, ValueGeneratedOnAdd
- Colonnes : Id (`Id_Member`), FirstName, LastName, Email (unique, index `IDX_Member__email`), PasswordHash, Role (string, default User), CreatedAt (`datetime`)
- Relations :
  - Addresses → Cascade
  - CartItems → Cascade
  - Orders → **Restrict** (protéger l'historique pour la comptabilité)

#### DbContext (dossier Database/)
**MelaninDbContext** → `public class`
- DbSets : Members, Addresses, Categories, Products, CartItems, Orders, OrderItems, Payments
- `ApplyConfigurationsFromAssembly` pour charger les configs automatiquement
- ⚠️ Warnings decimal (UnitPrice, TotalPrice, Amount) → à régler dans les configurations Fluent API des autres entités

#### Repository (dossier Database/Repositories/)
**MemberRepository** → `public class`, implémente `IMemberRepository`
- Toutes les méthodes async
- `FindAsync` pour GetById, `FirstOrDefaultAsync` pour GetByEmail
- `Select` pour GetHashPwd (récupère uniquement le hash, pas tout l'objet)
- `SaveChangesAsync` après chaque opération d'écriture
- Vérifie existence avant delete, lance `MemberNotFoundException` si null

#### Packages NuGet installés :
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`

#### Migration :
- `InitialCreate` → base de données `MelaninDB` créée et testée

---

### Melanin.API ✅ (Member)

#### DTOs (dossier DTO/)
Style : `required` sur les propriétés, validations DataAnnotations, séparation Request/Response.

**DTO/Request/MemberRequestDTOs.cs :**
- `RegisterDTO` → FirstName, LastName, Email, Password (avec `[RegularExpression]` pour majuscule, minuscule, chiffre, caractère spécial)
- `LoginDTO` → Email, Password
- `UpdateMemberDTO` → FirstName, LastName, Email

**DTO/Response/MemberResponseDTOs.cs :**
- `MemberResponseDto` → Id, FirstName, LastName, Email, CreatedAt (PAS de Role, il est dans le JWT)

#### Mapper (dossier DTO/Mappers/)
**MemberMapper** → `internal static class`, méthodes d'extension :
- `ToDto(this Member member)` → Member → MemberResponseDto
- `ToEntity(this RegisterDTO dto)` → RegisterDTO → Member

#### Token (dossier Token/)
**TokenTool** → `public class`, injection de `IConfiguration`
- Claims : NameIdentifier (MemberId) + Role
- Algorithme : HmacSha512
- Clé, Issuer, Audience, Expiration depuis `appsettings.json` section `Token`
- Classe interne `Data` avec `required int MemberId` et `required string Role`

#### Controller (dossier Controllers/)
**MemberController** → injection de `IMemberService` + `TokenTool`
- `POST /api/Member/register` → inscription, retourne message
- `POST /api/Member/login` → connexion, retourne token JWT
- `GET /api/Member/{id}` → récupérer un membre (via mapper `result.ToDto()`)
- `GET /api/Member` → récupérer tous les membres (via `.Select(m => m.ToDto())`)
- `PUT /api/Member/{id}` → modifier (via `member.Update(...)`)
- `DELETE /api/Member/{id}` → supprimer
- Gestion d'erreurs via `catch (XxxException ex) → return Status(new { ex.Message })`
- `[EndpointSummary]` sur chaque endpoint pour Scalar

#### Configs (dossier Configs/)
**BearerSecuritySchemeTransformer** → `internal sealed class` avec primary constructor (C# 12)
- Ajoute le bouton Bearer token dans Scalar pour tester les endpoints protégés

#### Program.cs :
- DbContext avec SqlServer
- Injection : IMemberRepository → MemberRepository (Scoped), IMemberService → MemberService (Scoped), TokenTool (Singleton)
- JWT Authentication avec validation (Issuer, Audience, SigningKey, Lifetime)
- CORS : `WithOrigins("http://localhost:5173")` + AllowAnyHeader + AllowAnyMethod
- Scalar avec `AddDocumentTransformer` (titre "Melanin API" v1) + BearerSecuritySchemeTransformer
- Middleware order : UseCors → UseHttpsRedirection → UseAuthentication → UseAuthorization → MapControllers

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

#### Packages NuGet installés :
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Scalar.AspNetCore`

#### launchSettings.json :
- `launchBrowser: true`, `launchUrl: "scalar"`

---

### Frontend React ✅ (Member + Design)

#### Packages installés :
- `react-router` (navigation)
- `axios` (appels API)
- `jotai` (state management — atoms)
- `jwt-decode` (décodage JWT)
- `tailwindcss` + `@tailwindcss/vite` (CSS)

#### Store (src/store.js) :
- `store` → `createStore()` de Jotai
- `tokenAtom` → `atomWithStorage("token", null)` — persiste dans localStorage
- `roleAtom` → atom dérivé, décode le rôle depuis le JWT avec `jwtDecode`

#### Services (src/services/) :
**authService.js** — style du prof :
- `authRegister(firstName, lastName, email, password)` → POST /Member/register
- `authLogin(email, password)` → POST /Member/login
- `getMemberById(id)` → GET /Member/{id} (avec token Bearer via `store.get(tokenAtom)`)
- Retourne `{ success: true/false, data/error }` avec try/catch dans le service
- BaseURL via `import.meta.env.VITE_API_URL`

#### .env :
```
VITE_API_URL=https://localhost:7138/api
```

#### Pages (src/pages/) :
- **Home.jsx** → vitrine "Collection 2026", boutons Découvrir + Nos Soins, stylisée dark/light
- **Login.jsx** → formulaire avec `form action={}` + `formData.get()` (style React 19), `useAtom(tokenAtom)`, `<Navigate>` si déjà connecté
- **Register.jsx** → formulaire inscription, prénom/nom sur même ligne, redirige vers /login
- **NotFound.jsx** → page 404 stylisée avec lien retour accueil

#### Composants (src/components/) :
- **Navbar.jsx** → logo MELANIN, liens navigation, switch dark/light (☀/☾) avec localStorage + `document.documentElement.classList`, boutons Connexion/Inscription ou Déconnexion selon token, lien Dashboard si Admin
- **Footer.jsx** → barre de réassurance (livraison, paiement, retours, service client), colonnes (Boutique, Informations, Contact), icônes réseaux sociaux (Instagram, TikTok, Facebook), icônes paiement (Visa, Mastercard, Stripe), copyright

#### App.jsx :
- Layout : `min-h-screen flex flex-col` avec Navbar + main (flex-1) + Footer
- Routes : `/` (Home), `/login`, `/register`, `*` (NotFound)

#### main.jsx :
- `BrowserRouter` → `Provider (Jotai)` → `App`

#### index.html :
- Mode clair par défaut, script qui ajoute `dark` si localStorage theme === "dark"

#### index.css :
- `@import "tailwindcss"`
- `@custom-variant dark (&:where(.dark, .dark *))` → dark mode via classe
- `@theme` → couleurs custom (rose-gold, gold, brown, beige + variantes)

---

## Décisions importantes prises

| Sujet | Décision |
|---|---|
| Approche | Vertical slicing : une feature complète (back + front) avant la suivante |
| Panier | Pas de table Cart, CartItem lié directement à Member |
| Panier invité | Géré en localStorage côté React |
| Adresse facturation | BillingAddressId nullable dans Order |
| Couleur cheveux | String libre, pas d'enum |
| Attributs cheveux | Nullable dans Product pour supporter huiles et crèmes |
| DTOs | Uniquement dans Melanin.API, séparation Request/Response dans fichiers séparés |
| Application | Travaille directement avec les entités Domain |
| Hashage | Argon2 via Soenneker.Hashing.Argon2 |
| Collections | ICollection<T> dans les entités |
| Constructeurs | EF Core private, principal avec paramètres et validations |
| Suppression Member | Restrict sur Orders (protéger historique comptabilité), soft delete prévu pour plus tard |
| Rôle dans DTO | NON — le rôle est dans le JWT Token, pas dans le MemberResponseDto |
| JWT | HmacSha512, config dans appsettings.json, TokenTool en Singleton |
| State management | Jotai (atoms) au lieu de createContext — plus simple, store accessible hors React |
| Dark mode | Tailwind dark: classes + classe `dark` sur HTML + localStorage |
| CSS | Full Tailwind, pas de CSS Modules, pas de fichiers CSS séparés |
| Documentation API | Scalar (au lieu de Swagger) |
| CORS | WithOrigins strict (localhost:5173), pas AllowAll |
| Formulaires React | `form action={}` + `formData.get()` (style React 19, pas useState par champ) |

---

## CE QUI RESTE À FAIRE ⏳

### Prochaines features backend (vertical slicing) :

#### Feature Category
1. **Infrastructure** → CategoryConfiguration (Fluent API)
2. **Application** → ICategoryRepository, ICategoryService, CategoryService
3. **Infrastructure** → CategoryRepository
4. **API** → CategoryDTOs, CategoryMapper, CategoryController
5. **Migration** → (déjà dans InitialCreate, mais vérifier la config)
6. **Frontend** → (utilisé dans la page Shop pour filtrer)

#### Feature Product
1. **Infrastructure** → ProductConfiguration (Fluent API) — ⚠️ ajouter HasPrecision(10,2) sur UnitPrice
2. **Application** → IProductRepository, IProductService, ProductService
3. **Infrastructure** → ProductRepository
4. **API** → ProductDTOs, ProductMapper, ProductController
5. **Migration**
6. **Frontend** → Page Shop (catalogue), Page ProductDetail, composant ProductCard

#### Feature Address
1. **Infrastructure** → AddressConfiguration — corriger warning Phone
2. **Application** → IAddressRepository, IAddressService, AddressService
3. **Infrastructure** → AddressRepository
4. **API** → AddressDTOs, AddressMapper, AddressController
5. **Frontend** → Formulaire d'adresse dans le checkout

#### Feature Cart
1. **Infrastructure** → CartItemConfiguration — ajouter HasPrecision(10,2)
2. **Application** → ICartItemRepository, ICartItemService, CartItemService
3. **Infrastructure** → CartItemRepository
4. **API** → CartDTOs, CartMapper, CartController
5. **Frontend** → Page Panier, composant CartItem, icône panier dans Navbar

#### Feature Order
1. **Infrastructure** → OrderConfiguration + OrderItemConfiguration — ajouter HasPrecision(10,2)
2. **Application** → IOrderRepository, IOrderService, OrderService
3. **Infrastructure** → OrderRepository
4. **API** → OrderDTOs, OrderMapper, OrderController
5. **Frontend** → Page Commande, historique des commandes

#### Feature Payment (Stripe)
1. **Infrastructure** → PaymentConfiguration — ajouter HasPrecision(10,2)
2. **Application** → IPaymentService
3. **Infrastructure** → StripeService
4. **API** → PaymentController (webhooks Stripe)
5. **Frontend** → Page Paiement, redirection Stripe

### Améliorations frontend prévues :
- Page Shop (catalogue produits avec filtres par catégorie)
- Page ProductDetail
- Panier avec icône dans Navbar
- Checkout (adresse + paiement)
- Dashboard Admin (ajout/suppression produits, gestion descriptions)
- Responsive design (mobile)
- Favicon / logo Melanin
- Pages légales (CGV, Confidentialité, Livraison, Retours)

### Améliorations backend prévues :
- Soft delete sur Member (IsActive au lieu de supprimer)
- `[Authorize]` et `[Authorize(Roles = "Admin")]` sur les endpoints sensibles
- ExceptionHandler global (pattern du prof)
- Seed data pour les catégories
- Gestion des images produits
- Pagination sur GetAll
