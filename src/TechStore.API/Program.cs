using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TechStore.Application.Interfaces;
using TechStore.Application.Services;
using TechStore.Core.Entities;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;
using TechStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "MinhaChaveSecretaSuperSeguraComPeloMenos32Caracteres!";

// Configuração do Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    // Configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Configurações de usuário
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<TechStoreDbContext>()
.AddDefaultTokenProviders();

// Configurar Identity
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"] ?? "TechStoreAPI",
        ValidAudience = jwtSettings["Audience"] ?? "TechStoreClients",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

        ClockSkew = TimeSpan.Zero // Remove margem de tempo do token
    };

    // Para desenvolvimento, aceitar tokens no header
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Permitir token via query string (apenas dev)
            if (string.IsNullOrEmpty(context.Token))
            {
                context.Token = context.Request.Query["access_token"];
            }
            return Task.CompletedTask;
        }
    };
});

// Registro ESSENCIAL do DbContext 
builder.Services.AddDbContext<TechStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<IFavoritoRepository, FavoritoRepository>();
builder.Services.AddScoped<ICupomRepository, CupomRepository>();

// Registrar serviços de aplicação
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<IFavoritoService, FavoritoService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ICupomService, CupomService>();

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(TechStore.Application.Mappings.MappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ClienteOnly", policy =>
        policy.RequireRole("Cliente"));

    options.AddPolicy("AdminOuCliente", policy =>
        policy.RequireRole("Admin", "Cliente"));
});

// Configurar CORS (importante para frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("AllowSpecific", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://meufrontend.com")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Configuração do Swagger para suportar JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads",
    OnPrepareResponse = ctx =>
    {
         ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
    }
});


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!await roleManager.RoleExistsAsync("Cliente"))
        await roleManager.CreateAsync(new IdentityRole("Cliente"));

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
}



app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();


