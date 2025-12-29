using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechStore.Application.Interfaces;
using TechStore.Application.Services;
using TechStore.Core.Interfaces;
using TechStore.Infrastructure.Data;
using TechStore.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Registro ESSENCIAL do DbContext 
builder.Services.AddDbContext<TechStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositórios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

// Registrar serviços de aplicação
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();

// Registrar AutoMapper
builder.Services.AddAutoMapper(typeof(TechStore.Application.Mappings.MappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


