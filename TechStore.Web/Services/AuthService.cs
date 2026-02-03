using System.Net.Http.Json;
using TechStore.Application.DTOs;
using Blazored.LocalStorage;


namespace TechStore.Web.Services
{
    // Serviço de autenticação
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;

        public bool IsAuthenticated { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public string Token { get; private set; } = string.Empty;

        public event Action? OnAuthChanged;

        public AuthService(HttpClient http, ILocalStorageService localStorage)
        {
            _http = http;
            _localStorage = localStorage;
            _ = LoadAuthFromStorage();
        }

        // Login
        public async Task<bool> LoginAsync(LoginDTO login)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/login", login);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

                    if (result?.Sucesso == true && !string.IsNullOrEmpty(result.Token))
                    {
                        await SaveAuthData(result);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Registrar
        public async Task<bool> RegistrarAsync(RegistrarUsuarioDTO registrar)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/auth/registrar", registrar);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                    return result?.Sucesso == true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Logout
        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userName");

            IsAuthenticated = false;
            UserName = string.Empty;
            Token = string.Empty;

            // Remove token do HttpClient
            _http.DefaultRequestHeaders.Authorization = null;

            OnAuthChanged?.Invoke();
        }

        // Salvar dados de autenticação
        private async Task SaveAuthData(AuthResponseDTO authResult)
        {
            await _localStorage.SetItemAsync("authToken", authResult.Token);
            await _localStorage.SetItemAsync("userName", authResult.NomeUsuario);

            Token = authResult.Token;
            UserName = authResult.NomeUsuario;
            IsAuthenticated = true;

            // Configura token no HttpClient
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            OnAuthChanged?.Invoke();
        }

        // Carregar do localStorage
        private async Task LoadAuthFromStorage()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var userName = await _localStorage.GetItemAsync<string>("userName");

            if (!string.IsNullOrEmpty(token))
            {
                Token = token;
                UserName = userName ?? "Usuário";
                IsAuthenticated = true;

                _http.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            }
        }
    }
}