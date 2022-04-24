using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.VOs
{
    public class EventoVo
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [MinLength(3, ErrorMessage = "{0} deve ter no mínimo 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "{0} deve ter no máximo 50 caracteres.")]
        public string Tema { get; set; }

        [Display(Name = "Qtd Pessoas")]
        [Range(1, 120000, ErrorMessage = "{0}")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida. (gif|jpe?g|bmp|png)")]
        public string ImagemUrl { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório.")]
        [Phone(ErrorMessage = "Não é um número válido")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório."),
        Display(Name = "e-mail"),
        EmailAddress(ErrorMessage = "O {0} deve ser válido.")]
        public string Email { get; set; }

        public IEnumerable<LoteVo> Lotes { get; set; }
        public IEnumerable<RedeSocialVo> RedesSociais { get; set; }

        public IEnumerable<PalestranteVo> Palestrantes { get; set; }
    }
}
