using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.Persistence;
using ProEventos.Domain.Models;
using ProEventos.Persistence.Context;
using ProEventos.Application.Contracts;
using Microsoft.AspNetCore.Http;

namespace ProEventos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoService eventoService;

        public EventoController(IEventoService eventoService)
        {
            this.eventoService = eventoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await this.eventoService.GetAllEventosAsync(true);
                if (eventos == null)
                {
                    return NotFound("Nenhum evento encontrado");
                }

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recurerar eventos. Erro {ex.Message}");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                var eventos = await this.eventoService.GetAllEventosByTemaAsync(tema, true);
                if (eventos == null)
                {
                    return NotFound($"Nenhum evento encontrado para o tema: {tema}.");
                }

                return Ok(eventos);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recurerar eventos. Erro {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            try
            {
                var evento = await this.eventoService.GetEventoByIdAsync(id, true);
                if (evento == null)
                {
                    return NotFound("Nenhum evento encontrado");
                }

                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recurerar eventos. Erro {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento eventoModel)
        {
            try
            {
                var evento = await this.eventoService.Add(eventoModel);
                if (evento == null)
                {
                    return BadRequest("Erro ao tetnar adicionar evento.");
                }

                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar adicionar eventos. Erro {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento eventoModel)
        {
            try
            {
                var evento = await this.eventoService.Update(id, eventoModel);
                if (evento == null)
                {
                    return BadRequest("Erro ao tetnar atualizar evento.");
                }

                return Ok(evento);
            }
            catch (System.Exception ex)
            {
                string inner = ex.InnerException?.Message;
                if (ex.InnerException.InnerException != null && string.IsNullOrEmpty(ex.InnerException.InnerException.Message))
                    inner += ex.InnerException.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualzar eventos. Erro {ex.Message} - {inner}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (await this.eventoService.Delete(id))
                {
                    return Ok("Deletado.");
                }
                else
                {
                    return BadRequest("Evento não deletado.");
                }

            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar deletar eventos. Erro {ex.Message}");
            }
        }
    }
}

