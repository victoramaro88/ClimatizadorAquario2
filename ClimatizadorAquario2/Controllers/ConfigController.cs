using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        [HttpGet("{idConfig}/{descLocal}/{dataAtualizacao}/{infoMACUltimoAcesso}/{temperatura}/{tempMaxResfr}/{tempMinAquec}/{tempDesliga}/{flagIluminacao}/{flagAquecedor}/{flagResfriador}/{flagFiltro}/{flagEncher}/{flagEsvaziar}")]
        [Produces("application/json")]
        public IActionResult ManterInfo(int idConfig, string descLocal, DateTime dataAtualizacao, string infoMACUltimoAcesso, decimal temperatura, decimal tempMaxResfr,
            decimal tempMinAquec, decimal tempDesliga, bool flagIluminacao, bool flagAquecedor, bool flagResfriador, bool flagFiltro, bool flagEncher, bool flagEsvaziar)
        {
            string validacaoConfig = ValidaEntradaConfig(idConfig, descLocal, dataAtualizacao, infoMACUltimoAcesso, temperatura, tempMaxResfr,
            tempMinAquec, tempDesliga, flagIluminacao, flagAquecedor, flagResfriador, flagFiltro, flagEncher, flagEsvaziar);
            if (validacaoConfig == "OK")
            {
                ConfigModel objModel = new ConfigModel();
                try
                {
                    objModel.idConfig = idConfig;
                    objModel.descLocal = descLocal;
                    objModel.dataAtualizacao = dataAtualizacao;
                    objModel.infoMACUltimoAcesso = infoMACUltimoAcesso;
                    objModel.temperatura = temperatura;
                    objModel.tempMaxResfr = tempMaxResfr;
                    objModel.tempMinAquec = tempMinAquec;
                    objModel.tempDesliga = tempDesliga;
                    objModel.flagIluminacao = flagIluminacao;
                    objModel.flagAquecedor = flagAquecedor;
                    objModel.flagResfriador = flagResfriador;
                    objModel.flagFiltro = flagFiltro;
                    objModel.flagEncher = flagEncher;
                    objModel.flagEsvaziar = flagEsvaziar;

                    var ret = _configRepo.ManterInfo(objModel);

                    if (ret == "OK")
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
            else
            {
                return BadRequest(validacaoConfig);
            }
        }

        private string ValidaEntradaConfig(int idConfig, string descLocal, DateTime dataAtualizacao, string infoMACUltimoAcesso, decimal temperatura, decimal tempMaxResfr,
            decimal tempMinAquec, decimal tempDesliga, bool flagIluminacao, bool flagAquecedor, bool flagResfriador, bool flagFiltro, bool flagEncher, bool flagEsvaziar)
        {
            if (idConfig > 0)
            {
                if (!String.IsNullOrEmpty(descLocal))
                {
                    Regex r = new Regex(@"(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})");
                    bool dataValida = r.Match(dataAtualizacao.ToString()).Success;
                    if (dataValida)
                    {
                        if (!String.IsNullOrEmpty(infoMACUltimoAcesso))
                        {
                            bool decimalTempValido = decimal.TryParse(temperatura.ToString(), out temperatura) && temperatura < 50;
                            if (decimalTempValido)
                            {
                                bool decimalTempMaxResfValido = decimal.TryParse(tempMaxResfr.ToString(), out tempMaxResfr) && tempMaxResfr < 50;
                                if (decimalTempMaxResfValido)
                                {
                                    bool decimalTempMinResfValido = decimal.TryParse(tempMinAquec.ToString(), out tempMinAquec) && tempMinAquec < 50;
                                    if (decimalTempMinResfValido)
                                    {
                                        bool decimalDeslResfValido = decimal.TryParse(tempDesliga.ToString(), out tempDesliga) && tempDesliga < 50;
                                        if (decimalDeslResfValido)
                                        {
                                            bool validaIluminacao = bool.TryParse(flagIluminacao.ToString(), out flagIluminacao);
                                            if (validaIluminacao)
                                            {
                                                bool validaAquecedor = bool.TryParse(flagAquecedor.ToString(), out flagAquecedor);
                                                if (validaAquecedor)
                                                {
                                                    bool validaResfriador = bool.TryParse(flagResfriador.ToString(), out flagResfriador);
                                                    if (validaResfriador)
                                                    {
                                                        bool validaFiltro = bool.TryParse(flagFiltro.ToString(), out flagFiltro);
                                                        if (validaFiltro)
                                                        {
                                                            bool validaEncher = bool.TryParse(flagEncher.ToString(), out flagEncher);
                                                            if (validaEncher)
                                                            {
                                                                bool validaEsvaziar = bool.TryParse(flagEsvaziar.ToString(), out flagEsvaziar);
                                                                if (validaEsvaziar)
                                                                {
                                                                    return "OK";
                                                                }
                                                                else
                                                                {
                                                                    return "Valor booleano de esvaziar inválido.";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                return "Valor booleano de encher inválido.";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return "Valor booleano do filtro inválido.";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return "Valor booleano do resfriador inválido.";
                                                    }
                                                }
                                                else
                                                {
                                                    return "Valor booleano do aquecedor inválido.";
                                                }
                                            }
                                            else
                                            {
                                                return "Valor booleano da iluminação inválido.";
                                            }
                                        }
                                        else
                                        {
                                            return "Temperatura para desligamento inválida.";
                                        }
                                    }
                                    else
                                    {
                                        return "Temperatura mínima inválida.";
                                    }
                                }
                                else
                                {
                                    return "Temperatura máxima inválida.";
                                }
                            }
                            else
                            {
                                return "Temperatura inválida.";
                            }
                        }
                        else
                        {
                            return "Número de MAC inválido.";
                        }
                    }
                    else
                    {
                        return "Data/hora inválida";
                    }
                }
                else
                {
                    return "Local de atualização inválido.";
                }
            }
            else
            {
                return "Id de Configuração inválido.";
            }
        }
    }
}