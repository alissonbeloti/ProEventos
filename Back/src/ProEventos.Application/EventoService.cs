using System;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist geralPersist;
        private readonly IEventoPersist eventoPersist;

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
        {
            this.geralPersist = geralPersist;
            this.eventoPersist = eventoPersist;
        }

        public async Task<Evento> Add(Evento model)
        {

            this.geralPersist.Add<Evento>(model);
            if (await this.geralPersist.SaveChagesAsync())
            {
                return await this.eventoPersist.GetEventoByIdAsync(model.Id, false);
            }
            return null;

        }

        public async Task<Evento> Update(int id, Evento model)
        {
            var evento = await this.eventoPersist.GetEventoByIdAsync(id, false);
            if (evento == null) return null;
            model.Id = id;
            geralPersist.Update(model);

            if (await this.geralPersist.SaveChagesAsync())
            {
                return await this.eventoPersist.GetEventoByIdAsync(model.Id, false);
            }
            return null;

        }

        public async Task<bool> Delete(int id)
        {
            var evento = await this.eventoPersist.GetEventoByIdAsync(id, false);
            if (evento == null) throw new Exception("Evento para delete não encontrado.");

            geralPersist.Delete<Evento>(evento);

            return await this.geralPersist.SaveChagesAsync();

        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            var eventos = await this.eventoPersist.GetAllEventosAsync(includePalestrantes);
            if (eventos == null || eventos.Length == 0) return null;

            return eventos;
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            var eventos = await this.eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null || eventos.Length == 0) return null;

            return eventos;
        }

        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
            if (evento == null) return null;

            return evento;
        }

    }
}
