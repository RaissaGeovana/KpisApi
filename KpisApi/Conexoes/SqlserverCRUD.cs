using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace ApiKPIs.Conexoes
{
    public class SqlServerCRUD
    {
        private readonly SqlConnection _conexao;

        public SqlServerCRUD()
        {
            string stringConexao = "Endereço do Banco";
            _conexao = new SqlConnection(stringConexao);
        }

        // -------------------------- QUERYS KPI-----------------------------------------
        public void InserirKPI(KpisApi.Entidades.KpiCRUD kpi)
        {
            try
            {
                _conexao.Open();

                string query = @"INSERT INTO KPIS
                                (Nome
                                ,UnidadeMedida
                                ,Status)

                                VALUES
                                (@Nome
                                ,@UnidadeMedida
                                ,@Status);";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", kpi.Nome);
                    cmd.Parameters.AddWithValue("@UnidadeMedida", kpi.UnidadeMedida);
                    cmd.Parameters.AddWithValue("@Status", kpi.Status);

                    cmd.ExecuteNonQuery();
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public void AtualizarKPI(KpisApi.Entidades.KpiCRUD kpi)
        {
            try
            {
                _conexao.Open();

                string query = @"UPDATE KPIS
                                SET Nome = @Nome
                                ,UnidadeMedida = @UnidadeMedida
                                ,Status = @Status
                                WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", kpi.Id);
                    cmd.Parameters.AddWithValue("@Nome", kpi.Nome);
                    cmd.Parameters.AddWithValue("@UnidadeMedida", kpi.UnidadeMedida);
                    cmd.Parameters.AddWithValue("@Status", kpi.Status);

                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new InvalidOperationException("KPI não encontrada!");
                    }
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public void DeletarKPI(int id)
        {
            try
            {
                _conexao.Open();

                string query = @"DELETE FROM KPIS
                                WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        throw new InvalidOperationException("KPI não encontrada!");
                    }
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public KpisApi.Entidades.KpiCRUD SelecionarKPI(int id)
        {
            try
            {
                _conexao.Open();

                string query = @"SELECT * FROM KPIS
                                WHERE ID = @ID";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    var rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        var kpi = new KpisApi.Entidades.KpiCRUD();
                        kpi.Id = id;
                        kpi.Nome = rdr["Nome"].ToString();
                        kpi.UnidadeMedida = rdr["UnidadeMedida"].ToString();
                        kpi.Status = Convert.ToBoolean(rdr["Status"].ToString());

                        return kpi;
                    }
                    else
                    {
                        throw new InvalidOperationException("KPI " + id + " não encontrada!");
                    }
                }

            }

            finally { _conexao.Close(); }
        }
        public List<KpisApi.Entidades.KpiCRUD> ListarKPIS()
        {
            var kpis = new List<KpisApi.Entidades.KpiCRUD>();
            try
            {
                _conexao.Open();

                string query = @"SELECT * FROM KPIS";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var kpi = new KpisApi.Entidades.KpiCRUD();
                        kpi.Id = Convert.ToInt32(rdr["Id"].ToString());
                        kpi.Nome = rdr["Nome"].ToString();
                        kpi.UnidadeMedida = rdr["UnidadeMedida"].ToString();
                        kpi.Status = Convert.ToBoolean(rdr["Status"].ToString());

                        kpis.Add(kpi);
                    }
                }

            }

            finally { _conexao.Close(); }
            return kpis;
        }

        public bool VerificarExistenciaKPI(string nome)
        {
            try
            {
                _conexao.Open();

                string query = @"select Count(ID) AS total 
                                 from KPIS WHERE Nome = @Nome;";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Nome", nome);

                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            finally
            {
                _conexao.Close();
            }
        }

        // -------------------------- QUERYS DADOS KPI-----------------------------------------
        public void InserirDadoKPI(KpisApi.Entidades.DadoKpiCRUD dadosKpi)
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
                    cmd.Parameters.AddWithValue("@ValorEsperado", dadosKpi.ValorEsperado);
                    cmd.Parameters.AddWithValue("@ValorObtido", dadosKpi.ValorObtido);

                    cmd.ExecuteNonQuery();
                }
            }

            finally
            {
                _conexao.Close();
            }
        }
        public void AtualizarDadoKPI(KpisApi.Entidades.DadoKpiCRUD dadosKpi)
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
        public KpisApi.Entidades.DadoKpiCRUD SelecionarDadoKPI(int id)
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
                        var dadoKpi = new KpisApi.Entidades.DadoKpiCRUD();
                        dadoKpi.ID = id;
                        dadoKpi.KPI = rdr["KPI"].ToString();
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
        public List<KpisApi.Entidades.DadoKpiCRUD> ListarDadosKPI()
        {
            var dadosKpi = new List<KpisApi.Entidades.DadoKpiCRUD>();
            try
            {
                _conexao.Open();

                string query = @"SELECT * FROM DadosKPI";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var dadoKpi = new KpisApi.Entidades.DadoKpiCRUD();
                        dadoKpi.ID = Convert.ToInt32(rdr["ID"].ToString());
                        dadoKpi.KPI = rdr["KPI"].ToString();
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

        public bool VerificarExistenciaDadoKPInoMes(KpisApi.Entidades.DadoKpiCRUD dadoKpi)
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

                    return false;
                }
            }
            finally
            {
                _conexao.Close();
            }
        }

        public List<KpisApi.Entidades.DadoKpiCRUD> SelecionarBaseTabela(string nome, int meses)
        {
            var dadosKpi = new List<KpisApi.Entidades.DadoKpiCRUD>();
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
                        var dadoKpi = new KpisApi.Entidades.DadoKpiCRUD();
                        dadoKpi.ID = Convert.ToInt32(rdr["ID"].ToString());
                        dadoKpi.KPI = rdr["KPI"].ToString();
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
    }
}
