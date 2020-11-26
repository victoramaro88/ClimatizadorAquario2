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
    public class ConfigController : ControllerBase
    {
        private readonly ConfigRepository _configRepo;
        public ConfigController(IConfiguration iConfig)
        {
            _configRepo = new ConfigRepository(iConfig);
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult RetornaInfo()
        {
            try
            {
                var a = _configRepo.RetornaInfo();

                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }            
        }
    }
}