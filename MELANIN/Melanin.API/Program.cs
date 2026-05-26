using Melanin.API.Configs;
using Melanin.API.Token;
using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Application.Interfaces.Utils;
using Melanin.Application.Services;
using Melanin.Infrastructure.Database;
using Melanin.Infrastructure.Database.Repositories;
using Melanin.Infrastructure.Stripe;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// === Database ===
// builder.Services.AddDbContext<MelaninDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
// );

var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

builder.Services.AddDbContext<MelaninDbContext>(options =>
{
    if (provider == "Sqlite")
    {
        options.UseSqlite(
            builder.Configuration.GetConnectionString("Sqlite"),
            b => b.MigrationsAssembly("Melanin.Infrastructure.Database.Sqlite")
        );
    }
    else
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("SqlServer"),
            b => b.MigrationsAssembly("Melanin.Infrastructure.Database")
        );
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFront", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// === Repositories ===
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();



// === Services ===
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


// === Token ===
builder.Services.AddSingleton<TokenTool>();

// === Authentication JWT ===
byte[] secretKey = Encoding.UTF8.GetBytes(
    builder.Configuration["Token:Key"]
    ?? throw new Exception("Clef du token non défini dans la config !")
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });

// === Stripe ===
builder.Services.AddScoped<IStripeUtil>(provider =>
    new StripeUtil(
        builder.Configuration["Stripe:SecretKey"]
            ?? throw new Exception("Stripe:SecretKey non défini dans la config !"),
        builder.Configuration["Stripe:WebhookSecret"]
            ?? throw new Exception("Stripe:WebhookSecret non défini dans la config !"),
        builder.Configuration["Stripe:SuccessUrl"]
            ?? throw new Exception("Stripe:SuccessUrl non défini dans la config !"),
        builder.Configuration["Stripe:CancelUrl"]
            ?? throw new Exception("Stripe:CancelUrl non défini dans la config !")
    )
);

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Melanin API",
            Version = "v1",
            Description = "API e-commerce pour la vente de wigs, mèches et produits capillaires"
        };
        return Task.CompletedTask;
    });
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});


var app = builder.Build();

// === Middleware ===
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFront");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();