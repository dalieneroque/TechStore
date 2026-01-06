using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechStore.Application.Interfaces;

namespace TechStore.Application.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHostEnvironment _environment;
        private const string UploadsFolder = "Uploads";
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public FileUploadService(IHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImagemProdutoAsync(IFormFile arquivo, int produtoId)
        {
            if (arquivo == null || arquivo.Length == 0)
                throw new ArgumentException("Arquivo inválido");

            if (arquivo.Length > MaxFileSize)
                throw new ArgumentException($"O arquivo não pode exceder {MaxFileSize / 1024 / 1024}MB");

            var extension = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
                throw new ArgumentException($"Extensão não permitida. Use: {string.Join(", ", AllowedExtensions)}");

            // Criar pasta se não existir
            var produtoFolder = Path.Combine(_environment.ContentRootPath, UploadsFolder, "Produtos", produtoId.ToString());
            Directory.CreateDirectory(produtoFolder);

            // Gerar nome único para o arquivo
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(produtoFolder, fileName);

            // Salvar arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Retornar caminho relativo para salvar no banco
            return $"/{UploadsFolder}/Produtos/{produtoId}/{fileName}";
        }

        public async Task<bool> DeleteImagemAsync(string caminhoRelativo)
        {
            if (string.IsNullOrEmpty(caminhoRelativo))
                return false;

            var caminhoAbsoluto = Path.Combine(_environment.ContentRootPath, caminhoRelativo.TrimStart('/'));

            if (File.Exists(caminhoAbsoluto))
            {
                await Task.Run(() => File.Delete(caminhoAbsoluto));
                return true;
            }

            return false;
        }

        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        public bool IsValidImage(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return false;

            var extension = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(extension) && arquivo.Length <= MaxFileSize;
        }
    }
}