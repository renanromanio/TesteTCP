using System;
using System.Collections.Generic;
using System.Text;

namespace Tcp.Teste.Domain.Util
{
    public class Mensagem<T>
    {
        public Mensagem(bool sucesso, string descricao, T retorno)
        {
            Sucesso = sucesso;
            Descricao = descricao;
            Retorno = retorno;
        }

        public bool Sucesso { get; set; }
        public string Descricao { get; set; }
        public T Retorno { get; set; }
    }
}
