using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KpisApi.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class KpiEDadoKpiController : ControllerBase
    {
        private readonly Conexoes.SqlServerDadoKpi _sql;

        public KpiEDadoKpiController()
        {
            _sql = new Conexoes.SqlServerDadoKpi();
        }

        [HttpGet("v1/ViewInnerJoinKpisDadosKpis")]
        public List<ViewModel.KpiEDadoKpi> ViewInnerJoinKpisDadosKpis()
        {
            var kpiEDadosKpi = _sql.ListarKpiEDadosKPI();
            return kpiEDadosKpi;

        }
        [HttpGet("v1/ListarDadosDeUmaKpi")]
        public List<ViewModel.KpiEDadoKpi> ListarDadosDeUmaKpi(int idKpi)
        {
            var kpiEDadosKpi = _sql.ListarDadosDeUmaKpi(idKpi);
            return kpiEDadosKpi;

        }
    }
}
