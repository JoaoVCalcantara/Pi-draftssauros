using Draft;
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
        public Form1()
        {
            InitializeComponent();
            label4.Text = Jogo.versao;
            
        }

        private void btn_Test_Click(object sender, EventArgs e)
        {
            try
            {
                
                string retorno = Jogo.ListarPartidas("T");
                txt_Test.Text = retorno;

                retorno = retorno.Replace("\r"," ");
                retorno = retorno.Substring(0, retorno.Length - 1);
                string[] partidas = retorno.Split('\n');

                listBox1.Items.Clear();
                for (int i = 0; i< partidas.Length -1; i++)
                {
                    listBox1.Items.Add(partidas[i]);
                }
                


            }
            catch (Exception)
            {

                throw;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string partida = listBox1.SelectedItem.ToString();
                string[] dadosPartida = partida.Split(',');

                int idPartida = Convert.ToInt32(dadosPartida[0]);
                string nomePartida = dadosPartida[1];
                string data = dadosPartida[2];

                label1.Text = idPartida.ToString();
                label2.Text = nomePartida;
                label3.Text = data;

                string retorno = Jogo.ListarJogadores(idPartida);
                if (retorno.Substring(0, 4) == "ERRO")
                {
                    MessageBox.Show("Deu erro bixu, dont tem partidas disponiveis \n" + retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                    return;
                }
                else
                {
                    retorno = retorno.Replace("\r", "");
                    string[] jogadore = retorno.Split('\n');

                    listBox2.Items.Clear();
                    for (int i = 0; i < jogadore.Length ; i++)
                    {
                        listBox2.Items.Add(jogadore[i]);
                    }
                }


            }
            catch (Exception)
            {
                MessageBox.Show("PARA DE CLICAR VARIAS VEZES", "ANIMAL",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;

            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_criar_partida_Click(object sender, EventArgs e)
        {
            try
            {
                 string retorno = Jogo.CriarPartida("P", "2024-06-20", "Jurássicos");
                
                if (retorno.Contains("ERRO"))
                {
                    MessageBox.Show(retorno, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                MessageBox.Show("Partida criada com sucesso, contendo o numero: " + retorno,"Pi 3", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnJogadores_Click(object sender, EventArgs e)
        {
            try
            {
                var jogadoresForm = new Jogadores();
                jogadoresForm.StartPosition = FormStartPosition.CenterParent;
                // Use ShowDialog(this) para modal com owner; se preferir não-modal troque para Show().
                jogadoresForm.ShowDialog(this);
                jogadoresForm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário Jogadores: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
