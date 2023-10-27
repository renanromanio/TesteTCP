using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tcp.Teste.Domain.Entities
{
    public class Exportador
    {
        public Exportador() { }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
