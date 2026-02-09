using System.Net.Http.Json;
using TechStore.Application.DTOs;

namespace TechStore.Web.Services
{
    public class PedidoService
    {
        private readonly HttpClient _http;

        public PedidoService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PedidoDTO?> CriarPedidoAsync(CriarPedidoDTO pedido)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/pedidos", pedido);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PedidoDTO>();
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<PedidoDTO>> ObterMeusPedidosAsync()
        {
            return await _http.GetFromJsonAsync<List<PedidoDTO>>("api/pedidos")
                   ?? new List<PedidoDTO>();
        }
    }
}
