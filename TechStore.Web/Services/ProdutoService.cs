using System.Net.Http.Json;
using TechStore.Application.DTOs;   

namespace TechStore.Web.Services
{   
    public class ProdutoService
    {
        private readonly HttpClient _http;

        public ProdutoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProdutoDTO>> GetProdutosAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos")
                    ?? new List<ProdutoDTO>();
            }
            catch
            {
                return new List<ProdutoDTO>();
            }
        }

        public async Task<ProdutoDTO?> GetProdutoByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");
            }
            catch
            {
                return null;
            }
        }
    }
}