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

        [HttpGet("{idConfig}/{senhaSecundaria}")]
        [Produces("application/json")]
        public IActionResult VerificaSenhaSecundaria(int idConfig, string senhaSecundaria)
        {
            try
            {
                var a = _configRepo.VerificaSenhaSecundaria(idConfig, senhaSecundaria);

                return Ok(a);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
                throw;
            }
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

        [HttpGet("{idConf}/{funcionalidade}/{flag}")]
        [Produces("application/json")]
        public IActionResult AtivaFuncoes(int idConf, string funcionalidade, bool flag)
        {
            try
            {
                var a = _configRepo.AtivaFuncoes(idConf, funcionalidade, flag);

                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet("{idConfig}/{tempMaxResfr}/{tempMinAquec}/{tempDesliga}/{iluminHoraLiga}/{iluminHoraDesliga}")]
        [Produces("application/json")]
        public IActionResult ManterOpcoes(int idConfig, decimal tempMaxResfr, decimal tempMinAquec, decimal tempDesliga, string iluminHoraLiga, string iluminHoraDesliga)
        {
            try
            {
                var a = _configRepo.ManterOpcoes(idConfig, tempMaxResfr, tempMinAquec, tempDesliga, iluminHoraLiga, iluminHoraDesliga);

                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet("{idConfig}/{descLocal}/{dataAtualizacao}/{infoMACUltimoAcesso}/{temperatura}/{tempMaxResfr}/{tempMinAquec}/{tempDesliga}/{iluminHoraLiga}/{iluminHoraDesliga}/{flagCirculador}/{flagBolhas}/{flagIluminacao}/{flagAquecedor}/{flagResfriador}/{flagEncher}/{senhaSecundaria}")]
        [Produces("application/json")]
        public IActionResult ManterInfo(int idConfig, string descLocal, DateTime dataAtualizacao, string infoMACUltimoAcesso, decimal temperatura, decimal tempMaxResfr,
            decimal tempMinAquec, decimal tempDesliga, string iluminHoraLiga, string iluminHoraDesliga, bool flagCirculador, bool flagBolhas, bool flagIluminacao, 
            bool flagAquecedor, bool flagResfriador, bool flagEncher, string senhaSecundaria)
        {
            string validacaoConfig = ValidaEntradaConfig(idConfig, descLocal, dataAtualizacao, infoMACUltimoAcesso, temperatura, tempMaxResfr,
            tempMinAquec, tempDesliga, iluminHoraLiga, iluminHoraDesliga, flagCirculador, flagBolhas, flagIluminacao, flagAquecedor, flagResfriador, flagEncher, senhaSecundaria);
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
                    objModel.iluminHoraLiga = iluminHoraLiga;
                    objModel.iluminHoraDesliga = iluminHoraDesliga;
                    objModel.flagCirculador = flagCirculador;
                    objModel.flagBolhas = flagBolhas;
                    objModel.flagIluminacao = flagIluminacao;
                    objModel.flagAquecedor = flagAquecedor;
                    objModel.flagResfriador = flagResfriador;
                    objModel.flagEncher = flagEncher;
                    objModel.senhaSecundaria = senhaSecundaria;

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

        [NonAction]
        private string ValidaEntradaConfig(int idConfig, string descLocal, DateTime dataAtualizacao, string infoMACUltimoAcesso, decimal temperatura, decimal tempMaxResfr,
            decimal tempMinAquec, decimal tempDesliga, string iluminHoraLiga, string iluminHoraDesliga, bool flagCirculador, bool flagBolhas, bool flagIluminacao, 
            bool flagAquecedor, bool flagResfriador, bool flagEncher, string senhaSecundaria)
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
                                                        bool validaCirculador = bool.TryParse(flagCirculador.ToString(), out flagCirculador);
                                                        if (validaCirculador)
                                                        {
                                                            bool validaBolhas = bool.TryParse(flagBolhas.ToString(), out flagBolhas);
                                                            if (validaBolhas)
                                                            {
                                                                bool validaEncher = bool.TryParse(flagEncher.ToString(), out flagEncher);
                                                                if (validaEncher)
                                                                {
                                                                    if (!String.IsNullOrEmpty(senhaSecundaria))
                                                                    {
                                                                        if (!String.IsNullOrEmpty(iluminHoraLiga))
                                                                        {
                                                                            if (!String.IsNullOrEmpty(iluminHoraDesliga))
                                                                            {
                                                                                return "OK";
                                                                            }
                                                                            else
                                                                            {
                                                                                return "Hora de desligar inválida.";
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            return "Hora de ligar inválida.";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        return "Senha secundária inválida.";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    return "Valor booleano de encher inválido.";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                return "Valor booleano de bolhas inválido.";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            return "Valor booleano do circulador inválido.";
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