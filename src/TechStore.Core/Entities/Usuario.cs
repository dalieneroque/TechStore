using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TechStore.Core.Entities
{
    public class Usuario : IdentityUser
    {
        public string NomeCompleto { get; protected set; }
        public string CPF { get; protected set; }
        public DateTime DataNascimento { get; protected set; }
        public DateTime DataCadastro { get; protected set; }
        public bool Ativo { get; protected set; }

        // Método público para EF
        public Usuario() { }

        // Endereço (simplificado)
        public string? CEP { get; protected set; }
        public string? Logradouro { get; protected set; }
        public string? Numero { get; protected set; }
        public string? Complemento { get; protected set; }
        public string? Bairro { get; protected set; }
        public string? Cidade { get; protected set; }
        public string? Estado { get; protected set; }


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
    }
}