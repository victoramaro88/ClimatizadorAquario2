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
                            SELECT [idConfig],[descLocal],[dataAtualizacao],[infoMACUltimoAcesso],[temperatura],[tempMaxResfr]
                                  ,[tempMinAquec],[tempDesliga],[iluminHoraLiga],[iluminHoraDesliga],[flagCirculador],[flagBolhas],[flagIluminacao]
                                  ,[flagAquecedor],[flagResfriador],[flagEncher],[flagEncher],[senhaSecundaria]
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
                                tempMaxResfr = decimal.Parse(reader["tempMaxResfr"].ToString()),
                                tempMinAquec = decimal.Parse(reader["tempMinAquec"].ToString()),
                                tempDesliga = decimal.Parse(reader["tempDesliga"].ToString()),
                                iluminHoraLiga = reader["iluminHoraLiga"].ToString(),
                                iluminHoraDesliga = reader["iluminHoraDesliga"].ToString(),
                                flagCirculador = bool.Parse(reader["flagCirculador"].ToString()),
                                flagBolhas = bool.Parse(reader["flagBolhas"].ToString()),
                                flagIluminacao = bool.Parse(reader["flagIluminacao"].ToString()),
                                flagAquecedor = bool.Parse(reader["flagAquecedor"].ToString()),
                                flagResfriador = bool.Parse(reader["flagResfriador"].ToString()),
                                flagEncher = bool.Parse(reader["flagEncher"].ToString())
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

        public string AtivaFuncoes(int idConf, string funcionalidade, bool flag)
        {
            string retorno = "";
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                                UPDATE [aquario].[amaro.victor].[ConfAquario]
                                   SET [dataAtualizacao] = GETDATE()
                                      ,[infoMACUltimoAcesso] = 'HOME'
                                      ,[" + funcionalidade + @"] = " + (flag ? "1" : "0") + @"
                                 WHERE idConfig = " + idConf + @"
                                 SELECT 'OK' AS Retorno";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            retorno = reader["Retorno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retorno;
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
			                              ,[tempMaxResfr] = " + objModel.tempMaxResfr.ToString().Replace(",", ".") + @"
			                              ,[tempMinAquec] = " + objModel.tempMinAquec.ToString().Replace(",", ".") + @"
			                              ,[tempDesliga] = " + objModel.tempDesliga.ToString().Replace(",", ".") + @"
			                              ,[iluminHoraLiga] = '" + objModel.iluminHoraLiga + @"'
			                              ,[iluminHoraDesliga] = '" + objModel.iluminHoraDesliga + @"'
			                              ,[flagCirculador] = " + (objModel.flagCirculador ? 1 : 0) + @"
			                              ,[flagBolhas] = " + (objModel.flagBolhas ? 1 : 0) + @"
			                              ,[flagIluminacao] = " + (objModel.flagIluminacao ? 1 : 0) + @"
			                              ,[flagAquecedor] = " + (objModel.flagAquecedor ? 1 : 0) + @"
			                              ,[flagResfriador] = " + (objModel.flagResfriador ? 1 : 0) + @"
			                              ,[flagEncher] = " + (objModel.flagEncher ? 1 : 0) + @"
			                              ,[senhaSecundaria] = Convert(varbinary(100), pwdEncrypt('" + objModel.senhaSecundaria + @"'))
		                             WHERE [idConfig] = " + objModel.idConfig + @"
		                             SELECT 'OK' AS Retorno
	                            END TRY  
	                            BEGIN CATCH
		                            SELECT  'ERRO SQL: ' + CONVERT(varchar(10), ERROR_NUMBER()) + ' -> ' + ERROR_MESSAGE() AS Retorno
	                            END CATCH;
                            ELSE
	                            BEGIN TRY
		                            INSERT INTO [amaro.victor].[ConfAquario]
			                               ([descLocal],[dataAtualizacao],[infoMACUltimoAcesso],[temperatura],[tempMaxResfr],[tempMinAquec],[tempDesliga]
			                               ,[iluminHoraLiga],[iluminHoraDesliga],[flagCirculador],[flagBolhas],[flagIluminacao],[flagAquecedor]
                                           ,[flagResfriador],[flagEncher],[senhaSecundaria])
		                             VALUES
			                               ('" + objModel.descLocal + @"'
			                               ,'" + objModel.dataAtualizacao + @"'
			                               ,'" + objModel.infoMACUltimoAcesso + @"'
			                               ," + objModel.temperatura.ToString().Replace(",", ".") + @"
			                               ," + objModel.tempMaxResfr.ToString().Replace(",", ".") + @"
			                               ," + objModel.tempMinAquec.ToString().Replace(",", ".") + @"
			                               ," + objModel.tempDesliga.ToString().Replace(",", ".") + @"
			                               ,'" + objModel.iluminHoraLiga + @"'
			                               ,'" + objModel.iluminHoraDesliga + @"'
			                               ," + (objModel.flagCirculador ? 1 : 0) + @"
			                               ," + (objModel.flagBolhas ? 1 : 0) + @"
			                               ," + (objModel.flagIluminacao ? 1 : 0) + @"
			                               ," + (objModel.flagAquecedor ? 1 : 0) + @"
			                               ," + (objModel.flagResfriador ? 1 : 0) + @"
			                               ," + (objModel.flagEncher ? 1 : 0) + @"
			                               ,Convert(varbinary(100), pwdEncrypt('" + objModel.senhaSecundaria + @"')))
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

        public string EnviaTemperatura(int idConf, decimal temp)
        {
            string retorno = "";
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"UPDATE [aquario].[amaro.victor].[ConfAquario]
                               SET [temperatura] = " + temp.ToString().Replace(",", ".") + @"
                             WHERE [idConfig] = " + idConf + @"
                             SELECT 'OK' AS Retorno";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            retorno = reader["Retorno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retorno;
        }

        public string ManterOpcoes(int idConfig, decimal tempMaxResfr, decimal tempMinAquec, decimal tempDesliga, string iluminHoraLiga, string iluminHoraDesliga)
        {
            string retorno = "";
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"UPDATE [amaro.victor].[ConfAquario]
                               SET [tempMaxResfr] = " + tempMaxResfr.ToString().Replace(",",".") + @"
                                  ,[tempMinAquec] = " + tempMinAquec.ToString().Replace(",", ".") + @"
                                  ,[tempDesliga] = " + tempDesliga.ToString().Replace(",", ".") + @"
                                  ,[iluminHoraLiga] = '" + iluminHoraLiga + @"'
                                  ,[iluminHoraDesliga] = '" + iluminHoraDesliga + @"'
                             WHERE [idConfig] = " + idConfig + @"
                             SELECT 'OK' AS Retorno";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            retorno = reader["Retorno"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retorno;
        }

        public bool VerificaSenhaSecundaria(int idConfig, string senhaSecundaria)
        {
            bool retorno = false;
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                            DECLARE @idConf int = " + idConfig + @";
                            DECLARE @Senha varchar(20) = '" + senhaSecundaria + @"'
                            DECLARE @SenhaBanco varbinary(100) = (SELECT [senhaSecundaria] FROM [aquario].[amaro.victor].[ConfAquario] WHERE idConfig = @idConf)
                            select pwdCompare(@Senha, @SenhaBanco, 0) AS Resultado";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = reader["Resultado"].ToString();

                            retorno = ret == "1" ? true : false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return retorno;
        }
    }
}
