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
        public string Equipe;

        public Lobby()
        {
            InitializeComponent();
            this.versao = Jogo.versao;
            AtualizarTela();
        }

        public void AtualizarTela()
        {
            Equipe = "Jurássicos";
            label8.Text = versao;
            label9.Text = Equipe;
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

            string nomeJogadorDado = idJogadorDado;

            string jogadores = Jogo.ListarJogadores(idPartida);

            if (!string.IsNullOrWhiteSpace(jogadores) && !jogadores.StartsWith("ERRO"))
            {
                jogadores = jogadores.Replace("\r", "");
                string[] listaJogadores = jogadores.Split('\n');

                foreach (string j in listaJogadores)
                {
                    if (!string.IsNullOrWhiteSpace(j))
                    {
                        string[] partes = j.Split(',');

                        if (partes.Length >= 2)
                        {
                            string id = partes[0].Trim();
                            string nome = partes[1].Trim();

                            if (id == idJogadorDado)
                            {
                                nomeJogadorDado = nome;
                                break;
                            }
                        }
                    }
                }
            }

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

            string turno = dadosPartida[1].Trim();

            lblRodada.Text = $"Turno {turno}: O jogador {nomeJogadorDado} rolou o dado e caiu {nomeFace}";

            lblStatusPartida.Text = "Partida iniciada com sucesso!";
            lblStatusPartida.ForeColor = Color.Green;

            TelaJogo tela = new TelaJogo();
            tela.IdPartida = idPartida;
            tela.IdJogadorPrincipal = idJogador;
            tela.SenhaJogadorPrincipal = senhaJogador;
            tela.InfoRodada = lblRodada.Text;
            tela.NomeJogadorPrincipal = txtNomeJogadorPrincipal.Text.Trim();

            tela.Show();
            this.Hide();
        }

        

        private void Lobby_Load(object sender, EventArgs e)
        {

        }

        private void btnVerTabuleiro_Click(object sender, EventArgs e)
        {
            {
                int idPartida;
                if (!int.TryParse(lblPartida.Text, out idPartida))
                {
                    MessageBox.Show("Selecione uma partida válida primeiro.");
                    return;
                }

                int idJogador;
                if (!int.TryParse(lblIDJogadorPrincipal.Text, out idJogador))
                {
                    MessageBox.Show("Entre na partida primeiro (clique em 'Entrar como Jogador').");
                    return;
                }

                string senhaJogador = lblKeyJogadorPrincipal.Text;

                if (string.IsNullOrWhiteSpace(senhaJogador))
                {
                    MessageBox.Show("Senha do jogador não encontrada. Entre na partida primeiro.");
                    return;
                }

                // Verifica se a partida já existe e está em andamento
                string statusPartida = Jogo.VerificarPartida(idPartida);

                if (string.IsNullOrWhiteSpace(statusPartida) || statusPartida.StartsWith("ERRO"))
                {
                    MessageBox.Show("Partida não encontrada ou ainda não iniciada.");
                    return;
                }

                string[] dadosPartida = statusPartida.Split(',');

                if (dadosPartida.Length < 5)
                {
                    MessageBox.Show("Dados da partida inválidos.");
                    return;
                }

                // Monta info da rodada atual (igual ao btnIniciarPartida)
                string idJogadorDado = dadosPartida[3].Trim();
                string nomeJogadorDado = idJogadorDado;

                string jogadores = Jogo.ListarJogadores(idPartida);
                if (!string.IsNullOrWhiteSpace(jogadores) && !jogadores.StartsWith("ERRO"))
                {
                    jogadores = jogadores.Replace("\r", "");
                    string[] listaJogadores = jogadores.Split('\n');

                    foreach (string j in listaJogadores)
                    {
                        if (!string.IsNullOrWhiteSpace(j))
                        {
                            string[] partes = j.Split(',');
                            if (partes.Length >= 2)
                            {
                                string id = partes[0].Trim();
                                string nome = partes[1].Trim();
                                if (id == idJogadorDado)
                                {
                                    nomeJogadorDado = nome;
                                    break;
                                }
                            }
                        }
                    }
                }

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

                string turno = dadosPartida[1].Trim();
                string infoRodada = $"Turno {turno}: O jogador {nomeJogadorDado} rolou o dado e caiu {nomeFace}";

                // Abre a TelaJogo diretamente
                TelaJogo tela = new TelaJogo();
                tela.IdPartida = idPartida;
                tela.IdJogadorPrincipal = idJogador;
                tela.SenhaJogadorPrincipal = senhaJogador;
                tela.InfoRodada = infoRodada;
                tela.NomeJogadorPrincipal = txtNomeJogadorPrincipal.Text.Trim();

                tela.Show();
                this.Hide();
            }

        }
    }
}
