using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Draft;

namespace Pi_3
{
    public partial class Lobby : Form
    {
        private string versao;
        private int idJogador;
        private string senhaJogador;
        public Lobby()
        {
            InitializeComponent();
            this.versao = Jogo.versao;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Versao = this.versao;
            f.AtualizarTela();
            f.ShowDialog();

            lbPartida.Text = f.idPartidaSelecionada.ToString();
            //id do jogador
            //senha do jogador

            Jogadores j = new Jogadores();


            if (j.ShowDialog() == DialogResult.OK)
            {
                lbId.Text = j.idJogador.ToString();
                lbSenha.Text = j.senhaJogador;
            }

        }

        private void btnIniciarPartida_Click(object sender, EventArgs e)
        {
            try
            {
                int idJogador = int.Parse(lbId.Text);
                string senhaJogador = lbSenha.Text;
                int idPartida = int.Parse(lbPartida.Text);

                string retorno = Jogo.Iniciar(idJogador, senhaJogador);

                if (string.IsNullOrWhiteSpace(retorno))
                {
                    MessageBox.Show("Sem resposta do servidor");
                    return;
                }

                if (retorno.StartsWith("ERRO"))
                {
                    MessageBox.Show(retorno);
                    return;
                }

                MessageBox.Show("Partida iniciada com sucesso!");

                string statusPartida = Jogo.VerificarPartida(idPartida);
                string[] verificarPartida = statusPartida.Split('\n');
                lblRodada.Text = verificarPartida[1];

                string statusTurno = Jogo.VerificarTurno(idPartida);
                string[] verificarTurno = statusTurno.Split('\n');

                foreach (string turno in verificarTurno)
                {
                    lstVerficarTurno.Items.Add(turno);
                }

                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro ao iniciar: " + ex.Message);
            }

        }


        private void btnListar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(lbId.Text);
                string senha = lbSenha.Text;

                string retorno = Jogo.ExibirMao(id, senha);

                string[] dinossauros = retorno.Split('\n');
                lstDinossauros.Items.Clear();
                for (int i = 0; i < dinossauros.Length; i++)
                {
                    lstDinossauros.Items.Add(dinossauros[i]);
                }

                if (string.IsNullOrWhiteSpace(retorno))
                {
                    MessageBox.Show("Sem resposta do servidor");
                    return;
                }

                if (retorno.StartsWith("ERRO"))
                {
                    MessageBox.Show(retorno);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar: " + ex.Message);
            }
        }

        private void btnJogar_Click(object sender, EventArgs e)
        {
            int idJogador = int.Parse(lbId.Text);
            int idPartida = int.Parse(lbPartida.Text);
            string senhaJogador = lbSenha.Text;
            string dinossauroSelecionado = txtDinossauro.Text;
            string cercadoSelecionado = txtCercado.Text;

            string statusPartida = Jogo.VerificarPartida(idPartida);
            //não sei qual a saída de VerificarPartida, então o valor do Split não está definido ainda
            string[] verificarPartida = statusPartida.Split(' ');

            string statusTabuleiro = Jogo.ExibirTabuleiro(idJogador, senhaJogador);
            string[] exibirTabuleiro = statusTabuleiro.Split('\n');

            Jogo.ListarCercados();


            foreach(string itemTabuleiro in exibirTabuleiro)
            {
                lstTabuleiro.Items.Add(itemTabuleiro);
            }

            string dadoSorteado = verificarPartida[verificarPartida.Length - 1];

            switch(dadoSorteado)
            {
                case "AL":
                    if (cercadoSelecionado == "rs" || cercadoSelecionado == "cd" || cercadoSelecionado == "is")
                    {
                        MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

                case "FL":
                    if (cercadoSelecionado == "cd" || cercadoSelecionado == "pa" || cercadoSelecionado == "is")
                    {
                        MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

                case "PR":
                    if (cercadoSelecionado == "fi" || cercadoSelecionado == "rs" || cercadoSelecionado == "mt")
                    {
                        MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

                case "WC":
                    if (cercadoSelecionado == "fi" || cercadoSelecionado == "mt" || cercadoSelecionado == "pa")
                    {
                        MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

                //ainda não consegui pensar numa lógica para essa situação
                case "TI":
                    foreach(string dinossauro in dinossaurosNoTabuleiro)
                    {
                        if(dinossauroSelecionado == dinossauro)
                        {
                            MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    break;

                case "VZ":
                    foreach(string cercado in cercadosNoTabuleiro)
                    {
                        if(cercadoSelecionado == "ri")
                        {
                            break;
                        }
                        if(cercadoSelecionado == cercado)
                        {
                            MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

            }
                 

        }
    }
}
