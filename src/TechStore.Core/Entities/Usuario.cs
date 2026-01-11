using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TechStore.Core.Entities
{
    public class Usuario : IdentityUser
    {
        public string NomeCompleto { get; private set; }
        public string CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public bool Ativo { get; private set; }

        // Endereço (simplificado)
        public string? CEP { get; private set; }
        public string? Logradouro { get; private set; }
        public string? Numero { get; private set; }
        public string? Complemento { get; private set; }
        public string? Bairro { get; private set; }
        public string? Cidade { get; private set; }
        public string? Estado { get; private set; }

        // Relacionamentos
        public virtual ICollection<Pedido> Pedidos { get; private set; } = new List<Pedido>();

        // Construtor
        public Usuario(string nomeCompleto, string email, string cpf, DateTime dataNascimento)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            UserName = email;
            CPF = cpf;
            DataNascimento = dataNascimento;
            DataCadastro = DateTime.UtcNow;
            Ativo = true;
            EmailConfirmed = true; // Para desenvolvimento
        }

        // Métodos de domínio
        public void AtualizarDadosPessoais(string nomeCompleto, string cpf, DateTime dataNascimento)
        {
            NomeCompleto = nomeCompleto;
            CPF = cpf;
            DataNascimento = dataNascimento;
        }

        public void AtualizarEndereco(string cep, string logradouro, string numero,
                                     string complemento, string bairro, string cidade, string estado)
        {
            CEP = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

        // Método público para EF
        public Usuario() { }
    }
}