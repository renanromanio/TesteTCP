using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Tcp.Teste.Domain.Entities;
using Tcp.Teste.Domain.Util;
using TCP.Teste.Data;

namespace TCP.Teste.Negocio
{
    public class ExportadorNegocio
    {
        private static ExportadorDAO dao;

        public ExportadorNegocio()
        {
            dao = new ExportadorDAO();
        }

        public Mensagem<List<Exportador>> GetExportadores()
        {
            return dao.GetExportadores();
        }

        public Mensagem<string> InsertExportador(Exportador entidade)
        {
            return dao.InsertExportador(entidade);
        }

        public Mensagem<Exportador> GetExportador(string documento)
        {
           return dao.GetExportador(documento);
        }

        public Mensagem<string> UpdateExportador(Exportador exportador, string documento)
        {
            return dao.UpdateExportador(exportador, documento);
        }

        public Mensagem<string> RemoveExportador(string documento)
        {
            return dao.RemoveExportador(documento);
        }
    }
}
