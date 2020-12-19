using ClimatizadorAquario2.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ClimatizadorAquario2.Repository
{
    public class LoginRepository
    {
        private string _bdAquario = "";
        IConfiguration Configuration;
        public LoginRepository(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            _bdAquario = Configuration.GetValue<string>("CONEXAO_BD");
        }

        public LoginModel LoginUsuario(string usuario, string senha)
        {
            LoginModel objRet = new LoginModel();
            SqlDataReader reader = null;
            var query = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_bdAquario))
                {
                    query = @"
                            DECLARE @usr varchar(100) = '" + usuario + @"'
                            , @senh Varchar(20) = '" + senha + @"', @idUsr int = 0

                            SET @idUsr = (SELECT [idUsuario] FROM [aquario].[amaro.victor].[UsuarioAcesso]
		                            WHERE CAST([usuarioLogin] AS varbinary(50)) = CAST(@usr AS varbinary(50)))

                            IF
	                            @idUsr > 0
	                            BEGIN
		                            DECLARE @SenhaBanco varbinary(100) = (SELECT usuarioSenha FROM [aquario].[amaro.victor].[UsuarioAcesso] WHERE [idUsuario] = @idUsr)
		                            IF
			                            (select pwdCompare(@senh, @SenhaBanco, 0)) = 1
			                            BEGIN
				                            SELECT [idUsuario],[usuarioNome],[usuarioLogin],[usuarioSenha]
				                            FROM [aquario].[amaro.victor].[UsuarioAcesso]
				                            WHERE [idUsuario] = @idUsr
			                            END
	                            END";

                    SqlCommand com = new SqlCommand(query, con);
                    con.Open();

                    reader = com.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var ret = new LoginModel()
                            {
                                idUsuario = int.Parse(reader["idUsuario"].ToString()),
                                usuarioNome = reader["usuarioNome"].ToString(),
                                usuarioLogin = reader["usuarioLogin"].ToString()
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
