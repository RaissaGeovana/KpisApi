using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace KpisApi.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class KpiControllerCRUD : ControllerBase
    {
        private readonly ApiKPIs.Conexoes.SqlServerCRUD _sql;

        public KpiControllerCRUD()
        {
            _sql = new ApiKPIs.Conexoes.SqlServerCRUD();
        }

        [HttpPost("v1/InserirKPI")]
        public IActionResult InserirKPI(Entidades.KpiCRUD kpi)
        {
            if (_sql.VerificarExistenciaKPI(kpi.Nome))
            {
                return StatusCode(400, "Já existe uma KPI cadastrada com esse nome!");
            }

            if (!Utils.ValidacaoCRUD.IsNome(kpi.Nome))
            {
                return StatusCode(400, "Nome inválido! Mínimo 5 caracteres, máximo 255");
            }

            if (!Utils.ValidacaoCRUD.IsUnidadeMedida(kpi.UnidadeMedida))
            {
                return StatusCode(400, "Unidade de Medida inválida! Mínimo 1 caracter, máximo 255");
            }

            try
            {
                _sql.InserirKPI(kpi);
            }

            catch (InvalidOperationException)
            {
                return StatusCode(400, "Dados incorretos!");
            }

            catch (Exception)
            {
                return StatusCode(500, "Algo deu errado!");
            }
            return StatusCode(200, "KPI inserida com sucesso!");
        }

        [HttpPut("v1/AtualizarKPI")]
        public IActionResult AtualizarKPI(Entidades.KpiCRUD kpi)
        {
            if (!Utils.ValidacaoCRUD.IsNome(kpi.Nome))
            {
                return StatusCode(400, "Nome inválido! Mínimo 5 caracteres, máximo 255");
            }

            if (!Utils.ValidacaoCRUD.IsUnidadeMedida(kpi.UnidadeMedida))
            {
                return StatusCode(400, "Unidade de Medida inválida! Mínimo 1 caracter, máximo 255");
            }

            try
            {
                _sql.AtualizarKPI(kpi);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(400, "KPI não encontrada!");
            }
            catch (Exception)
            {
                return StatusCode(500, "Algo deu errado!");
            }
            return StatusCode(200, "KPI atualizada com sucesso!");
        }
        [HttpDelete("v1/DeletarKPI")]
        public IActionResult DeletarKPI(int id)
        {
            try
            {
                _sql.DeletarKPI(id);
            }


            catch (InvalidOperationException)
            {
                return StatusCode(400, "KPI não encontrada!");
            }

            catch (System.Data.SqlClient.SqlException)
            {
                return StatusCode(400, "Antes de deletar a KPI, delete seus dados!");
            }


            return StatusCode(200, "KPI deletada com sucesso!");
        }

        [HttpGet("v1/SelecionarKPI")]
        public IActionResult SelecionarKPI(int id)
        {
            Entidades.KpiCRUD kpi;
            try
            {
                kpi = _sql.SelecionarKPI(id);
            }


            catch (InvalidOperationException)
            {
                return StatusCode(400, "KPI não encontrada!");
            }

            catch (System.Data.SqlClient.SqlException)
            {
                return StatusCode(400, "Insira o ID da KPI!");
            }

            return StatusCode(200, kpi);
        }
        [HttpGet("v1/ListarKPIs")]
        public List<Entidades.KpiCRUD> ListarKPIs()
        {
            var kpis = _sql.ListarKPIS();
            return kpis;

        }

       
    }
}