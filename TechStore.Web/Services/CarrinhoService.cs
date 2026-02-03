using System.Net.Http.Json;
using TechStore.Application.DTOs;

namespace TechStore.Web.Services
{  
    public class CarrinhoService
    {
        private readonly HttpClient _http;
        private readonly AuthService _authService;
        private CarrinhoDTO _carrinho = new();

        public event Action? OnCarrinhoChanged;
        public List<CarrinhoItemDTO> Itens => _carrinho.Itens;
        public int QuantidadeTotal => _carrinho.TotalItens;
        public decimal ValorTotal => _carrinho.ValorTotal;

        public CarrinhoService(HttpClient http, AuthService authService)
        {
            _http = http;
            _authService = authService;
            _ = LoadCarrinhoAsync();
        }

        // Carrega carrinho da API
        public async Task LoadCarrinhoAsync()
        {
            if (!_authService.IsAuthenticated)
                return;

            try
            {
                _carrinho = await _http.GetFromJsonAsync<CarrinhoDTO>("api/carrinho")
                    ?? new CarrinhoDTO();
                OnCarrinhoChanged?.Invoke();
            }
            catch
            {
                _carrinho = new CarrinhoDTO();
            }
        }

        // Adiciona item (chama API)
        public async Task<bool> AdicionarItemAsync(int produtoId, int quantidade = 1)
        {
            if (!_authService.IsAuthenticated)
                return false;

            try
            {
                var itemDTO = new AdicionarItemCarrinhoDTO
                {
                    ProdutoId = produtoId,
                    Quantidade = quantidade
                };

                var response = await _http.PostAsJsonAsync("api/carrinho/itens", itemDTO);

                if (response.IsSuccessStatusCode)
                {
                    await LoadCarrinhoAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Atualiza quantidade (chama API)
        public async Task<bool> AtualizarQuantidadeAsync(int produtoId, int novaQuantidade)
        {
            if (!_authService.IsAuthenticated)
                return false;

            try
            {
                var itemDTO = new { ProdutoId = produtoId, Quantidade = novaQuantidade };
                var response = await _http.PutAsJsonAsync($"api/carrinho/itens", itemDTO);

                if (response.IsSuccessStatusCode)
                {
                    await LoadCarrinhoAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Remove item (chama API)
        public async Task<bool> RemoverItemAsync(int produtoId)
        {
            if (!_authService.IsAuthenticated)
                return false;

            try
            {
                var response = await _http.DeleteAsync($"api/carrinho/itens/{produtoId}");

                if (response.IsSuccessStatusCode)
                {
                    await LoadCarrinhoAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Limpa carrinho (chama API)
        public async Task<bool> LimparCarrinhoAsync()
        {
            if (!_authService.IsAuthenticated)
                return false;

            try
            {
                var response = await _http.DeleteAsync("api/carrinho");

                if (response.IsSuccessStatusCode)
                {
                    _carrinho = new CarrinhoDTO();
                    OnCarrinhoChanged?.Invoke();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}