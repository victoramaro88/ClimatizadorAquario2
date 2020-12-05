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
    public class LoginUsuarioController : ControllerBase
    {
        private readonly LoginRepository _loginRepo;
        public LoginUsuarioController(IConfiguration iConfig)
        {
            _loginRepo = new LoginRepository(iConfig);
        }

        [HttpGet("{usuario}/{senha}")]
        [Produces("application/json")]
        public IActionResult LoginUsuario(string usuario, string senha)
        {
            try
            {
                var a = _loginRepo.LoginUsuario(usuario, senha);

                if(a.idUsuario > 0)
                {
                    return Ok(a);
                }
                else
                {
                    return BadRequest("Usuário e/ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}
