using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Application.Contracts
{
    public interface IEventoService
    {
        Task<Evento> Add(Evento model);
        Task<Evento> Update(int id, Evento model);
        Task<bool> Delete(int id);

        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false);

    }
}
