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


// IMPORTANTE: Carregar configurações de ambiente (Render)
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables(); // ESSENCIAL para o Render

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

//// Registro ESSENCIAL do DbContext 
//builder.Services.AddDbContext<TechStoreDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


Console.WriteLine("=== INÍCIO DO DIAGNÓSTICO DE CONNECTION STRING ===");

// 1. Tenta ler de todas as formas possíveis
var conn1 = builder.Configuration.GetConnectionString("DefaultConnection");
var conn2 = builder.Configuration["ConnectionStrings__DefaultConnection"];
var conn3 = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
var conn4 = Environment.GetEnvironmentVariable("DATABASE_URL"); // Formato comum no Render

Console.WriteLine($"GetConnectionString('DefaultConnection'): {(conn1 != null ? "ENCONTRADA" : "NÃO encontrada")}");
if (conn1 != null) Console.WriteLine($"  Valor (início): {conn1[..Math.Min(30, conn1.Length)]}...");

Console.WriteLine($"Configuration['ConnectionStrings__DefaultConnection']: {(conn2 != null ? "ENCONTRADA" : "NÃO encontrada")}");

Console.WriteLine($"Environment.GetEnvironmentVariable('ConnectionStrings__DefaultConnection'): {(conn3 != null ? "ENCONTRADA" : "NÃO encontrada")}");

Console.WriteLine($"Environment.GetEnvironmentVariable('DATABASE_URL'): {(conn4 != null ? "ENCONTRADA" : "NÃO encontrada")}");

// 2. Lista as primeiras variáveis de ambiente disponíveis (para ver o que existe)
Console.WriteLine("\nPrimeiras 10 variáveis de ambiente disponíveis:");
var envVars = Environment.GetEnvironmentVariables();
var count = 0;
foreach (System.Collections.DictionaryEntry entry in envVars)
{
    if (count++ >= 10) break;
    Console.WriteLine($"  {entry.Key} = {entry.Value?.ToString()?.Substring(0, Math.Min(20, entry.Value?.ToString()?.Length ?? 0))}...");
}

// 3. Escolhe a primeira connection string válida encontrada
string? connectionString = conn1 ?? conn2 ?? conn3 ?? conn4;

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("=== FALHA CRÍTICA: NENHUMA CONNECTION STRING ENCONTRADA ===");
    throw new InvalidOperationException("Nenhuma connection string encontrada. Verifique as variáveis de ambiente no Render.");
}

Console.WriteLine($"\nConnection string SELECIONADA (origem: {(conn1 != null ? "conn1" : conn2 != null ? "conn2" : conn3 != null ? "conn3" : "conn4")})");
Console.WriteLine($"=== FIM DO DIAGNÓSTICO ===\n");

builder.Services.AddDbContext<TechStoreDbContext>(options =>
    options.UseNpgsql(connectionString));

//

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
    options.AddPolicy("AllowBlazorFrontend",
        policy => policy.WithOrigins("https://localhost:7258", "http://localhost:5218")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
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

// Configurar porta do Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// INICIALIZAÇÃO DO BANCO DE DADOS (CORRIGIDO)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TechStoreDbContext>();

        // Verifica se o banco existe e pode ser conectado
        if (context.Database.CanConnect())
        {
            await context.Database.MigrateAsync(); // Aplica migrações pendentes

            // Agora sim, verifica as roles
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            // ... resto do seu código
        }
        else
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Banco de dados não disponível. Pulando inicialização de dados.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao inicializar o banco de dados");
    }
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //app.UseHttpsRedirection();
}

// Configurar diretório de uploads
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
    
    var services = scope.ServiceProvider;
    
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<Usuario>>();

    //Criar roles se não existirem
    if (!await roleManager.RoleExistsAsync("Cliente"))
        await roleManager.CreateAsync(new IdentityRole("Cliente"));

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    //Criar usuário admin se não existir
    var adminEmail = "admin";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        var novoAdmin = new Usuario(
            "Administrador",
            adminEmail,
            "00000000000",
            DateTime.Now.AddYears(-30)
        );

        var resultado = await userManager.CreateAsync(novoAdmin, "Admin@123");

        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(novoAdmin, "Admin");
        }
    }
}





app.UseHttpsRedirection();

app.UseCors("AllowBlazorFrontend");

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();


