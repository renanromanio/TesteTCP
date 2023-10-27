using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Tcp.Teste.Api.Model;
using Tcp.Teste.Domain.Entities;
using TCP.Teste.Negocio;

namespace Tcp.Teste.Api.Controllers
{
    public class ExportadorController : Controller
    {
        private ExportadorNegocio exportador;
        public ExportadorController()
        {
            exportador = new ExportadorNegocio();
        }

        [HttpGet]
        [Route("/exportadores")]
        public IActionResult ListarExportadores()
        {
            var retorno = exportador.GetExportadores();
            if (retorno.Sucesso)
            {
                string json = "";
                if(retorno.Retorno != null && retorno.Retorno.Any())
                {
                    json = JsonConvert.SerializeObject(retorno.Retorno, Formatting.Indented, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                }
                else
                {
                    json = "Nenhum Exportador cadastrado.";
                }
                return Ok(json);
            }
            else
            {
                return BadRequest(retorno.Descricao);
            }
        }

        [HttpPost]
        [Route("/exportadores/inserir")]
        public IActionResult InserirExportador([FromBody] ExportadorModel model)
        {
            var retorno = exportador.InsertExportador(RetornarEntidade(model));
            if (retorno.Sucesso)
            {
                return Ok(retorno.Descricao);
            }
            else
            {
                return BadRequest(retorno.Descricao);
            }
        }

        [HttpGet]
        [Route("/exportadores/{documento}")]
        public IActionResult BuscarExportador(string documento)
        {
            var retorno = exportador.GetExportador(documento);
            if (retorno.Sucesso)
            {
                string json = JsonConvert.SerializeObject(retorno.Retorno, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
                return Ok(json);
            }
            else
            {
                return BadRequest(retorno.Descricao);
            }
        }

        [HttpPost]
        [Route("/exportadores/alterar")]
        public IActionResult AlterarExportador([FromBody] ExportadorModel model)
        {
            var documento = !string.IsNullOrWhiteSpace(model.CNPJ) ? model.CNPJ : model.RuC;
            var entidade = RetornarEntidade(model);
            var retorno = exportador.UpdateExportador(entidade, documento);
            if (retorno.Sucesso)
            {
                return Ok(retorno.Descricao);
            }
            else
            {
                return BadRequest(retorno.Descricao);
            }
        }

        [HttpGet]
        [Route("/exportadores/remover/{documento}")]
        public IActionResult RemoverExportador(string documento)
        {
            var retorno = exportador.RemoveExportador(documento);
            if (retorno.Sucesso)
            {
            return Ok(retorno.Descricao);
            }
            else
            {
                return BadRequest(retorno.Descricao);
            }
        }

        private Exportador RetornarEntidade(ExportadorModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.CNPJ))
            {
                return new ExportadorBrasileiro()
                {
                    Nome = model.Nome,
                    CNPJ = model.CNPJ,
                    Email = model.Email,
                    Telefone = model.Telefone
                };
            }
            else
            {
                return new ExportadorParaguai()
                {
                    Nome = model.Nome,
                    RuC = model.RuC,
                    Email = model.Email,
                    Telefone = model.Telefone
                };
            }
        }
    }
}
