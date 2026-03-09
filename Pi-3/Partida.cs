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

namespace Pi_3
{
    public partial class Partida : Form
    {
        public Partida()
        {
            InitializeComponent();
        }

        private void btnPartida_Click(object sender, EventArgs e)
        {
            try
            {
                string nome = (txtPartida?.Text ?? "").Trim();
                string senha = (txtSenha?.Text ?? "").Trim();
                string grupo = (txtGrupo?.Text ?? "").Trim();

                if (string.IsNullOrEmpty(nome))
                {
                    MessageBox.Show("Informe o nome da partida.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(senha))
                {
                    MessageBox.Show("Informe a senha da partida.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(grupo))
                {
                    MessageBox.Show("Informe o nome do grupo.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (nome.Length > 20)
                {
                    MessageBox.Show("Nome da partida deve ter no máximo 20 caracteres.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (senha.Length > 10)
                {
                    MessageBox.Show("Senha deve ter no máximo 10 caracteres.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (grupo.Length > 40)
                {
                    MessageBox.Show("Nome do grupo deve ter no máximo 40 caracteres.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (btnPartida != null) btnPartida.Enabled = false;

                string retorno = Jogo.CriarPartida(nome, senha, grupo);

                if (string.IsNullOrWhiteSpace(retorno))
                {
                    MessageBox.Show("Sem resposta do servidor ao criar partida.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                retorno = retorno.Trim();

                if (retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (int.TryParse(retorno, out int idPartida))
                {
                    MessageBox.Show("Partida criada com sucesso. ID: " + idPartida, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Partida criada (retorno não-numérico): " + retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar partida: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (btnPartida != null) btnPartida.Enabled = true;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
