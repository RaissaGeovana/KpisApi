using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KpisApi.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class DadoKpiController : ControllerBase
    {
        private readonly Conexoes.SqlServerDadoKpi _sql;

        public DadoKpiController()
        {
            _sql = new Conexoes.SqlServerDadoKpi();
        }

        [HttpPost("v1/InserirDadoKPI")]
        public IActionResult InserirDadoKPI(Entidades.DadoKpi dadoKpi)
        {
            if (_sql.VerificarExistenciaDadoKPInoMes(dadoKpi))
            {
                return StatusCode(400, "KPI já possui dados no mês!");
            }
            try
            {
                _sql.InserirDadoKPI(dadoKpi);
            }

            catch (InvalidOperationException)
            {
                return StatusCode(400, "Dados incorretos!");
            }

            catch (Exception)
            {
                return StatusCode(500, "Algo deu errado!");
            }
            return StatusCode(200, "Dado da KPI inserido com sucesso!");
        }

        [HttpPut("v1/AtualizarDadoKPI")]
        public IActionResult AtualizarDadoKPI(Entidades.DadoKpi dadoKpi)
        {

            if (_sql.VerificarExistenciaDadoKPIParaAtualizar(dadoKpi))
            {
                return StatusCode(400, "KPI já possui dados no mês!");
            }

            try
            {
                _sql.AtualizarDadoKPI(dadoKpi);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400, "Dado da KPI não encontrado!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Algo deu errado!");
            }
            return StatusCode(200, "Dado da KPI atualizado com sucesso!");
        }
        [HttpDelete("v1/DeletarDadoKPI")]
        public IActionResult DeletarDadoKPI(int id)
        {
            try
            {
                _sql.DeletarDadoKPI(id);
            }


            catch (InvalidOperationException)
            {
                return StatusCode(400, "Dado da KPI não encontrado!");
            }

            //catch (System.Data.SqlClient.SqlException) DELETE NA KPI COM STRING EM BRANCO GERA ERRO MAS NO DADOKPI NAO
            //{
            //    return StatusCode(400, "Insira a ID do dado da KPI!");
            //}


            return StatusCode(200, "Dado da KPI deletado com sucesso!");
        }
        [HttpGet("v1/SelecionarDadoKPI")]
        public IActionResult SelecionarDadoKPI(int id)
        {
            Entidades.DadoKpi dadoKpi;
            try
            {
                dadoKpi = _sql.SelecionarDadoKPI(id);
            }


            catch (InvalidOperationException)
            {
                return StatusCode(400, "Dado da KPI não encontrado!");
            }

            catch (Exception)
            {
                return StatusCode(400, "Insira a ID do dado da KPI!");
            }

            return StatusCode(200, dadoKpi);
        }
        [HttpGet("v1/ListarDadosKPI")]
        public List<Entidades.DadoKpi> ListarDadosKPI()
        {
            var dadosKpi = _sql.ListarDadosKPI();
            return dadosKpi;

        }

            [HttpGet("v1/SelecionarBaseTabela")]
        public List<Entidades.DadoKpi> SelecionarBaseTabela(string nome, int meses)
        {
            var dadosKpi = _sql.SelecionarBaseTabela(nome, meses);
            return dadosKpi;

        }
    }
}

