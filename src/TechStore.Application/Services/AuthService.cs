using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TechStore.Application.DTOs;
using TechStore.Application.Interfaces;
using TechStore.Core.Entities;

namespace TechStore.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<AuthResponseDTO> RegistrarAsync(RegistrarUsuarioDTO registrarDTO)
        {
            try
            {
                // Verificar se email já existe
                var usuarioExistente = await _userManager.FindByEmailAsync(registrarDTO.Email);
                if (usuarioExistente != null)
                {
                    return new AuthResponseDTO
                    {
                        Sucesso = false,
                        Mensagem = "Email já cadastrado"
                    };
                }

                // Criar usuário
                var usuario = new Usuario(
                    registrarDTO.NomeCompleto,
                    registrarDTO.Email,
                    registrarDTO.CPF,
                    registrarDTO.DataNascimento);

                var resultado = await _userManager.CreateAsync(usuario, registrarDTO.Senha);

                if (!resultado.Succeeded)
                {
                    var erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    return new AuthResponseDTO
                    {
                        Sucesso = false,
                        Mensagem = $"Erro ao criar usuário: {erros}"
                    };
                }

                // Atribuir role padrão (Cliente)
                await _userManager.AddToRoleAsync(usuario, "Cliente");

                // Gerar token JWT
                var token = await GerarTokenAsync(usuario);

                return new AuthResponseDTO
                {
                    Sucesso = true,
                    Token = token,
                    ExpiraEm = DateTime.UtcNow.AddMinutes(60),
                    NomeUsuario = usuario.NomeCompleto,
                    Email = usuario.Email,
                    Roles = new List<string> { "Cliente" },
                    Mensagem = "Registro realizado com sucesso"
                };
            }
            catch (Exception ex)
            {
                var erroReal =
                    ex.InnerException?.Message ??
                    ex.InnerException?.InnerException?.Message ??
                    ex.Message;

                _logger.LogError(ex, "Erro ao registrar usuário: {ErroReal}", erroReal);

                return new AuthResponseDTO
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro interno. Tente novamente mais tarde."
                };
            }

        }


        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var usuario = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (usuario == null || !await _userManager.CheckPasswordAsync(usuario, loginDTO.Senha))
                {
                    return new AuthResponseDTO
                    {
                        Sucesso = false,
                        Mensagem = "Email ou senha incorretos"
                    };
                }

                if (!usuario.Ativo)
                {
                    return new AuthResponseDTO
                    {
                        Sucesso = false,
                        Mensagem = "Usuário desativado"
                    };
                }

                // Gerar token JWT
                var token = await GerarTokenAsync(usuario);
                var roles = await _userManager.GetRolesAsync(usuario);

                return new AuthResponseDTO
                {
                    Sucesso = true,
                    Token = token,
                    ExpiraEm = DateTime.UtcNow.AddMinutes(60),
                    NomeUsuario = usuario.NomeCompleto,
                    Email = usuario.Email,
                    Roles = roles.ToList(),
                    Mensagem = "Login realizado com sucesso"
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erro interno ao processar autenticação");

                return new AuthResponseDTO
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro interno. Tente novamente mais tarde."
                };
            }
        }

        private async Task<string> GerarTokenAsync(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("nome", usuario.NomeCompleto),
                new Claim("cpf", usuario.CPF)
            };

            // Adicionar roles como claims
            var roles = await _userManager.GetRolesAsync(usuario);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = jwtSettings["SecretKey"]
                ?? throw new Exception("JWT SecretKey não configurada");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string token)
        {
            // Implementação simplificada
            // Em produção, use refresh tokens separados
            throw new NotImplementedException("Refresh token será implementado na próxima fase");
        }

        public async Task<bool> AlterarSenhaAsync(string usuarioId, AlterarSenhaDTO alterarSenhaDTO)
        {
            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return false;

            var resultado = await _userManager.ChangePasswordAsync(
                usuario,
                alterarSenhaDTO.SenhaAtual,
                alterarSenhaDTO.NovaSenha);

            return resultado.Succeeded;
        }

        public async Task<bool> LogoutAsync(string usuarioId)
        {
            // Para JWT stateless, apenas do lado do cliente
            // Em sistemas stateful, invalidar token aqui
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}