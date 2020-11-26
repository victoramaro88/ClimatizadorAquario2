using ClimatizadorAquario2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Repository
{
    public class ConfigRepository
    {
        private string _bdAquario = "";
        IConfiguration Configuration;
        public ConfigRepository(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            _bdAquario = Configuration.GetValue<string>("CONEXAO_BD");
        }

        public ConfigModel RetornaInfo()
        {
            ConfigModel objRet = new ConfigModel();
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                            SELECT [idConfig],[descLocal],[dataAtualizacao],[infoMACUltimoAcesso],[temperatura],[flagIluminacao]
                                  ,[flagAquecedor],[flagResfriador],[flagFiltro],[flagEncher],[flagEsvaziar]
                              FROM [aquario].[amaro.victor].[ConfAquario]";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = new ConfigModel()
                            {
                                idConfig = int.Parse(reader["idConfig"].ToString()),
                                descLocal = reader["descLocal"].ToString(),
                                dataAtualizacao = DateTime.Parse(reader["dataAtualizacao"].ToString()),
                                infoMACUltimoAcesso = reader["infoMACUltimoAcesso"].ToString(),
                                temperatura = decimal.Parse(reader["temperatura"].ToString()),
                                flagIluminacao = bool.Parse(reader["flagIluminacao"].ToString()),
                                flagAquecedor = bool.Parse(reader["flagAquecedor"].ToString()),
                                flagResfriador = bool.Parse(reader["flagResfriador"].ToString()),
                                flagFiltro = bool.Parse(reader["flagFiltro"].ToString()),
                                flagEncher = bool.Parse(reader["flagEncher"].ToString()),
                                flagEsvaziar = bool.Parse(reader["flagEsvaziar"].ToString())
                            };

                            objRet = ret;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return objRet;
        }
    }
}
