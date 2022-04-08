using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KpisApi.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class KpiNomeIdController : ControllerBase
    {
        private readonly Conexoes.SqlServerDadoKpi _sql;

        public KpiNomeIdController()
        {
            _sql = new Conexoes.SqlServerDadoKpi();
        }

        [HttpGet("v1/ListarNomesKpis")]
        public List<ViewModel.KpiNomeId> ListarNomesKpis()
        {
            var kpiEDadosKpi = _sql.ListarNomesKpis();
            return kpiEDadosKpi;

        }
    }
}
