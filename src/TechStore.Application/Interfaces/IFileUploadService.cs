using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TechStore.Application.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadImagemProdutoAsync(IFormFile arquivo, int produtoId);
        Task<bool> DeleteImagemAsync(string caminhoRelativo);
        string GetContentType(string fileName);
        bool IsValidImage(IFormFile arquivo);
    }
}