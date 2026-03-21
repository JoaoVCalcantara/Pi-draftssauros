using System;
using System.Linq;
using System.Windows.Forms;
using Draft;

namespace Pi_3
{
    public partial class Jogadores : Form
    {
        public int idJogador { get; set; }
        public string senhaJogador{ get; set; }
        public Jogadores()
        {
            InitializeComponent();
            this.Load += Jogadores_Load;
        }

        private void Jogadores_Load(object sender, EventArgs e)
        {
            try
            {
                // Garantir que a caixa de senha esteja limpa ao abrir o formulário
                txtSenha?.Clear();

                // Preenche o combo com as partidas disponíveis ao abrir o formulário
                string retorno = Jogo.ListarPartidas("T"); // ajustar filtro se necessário
                if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                {
                    // opcional: mostrar mensagem ou log
                    return;
                }
                
                retorno = retorno.Replace("\r", "");
                var partidas = retorno
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToArray();

                cboJogadores.Items.Clear();
                foreach (var p in partidas)
                    cboJogadores.Items.Add(p);

                // seleciona o primeiro item (opcional)
                if (cboJogadores.Items.Count > 0)
                    cboJogadores.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar partidas: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboJogadores.SelectedItem == null)
                    return;

                string selecionado = cboJogadores.SelectedItem.ToString();
                if (string.IsNullOrWhiteSpace(selecionado))
                    return;

                // Se o item tiver formato "id,nome,data", usa o id; caso contrário, assume que é um filtro
                string retorno;
                var partes = selecionado.Split(',');
                if (partes.Length > 0 && int.TryParse(partes[0].Trim(), out int idPartida))
                {
                    retorno = Jogo.ListarJogadores(idPartida);
                }
                else
                {
                    // fallback: lista partidas com o texto selecionado
                    retorno = Jogo.ListarPartidas(selecionado);
                }

                if (string.IsNullOrWhiteSpace(retorno))
                {
                    txtSenha.Text = "Nenhum resultado encontrado.";
                    return;
                }

                if (retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                {
                    txtSenha.Text = retorno;
                    return;
                }

                // Normaliza e mostra os jogadores no textBox1 (não no txtSenha)
                retorno = retorno.Replace("\r", "");
                var jogadores = retorno
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();

                txtSenha.Clear();
                foreach (var j in jogadores)
                    txtSenha.AppendText(j + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar jogadores: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                // Valida seleção da partida
                if (cboJogadores.SelectedItem == null)
                {
                    MessageBox.Show("Selecione uma partida antes de entrar.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selecionado = cboJogadores.SelectedItem.ToString();
                if (string.IsNullOrWhiteSpace(selecionado))
                {
                    MessageBox.Show("Item de partida inválido.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var partes = selecionado.Split(',');
                if (partes.Length < 1 || !int.TryParse(partes[0].Trim(), out int idPartida))
                {
                    MessageBox.Show("Não foi possível obter o ID da partida selecionada.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pega nome do jogador a partir de textBox2 (campo esperado para o nickname)
                string jogador = (textBox2 != null) ? textBox2.Text.Trim() : string.Empty;
                if (string.IsNullOrWhiteSpace(jogador))
                {
                    MessageBox.Show("Informe o nome do jogador no campo apropriado.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // PEGAR A SENHA DA TEXTBOX txtSenha (o que você solicitou)
                string senhaPartida = (txtSenha != null) ? txtSenha.Text.Trim() : string.Empty;
                if (string.IsNullOrWhiteSpace(senhaPartida))
                {
                    MessageBox.Show("Informe a senha da partida no campo 'txtSenha'.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Chamada ao método Entrar com as informações coletadas
                // DEBUG: mostra os valores que serão enviados
                // MessageBox.Show($"DEBUG -> id: [{idPartida}], jogador: [{jogador}], senha: [{senhaPartida}]");

                string retorno = Jogo.Entrar(idPartida, jogador, senhaPartida);
                var partesRetorno = retorno.Split(',');

                idJogador = int.Parse(partesRetorno[0]);
                senhaJogador = partesRetorno[1];
                //retornar o id e a senha
                if (string.IsNullOrWhiteSpace(retorno))
                {
                    MessageBox.Show("Sem resposta do servidor ao tentar entrar.", "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (retorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Sucesso: mostra retorno e atualiza lista de jogadores exibida (se aplicável)
                MessageBox.Show("Entrada realizada: " + retorno, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Atualiza lista de jogadores exibida
                try
                {
                    string jogadoresRetorno = Jogo.ListarJogadores(idPartida);
                    if (!string.IsNullOrWhiteSpace(jogadoresRetorno) && !jogadoresRetorno.StartsWith("ERRO", StringComparison.OrdinalIgnoreCase))
                    {
                        jogadoresRetorno = jogadoresRetorno.Replace("\r", "");
                        var jogadores = jogadoresRetorno.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
                        txtSenha.Clear();
                        foreach (var j in jogadores)
                            txtSenha.AppendText(j + Environment.NewLine);
                    }
                }
                catch { /* falha em atualizar lista não impede fluxo principal */ }
                //this.idJogador = Convert.ToInt32(label1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar entrar na partida: " + ex.Message, "PI 3", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
