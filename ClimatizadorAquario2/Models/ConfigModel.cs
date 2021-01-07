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
        public decimal tempMaxResfr { get; set; }
        public decimal tempMinAquec { get; set; }
        public decimal tempDesliga { get; set; }
        public string iluminHoraLiga { get; set; }
        public string iluminHoraDesliga { get; set; }
        public bool flagManual { get; set; }
        public bool flagNivelAgua { get; set; }
        public bool flagCirculador { get; set; }
        public bool flagBolhas { get; set; }
        public bool flagIluminacao { get; set; }
        public bool flagAquecedor { get; set; }
        public bool flagResfriador { get; set; }
        public bool flagEncher { get; set; }
        public string senhaSecundaria { get; set; }
    }
}
