﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HairLumos.Views.Funcoes_Fundamentais.RF_F14_Contratar_Pacotes
{
    public partial class ContratarPacotes : Form
    {
        //private List<Entidades.PacotesAdicionais> listaPacotesAdc;
        private List<Entidades.TabelaPacotes> listaTabela;
        private List<Entidades.PacoteServico> listaPacoteServico;
        private List<Entidades.PacotesAdicionais> listaPacoteAdicionais;
        private Entidades.Pacote pacote;
        private Entidades.PessoaFisica pessoa;

        public ContratarPacotes()
        {
            InitializeComponent();
            inicializa(false);
            carregaCbbPacote();
            carregaCbbServico();
            dgvPacote.AutoGenerateColumns = false;
            listaTabela = new List<Entidades.TabelaPacotes>();
            listaPacoteServico = new List<Entidades.PacoteServico>();
            listaPacoteAdicionais = new List<Entidades.PacotesAdicionais>();
            pacote = new Entidades.Pacote();
            pessoa = new Entidades.PessoaFisica();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            Controller.PacoteController cc = new Controller.PacoteController();
            int codigo = 0;
            double valor = 0;
            int result = 0;
            if (ttbCodigo.Text != null && ttbCodigo.Text != "")
            {
                codigo = Convert.ToInt32(ttbCodigo.Text.ToString());
            }
            if(ttbTotal.Text!=null && ttbTotal.Text != "")
            {
                valor = Convert.ToDouble(ttbTotal.Text.ToString());
            }
            preencheLista(listaTabela);
            result = cc.contratarPacote(codigo, dateTimePicker1.Value, ttbObservacao.Text.ToString(), pacote, pessoa, listaPacoteAdicionais);
            if(result > 0)
            {
                MessageBox.Show("Gravado com sucesso!");
                limpatela();
                inicializa(false);
            }
            else
            {
                MessageBox.Show("Erro ao gravar!");
            }

        }

        private void preencheLista(List<Entidades.TabelaPacotes> listap)
        {
            for(int i = 0; i<listap.Count; i++)
            {
                Entidades.PacotesAdicionais paca = new Entidades.PacotesAdicionais();
                Controller.ServicoController sc = new Controller.ServicoController();
                Entidades.Servico serv = new Entidades.Servico();
                if (listap.ElementAt(i).Tipo == "Adcional")
                {
                    paca.QtdeServico = listap.ElementAt(i).Qtde;
                    DataTable dt = sc.retornaObjServico(listap.ElementAt(i).Codigo);
                    DataRow drServ = dt.Rows[0];
                    serv.carregaServico(Convert.ToInt32(drServ["codtiposervico"].ToString()), drServ["tiposerv_descricao"].ToString(),
                    drServ["tiposerv_obs"].ToString(), Convert.ToDouble(drServ["tiposerv_valor"].ToString()), drServ["tiposerv_temposervico"].ToString());
                    paca.Servico = serv;
                    listaPacoteAdicionais.Add(paca);
                }
            }

        }

        private void btnPesquisaCliente_Click(object sender, EventArgs e)
        {
            Views.Pesquisa_Pessoa pesquisa_Pessoa = new Pesquisa_Pessoa();

            pesquisa_Pessoa.ShowDialog();
            if(pesquisa_Pessoa.intCodigoPessoa > 0)
            {
                Controller.PessoaController pessoaController = new Controller.PessoaController();
                DataTable dtRetorno = pessoaController.retornaPessoaCod(pesquisa_Pessoa.intCodigoPessoa.ToString());

                if (dtRetorno != null && dtRetorno.Rows.Count > 0)
                {
                    
                    DataRow dr = dtRetorno.Rows[0];
                    ttbCliente.Text = dr["pes_nome"].ToString();
                    pessoa.Codigo = Convert.ToInt32(dr["codpessoa"].ToString());
                    pessoa.Nome = dr["pes_nome"].ToString();

                    DataTable dtFisica = pessoaController.retornaPessoaFisicaCod(pessoa.Codigo);
                    if (dtFisica != null && dtFisica.Rows.Count > 0)
                    {

                        DataRow drFisica = dtFisica.Rows[0];
                        pessoa.CPF = drFisica["fis_cpf"].ToString();
                    }

                        
                }
            }
            
            
        }

        public void carregaCbbPacote()
        {
            Controller.PacoteController pacoteController = new Controller.PacoteController();

            DataTable dtPacote = pacoteController.retornaPacote();
            if (dtPacote != null && dtPacote.Rows.Count > 0)
            {
                this.cbbPacote.ValueMember = "codpacote";
                this.cbbPacote.DisplayMember = "pac_pacote";
                this.cbbPacote.DataSource = dtPacote;
            }
        }

        public void carregaCbbServico()
        {
            Controller.ServicoController servicoController = new Controller.ServicoController();

            DataTable dtServico = servicoController.retornaServico();
            if (dtServico != null && dtServico.Rows.Count > 0)
            {
                this.cbbServico.ValueMember = "codtiposervico";
                this.cbbServico.DisplayMember = "tiposerv_descricao";
                this.cbbServico.DataSource = dtServico;
            }
        }

        private void carregaDgvPacotesAdcinais(List<Entidades.TabelaPacotes> listPacoteAdc)
        {
            BindingSource bd = new BindingSource();
            bd.DataSource = listPacoteAdc;
            dgvPacote.DataSource = bd;
            dgvPacote.Refresh();

        }
        

        private void btnIncluirPacote_Click(object sender, EventArgs e)
        {
            Controller.PacoteController pacoteController = new Controller.PacoteController();
            Controller.ServicoController sc = new Controller.ServicoController();
            Entidades.Pacote _pacote = new Entidades.Pacote();
            Entidades.PacotesAdicionais _pacotesAdicionais = new Entidades.PacotesAdicionais();

            try
            {
                int intCodPacote = 0;

                if (string.IsNullOrWhiteSpace(cbbPacote.SelectedText.ToString()))
                {
                     intCodPacote = Convert.ToInt32(cbbPacote.SelectedValue.ToString());
                }
                else
                {
                    MessageBox.Show("Informe o Produto!");
                }
                

                DataTable dtPacote = pacoteController.retornaObjPacote(intCodPacote);
                DataTable dtLista = pacoteController.retornaListaPacote(intCodPacote);
                DataRow drPacote = dtPacote.Rows[0];

                _pacote.carregaPacote(Convert.ToInt32(drPacote["codpacote"].ToString()), drPacote["pac_pacote"].ToString(),
                    Convert.ToDouble(drPacote["pac_valor"].ToString()), drPacote["pac_obs"].ToString(), drPacote["pac_periodicidade"].ToString(), listaPacoteServico,
                    Convert.ToDateTime(drPacote["pac_datainicio"].ToString()), Convert.ToDateTime(drPacote["pac_datafim"].ToString()));

                pacote = _pacote;

                if (dtLista != null && dtLista.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLista.Rows.Count; i++)
                    {
                        DataRow dr = dtLista.Rows[i];
                        Entidades.PacoteServico pac = new Entidades.PacoteServico();
                        Entidades.Servico serv = new Entidades.Servico();
                        DataTable dtServico = sc.retornaObjServico(Convert.ToInt32(dr["codtiposervico"].ToString()));
                        DataRow drServ = dtServico.Rows[0];
                        serv.Codigo = Convert.ToInt32(drServ["codtiposervico"].ToString());
                        serv.ServicoNome = drServ["tiposerv_descricao"].ToString();
                        serv.Observacao = drServ["tiposerv_obs"].ToString();
                        serv.Tempo = drServ["tiposerv_temposervico"].ToString();
                        serv.Valor = Convert.ToDouble(drServ["tiposerv_valor"].ToString());
                        pac.Pacote = _pacote;
                        pac.Servico = serv;
                        pac.Quantidade = Convert.ToInt32(dr["pacserv_qtde"].ToString());
                        pac.Periodicidade = dr["pacserv_periodicidade"].ToString();
                        listaPacoteServico.Add(pac);
                    }
                }


                addListaTabela(listaPacoteServico);
                carregaDgvPacotesAdcinais(listaTabela);
                ttbTotal.Text = _pacote.Valor.ToString();
                btnIncluirPacote.Enabled = false;
            }
            catch
            {
                
            }
            

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void addListaTabela(List<Entidades.PacoteServico> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {

                Entidades.TabelaPacotes tab = new Entidades.TabelaPacotes();
                tab.Codigo = lista.ElementAt(i).Servico.Codigo;
                tab.Descr = lista.ElementAt(i).Servico.ServicoNome;
                tab.Qtde = lista.ElementAt(i).Quantidade;
                tab.Tipo = lista.ElementAt(i).Pacote.PaccoteNome;
                tab.Valor = lista.ElementAt(i).Pacote.Valor;
                listaTabela.Add(tab);

                carregaDgvPacotesAdcinais(listaTabela);
            }
        }

        private void btnIncluirServico_Click(object sender, EventArgs e)
        {
            try
            {
                Entidades.TabelaPacotes tab = new Entidades.TabelaPacotes();
                Controller.ServicoController sc = new Controller.ServicoController();
                Entidades.Servico serv = new Entidades.Servico();
                tab.Codigo = Convert.ToInt32(cbbServico.SelectedValue);
                tab.Descr = cbbServico.Text.ToString();
                DataTable dtserv = sc.retornaObjServico(Convert.ToInt32(cbbServico.SelectedValue));
                DataRow drServ = dtserv.Rows[0];
                serv.carregaServico(Convert.ToInt32(drServ["codtiposervico"].ToString()), drServ["tiposerv_descricao"].ToString(),
                    drServ["tiposerv_obs"].ToString(), Convert.ToDouble(drServ["tiposerv_valor"].ToString()), drServ["tiposerv_temposervico"].ToString());

                if (string.IsNullOrWhiteSpace(ttbQtde.Text))
                    MessageBox.Show("Informe a quantidade do serviço.");
                else
                {
                    tab.Qtde = Convert.ToInt32(ttbQtde.Text.ToString());
                    tab.Tipo = "Adcional";
                    int index = verificalista(tab, listaTabela);
                    if (index > 0)
                    {
                        listaTabela.ElementAt(index).Qtde += tab.Qtde;
                    }
                    else
                    {
                        listaTabela.Add(tab);
                    }
					carregaDgvPacotesAdcinais(listaTabela);
                    ttbTotal.Text = (Convert.ToDouble(ttbTotal.Text.ToString()) + (tab.Qtde*serv.Valor)).ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro: "+ex);
                throw;
            }
            
        }

        private int verificalista(Entidades.TabelaPacotes tab, List<Entidades.TabelaPacotes> lista) 
        {
            for(int i = 0; i<lista.Count; i++)
            {
                if (tab.Descr==lista.ElementAt(i).Descr && lista.ElementAt(i).Tipo=="Adcional")
                {
                    return i;
                }
            }
            return -1;
        }

        private void btnExcluirServicoPac_Click(object sender, EventArgs e)
        {
            if (dgvPacote.Rows.Count > 0)
            {
                if (listaTabela.ElementAt(dgvPacote.CurrentRow.Index).Tipo == "Adcional")
                {
                    listaTabela.Remove(listaTabela.ElementAt(dgvPacote.CurrentRow.Index));
                    carregaDgvPacotesAdcinais(listaTabela);
                }
                else
                {
                    MessageBox.Show("Não pode excluit itens do pacote!");
                }
            }
        }

        private void limpatela()
        {
            ttbCliente.Text = "";
            ttbCodigo.Text = "";
            ttbObservacao.Text = "";
            ttbQtde.Text = "";
            ttbTotal.Text = "";
            dgvPacote.DataSource = new List<Entidades.TabelaPacotes>();
            listaTabela = new List<Entidades.TabelaPacotes>();
            listaPacoteServico = new List<Entidades.PacoteServico>();
            btnIncluirPacote.Enabled = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpatela();
        }

        private void inicializa(bool estado)
        {
            btnAlterar.Enabled = estado;
            btnCancelar.Enabled = estado;
            btnGravar.Enabled = estado;
            btnIncluirPacote.Enabled = estado;
            btnIncluirServico.Enabled = estado;
            btnExcluir.Enabled = estado;
            btnExcluirServicoPac.Enabled = estado;
            btnPesquisaCliente.Enabled = estado;
            ttbCliente.Enabled = estado;
            ttbCodigo.Enabled = estado;
            ttbObservacao.Enabled = estado;
            ttbQtde.Enabled = estado;
            ttbTotal.Enabled = estado;
            dgvPacote.Enabled = estado;
            cbbPacote.Enabled = estado;
            cbbServico.Enabled = estado;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            inicializa(true);
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {

        }
    }

    
}
