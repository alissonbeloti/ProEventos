using System.Threading.Tasks;
using ProEventos.Application.VOs;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<EventoVo> Add(EventoVo model);
        Task<EventoVo> Update(int id, EventoVo model);
        Task<bool> Delete(int id);

        Task<EventoVo[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoVo[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoVo> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);

    }
}
