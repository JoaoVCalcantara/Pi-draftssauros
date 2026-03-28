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

            lblPartida.Text = f.idPartidaSelecionada.ToString();
        }

        private void btnEntrarJogadorPrincipal_Click(object sender, EventArgs e)
        {
            int idPartida;
            if (!int.TryParse(lblPartida.Text, out idPartida))
            {
                MessageBox.Show("Selecione uma partida válida.");
                return;
            }

            string nomeJogador = txtNomeJogadorPrincipal.Text.Trim();
            string senhaPartida = txtSenhaPartida.Text.Trim();

            if (string.IsNullOrEmpty(nomeJogador))
            {
                MessageBox.Show("Informe o nome do jogador principal.");
                return;
            }

            if (string.IsNullOrEmpty(senhaPartida))
            {
                MessageBox.Show("Informe a senha da partida.");
                return;
            }

            string retorno = Jogo.Entrar(idPartida, nomeJogador, senhaPartida);

            if (string.IsNullOrWhiteSpace(retorno))
            {
                MessageBox.Show("Sem resposta do servidor.");
                return;
            }

            retorno = retorno.Trim();

            if (retorno.StartsWith("ERRO"))
            {
                MessageBox.Show(retorno);
                return;
            }

            string[] partesRetorno = retorno.Split(',');

            if (partesRetorno.Length < 2)
            {
                MessageBox.Show("Retorno inválido ao entrar na partida.");
                return;
            }

            lblIDJogadorPrincipal.Text = partesRetorno[0].Trim();
            lblKeyJogadorPrincipal.Text = partesRetorno[1].Trim();
            lblStatusJogadorPrincipal.Text = "Você entrou na partida!";
        }


        private void btnIniciarPartida_Click(object sender, EventArgs e)
        {
            lblStatusPartida.Text = "";

            int idJogador;
            if (!int.TryParse(lblIDJogadorPrincipal.Text, out idJogador))
            {
                MessageBox.Show("ID do jogador principal inválido.");
                return;
            }

            int idPartida;
            if (!int.TryParse(lblPartida.Text, out idPartida))
            {
                MessageBox.Show("ID da partida inválido.");
                return;
            }

            string senhaJogador = lblKeyJogadorPrincipal.Text;

            if (string.IsNullOrWhiteSpace(senhaJogador))
            {
                MessageBox.Show("Senha do jogador principal não informada.");
                return;
            }

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

            string statusPartida = Jogo.VerificarPartida(idPartida);

            if (string.IsNullOrWhiteSpace(statusPartida))
            {
                MessageBox.Show("Não foi possível verificar o status da partida.");
                return;
            }

            string[] dadosPartida = statusPartida.Split(',');

            if (dadosPartida.Length < 5)
            {
                MessageBox.Show("Retorno da partida em formato inválido.");
                return;
            }

            string idJogadorDado = dadosPartida[3].Trim();
            string faceDado = dadosPartida[4].Trim();

            string nomeFace = "";

            if (faceDado == "AL")
                nomeFace = "Alimentação";
            else if (faceDado == "FL")
                nomeFace = "Floresta";
            else if (faceDado == "PR")
                nomeFace = "Pradaria";
            else if (faceDado == "TI")
                nomeFace = "Tiranossauro Rex";
            else if (faceDado == "VZ")
                nomeFace = "Cercado Vazio";
            else if (faceDado == "WC")
                nomeFace = "Banheiros";
            else
                nomeFace = faceDado;

            lblRodada.Text = "O jogador " + idJogadorDado + " rolou o dado, e caiu o lado " + nomeFace;

            lblStatusPartida.Text = "Partida iniciada com sucesso!";
            lblStatusPartida.ForeColor = Color.Green;
        }


        private void btnListar_Click(object sender, EventArgs e)
        {
            int idPartida;
            if (!int.TryParse(lblPartida.Text, out idPartida))
            {
                MessageBox.Show("ID da partida inválido.");
                return;
            }

            int idJogador;
            if (!int.TryParse(lblIDJogadorPrincipal.Text, out idJogador))
            {
                MessageBox.Show("ID do jogador inválido.");
                return;
            }

            string senhaJogador = lblKeyJogadorPrincipal.Text;

            if (string.IsNullOrWhiteSpace(senhaJogador))
            {
                MessageBox.Show("Senha do jogador não informada.");
                return;
            }

            string statusPartida = Jogo.VerificarPartida(idPartida);

            if (string.IsNullOrWhiteSpace(statusPartida))
            {
                MessageBox.Show("Erro ao verificar partida.");
                return;
            }

            string retorno = Jogo.ExibirMao(idJogador, senhaJogador);

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

            string[] dinossauros = retorno.Split('\n');

            lstDinossauros.Items.Clear();
            foreach (string dinossauro in dinossauros)
            {
                if (!string.IsNullOrWhiteSpace(dinossauro))
                    lstDinossauros.Items.Add(dinossauro.Trim());
            }
        }

        private void ValidarTurno()
        {
            int idPartida;
            if (!int.TryParse(lblPartida.Text, out idPartida))
            {
                MessageBox.Show("ID da partida inválido.");
                return;
            }

            string statusTurno = Jogo.VerificarTurno(idPartida);

            if (string.IsNullOrWhiteSpace(statusTurno))
            {
                MessageBox.Show("Erro ao verificar turno.");
                return;
            }

            if (statusTurno.StartsWith("ERRO"))
            {
                MessageBox.Show(statusTurno);
                return;
            }

            string[] verificarTurno = statusTurno.Split('.');

            lstVerficarTurno.Items.Clear();
            foreach (string turno in verificarTurno)
            {
                if (!string.IsNullOrWhiteSpace(turno))
                    lstVerficarTurno.Items.Add(turno.Trim());
            }
        }

        private void btnValidarTurno_Click(object sender, EventArgs e)
        {
            ValidarTurno();
        }

        private void btnJogar_Click(object sender, EventArgs e)
        {
            int idJogador = int.Parse(lblIDJogadorPrincipal.Text);
            string senhaJogador = lblKeyJogadorPrincipal.Text;
            int idPartida = int.Parse(lblPartida.Text);
            string dinossauroSelecionado = txtDinossauro.Text;
            string cercadoSelecionado = txtCercado.Text;

            string statusPartida = Jogo.VerificarPartida(idPartida);
            statusPartida = statusPartida.Replace("\r", "");
            statusPartida = statusPartida.Replace("\n", "");
            string[] verificarPartida = statusPartida.Split(',');

            string statusMao = Jogo.ExibirMao(idJogador, senhaJogador);
            statusMao = statusMao.Replace("\r", "");
            string[] exibirMao = statusMao.Split('\n');

            string statusTabuleiro = Jogo.ExibirTabuleiro(idJogador, senhaJogador);
            string[] exibirTabuleiro = statusTabuleiro.Split('\n');

            List<string> dinossaurosNoTabuleiro = new List<string>();
            List<string> cercadosNoTabuleiro = new List<string>();
            List<string> dinossaurosNaMao = new List<string>();

            lstTabuleiro.Items.Clear();
            foreach (string itemTabuleiro in exibirTabuleiro)
            {
                lstTabuleiro.Items.Add(itemTabuleiro);
            }

            foreach (string dinossauro in exibirMao)
            {
                exibirMao = dinossauro.Split(',');
            }

            for (int i = 1; i < exibirMao.Length - 1; i++)
            {
                dinossaurosNaMao.Add(exibirMao[i]);
            }

            foreach (string cercado in exibirTabuleiro)
            {
                string[] infoCercados = cercado.Split(' ');
                cercadosNoTabuleiro.Add(infoCercados[0]);
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
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

                case "TI":
                    foreach(string dinossauro in dinossaurosNoTabuleiro)
                    {
                        if(dinossauroSelecionado == "ti" && dinossauroSelecionado == dinossauro)
                        {
                            MessageBox.Show("Você deve seguir as regras do dado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
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
                    foreach (string dinossauro in dinossaurosNaMao)
                    {
                        if (dinossauroSelecionado == dinossauro)
                        {
                            break;
                        }
                        MessageBox.Show("Você deve jogar um dinossauro válido", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Jogo.Jogar(idJogador, senhaJogador, dinossauroSelecionado, cercadoSelecionado);
                    break;

            }
                 

        }

        private void Lobby_Load(object sender, EventArgs e)
        {

        }
    }
}
