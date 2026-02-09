using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using Microsoft.AspNetCore.Identity;
using NetShopAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NetShopAPI.Services.CartServices;
using NetShopAPI.Services.OrderServices;
using NetShopAPI.Services.SupplyServices;
using FluentValidation;
using FluentValidation.AspNetCore;
using NetShopAPI.Services.ClientServices;
using NetShopAPI.Services.CurrentUserServices;
using NetShopAPI.Infrastructure.Persistence.Middlewares;
using NetShopAPI.Features.Stock.Commands.AddToStock;
using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;
using NetShopAPI.Features.Catalog.Positions.Queries.GetPositionById;
using NetShopAPI.Features.Catalog.Positions.Queries.GetPositionsByCategory;
using NetShopAPI.Features.Catalog.Categories.Command;
using NetShopAPI.Features.Catalog.Categories.Query;
using NetShopAPI.Infrastructure.Identity;
using NetShopAPI.Features.Authorized.Services;
using NetShopAPI.Features.Authorized.Register.Commands;
using NetShopAPI.Features.Authorized.Login.Query;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "¬ведите JWT токен без слова Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<CreateRegisterUserHandler>();
builder.Services.AddScoped<AuthorizationUserHandler>();
builder.Services.AddScoped<GetAllCategoriesHandler>();
builder.Services.AddScoped<CreateCategoryHandler>();
builder.Services.AddScoped<GetPositionByIdHandler>();
builder.Services.AddScoped<GetPositionsByCategoryHandler>();
builder.Services.AddScoped<BulkCreatePositionsHandler>();
builder.Services.AddScoped<AddToStockHandler>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplyService, SupplyService>();

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions!.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ShopDbContext>(options => options.UseMySql(
    connectionString,
    ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
