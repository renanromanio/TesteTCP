using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;

namespace TCP.Teste.Data
{
    public class DatabaseHelper
    {
        public static SqliteConnection sqliteConnection;
        public DatabaseHelper()
        { }
        public static SqliteConnection DbConnection()
        {
            sqliteConnection = new SqliteConnection("Data Source=c:\\dados\\TCPTeste.db3");
            sqliteConnection.Open();
            return sqliteConnection;
        }
        public static void CriarBancoSQLite()
        {
            try
            {
                String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string dbFile = Path.Combine(path, "TCPTeste.db3");
                CriarTabelas();
            }
            catch
            {
                throw;
            }
        }
        public static void CriarTabelas()
        {
            try
            {
                using (var cmd = DbConnection().CreateCommand())
                {
                    cmd.CommandText = $@"
                        DROP TABLE IF EXISTS EXPORTADOR_BRASILEIRO;
                        DROP TABLE IF EXISTS EXPORTADOR_PARAGUAI;
                        CREATE TABLE IF NOT EXISTS EXPORTADOR_BRASILEIRO(
                          ID_EXPORTADOR_BRASILEIRO INTEGER NOT NULL,
                          NOME VARCHAR(50) NOT NULL,
                          CNPJ VARCHAR(20) NOT NULL,
                          EMAIL VARCHAR(80),
                          TELEFONE VARCHAR(15),
                          CONSTRAINT PK_EXPORTADOR_BRASILEIRO PRIMARY KEY (ID_EXPORTADOR_BRASILEIRO)
                        );

                        CREATE TABLE IF NOT EXISTS EXPORTADOR_PARAGUAI(
                          ID_EXPORTADOR_PARAGUAI INTEGER NOT NULL,
                          NOME VARCHAR(50) NOT NULL,
                          RUC VARCHAR(50) NOT NULL,
                          EMAIL VARCHAR(80),
                          TELEFONE VARCHAR(15),
                          CONSTRAINT PK_EXPORTADOR_PARAGUAI PRIMARY KEY (ID_EXPORTADOR_PARAGUAI)
                        );
                        
                       DELETE FROM EXPORTADOR_BRASILEIRO;
                       DELETE FROM EXPORTADOR_PARAGUAI;

                       INSERT INTO EXPORTADOR_BRASILEIRO(NOME, CNPJ, EMAIL, TELEFONE)
                       VALUES('EMPRESA 01', '000000000100', 'EMPRESA1@TESTE.COM', '41999995555');

                       INSERT INTO EXPORTADOR_PARAGUAI(NOME, RUC, EMAIL, TELEFONE)
                       VALUES('EMPRESA 02', '510000000100', 'EMPRESA2@TESTE.COM', '78944441512')
                    ";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}