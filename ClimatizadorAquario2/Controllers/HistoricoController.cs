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

        [HttpGet("{tipoPesquisa?}/{dataInicio?}/{dataFim?}")] // -> Se for "1", traz por data, se for "2", traz os últimos 10.
        [Produces("application/json")]
        public IActionResult RetornaHistorico(byte tipoPesquisa, string dataInicio = "", string dataFim = "")
        {
            try
            {
                var a = _historicoRepo.RetornaHistorico(tipoPesquisa, dataInicio, dataFim);

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
