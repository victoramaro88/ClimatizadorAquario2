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

        public List<HistoricoTemperaturaModel> RetornaHistorico(string dataInicio, string dataFim)
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
                                -- DECLARE @dataInicio varchar(10) = '01-01-2021';
                                -- DECLARE @dataFim varchar(10) = '31-01-2021';
                                DECLARE @dataInicio varchar(10) = '" + dataInicio + @"';
                                DECLARE @dataFim varchar(10) = '" + dataFim + @"';
                                IF @dataInicio <> '' AND @dataFim <> ''
	                                BEGIN
		                                SELECT [idHistorico]
			                                    ,[idConfig]
			                                    ,[temperatura]
			                                    ,[dataHoraRegistro]
		                                    FROM [aquario].[amaro.victor].[HistoricoTemperatura]
		                                    WHERE [dataHoraRegistro] BETWEEN CONVERT(DATE,  @dataInicio, 103) AND CONVERT(DATE, @dataFim, 103)
                                    END
                                ELSE
	                                BEGIN
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
                        var ret = new HistoricoTemperaturaModel();
                        while (reader.Read())
                        {
                            ret.idHistorico = reader["idHistorico"] != DBNull.Value ? long.Parse(reader["idHistorico"].ToString()) : (long)0;
                            ret.idConfig = reader["idConfig"] != DBNull.Value ? int.Parse(reader["idConfig"].ToString()) : (int)0;
                            ret.temperatura = reader["temperatura"] != DBNull.Value ? decimal.Parse(reader["temperatura"].ToString()) : (decimal)0;
                            ret.dataHoraRegistro = reader["dataHoraRegistro"] != DBNull.Value ? DateTime.Parse(reader["dataHoraRegistro"].ToString()) : default;

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

        public string InsereHistorico(int idConfig, decimal temperatura)
        {
            string ret = "";

            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                                DECLARE @idConfig int = " + idConfig + @";
                                DECLARE @temperatura decimal(10,2) = " + temperatura.ToString().Replace(",", ".") + @";
                                INSERT INTO [aquario].[amaro.victor].[HistoricoTemperatura]
                                           ([idConfig]
                                           ,[temperatura]
                                           ,[dataHoraRegistro])
                                     VALUES
                                           (@idConfig
                                           ,@temperatura
                                           ,(SELECT GETDATE()))
                            ";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ret = reader["Retorno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return ret;
        }
    }
}
