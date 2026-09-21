using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Soenneker.Hashing.Argon2;

namespace Melanin.Infrastructure.Database;

public static class DatabaseSeeder
{
    // Remplit la base avec des données de départ (admin, clients, catégories, produits).
    // Idempotent : si la base contient déjà des catégories, on ne fait rien.
    public static async Task SeedAsync(MelaninDbContext context)
    {
        // Sentinelle : si des catégories existent déjà, la base est considérée
        // comme déjà remplie → on sort sans rien créer (évite les doublons au redémarrage).
        if (context.Categories.Any())
            return;

        #region  --- Admin ---

        // Mot de passe lu depuis une variable d'environnement (jamais en dur / jamais commité).
        string adminPassword = Environment.GetEnvironmentVariable("MELANIN_ADMIN_PASSWORD")
            ?? throw new InvalidOperationException(
                "La variable d'environnement MELANIN_ADMIN_PASSWORD est manquante.");

        // Même hachage Argon2 que MemberService → l'admin pourra se connecter normalement.
        string adminHash = await Argon2HashingUtil.Hash(adminPassword);

        Member admin = new Member(
            firstName: "Admin",
            lastName: "Melanin",
            email: "melaninshop.noreply@gmail.com",
            passwordHash: adminHash);

        admin.PromoteToAdmin(); // le constructeur crée un User → on le promeut

        context.Members.Add(admin);

        // --- Admin démo (partagé avec les collègues pour tester le back-office) ---
        // Mot de passe en dur volontairement : ce compte est fait pour être partagé,
        // donc pas un secret à protéger (contrairement à l'admin principal).
        string adminDemoHash = await Argon2HashingUtil.Hash("AdminDemo1234=");

        Member adminDemo = new Member(
            firstName: "Admin",
            lastName: "Démo",
            email: "admin.demo@melanin.be",
            passwordHash: adminDemoHash);

        adminDemo.PromoteToAdmin();

        context.Members.Add(adminDemo);

        #endregion

        #region  --- Clients démo ---

        // Comptes figurants pour peupler le site. Mot de passe de démo commun,
        // en dur volontairement (pas de secret réel ici, contrairement à l'admin).
        // Hashé une seule fois puis réutilisé pour les 6 (même mot de passe).
        string demoHash = await Argon2HashingUtil.Hash("Test1234=");

        Member[] demoMembers =
        [
            new Member("Awa",     "Koné",     "awa.kone@example.com",     demoHash),
            new Member("Inaya",   "Ndiaye",   "inaya.ndiaye@example.com",   demoHash),
            new Member("Kevin",   "Mercier",  "kevin.mercier@example.com",  demoHash),
            new Member("Leïla",   "Benali",   "leila.benali@example.com",   demoHash),
            new Member("Sophie",  "Dubois",   "sophie.dubois@example.com",  demoHash),
            new Member("Koffi",   "Mensah",   "koffi.mensah@example.com", demoHash)
        ];

        context.Members.AddRange(demoMembers);

        // Save : génère les Id des membres → nécessaires pour créer leurs adresses.
        await context.SaveChangesAsync();

        #endregion

        #region  --- Adresses ---
        // Une adresse par client démo. Les membres ont leur Id (save juste au-dessus).
        Address[] addresses =
        [
            new Address("Bruxelles", "1000", "Belgique",   "Rue Neuve 12",         "+32470000001", demoMembers[0].Id, "Awa Koné"),
            new Address("Ixelles",   "1050", "Belgique",   "Chaussée d'Ixelles 45","+32470000002", demoMembers[1].Id, "Inaya Ndiaye"),
            new Address("Anvers",    "2000", "Belgique",   "Meir 78",              "+32470000003", demoMembers[2].Id, "Kevin Mercier"),
            new Address("Paris",     "75001","France",     "Rue de Rivoli 30",     "+33600000004", demoMembers[3].Id, "Leïla Benali"),
            new Address("Amsterdam", "1012", "Pays-Bas",   "Damrak 55",            "+31600000005", demoMembers[4].Id, "Sophie Dubois"),
            new Address("Charleroi", "6000", "Belgique",   "Boulevard Tirou 15",   "+32470000006", demoMembers[5].Id, "Koffi Mensah"),
        ];

        context.Addresses.AddRange(addresses);

        #endregion

        #region  --- Catégories ---

        // Category = nom (affiché) + slug (version URL : minuscules, tirets, sans accents).
        Category[] categories =
        [
            new Category("Soins Capillaires", "soins-capillaires"),
            new Category("Tissages",          "tissages"),
            new Category("Perruques",         "perruques"),
            new Category("Accessoires",       "accessoires"),
        ];

        context.Categories.AddRange(categories);

        // Save : génère les Id des catégories → nécessaires pour créer les produits.
        await context.SaveChangesAsync();

        #endregion

        #region  --- Produits ---

        // Chaque produit référence l'Id d'une catégorie déjà sauvegardée (donc Id connu).
        // On lit ces Id via categories[x].Id. Pour la lisibilité, on les nomme :
        int soinsId = categories[0].Id; // "Soins Capillaires"
        int tissagesId = categories[1].Id; // "Tissages"
        int perruquesId = categories[2].Id; // "Perruques"
        int accessoiresId = categories[3].Id; // "Accessoires"

        Product[] products =
        [
            // -- Soins Capillaires --
            new Product("Huile de Ricin 100% Pure", "Huile 100% pure pour stimuler la pousse et renforcer les racines. 60 ml.", 14.90m, 50, soinsId),
            new Product("Huile de Ricin & Romarin", "Huile fortifiante 100% naturelle, pousse et réparation. 100 ml.", 16.90m, 40, soinsId),
            new Product("Beurre de Karité Pur", "Beurre 100% naturel, nourrit en profondeur les cheveux secs et abîmés. 200 g.", 12.90m, 60, soinsId),
            new Product("Pack Shampooing + Après-Shampooing", "Duo soins complets : nettoie, nourrit, fortifie, hydrate, démêle et protège. 250 ml + 200 ml.", 22.90m, 40, soinsId),

            // -- Tissages (Raw Hair) --
            new Product("Raw Hair Lisse", "Tissage 100% cheveux naturels, texture lisse, qualité premium.", 89.90m, 20, tissagesId, hairColor: "Naturel", hairLength: HairLength.Inches18, hairTexture: HairTexture.Straight),
            new Product("Raw Hair Wavy", "Tissage 100% cheveux naturels, texture ondulée, doux et soyeux.", 94.90m, 20, tissagesId, hairColor: "Naturel", hairLength: HairLength.Inches20, hairTexture: HairTexture.BodyWave),
            new Product("Raw Hair Curly", "Tissage 100% cheveux naturels, texture bouclée, sans enchevêtrement.", 99.90m, 20, tissagesId, hairColor: "Naturel", hairLength: HairLength.Inches18, hairTexture: HairTexture.Curly),

            // -- Perruques --
            new Product("Perruque Lissée Brun Foncé", "Perruque lace, texture lisse, brun foncé, rendu naturel et longueur généreuse.", 149.90m, 12, perruquesId, hairColor: "Brun foncé", hairLength: HairLength.Inches24, hairTexture: HairTexture.Straight, capSize: CapSize.Medium),
            new Product("Perruque Curly Noire", "Perruque bouclée, volume naturel, noir profond.", 159.90m, 12, perruquesId, hairColor: "Noir", hairLength: HairLength.Inches20, hairTexture: HairTexture.Curly, capSize: CapSize.Medium),
            new Product("Perruque Blonde Ondulée", "Perruque ondulée blond miel, effet lumineux.", 169.90m, 10, perruquesId, hairColor: "Blond", hairLength: HairLength.Inches24, hairTexture: HairTexture.BodyWave, capSize: CapSize.Large),
            new Product("Perruque Ondulée Brune", "Perruque ondulée, brun foncé, effet volume et mouvement naturel.", 154.90m, 10, perruquesId, hairColor: "Brun foncé", hairLength: HairLength.Inches22, hairTexture: HairTexture.BodyWave, capSize: CapSize.Medium),

            // -- Accessoires --
            new Product("Bonnets / Wig Caps", "Lot de bonnets pour protéger vos cheveux et maintenir la perruque en place.", 6.90m, 100, accessoiresId),
            new Product("Colle & Dissolvant à Lace", "Kit lace glue + remover pour une fixation fiable et un retrait en douceur.", 19.90m, 35, accessoiresId),
            new Product("Peignes & Brosses", "Set pour démêler, lisser et préserver la qualité de votre perruque.", 14.90m, 40, accessoiresId),
            new Product("Spray Fixateur (Edge Control)", "Pour des baby hairs bien plaqués et une coiffure longue durée. 100 ml.", 9.90m, 50, accessoiresId),
            new Product("Tête / Mannequin d'Exposition", "Support idéal pour coiffer, ajuster et sécher votre perruque.", 24.90m, 25, accessoiresId),
            new Product("Épingles & Pinces", "Lot d'épingles et pinces pour maintenir et coiffer facilement.", 7.90m, 60, accessoiresId),
        ];

        context.Products.AddRange(products);

        // Save final : enregistre les produits.
        await context.SaveChangesAsync();

        #endregion

    }
}