using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Models
{
    public class ConfigModel
    {
        public int idConfig { get; set; }
        public string descLocal { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public string infoMACUltimoAcesso { get; set; }
        public decimal temperatura { get; set; }
        public bool flagIluminacao { get; set; }
        public bool flagAquecedor { get; set; }
        public bool flagResfriador { get; set; }
        public bool flagFiltro { get; set; }
        public bool flagEncher { get; set; }
        public bool flagEsvaziar { get; set; }
    }
}
