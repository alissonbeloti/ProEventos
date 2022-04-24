using System.Collections.Generic;

namespace ProEventos.Application.VOs
{
    public class PalestranteVo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string MiniCurriculo { get; set; }
        public string ImagemUrl { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public IEnumerable<RedeSocialVo> RedesSociais { get; set; }
        public IEnumerable<PalestranteVo> PalestrantesEventos { get; set; }
    }
}
