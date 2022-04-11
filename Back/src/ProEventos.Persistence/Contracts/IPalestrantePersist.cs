using System.Threading.Tasks;
using ProEventos.Domain.Models;

namespace ProEventos.Persistence.Contracts
{
    public interface IPalestrantePersist
    {
        ///Palestrantes
        Task<Palestrante[]> GetAllPalestranteByNameAsync(string nome, bool includeEventos);
        Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos);
        Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos);

    }
}
