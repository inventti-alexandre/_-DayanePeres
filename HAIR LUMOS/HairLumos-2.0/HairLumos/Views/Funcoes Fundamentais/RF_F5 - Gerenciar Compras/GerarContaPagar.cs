﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HairLumos.Views.Funcoes_Fundamentais.RF_F5
{
    public partial class GerarContaPagar : Form
    {
        private int codCompra { get; set; }
        private double valorTotal { get; set; }

        private List<Entidades.ContasPagar> listacontasPagars = new List<Entidades.ContasPagar>();
        private Controller.CompraController CompraController = new Controller.CompraController();
        private Entidades.ContasPagar ContasPagar = new Entidades.ContasPagar();

        public GerarContaPagar()
        {
            InitializeComponent();
            mskValorTotal.Enabled = false;
            mskValorParcela.Enabled = false;

            dgvContas.AutoGenerateColumns = false;

            _inicializa();

            DataTable dt = CompraController.retornaCompraMAX();

            if(dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                codCompra = Convert.ToInt32(dr["codcompra"].ToString());
                mskValorTotal.Text = Convert.ToDouble(dr["comp_valortotal"]).ToString();
                valorTotal = Convert.ToDouble(mskValorTotal.Text);
            }
            
        }

        private void _inicializa()
        {
            dtpDataVencimento.Value = DateTime.Now;
            ttbQtdeParcela.Text = "";
            mskValorTotal.Text = "";
            mskValorParcela.Text = "";
            dgvContas.Rows.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _inicializa();
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            Controller.ContasPagarController contasPagarController = new Controller.ContasPagarController();

            try
            {
                int i = 0;
                bool retur = false;

                while (i < listacontasPagars.Count || !retur)
                {
                    int res = contasPagarController.insereLancamento(listacontasPagars.ElementAt(i));

                    if (res < 0)
                    {
                        MessageBox.Show("Erro");
                        retur = true;
                    }
                    i++;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
            Controller.ContasPagarController contasPagarController = new Controller.ContasPagarController();

            Controller.CompraController compraController = new Controller.CompraController();
            Entidades.Compra compra = new Entidades.Compra();

            Controller.DespesaController despesaController = new Controller.DespesaController();
            Entidades.Despesa despesa = new Entidades.Despesa();

            Controller.PessoaController pessoaController = new Controller.PessoaController();
            Entidades.Pessoa pessoa = new Entidades.Pessoa();

            Controller.CaixaController caixaController = new Controller.CaixaController();
            Entidades.Caixa caixa = new Entidades.Caixa();


            string codPessoa = "";

            try
            {
                if (string.IsNullOrWhiteSpace(ttbQtdeParcela.Text))
                    MessageBox.Show("Informe uma valor para a parcela ou 0");

                if (dtpDataVencimento.Value >= DateTime.Now)
                    MessageBox.Show("Data inválida.");

                double valorParc = Convert.ToDouble(mskValorTotal.Text) / Convert.ToInt32(ttbQtdeParcela.Text);


                DataTable dt = compraController.retornaCompra();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    compra = new Entidades.Compra();
                    compra.Codigo = Convert.ToInt32(dr["codcompra"].ToString());
                    
                    compra.Data = Convert.ToDateTime(dr["comp_datacompra"].ToString());
                    compra.Situacao = dr["comp_situacao"].ToString();
                    compra.Consignado = Convert.ToBoolean(dr["comp_statusconsignado"].ToString());
                    compra.ValorTotal = Convert.ToDouble(dr["comp_valortotal"].ToString());
                    compra.Obs = dr["comp_obs"].ToString();

                    codPessoa = dr["codpessoa"].ToString();
                    compra.Pessoa.Codigo =  Convert.ToInt32(dr["codpessoa"].ToString());

                    DataTable dtDespesa = despesaController.retronaDespesa();

                    if(dtDespesa != null && dt.Rows.Count > 0)
                    {
                        DataRow drDespesa = dtDespesa.Rows[0];
                        despesa.Codigo = Convert.ToInt32(drDespesa["coddespesa"].ToString());
                        despesa.Descricao = drDespesa["desp_descricao"].ToString();
                        despesa.Status = drDespesa["desp_status"].ToString();

                    }

                    compra.Despesa = despesa;
                    
                    DataTable dtPessoa = pessoaController.retornaPessoaCod(codPessoa);

                    if(dtPessoa != null && dtPessoa.Rows.Count > 0)
                    {
                        DataRow drPessoa = dtPessoa.Rows[0];
                        pessoa.Codigo = Convert.ToInt32(drPessoa["codPessoa"].ToString());
                        pessoa.Nome = drPessoa["pes_nome"].ToString();
                    }

                    compra.Pessoa = pessoa;

                }
                
                DataTable dtCaixa = caixaController.retornacaixaAbetoDia();

                if(dtCaixa != null && dtCaixa.Rows.Count > 0)
                {
                    DataRow drCaixa = dtCaixa.Rows[0];
                    caixa.DataAbertura = Convert.ToDateTime(drCaixa["caixa_datahoraabertura"].ToString());
                    caixa.DataFechamento = Convert.ToDateTime(drCaixa["caixa_datahorafecha"].ToString());
                    caixa.SaldoInicial = Convert.ToDouble(drCaixa["caixa_saldoinicial"].ToString());
                    caixa.Troco = Convert.ToDouble(drCaixa["caixa_troco"].ToString());
                    caixa.TotalEntrada = Convert.ToDouble(drCaixa["caixa_totalentra"].ToString());
                    caixa.TotalSaida = Convert.ToDouble(drCaixa["caixa_totalsaida"].ToString());

                }

                for (int i = 0; i < Convert.ToInt32(ttbQtdeParcela.Text); i++)
                {
                    ContasPagar = new Entidades.ContasPagar();
                    ContasPagar.Parcela = Convert.ToInt32(ttbQtdeParcela.Text);
                    ContasPagar.ValorTotal = Convert.ToDouble(mskValorTotal.Text);
                    ContasPagar.DataVencimento = dtpDataVencimento.Value.AddDays(i*30);
                    ContasPagar.CodParcela = i + 1;
                    ContasPagar.ValorParcela = valorParc;
                    ContasPagar.Status = false;
                    ContasPagar.Compra = compra;
                    ContasPagar.Despesa = despesa;
                    ContasPagar.Caixa = caixa;
                    //ContasPagar.FormaPagamento =
                    //ContasPagar.Comissao = 


                    listacontasPagars.Add(ContasPagar);
                                        
                }
                carregaDGV(listacontasPagars);

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void carregaDGV(List<Entidades.ContasPagar> list)
        {
            BindingSource bd = new BindingSource();
            bd.DataSource = list;
            dgvContas.DataSource = bd;
            dgvContas.Refresh();

        }

        private void ttbQtdeParcela_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double valor= 0;
                int qtdPar = 0;

                if (!string.IsNullOrWhiteSpace(mskValorTotal.Text))
                    valor = Convert.ToDouble(mskValorTotal.Text);

                if (!string.IsNullOrWhiteSpace(ttbQtdeParcela.Text))
                    qtdPar = Convert.ToInt32(ttbQtdeParcela.Text);

                mskValorParcela.Text = Convert.ToString(valor / qtdPar); ;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
