using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClimatizadorAquario2.Models;
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

        [HttpGet("{idConfig}/{descLocal}/{dataAtualizacao}/{infoMACUltimoAcesso}/{temperatura}/{flagIluminacao}/{flagAquecedor}/{flagResfriador}/{flagFiltro}/{flagEncher}/{flagEsvaziar}")]
        [Produces("application/json")]
        public IActionResult ManterInfo(int idConfig, string descLocal, DateTime dataAtualizacao, string infoMACUltimoAcesso, decimal temperatura,
            bool flagIluminacao, bool flagAquecedor, bool flagResfriador, bool flagFiltro, bool flagEncher, bool flagEsvaziar)
        {
            ConfigModel objModel = new ConfigModel();
            try
            {
                objModel.idConfig = idConfig;
                objModel.descLocal = descLocal;
                objModel.dataAtualizacao = dataAtualizacao;
                objModel.infoMACUltimoAcesso = infoMACUltimoAcesso;
                objModel.temperatura = temperatura;
                objModel.flagIluminacao = flagIluminacao;
                objModel.flagAquecedor = flagAquecedor;
                objModel.flagResfriador = flagResfriador;
                objModel.flagFiltro = flagFiltro;
                objModel.flagEncher = flagEncher;
                objModel.flagEsvaziar = flagEsvaziar;

                var ret = _configRepo.ManterInfo(objModel);

                if(ret == "OK")
                    return Ok(ret);
                else
                    return BadRequest(ret);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}