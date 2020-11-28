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

        public string ManterInfo(ConfigModel objModel)
        {
            string objRet = "";
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                            DECLARE @valorRetorno int;

                            SET @valorRetorno = (SELECT COUNT([idConfig]) AS Retorno 
						                            FROM [aquario].[amaro.victor].[ConfAquario]
						                            WHERE [idConfig] = " + objModel.idConfig + @")

                            IF @valorRetorno = 1
	                            BEGIN TRY  
		                            UPDATE [amaro.victor].[ConfAquario]
		                               SET [descLocal] = '" + objModel.descLocal + @"'
			                              ,[dataAtualizacao] = '" + objModel.dataAtualizacao + @"'
			                              ,[infoMACUltimoAcesso] = '" + objModel.infoMACUltimoAcesso + @"'
			                              ,[temperatura] = " + objModel.temperatura.ToString().Replace(",", ".") + @"
			                              ,[flagIluminacao] = " + (objModel.flagIluminacao ? 1 : 0) + @"
			                              ,[flagAquecedor] = " + (objModel.flagAquecedor ? 1 : 0) + @"
			                              ,[flagResfriador] = " + (objModel.flagResfriador ? 1 : 0) + @"
			                              ,[flagFiltro] = " + (objModel.flagFiltro ? 1 : 0) + @"
			                              ,[flagEncher] = " + (objModel.flagEncher ? 1 : 0) + @"
			                              ,[flagEsvaziar] = " + (objModel.flagEsvaziar ? 1 : 0) + @"
		                             WHERE [idConfig] = " + objModel.idConfig + @"
		                             SELECT 'OK' AS Retorno
	                            END TRY  
	                            BEGIN CATCH
		                            SELECT  'ERRO SQL: ' + CONVERT(varchar(10), ERROR_NUMBER()) + ' -> ' + ERROR_MESSAGE() AS Retorno
	                            END CATCH;
                            ELSE
	                            BEGIN TRY
		                            INSERT INTO [amaro.victor].[ConfAquario]
			                               ([descLocal],[dataAtualizacao],[infoMACUltimoAcesso],[temperatura]
			                               ,[flagIluminacao],[flagAquecedor],[flagResfriador],[flagFiltro]
			                               ,[flagEncher],[flagEsvaziar])
		                             VALUES
			                               ('" + objModel.descLocal + @"'
			                               ,'" + objModel.dataAtualizacao + @"'
			                               ,'" + objModel.infoMACUltimoAcesso + @"'
			                               ," + objModel.temperatura.ToString().Replace(",", ".") + @"
			                               ," + (objModel.flagIluminacao ? 1 : 0) + @"
			                               ," + (objModel.flagAquecedor ? 1 : 0) + @"
			                               ," + (objModel.flagResfriador ? 1 : 0) + @"
			                               ," + (objModel.flagFiltro ? 1 : 0) + @"
			                               ," + (objModel.flagEncher ? 1 : 0) + @"
			                               ," + (objModel.flagEsvaziar ? 1 : 0) + @")
		                             SELECT 'OK' AS Retorno
	                            END TRY  
	                            BEGIN CATCH
		                            SELECT  'ERRO SQL: ' + CONVERT(varchar(10), ERROR_NUMBER()) + ' -> ' + ERROR_MESSAGE() AS Retorno
	                            END CATCH;";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objRet = reader["Retorno"].ToString();
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
