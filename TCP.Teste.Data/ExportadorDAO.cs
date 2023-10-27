using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Tcp.Teste.Domain.Entities;
using Tcp.Teste.Domain.Util;

namespace TCP.Teste.Data
{
    public class ExportadorDAO
    {
        private static SqliteConnection connection;
        public ExportadorDAO()
        {
            connection = DatabaseHelper.DbConnection();
        }
        public Mensagem<List<Exportador>> GetExportadores()
        {
            try
            {
                var exportadoresBrasileiros = new List<ExportadorBrasileiro>();
                var exportadoresParaguai = new List<ExportadorParaguai>();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM EXPORTADOR_BRASILEIRO";
                    var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var exportadorBrasileiro = new ExportadorBrasileiro()
                        {
                            Nome = r.GetString("NOME"),
                            Telefone = r.GetString("TELEFONE"),
                            Email = r.GetString("EMAIL"),
                            CNPJ = r.GetString("CNPJ")
                        };
                        exportadoresBrasileiros.Add(exportadorBrasileiro);
                    }
                    connection.Close();
                }
                using (var cmd = connection.CreateCommand())
                {
                    connection.Open();
                    cmd.CommandText = "SELECT * FROM EXPORTADOR_PARAGUAI";
                    var r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        var exportadorParaguai = new ExportadorParaguai()
                        {
                            Nome = r.GetString("NOME"),
                            Telefone = r.GetString("TELEFONE"),
                            Email = r.GetString("EMAIL"),
                            RuC = r.GetString("RUC")
                        };
                        exportadoresParaguai.Add(exportadorParaguai);
                    }
                    connection.Close();
                }
                var exportadores = new List<Exportador>();
                exportadores.AddRange(exportadoresBrasileiros);
                exportadores.AddRange(exportadoresParaguai);
                return new Mensagem<List<Exportador>>(true, "", exportadores);
            }
            catch (Exception ex)
            {
                connection.Close();
                return new Mensagem<List<Exportador>>(false, $"Erro ao listar exportadores. Contate o administrador do sistema.", null);
            }
        }

        public Mensagem<string> InsertExportador(Exportador entidade)
        {
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    if (entidade is ExportadorBrasileiro)
                    {
                        cmd.CommandText = $@"

                        INSERT INTO EXPORTADOR_BRASILEIRO(NOME, CNPJ, EMAIL, TELEFONE)
                        VALUES('{entidade.Nome}', '{((ExportadorBrasileiro)entidade).CNPJ}', '{entidade.Email}', '{entidade.Telefone}');
                        ";
                        var r = cmd.ExecuteReader();
                    }
                    else
                    {
                        cmd.CommandText = $@"

                        INSERT INTO EXPORTADOR_PARAGUAI(NOME, RUC, EMAIL, TELEFONE)
                        VALUES('{entidade.Nome}', {((ExportadorParaguai)entidade).RuC}, '{entidade.Email}', '{entidade.Telefone}');
                        ";
                        var r = cmd.ExecuteReader();
                    }
                    connection.Close();
                }

                return new Mensagem<string>(true, "Exportador inserido com sucesso!", "");
            }
            catch (Exception ex)
            {
                return new Mensagem<string>(false, "Ocorreu um erro ao tentar inserir o Exportador. Contate o administrador do sistema.", "");
            }
        }

        public Mensagem<Exportador> GetExportador(string documento)
        {   
            try
            {
                Exportador retorno = new Exportador();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"
                                        SELECT
                                            1 AS TIPOEXPORTADOR,
                                            NOME,
                                            CNPJ AS DOCUMENTO,
                                            EMAIL,
                                            TELEFONE
                                        FROM 
                                            EXPORTADOR_BRASILEIRO
                                        WHERE 
                                            CNPJ LIKE '%{documento}%'

                                        UNION

                                        SELECT
                                            2 AS TIPOEXPORTADOR,
                                            NOME,
                                            RUC AS DOCUMENTO,
                                            EMAIL,
                                            TELEFONE
                                        FROM 
                                            EXPORTADOR_PARAGUAI
                                        WHERE 
                                            RUC LIKE '%{documento}%';";

                    var r = cmd.ExecuteReader();

                    while (r.Read())
                    {
                        var tipoExportador = r.GetInt16("TIPOEXPORTADOR");
                        if (tipoExportador == 1)
                        {
                            var exportadorBrasileiro = new ExportadorBrasileiro()
                            {
                                CNPJ = r.GetString("DOCUMENTO"),
                                Email = r.GetString("EMAIL"),
                                Nome = r.GetString("NOME"),
                                Telefone = r.GetString("TELEFONE")
                            };
                            retorno = exportadorBrasileiro;
                        }
                        
                        if (tipoExportador == 2)
                        {
                            var exportadorParaguai = new ExportadorParaguai()
                            {
                                RuC = r.GetString("DOCUMENTO"),
                                Email = r.GetString("EMAIL"),
                                Nome = r.GetString("NOME"),
                                Telefone = r.GetString("TELEFONE")
                            };
                            retorno = exportadorParaguai;
                        }
                    }
                    connection.Close();
                }

                return new Mensagem<Exportador>(true, "", retorno);
            }
            catch (Exception ex)
            {
                return new Mensagem<Exportador>(false, "Ocorreu um erro ao recuperar os dados do Exportador. Analise os dados enviados na requisição e se o erro persistir contate o administrador do sistema.", null);
            }
        }

        public Mensagem<string> UpdateExportador(Exportador exportador, string documento)
        {
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"
                                        UPDATE EXPORTADOR_BRASILEIRO
                                        SET
                                            NOME = '{exportador.Nome}',
                                            EMAIL = '{exportador.Email}',
                                            TELEFONE = '{exportador.Telefone}'
                                        WHERE
                                            CNPJ LIKE '%{documento}%';

                                        UPDATE EXPORTADOR_PARAGUAI
                                        SET
                                            NOME = '{exportador.Nome}',
                                            EMAIL = '{exportador.Email}',
                                            TELEFONE = '{exportador.Telefone}'
                                        WHERE
                                            RUC = '{documento}';";

                    var r = cmd.ExecuteReader();
                    connection.Close();
                    return new Mensagem<string>(true, "Exportador alterado com sucesso.", "");
                }
            }
            catch (Exception ex)
            {
                return new Mensagem<string>(false, "Ocorreu um erro ao alterar Exportador. Analise os dados enviados na requisição e se o erro persistir contate o administrador do sistema.", "");
            }
        }

        public Mensagem<string> RemoveExportador(string documento)
        {
            try
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = $@"                                       
                        DELETE FROM EXPORTADOR_BRASILEIRO
                        WHERE CNPJ LIKE '%{documento}%';

                        DELETE FROM EXPORTADOR_PARAGUAI
                        WHERE RUC LIKE '%{documento}%';";

                    var r = cmd.ExecuteReader();
                    connection.Close();

                    return new Mensagem<string>(true, "Exportador removido com sucesso!", "");
                }
            }
            catch (Exception ex)
            {
                return new Mensagem<string>(false, "Ocorreu um erro ao remover Exportador. Analise o documento enviado na requisição e se o erro persistir contate o administrador do sistema.", "");
            }
        }

    }
}
