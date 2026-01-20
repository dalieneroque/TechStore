using System;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Application.DTOs
{
    public class CupomDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(20, ErrorMessage = "Código não pode exceder 20 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(200, ErrorMessage = "Descrição não pode exceder 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Tipo de desconto é obrigatório")]
        public string Tipo { get; set; } // "Porcentagem" ou "ValorFixo"

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "Valor deve estar entre 0,01 e 1.000.000")]
        public decimal Valor { get; set; }

        [Range(0, 1000000, ErrorMessage = "Valor mínimo deve estar entre 0 e 1.000.000")]
        public decimal? ValorMinimoPedido { get; set; }

        [Range(1, 1000000, ErrorMessage = "Usos máximos deve estar entre 1 e 1.000.000")]
        public int? UsosMaximos { get; set; }

        public int UsosAtuais { get; set; }

        [Required(ErrorMessage = "Data de validade é obrigatória")]
        public DateTime DataValidade { get; set; }

        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CriarCupomDTO
    {
        [Required(ErrorMessage = "Código é obrigatório")]
        [StringLength(20, ErrorMessage = "Código não pode exceder 20 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(200, ErrorMessage = "Descrição não pode exceder 200 caracteres")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Tipo de desconto é obrigatório")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, 1000000, ErrorMessage = "Valor deve estar entre 0,01 e 1.000.000")]
        public decimal Valor { get; set; }

        [Range(0, 1000000, ErrorMessage = "Valor mínimo deve estar entre 0 e 1.000.000")]
        public decimal? ValorMinimoPedido { get; set; }

        [Range(1, 1000000, ErrorMessage = "Usos máximos deve estar entre 1 e 1.000.000")]
        public int? UsosMaximos { get; set; }

        [Required(ErrorMessage = "Data de validade é obrigatória")]
        public DateTime DataValidade { get; set; }
    }

    public class AplicarCupomDTO
    {
        [Required(ErrorMessage = "Código do cupom é obrigatório")]
        public string CodigoCupom { get; set; }
    }

    public class ValidarCupomDTO
    {
        public string CodigoCupom { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class ValidarCupomResponseDTO
    {
        public bool Valido { get; set; }
        public string Mensagem { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorFinal { get; set; }
        public CupomDTO Cupom { get; set; }
    }
}