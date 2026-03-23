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

        public string Versao { get; set; }
        public string Equipe { get; set; }
        public int idPartidaSelecionada { get; set; }
        
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
            try
            {
                string retorno = Jogo.ListarPartidas("T");
                retorno = retorno.Replace("\r", " ");
                retorno = retorno.Substring(0, retorno.Length - 1);
                string[] partidas = retorno.Split('\n');
                listBox1.Items.Clear();
                for (int i = 0; i < partidas.Length; i++)
                {
                    listBox1.Items.Add(partidas[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar partidas: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selected = listBox1.SelectedItem;
                if (selected == null)
                {
                    // Nada selecionado
                    return;
                }

                string partida = selected.ToString();
                if (string.IsNullOrWhiteSpace(partida))
                {
                    return;
                }

                string[] dadosPartida = partida.Split(',');
                if (dadosPartida.Length < 3)
                {
                    MessageBox.Show("Formato de partida inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(dadosPartida[0], out int idPartida))
                {
                    MessageBox.Show("ID da partida inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nomePartida = dadosPartida[1].Trim();
                string data = dadosPartida[2].Trim();

                label1.Text = idPartida.ToString();
                label2.Text = nomePartida;
                label3.Text = data;

                string retorno = Jogo.ListarJogadores(idPartida);

                if (string.IsNullOrEmpty(retorno))
                {
                    listBox2.Items.Clear();
                    MessageBox.Show("Nenhum jogador registrado para esta partida.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                retorno = retorno.Replace("\r", "");

                // Verifica se a resposta indica erro
                if (retorno.Length >= 4 && retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Deu erro bixu, não tem partidas disponíveis \n" + retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Quebra em linhas e elimina entradas vazias
                string[] jogadores = retorno.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                listBox2.Items.Clear();
                foreach (var j in jogadores)
                {
                    var item = j?.Trim();
                    if (!string.IsNullOrEmpty(item))
                        listBox2.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Evita relançar a exceção e informa o usuário
                MessageBox.Show("Ocorreu um erro ao carregar jogadores: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_criar_partida_Click(object sender, EventArgs e)
        {
            try
            {
                var partidaForm = new Partida();
                partidaForm.StartPosition = FormStartPosition.CenterParent;
                // Use ShowDialog(this) para modal com owner; se preferir não-modal troque para Show().
                partidaForm.ShowDialog(this);
                partidaForm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir o formulário Jogadores: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void lb_Jurassicos_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.idPartidaSelecionada = Convert.ToInt32(label1.Text); 
                this.Close();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnSelecionarPartida_Click(object sender, EventArgs e)
        {
            try
            {
                this.idPartidaSelecionada = Convert.ToInt32(label1.Text);
                this.Close();

            }
            catch (Exception)
            {

                throw;
            }

            this.Close();

        }

        private void btnAtualizarPartidas_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ListarPartidas();
            Cursor.Current = Cursors.Default;
        }

        private void txt_Test_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
