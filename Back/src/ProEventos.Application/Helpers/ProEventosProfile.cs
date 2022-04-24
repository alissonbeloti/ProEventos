using AutoMapper;
using ProEventos.Domain.Models;
using ProEventos.Application.VOs;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile
    {
        public ProEventosProfile()
        {
            CreateMap<Evento, EventoVo>().ReverseMap();
            CreateMap<Lote, LoteVo>().ReverseMap();
            CreateMap<Palestrante, PalestranteVo>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialVo>().ReverseMap();
        }
    }
}
