using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Draft;

namespace Pi_3
{
    public partial class TelaJogo : Form
    {
        public int IdPartida { get; set; }
        public int IdJogadorPrincipal { get; set; }
        public string SenhaJogadorPrincipal { get; set; }
        public string InfoRodada { get; set; }
        public string NomeJogadorPrincipal { get; set; }

        private static readonly Dictionary<string, Point> PosicoesCercados = new Dictionary<string, Point>
        {
            { "CD", new Point(485, 265) },
            { "FI", new Point(190, 90) },
            { "MT", new Point(240, 240) },
            { "PA", new Point(275, 400) },
            { "RS", new Point(550, 70) },
            { "IS", new Point(615, 380) },
            { "RI", new Point(0, 0) },
        };

        private static readonly Dictionary<string, int> MaxDinosCercado = new Dictionary<string, int>
        {
            { "CD", 6 }, { "FI", 6 }, { "MT", 3 },
            { "PA", 6 }, { "RS", 1 }, { "IS", 1 }, { "RI", 6 },
        };

        private const int MAO_SPACING = 58;
        private const int MAO_SIZE = 50;
        private const int MAO_GAP = 12;
        private const int DINO_SIZE = 45;
        private const int DINO_SIZE_GG = 60;
        private readonly List<PictureBox> _dinosPictureBoxes = new List<PictureBox>();
        private readonly List<PictureBox> _maoPictureBoxes = new List<PictureBox>();
        private readonly List<string> _maoSiglas = new List<string>();
        private PictureBox PicDado;
        private int TurnoAtual = 0;

        public TelaJogo()
        {
            InitializeComponent();
            this.Load += TelaJogo_Load;
        }


        private void TelaJogo_Load(object sender, EventArgs e)
        {
            lblIDJogadorPrincipal.Text = IdJogadorPrincipal.ToString();
            lblKeyJogadorPrincipal.Text = SenhaJogadorPrincipal;

            string statusInicial = Jogo.VerificarPartida(IdPartida);
            if (!string.IsNullOrWhiteSpace(statusInicial) && !statusInicial.StartsWith("ERRO"))
            {
                string[] d = statusInicial.Split(',');
                if (d.Length >= 2) int.TryParse(d[1].Trim(), out TurnoAtual);
            }

            lblTurno.Text = $"{TurnoAtual}";

            CarregarJogadoresPartida();
            AtualizarInfoPartida();
            DesenharTabuleiroComDinos();
            CarregarMaoVisual();
            AtualizarHistorico();
        }

        private void LimparDinos()
        {
            foreach (var pb in _dinosPictureBoxes)
            {
                picboxTabuleiro.Controls.Remove(pb);
                pb.Dispose();
            }
            _dinosPictureBoxes.Clear();
        }

        private void DesenharTabuleiroComDinos()
        {
            LimparDinos();

            string retorno = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO")) return;

            retorno = retorno.Replace("\r", "");
            var contadorPorCercado = new Dictionary<string, int>();

            foreach (string linha in retorno.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;

                string[] partes = linha.Split(',');
                if (partes.Length < 3) continue;

                string cercado = partes[0].Trim();
                string dino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int quantidade)) continue;
                if (!PosicoesCercados.ContainsKey(cercado)) continue;

                Image imgDino = ObterImagemDinossauro(dino);
                if (imgDino == null) continue;

                Image imgRotacionada = RotacionarEsquerda(imgDino);

                if (!contadorPorCercado.ContainsKey(cercado))
                    contadorPorCercado[cercado] = 0;

                int max = MaxDinosCercado.ContainsKey(cercado) ? MaxDinosCercado[cercado] : 6;
                int size = (dino == "Ti" || dino == "Tr") ? DINO_SIZE_GG : DINO_SIZE;

                for (int i = 0; i < quantidade; i++)
                {
                    int slot = contadorPorCercado[cercado];
                    if (slot >= max) break;

                    Point formPos = PosicoesCercados[cercado];
                    int espacamento = (cercado == "MT") ? 22 : 30;

                    int relX = formPos.X - picboxTabuleiro.Left + slot * espacamento;
                    int relY = formPos.Y - picboxTabuleiro.Top;

                    var pb = new PictureBox
                    {
                        Image = imgRotacionada,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BackColor = Color.Transparent,
                        Width = size,
                        Height = size,
                        Left = relX,
                        Top = relY,
                    };

                    picboxTabuleiro.Controls.Add(pb);
                    pb.BringToFront();
                    _dinosPictureBoxes.Add(pb);
                    contadorPorCercado[cercado]++;
                }
            }
        }

        private void LimparMao()
        {
            foreach (var pb in _maoPictureBoxes) { this.Controls.Remove(pb); pb.Dispose(); }
            _maoPictureBoxes.Clear();
            _maoSiglas.Clear();
        }

        private void CarregarMaoVisual()
        {
            LimparMao();

            string retorno = Jogo.ExibirMao(IdJogadorPrincipal, SenhaJogadorPrincipal);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO")) return;

            retorno = retorno.Replace("\r", "");
            bool primeiraLinha = true;

            foreach (string linha in retorno.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                if (primeiraLinha) { primeiraLinha = false; continue; }

                string[] partes = linha.Split(',');
                if (partes.Length < 2) continue;

                string sigla = partes[0].Trim();
                if (!int.TryParse(partes[1].Trim(), out int qtd)) continue;

                for (int i = 0; i < qtd; i++)
                    AdicionarDinoMao(sigla);
            }

            lstDinossauros.Items.Clear();
            foreach (string s in _maoSiglas)
                lstDinossauros.Items.Add(s);
        }

        private void AdicionarDinoMao(string sigla)
        {
            Image img = ObterImagemDinossauro(sigla);
            if (img == null) return;

            int index = _maoSiglas.Count;

            var pb = new PictureBox
            {
                Image = img,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Width = MAO_SIZE,
                Height = MAO_SIZE,
                Left = picboxTabuleiro.Right + MAO_GAP,
                Top = picboxTabuleiro.Top + index * MAO_SPACING,
                Tag = sigla,
            };

            pb.Click += (s, ev) => txtDinossauro.Text = ((PictureBox)s).Tag.ToString();

            this.Controls.Add(pb);
            pb.BringToFront();
            _maoPictureBoxes.Add(pb);
            _maoSiglas.Add(sigla);
        }

        private void RemoverDinoDaMao(string sigla)
        {
            for (int i = 0; i < _maoSiglas.Count; i++)
            {
                if (string.Equals(_maoSiglas[i], sigla, StringComparison.OrdinalIgnoreCase))
                {
                    this.Controls.Remove(_maoPictureBoxes[i]);
                    _maoPictureBoxes[i].Dispose();
                    _maoPictureBoxes.RemoveAt(i);
                    _maoSiglas.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < _maoPictureBoxes.Count; i++)
                _maoPictureBoxes[i].Top = picboxTabuleiro.Top + i * MAO_SPACING;
        }

        private void AtualizarHistorico()
        {
            string historico = Jogo.ListarHistorico(IdPartida);
            if (string.IsNullOrWhiteSpace(historico) || historico.StartsWith("ERRO")) return;

            historico = historico.Replace("\r", "");

            lstHistorico.Items.Clear();
            foreach (string linha in historico.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha))
                    lstHistorico.Items.Add(linha.Trim());

            if (lstHistorico.Items.Count > 0)
                lstHistorico.TopIndex = lstHistorico.Items.Count - 1;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            CarregarMaoVisual();
        }

        private void btnJogar_Click(object sender, EventArgs e)
        {
            string dinossauro = txtDinossauro.Text.Trim();
            string cercado = txtCercado.Text.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(dinossauro) || string.IsNullOrWhiteSpace(cercado))
            { MessageBox.Show("Informe o dinossauro e o cercado."); return; }

            string dinoParaApi = CapitalizarSigla(dinossauro);
            string retorno = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dinoParaApi, cercado);

            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO"))
            { MessageBox.Show(string.IsNullOrWhiteSpace(retorno) ? "Sem resposta do servidor." : retorno); return; }

            txtDinossauro.Text = "";
            RemoverDinoDaMao(dinoParaApi);
            DesenharTabuleiroComDinos();
            AtualizarHistorico();

            // Verifica se o turno finalizou (todos os jogadores jogaram)
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (string.IsNullOrWhiteSpace(statusPartida) || statusPartida.StartsWith("ERRO")) return;

            string[] d = statusPartida.Split(',');
            if (d.Length < 5) return;

            string statusTurno = d[2].Trim();
            int.TryParse(d[1].Trim(), out int turnoAtual);

            if (statusTurno == "F")
            {
                TurnoAtual = turnoAtual;
                lblTurno.Text = $"Turno: {TurnoAtual}";
                lblStatusJogada.Text = $"Turno {TurnoAtual} finalizado!";
                lblStatusJogada.ForeColor = Color.Green;
                AtualizarInfoPartida();
                CarregarMaoVisual();
                AtualizarHistorico();
            }
            else
            {
                lblStatusJogada.Text = "Jogada registrada. Aguardando outros jogadores...";
                lblStatusJogada.ForeColor = Color.DarkOrange;
            }
        }

        private void btnAtualizarHistorico_Click(object sender, EventArgs e)
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (!string.IsNullOrWhiteSpace(statusPartida) && !statusPartida.StartsWith("ERRO"))
            {
                string[] d = statusPartida.Split(',');
                if (d.Length >= 5)
                {
                    int.TryParse(d[1].Trim(), out int turnoAtual);
                    string statusJogo = d[0].Trim();

                    if (turnoAtual != TurnoAtual)
                    {
                        TurnoAtual = turnoAtual;
                        lblTurno.Text = $"{TurnoAtual}";
                        lblStatusJogada.Text = $"Turno {TurnoAtual} iniciado!";
                        lblStatusJogada.ForeColor = Color.Green;
                        DesenharTabuleiroComDinos();
                        CarregarMaoVisual();
                        AtualizarInfoPartida();
                    }
                    else
                    {
                        AtualizarInfoPartida();
                    }

                    if (statusJogo == "E")
                    {
                        lblStatusJogada.Text = "Partida encerrada!";
                        lblStatusJogada.ForeColor = Color.Red;
                    }
                }
            }

            AtualizarHistorico();
        }

        private void btnValidarTurno_Click(object sender, EventArgs e) => ValidarTurno();

        private void btnVerificarTabuleiro_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIDJogadorTabuleiro.Text, out int idJogador))
            { MessageBox.Show("Informe um ID válido."); return; }

            string retorno = Jogo.ExibirTabuleiro(idJogador, SenhaJogadorPrincipal);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO"))
            { MessageBox.Show(string.IsNullOrWhiteSpace(retorno) ? "Sem resposta do servidor." : retorno); return; }

            retorno = retorno.Replace("\r", "");
            lstTabuleiro.Items.Clear();
            foreach (string linha in retorno.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha))
                    lstTabuleiro.Items.Add(linha.Trim());

            if (idJogador == IdJogadorPrincipal)
                DesenharTabuleiroComDinos();
        }

        private void btnAtualizarInfoPartida_Click(object sender, EventArgs e)
        {
            AtualizarInfoPartida();
            AtualizarHistorico();
        }

        private void AtualizarInfoPartida()
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (string.IsNullOrWhiteSpace(statusPartida) || statusPartida.StartsWith("ERRO")) return;

            string[] d = statusPartida.Split(',');
            if (d.Length < 5) return;

            string turno = d[1].Trim();
            string idJogadorDado = d[3].Trim();
            string faceDado = d[4].Trim();
            string nomeJogadorDado = idJogadorDado;

            string jogadores = Jogo.ListarJogadores(IdPartida);
            if (!string.IsNullOrWhiteSpace(jogadores) && !jogadores.StartsWith("ERRO"))
            {
                foreach (string j in jogadores.Replace("\r", "").Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(j)) continue;
                    string[] p = j.Split(',');
                    if (p.Length >= 2 && p[0].Trim() == idJogadorDado)
                    { nomeJogadorDado = p[1].Trim(); break; }
                }
            }

            if (PicDado == null)
            {
                PicDado = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Width = 50,
                    Height = 50,
                    Left = this.ClientSize.Width - 62,
                    Top = this.ClientSize.Height - 62,
                };
                this.Controls.Add(PicDado);
                PicDado.BringToFront();
            }

            PicDado.Image = ObterImagemDado(faceDado);
        }

        private void ValidarTurno()
        {
            string statusTurno = Jogo.VerificarTurno(IdPartida);
            if (string.IsNullOrWhiteSpace(statusTurno) || statusTurno.StartsWith("ERRO"))
            { MessageBox.Show(string.IsNullOrWhiteSpace(statusTurno) ? "Erro ao verificar turno." : statusTurno); return; }

            statusTurno = statusTurno.Trim();
            foreach (string lado in new[] { "AL", "FL", "PR", "TI", "VZ", "WC" })
            {
                int idx = statusTurno.IndexOf(lado);
                if (idx != -1 && idx + lado.Length < statusTurno.Length)
                { statusTurno = statusTurno.Insert(idx + lado.Length, "\n"); break; }
            }

            lstVerficarTurno.Items.Clear();
            foreach (string linha in statusTurno.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha))
                    lstVerficarTurno.Items.Add(linha.Trim());
        }

        private void CarregarJogadoresPartida()
        {
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO")) return;

            retorno = retorno.Replace("\r", "");
            lstJogadoresPartida.Items.Clear();
            foreach (string jogador in retorno.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(jogador)) continue;
                string[] p = jogador.Split(',');
                if (p.Length >= 2)
                    lstJogadoresPartida.Items.Add(p[0].Trim() + " - " + p[1].Trim());
            }
        }


        private Image RotacionarEsquerda(Image img)
        {
            Bitmap bmp = new Bitmap(img);
            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
            return bmp;
        }

        private string CapitalizarSigla(string sigla)
        {
            if (string.IsNullOrEmpty(sigla)) return sigla;
            return char.ToUpper(sigla[0]) + (sigla.Length > 1 ? sigla.Substring(1).ToLower() : "");
        }

        private Image ObterImagemDinossauro(string codigo)
        {
            switch (codigo)
            {
                case "Br": return Properties.Resources.brontossauro_removebg_preview;
                case "Ep": return Properties.Resources.espinossauro_removebg_preview;
                case "Et": return Properties.Resources.dino_azul_removebg_preview;
                case "Pa": return Properties.Resources.dino_verde_removebg_preview;
                case "Ti": return Properties.Resources.Trex_removebg_preview;
                case "Tr": return Properties.Resources.triceratops_removebg_preview;
                default: return null;
            }
        }

        private Image ObterImagemDado(string face)
        {
            switch (face)
            {
                case "FL": return Properties.Resources.Floresta;
                case "PR": return Properties.Resources.Pradaria;
                case "WC": return Properties.Resources.Banheiros;
                case "AL": return Properties.Resources.PracaAlimentacao;
                case "TI": return Properties.Resources.CuidadoTrex;
                case "VZ": return Properties.Resources.CercadoVazio;
                default: return null;
            }
        }
    }
}