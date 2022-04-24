using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.VOs;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Contracts;


namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist geralPersist;
        private readonly IEventoPersist eventoPersist;
        private readonly IMapper mapper;

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            this.mapper = mapper;
            this.geralPersist = geralPersist;
            this.eventoPersist = eventoPersist;
        }

        public async Task<EventoVo> Add(EventoVo model)
        {
            var resultEntry = this.mapper.Map<Evento>(model);
            this.geralPersist.Add<Evento>(resultEntry);
            if (await this.geralPersist.SaveChagesAsync())
            {
                var evt = await this.eventoPersist.GetEventoByIdAsync(resultEntry.Id, false);
                var result = this.mapper.Map<EventoVo>(evt);
                return result;
            }
            return null;

        }

        public async Task<EventoVo> Update(int id, EventoVo model)
        {
            var evento = await this.eventoPersist.GetEventoByIdAsync(id, false);
            if (evento == null) return null;
            var eventoUpdate = this.mapper.Map<Evento>(model);
            eventoUpdate.Id = id;
            geralPersist.Update(eventoUpdate);

            if (await this.geralPersist.SaveChagesAsync())
            {
                var evt = await this.eventoPersist.GetEventoByIdAsync(eventoUpdate.Id, false);
                var result = this.mapper.Map<EventoVo>(evt);
                return result;
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

        public async Task<EventoVo[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            var eventos = await this.eventoPersist.GetAllEventosAsync(includePalestrantes);
            var eventosRetorno = new List<EventoVo>();

            foreach (var evento in eventos)
            {
                eventosRetorno.Add(new EventoVo
                {
                    DataEvento = evento.DataEvento.ToString(),
                    Email = evento.Email,
                    Id = evento.Id,
                    ImagemUrl = evento.ImagemUrl,
                    Local = evento.Local,
                    QtdPessoas = evento.QtdPessoas,
                    Telefone = evento.Telefone,
                    Tema = evento.Tema,
                });
            }
            if (eventos == null || eventos.Length == 0) return null;
            var resultado = this.mapper.Map<EventoVo[]>(eventos);
            return resultado;
        }

        public async Task<EventoVo[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            var eventos = await this.eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
            if (eventos == null || eventos.Length == 0) return null;
            var resultado = this.mapper.Map<EventoVo[]>(eventos);
            return resultado;
        }

        public async Task<EventoVo> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);

            if (evento == null) return null;
            var resultado = this.mapper.Map<EventoVo>(evento);
            return resultado;
        }

    }
}
