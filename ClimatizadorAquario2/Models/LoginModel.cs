using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Models
{
    public class LoginModel
    {
        public int idUsuario { get; set; }
        public string usuarioNome { get; set; }
        public string usuarioLogin { get; set; }
        public string usuarioSenha { get; set; }
    }
}
