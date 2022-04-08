using System;

namespace KpisApi.ViewModel
{
    public class KpiEDadoKpi
    {
        public int ID_DadoKpi { get; set; }
        public int ID_Kpi { get; set; }
        public string Kpi { get; set; }
        public string UnidadeMedida { get; set; }
        public bool Status { get; set; }
        public DateTime MesReferencia { get; set; }
        public decimal ValorEsperado { get; set; }
        public decimal ValorObtido { get; set; }
    }
}
