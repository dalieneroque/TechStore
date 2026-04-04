using System.Net.Http.Json;
using TechStore.Application.DTOs;

namespace TechStore.Web.Services
{
    public class CategoriaService
    {
        private readonly HttpClient _http;

        public CategoriaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<CategoriaDTO>> ObterCategorias()
        {
            return await _http.GetFromJsonAsync<List<CategoriaDTO>>("api/Categorias");
        }

        public async Task CriarCategoriaAsync(CriarCategoriaDTO dto)
        {
            var response = await _http.PostAsJsonAsync("api/Categorias", dto);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao criar categoria");
        }
    }
}