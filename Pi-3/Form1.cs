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
    public partial class Form1 : Form
    {
        public int Selected {  get; set; }
        public string Versao { get; set; }
        public string Equipe { get; set; }
        public string idPartidaSelecionada = "0"; 



        public Form1()
        {
            InitializeComponent();
            ListarPartidas();
        }

        public void AtualizarTela()
        {
            Equipe = "Jurássicos";
            label4.Text = Versao;
            label5.Text = Equipe;
        }

        private void ListarPartidas()
        {
            string retorno = Jogo.ListarPartidas("T");

            if (string.IsNullOrWhiteSpace(retorno))
            {
                listBox1.Items.Clear();
                MessageBox.Show("Sem partidas disponíveis.");
                return;
            }

            retorno = retorno.Replace("\r", "").TrimEnd();

            string[] partidas = retorno.Split('\n');

            listBox1.Items.Clear();
            foreach (var p in partidas)
            {
                if (!string.IsNullOrWhiteSpace(p))
                    listBox1.Items.Add(p);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = listBox1.SelectedItem;
            if (selected == null) return;

            string partida = selected.ToString();
            if (string.IsNullOrWhiteSpace(partida)) return;

            string[] dadosPartida = partida.Split(',');

            if (dadosPartida.Length < 4)
            {
                MessageBox.Show("Formato de partida inválido.");
                return;
            }

            if (!int.TryParse(dadosPartida[0], out int idPartida))
            {
                MessageBox.Show("ID da partida inválido.");
                return;
            }

            string nomePartida = dadosPartida[1].Trim();
            string data = dadosPartida[2].Trim();
            string statusPartida = dadosPartida[3].Trim();
            string statusFormatado = "";

            if (statusPartida == "A")
                statusFormatado = "Aberta";
            else if (statusPartida == "J")
                statusFormatado = "Jogando";
            else if (statusPartida == "E")
                statusFormatado = "Encerrado";
            else
                statusFormatado = "Desconhecido";

            label1.Text = idPartida.ToString();
            idPartidaSelecionada = idPartida.ToString();
            label2.Text = nomePartida;
            label3.Text = data;
            label12.Text = statusFormatado;

            string retorno = Jogo.ListarJogadores(idPartida);

            if (string.IsNullOrWhiteSpace(retorno))
            {
                listBox2.Items.Clear();
                MessageBox.Show("Nenhum jogador registrado.");
                return;
            }

            if (retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(retorno);
                return;
            }

            retorno = retorno.Replace("\r", "");

            string[] jogadores = retorno.Split('\n');

            listBox2.Items.Clear();
            foreach (var j in jogadores)
            {
                if (!string.IsNullOrWhiteSpace(j))
                    listBox2.Items.Add(j.Trim());
            }
        }

        private void btn_criar_partida_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string nome = txtPartida.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string grupo = txtGrupo.Text.Trim();

            if (string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Informe o nome da partida.");
                return;
            }

            if (string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Informe a senha da partida.");
                return;
            }

            if (string.IsNullOrEmpty(grupo))
            {
                MessageBox.Show("Informe o nome do grupo.");
                return;
            }

            if (nome.Length > 20)
            {
                MessageBox.Show("Nome da partida deve ter no máximo 20 caracteres.");
                return;
            }

            if (senha.Length > 10)
            {
                MessageBox.Show("Senha deve ter no máximo 10 caracteres.");
                return;
            }

            if (grupo.Length > 40)
            {
                MessageBox.Show("Nome do grupo deve ter no máximo 40 caracteres.");
                return;
            }

            btn.Enabled = false;

            string retorno = Jogo.CriarPartida(nome, senha, grupo);
            idPartidaSelecionada = retorno;

            if (string.IsNullOrWhiteSpace(retorno))
            {
                MessageBox.Show("Sem resposta do servidor.");
                btn.Enabled = true;
                return;
            }

            retorno = retorno.Trim();

            if (retorno.StartsWith("ERRO"))
            {
                MessageBox.Show(retorno);
                btn.Enabled = true;
                return;
            }

           
            label15.Text = "Partida criada com sucesso!";

           
            ListarPartidas();

            btn.Enabled = true;
        }

        private void btnJogadores_Click(object sender, EventArgs e)
        {
            string nomeJogador = txtNomeJogador.Text.Trim();
            string senhaPartida = txtSenha2.Text.Trim();

            int idPartida;
            if (!int.TryParse(txtIDPartida.Text.Trim(), out idPartida))
            {
                MessageBox.Show("Informe um ID de partida válido.");
                return;
            }

            if (string.IsNullOrEmpty(nomeJogador))
            {
                MessageBox.Show("Informe o nome do jogador.");
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

            if (partesRetorno.Length >= 2)
            {
                lblIDJogador.Text = partesRetorno[0].Trim();
                lblSenhaJogador.Text = partesRetorno[1].Trim();
                label20.Text = "Jogador adicionado!";
            }
            else
            {
                MessageBox.Show("Retorno inválido ao entrar na partida.");
                return;
            }

            string jogadores = Jogo.ListarJogadores(idPartida);

            if (!string.IsNullOrWhiteSpace(jogadores) && !jogadores.StartsWith("ERRO"))
            {
                jogadores = jogadores.Replace("\r", "");
                string[] listaJogadores = jogadores.Split('\n');

                listBox2.Items.Clear();

                foreach (string j in listaJogadores)
                {
                    if (!string.IsNullOrWhiteSpace(j))
                        listBox2.Items.Add(j.Trim());
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelecionarPartida_Click(object sender, EventArgs e)
        {
            idPartidaSelecionada = label1.Text;
            Close();
        }
    }
}