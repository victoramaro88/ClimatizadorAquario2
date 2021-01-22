using ClimatizadorAquario2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Repository
{
    public class HistoricoTemperaturaRepository
    {
        private string _bdAquario = "";
        IConfiguration Configuration;
        public HistoricoTemperaturaRepository(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            _bdAquario = Configuration.GetValue<string>("CONEXAO_BD");
        }

        public List<HistoricoTemperaturaModel> RetornaHistorico(DateTime dataInicio, DateTime dataFim)
        {
            List<HistoricoTemperaturaModel> listaRetorno = new List<HistoricoTemperaturaModel>();
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    // ->FALTA VALIDAR ESSA QUERY AINDA, NÃO ESTÁ OK, COM ERRO.
                    query = @"
                                DECLARE @dataInicio date = '2021-01-01';
                                DECLARE @dataFim date = '2021-01-31';
                                select @dataInicio
                                select @dataFim
                                IF @dataInicio <> is null AND @dataFim <> IS NULL
	                                BEGIN
		                                SELECT 'COM DATA'
		                                SELECT [idHistorico]
			                                  ,[idConfig]
			                                  ,[temperatura]
			                                  ,[dataHoraRegistro]
		                                  FROM [aquario].[amaro.victor].[HistoricoTemperatura]
		                                  WHERE [dataHoraRegistro] >= @dataInicio AND [dataHoraRegistro] <= @dataFim
                                  END
                                ELSE
	                                BEGIN
		                                SELECT 'SEM DATA'
		                                SELECT [idHistorico]
			                                  ,[idConfig]
			                                  ,[temperatura]
			                                  ,[dataHoraRegistro]
		                                  FROM [aquario].[amaro.victor].[HistoricoTemperatura]
	                                END";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = new HistoricoTemperaturaModel()
                            {
                                idHistorico = long.Parse(reader["idHistorico"].ToString()),
                                idConfig = int.Parse(reader["idConfig"].ToString()),
                                temperatura = decimal.Parse(reader["temperatura"].ToString()),
                                dataHoraRegistro = DateTime.Parse(reader["dataHoraRegistro"].ToString())
                            };

                            listaRetorno.Add(ret);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return listaRetorno;
        }
    }
}
