using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClimatizadorAquario2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClimatizadorAquario2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HistoricoController : ControllerBase
    {
        private readonly HistoricoTemperaturaRepository _historicoRepo;
        public HistoricoController(IConfiguration iConfig)
        {
            _historicoRepo = new HistoricoTemperaturaRepository(iConfig);
        }

        [HttpGet("{dataInicio?}/{dataFim?}")]
        [Produces("application/json")]
        public IActionResult RetornaHistorico(string dataInicio = "", string dataFim = "")
        {
            try
            {
                var a = _historicoRepo.RetornaHistorico(dataInicio, dataFim);

                return Ok(a);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
                throw;
            }
        }

        [NonAction]
        public string InsereHistorico(int idConfig, decimal temperatura)
        {
            try
            {
                return _historicoRepo.InsereHistorico(idConfig, temperatura);
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
    }
}
