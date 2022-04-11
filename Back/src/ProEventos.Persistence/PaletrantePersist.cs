using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext context;
        public PalestrantePersist(ProEventosContext context)
        {
            this.context = context;
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int id, bool includeEventos)
        {
            IQueryable<Palestrante> query = QueryPalestrantes(includeEventos);

            query = query
                .Where(p => p.Id == id)
                .OrderBy(p => p.Id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteAsync(bool includeEventos = false)
        {
            IQueryable<Palestrante> query = QueryPalestrantes(includeEventos);

            query = query.OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestranteByNameAsync(string nome, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = QueryPalestrantes(includeEventos);

            query = query
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
                .OrderBy(p => p.Id);

            return await query.ToArrayAsync();
        }

        private IQueryable<Palestrante> QueryPalestrantes(bool includeEventos)
        {
            IQueryable<Palestrante> query = this.context.Palestrantes
                            .Include(p => p.RedesSociais);

            if (includeEventos)
            {
                query = query
                    .Include(p => p.PalestrantesEventos)
                    .ThenInclude(pe => pe.Evento);
            }

            return query;
        }
    }
}
