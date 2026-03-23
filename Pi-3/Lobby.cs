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
                int id = int.Parse(lbId.Text);
                string senha = lbSenha.Text;

                string retorno = Jogo.Iniciar(id, senha);

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
    }
}
