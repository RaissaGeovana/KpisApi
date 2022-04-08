using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace KpisApi.Conexoes
{
    public class SqlServerDadoKpi
    {
        private readonly SqlConnection _conexao;

        public SqlServerDadoKpi()
        {
            string stringConexao = "Endereço do Banco";
            _conexao = new SqlConnection(stringConexao);
        }

        // -------------------------- QUERYS DADOS KPI-----------------------------------------
        public void InserirDadoKPI(Entidades.DadoKpi dadosKpi)
        {
            try
            {
                _conexao.Open();

                string query = @"INSERT INTO DadosKPI
                                (KPI
                                ,MesReferencia
                                ,ValorEsperado
                                ,ValorObtido)
                                VALUES
                                (@KPI
                                ,@MesReferencia
                                ,@ValorEsperado
                                ,@ValorObtido);";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@KPI", dadosKpi.KPI);
                    cmd.Parameters.AddWithValue("@MesReferencia", dadosKpi.MesReferencia);
                    cmd.Parameters.AddWithValue(("@ValorEsperado").Replace(",", "."), dadosKpi.ValorEsperado);
                    cmd.Parameters.AddWithValue(("@ValorObtido").Replace(",", "."), dadosKpi.ValorObtido);

                    cmd.ExecuteNonQuery();
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public void AtualizarDadoKPI(Entidades.DadoKpi dadosKpi)
        {
            try
            {
                _conexao.Open();

                string query = @"UPDATE DadosKPI
                                SET KPI = @KPI
                                ,MesReferencia = @MesReferencia
                                ,ValorEsperado = @ValorEsperado
                                ,ValorObtido = @ValorObtido
                                WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", dadosKpi.ID);
                    cmd.Parameters.AddWithValue("@KPI", dadosKpi.KPI);
                    cmd.Parameters.AddWithValue("@MesReferencia", dadosKpi.MesReferencia);
                    cmd.Parameters.AddWithValue("@ValorEsperado", dadosKpi.ValorEsperado);
                    cmd.Parameters.AddWithValue("@ValorObtido", dadosKpi.ValorObtido);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new InvalidOperationException("Dado da KPI não encontrado!");
                    }
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public void DeletarDadoKPI(int id)
        {
            try
            {
                _conexao.Open();

                string query = @"DELETE FROM DadosKPI
                                WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new InvalidOperationException("Dado da KPI não encontrado!");
                    }
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public Entidades.DadoKpi SelecionarDadoKPI(int id)
        {
            try
            {
                _conexao.Open();

                string query = @"SELECT * FROM DadosKPI
                                WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        var dadoKpi = new Entidades.DadoKpi();
                        dadoKpi.ID = id;
                        dadoKpi.KPI = Convert.ToInt32(rdr["KPI"].ToString());
                        dadoKpi.MesReferencia = Convert.ToDateTime(rdr["MesReferencia"].ToString());
                        dadoKpi.ValorEsperado = Convert.ToDecimal(rdr["ValorEsperado"].ToString());
                        dadoKpi.ValorObtido = Convert.ToDecimal(rdr["ValorObtido"].ToString());

                        return dadoKpi;
                    }
                    else
                    {
                        throw new InvalidOperationException("Dado KPI " + id + " não encontrado!");
                    }
                }

            }

            finally { _conexao.Close(); }
        }
        public List<Entidades.DadoKpi> ListarDadosKPI()
        {
            var dadosKpi = new List<Entidades.DadoKpi>();
            try
            {
                _conexao.Open();

                string query = @"SELECT * FROM DadosKPI";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var dadoKpi = new Entidades.DadoKpi();
                        dadoKpi.ID = Convert.ToInt32(rdr["ID"].ToString());
                        dadoKpi.KPI = Convert.ToInt32(rdr["KPI"].ToString());
                        dadoKpi.MesReferencia = Convert.ToDateTime(rdr["MesReferencia"].ToString());
                        dadoKpi.ValorEsperado = Convert.ToDecimal(rdr["ValorEsperado"].ToString());
                        dadoKpi.ValorObtido = Convert.ToDecimal(rdr["ValorObtido"].ToString());

                        dadosKpi.Add(dadoKpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return dadosKpi;
        }

        public bool VerificarExistenciaDadoKPInoMes(Entidades.DadoKpi dadoKpi)
        {
            try
            {
                _conexao.Open();

                string query = @"SELECT MesReferencia FROM DadosKPI 
                                 WHERE KPI = @KPI;";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@KPI", dadoKpi.KPI);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        if (dadoKpi.MesReferencia.Month == Convert.ToDateTime(rdr["MesReferencia"].ToString()).Month
                            && dadoKpi.MesReferencia.Year == Convert.ToDateTime(rdr["MesReferencia"].ToString()).Year)
                        {
                            return true;
                        }
                    }

                    return false;// DÚVIDA - TEM PROBLEMA RETORNAR FALSE ANTES DE EXECUTAR O _CONEXAO.CLOSE()?
                }
            }
            finally
            {
                _conexao.Close();
            }
        }

        public List<Entidades.DadoKpi> SelecionarBaseTabela(string nome, int meses)
        {
            var dadosKpi = new List<Entidades.DadoKpi>();
            try
            {
                _conexao.Open();

                string query = @"SELECT * from (SELECT TOP (@meses) ID
                              ,KPI
                              ,MesReferencia
                              ,ValorEsperado
                              ,ValorObtido
                                FROM DadosKPI
                                WHERE KPI = @KPI
                             ORDER BY 
                                MesReferencia DESC) as tabela
                             ORDER BY
                                MesReferencia ASC;";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@meses", meses);
                    cmd.Parameters.AddWithValue("@KPI", nome);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var dadoKpi = new Entidades.DadoKpi();
                        dadoKpi.ID = Convert.ToInt32(rdr["ID"].ToString());
                        dadoKpi.KPI = Convert.ToInt32(rdr["KPI"].ToString());
                        dadoKpi.MesReferencia = Convert.ToDateTime(rdr["MesReferencia"].ToString());
                        dadoKpi.ValorEsperado = Convert.ToDecimal(rdr["ValorEsperado"].ToString());
                        dadoKpi.ValorObtido = Convert.ToDecimal(rdr["ValorObtido"].ToString());

                        dadosKpi.Add(dadoKpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return dadosKpi;
        }

        // -------------------------- QUERYS DADOS KPI + KPI------------------------------------

        public List<ViewModel.KpiEDadoKpi> ListarKpiEDadosKPI()
        {
            var kpiseDadosKpis = new List<ViewModel.KpiEDadoKpi>();
            try
            {
                _conexao.Open();

                string query = @"SELECT dk.[KPI] as [ID_KPI]
                                  ,k.[Nome] as [KPI]
                                  ,k.[UnidadeMedida]
                                  ,k.Status as [StatusKPI]
                                  ,dk.[ID] as [ID_DadoKpi]
                                  ,dk.[MesReferencia]
                                  ,dk.[ValorEsperado]
                                  ,dk.[ValorObtido]
                              FROM [TCC_TARDE].[dbo].[DadosKPI] dk
                            inner join KPIS k on k.ID = dk.KPI";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var kpieDadokpi = new ViewModel.KpiEDadoKpi();
                        kpieDadokpi.ID_Kpi = Convert.ToInt32(rdr["ID_KPI"].ToString());
                        kpieDadokpi.Kpi = rdr["KPI"].ToString();
                        kpieDadokpi.UnidadeMedida = rdr["UnidadeMedida"].ToString();
                        kpieDadokpi.Status = Convert.ToBoolean(rdr["StatusKPI"].ToString());
                        kpieDadokpi.ID_DadoKpi = Convert.ToInt32(rdr["ID_DadoKpi"].ToString());
                        kpieDadokpi.MesReferencia = Convert.ToDateTime(rdr["MesReferencia"].ToString());
                        kpieDadokpi.ValorEsperado = Convert.ToDecimal(rdr["ValorEsperado"].ToString());
                        kpieDadokpi.ValorObtido = Convert.ToDecimal(rdr["ValorObtido"].ToString());

                        kpiseDadosKpis.Add(kpieDadokpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return kpiseDadosKpis;
        }

        public List<ViewModel.KpiEDadoKpi> ListarDadosDeUmaKpi(int idKpi)
        {
            var kpiseDadosKpis = new List<ViewModel.KpiEDadoKpi>();
            try
            {
                _conexao.Open();

                string query = @"SELECT dk.[KPI] as [ID_KPI]
                                  ,k.[Nome] as [KPI]
                                  ,k.[UnidadeMedida]
                                  ,k.Status as [StatusKPI]
                                  ,dk.[ID] as [ID_DadoKpi]
                                  ,dk.[MesReferencia]
                                  ,dk.[ValorEsperado]
                                  ,dk.[ValorObtido]
                              FROM [TCC_TARDE].[dbo].[DadosKPI] dk
                            inner join KPIS k on k.ID = dk.KPI
                            WHERE k.ID = @IDKpi";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@IDKpi", idKpi);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var kpieDadokpi = new ViewModel.KpiEDadoKpi();
                        kpieDadokpi.ID_Kpi = Convert.ToInt32(rdr["ID_KPI"].ToString());
                        kpieDadokpi.Kpi = rdr["KPI"].ToString();
                        kpieDadokpi.UnidadeMedida = rdr["UnidadeMedida"].ToString();
                        kpieDadokpi.Status = Convert.ToBoolean(rdr["StatusKPI"].ToString());
                        kpieDadokpi.ID_DadoKpi = Convert.ToInt32(rdr["ID_DadoKpi"].ToString());
                        kpieDadokpi.MesReferencia = Convert.ToDateTime(rdr["MesReferencia"].ToString());
                        kpieDadokpi.ValorEsperado = Convert.ToDecimal(rdr["ValorEsperado"].ToString());
                        kpieDadokpi.ValorObtido = Convert.ToDecimal(rdr["ValorObtido"].ToString());

                        kpiseDadosKpis.Add(kpieDadokpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return kpiseDadosKpis;
        }

        // -------------------------- QUERYS KPI--------------------------------------------------
        public List<ViewModel.KpiNomeId> ListarNomesKpis()
        {
            var kpis = new List<ViewModel.KpiNomeId>();
            try
            {
                _conexao.Open();

                string query = @"SELECT ID
                                       ,Nome
                                       FROM KPIS

                                        ORDER BY Nome";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var kpi = new ViewModel.KpiNomeId();
                        kpi.Id = Convert.ToInt32(rdr["ID"].ToString());
                        kpi.Nome= rdr["Nome"].ToString();


                        kpis.Add(kpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return kpis;
        }
        public bool VerificarExistenciaDadoKPIParaAtualizar(Entidades.DadoKpi dadoKpi)
        {
            try
            {
                _conexao.Open();

                string query = @"SELECT MesReferencia FROM DadosKPI 
                                 WHERE KPI = @KPI
                                 AND 
                                  ID != @ID;";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@KPI", dadoKpi.KPI);
                    cmd.Parameters.AddWithValue("@ID", dadoKpi.ID);
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        if (dadoKpi.MesReferencia.Month == Convert.ToDateTime(rdr["MesReferencia"].ToString()).Month
                            && dadoKpi.MesReferencia.Year == Convert.ToDateTime(rdr["MesReferencia"].ToString()).Year)
                        {
                            return true;
                        }
                    }

                    return false;// DÚVIDA - TEM PROBLEMA RETORNAR FALSE ANTES DE EXECUTAR O _CONEXAO.CLOSE()?
                }
            }
            finally
            {
                _conexao.Close();
            }
        }
    }
}