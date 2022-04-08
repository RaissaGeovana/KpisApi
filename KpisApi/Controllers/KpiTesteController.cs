using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KpisApi.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class KpiTesteController : ControllerBase
    {
        private readonly Conexoes.SqlServerDadoKpi _sql;

        public KpiTesteController()
        {
            _sql = new Conexoes.SqlServerDadoKpi();
        }
        [HttpGet("v1/ListarNomesKpis")]
        public List<Entidades.KpiTeste> ListarNomesKpis()
        {
            var kpiEDadosKpi = _sql.ListarNomesKpis();
            return kpiEDadosKpi;

        }

    }
}
