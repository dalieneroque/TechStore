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

        public async Task<List<ProdutoDTO>> ObterProdutosAdmin() 
        {
            return await _http.GetFromJsonAsync<List<ProdutoDTO>>("api/produtos");
              
        }

        public async Task CriarProduto(CriarProdutoDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/produtos", dto);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro API: {response.StatusCode} - {erro}");
            }
        }

        public async Task AtualizarProduto(int id, AtualizarProdutoDTO dto)
        {
            var response = await _http.PutAsJsonAsync($"api/produtos/{id}", dto);

            if(!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();
                throw new Exception(erro);
            }
        }

        public async Task DeletarProduto(int id)
        {
            await _http.DeleteAsync($"api/produtos/{id}");
        }
    }
}