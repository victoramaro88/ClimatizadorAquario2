using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Models
{
    public class HistoricoTemperaturaModel
    {
        public long idHistorico { get; set; }
        public int idConfig { get; set; }
        public decimal temperatura { get; set; }
        public DateTime dataHoraRegistro { get; set; }
    }
}
