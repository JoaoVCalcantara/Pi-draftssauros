using Draft;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pi_3
{
    public partial class TelaJogo : Form
    {
        public int IdPartida { get; set; }
        public int IdJogadorPrincipal { get; set; }
        public string SenhaJogadorPrincipal { get; set; }
        public string InfoRodada { get; set; }
        public string NomeJogadorPrincipal { get; set; }
        private bool _processandoTick = false;


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

        // ═══════════════════════════════════════════════
        // AUTOMAÇÃO INTELIGENTE - PARTIDA COMPLETA
        // ═══════════════════════════════════════════════

        private Timer timerAutomacao;
        private int ultimoTurnoVerificado = -1;
        private bool jogouEsteturno = false;
        private Random random = new Random();


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
        private void IniciarAutomacao()
        {
            if (timerAutomacao != null && timerAutomacao.Enabled)
            {
                MessageBox.Show("Automação já está em andamento!");
                return;
            }

            ultimoTurnoVerificado = -1;
            jogouEsteturno = false;

            timerAutomacao = new Timer();
            timerAutomacao.Interval = 3000;
            timerAutomacao.Tick += TimerAutomacao_Tick;
            timerAutomacao.Start();

            lblStatusJogada.Text = "🟢 Automação iniciada!";
            lblStatusJogada.ForeColor = Color.Blue;
        }

        private int _ticksSemJogar = 0;
        private const int MAX_TICKS_SEM_JOGAR = 5; // 5 ticks × 3s = 15s sem jogar → força Rio

        private void TimerAutomacao_Tick(object sender, EventArgs e)
        {
            // Evita execução simultânea do tick
            if (_processandoTick) return;
            _processandoTick = true;

            try
            {
                string statusPartida = Jogo.VerificarPartida(IdPartida);
                if (string.IsNullOrWhiteSpace(statusPartida) || statusPartida.StartsWith("ERRO"))
                {
                    lblStatusJogada.Text = $"❌ Erro: {statusPartida}";
                    lblStatusJogada.ForeColor = Color.Red;
                    return;
                }

                string[] d = statusPartida.Split(',');
                if (d.Length < 5) return;

                string statusGeral = d[0].Trim();
                if (statusGeral == "E")
                {
                    timerAutomacao.Stop();
                    lblStatusJogada.Text = "🏁 Partida encerrada!";
                    lblStatusJogada.ForeColor = Color.DarkGreen;
                    AtualizarHistorico();
                    return;
                }

                string idJogadorComDado = d[3].Trim();
                string faceDado = d[4].Trim();
                int turnoAtual = 0;
                int.TryParse(d[1].Trim(), out turnoAtual);

                lblTurno.Text = $"{turnoAtual}";

                if (ultimoTurnoVerificado != turnoAtual)
                {
                    ultimoTurnoVerificado = turnoAtual;
                    jogouEsteturno = false;
                    _ticksSemJogar = 0;
                    // NÃO carrega mão aqui, NÃO joga ainda
                    // Apenas retorna e espera o próximo tick
                    lblStatusJogada.Text = $"🔄 Turno {turnoAtual} detectado, aguardando servidor...";
                    lblStatusJogada.ForeColor = Color.Purple;
                    return; // ← sai e espera o próximo tick
                }

                if (jogouEsteturno)
                {
                    lblStatusJogada.Text = $"✔️ Turno {turnoAtual} | Aguardando próximo turno...";
                    lblStatusJogada.ForeColor = Color.Gray;
                    return;
                }

                if (idJogadorComDado != IdJogadorPrincipal.ToString())
                {
                    _ticksSemJogar++;
                    lblStatusJogada.Text = $"⏳ Turno {turnoAtual} | Vez do jogador {idJogadorComDado} | ({_ticksSemJogar}/{MAX_TICKS_SEM_JOGAR})";
                    lblStatusJogada.ForeColor = Color.DarkOrange;

                    if (_ticksSemJogar >= MAX_TICKS_SEM_JOGAR)
                    {
                        // Verifica a mão antes de forçar jogada
                        CarregarMaoVisual();

                        if (_maoSiglas.Count == 0)
                        {
                            // Mão vazia = já jogou esse turno, só aguarda
                            _ticksSemJogar = 0;
                            lblStatusJogada.Text = "✔️ Mão vazia, aguardando próximo turno...";
                            lblStatusJogada.ForeColor = Color.Gray;
                            return;
                        }

                        // Só força Rio se realmente ainda tem dinos E o servidor confirma
                        // que este jogador ainda não jogou verificando o histórico
                        string historico = Jogo.ListarHistorico(IdPartida);
                        bool jaJogouNoTurno = false;

                        if (!string.IsNullOrWhiteSpace(historico) && !historico.StartsWith("ERRO"))
                        {
                            string[] linhas = historico.Replace("\r", "").Split('\n');
                            // Pega as últimas linhas e verifica se já jogou neste turno
                            for (int i = linhas.Length - 1; i >= Math.Max(0, linhas.Length - 5); i--)
                            {
                                string linha = linhas[i].ToLower();
                                if (linha.Contains(NomeJogadorPrincipal.ToLower()) &&
                                    linha.Contains($"turno {ultimoTurnoVerificado}"))
                                {
                                    jaJogouNoTurno = true;
                                    break;
                                }
                            }
                        }

                        if (jaJogouNoTurno)
                        {
                            _ticksSemJogar = 0;
                            jogouEsteturno = true;
                            lblStatusJogada.Text = "✔️ Já joguei neste turno (histórico confirmado)";
                            lblStatusJogada.ForeColor = Color.Gray;
                            return;
                        }

                        // Confirma que realmente não jogou → força Rio
                        lblStatusJogada.Text = "⚠️ Timeout! Forçando Rio...";
                        lblStatusJogada.ForeColor = Color.OrangeRed;

                        string dinoFallback = CapitalizarSigla(_maoSiglas[0]);
                        string retornoRio = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dinoFallback, "RI");

                        if (!string.IsNullOrWhiteSpace(retornoRio) && !retornoRio.StartsWith("ERRO"))
                        {
                            lblStatusJogada.Text = $"🌊 Timeout → Rio: {dinoFallback} | {retornoRio}";
                            lblStatusJogada.ForeColor = Color.SteelBlue;
                            jogouEsteturno = true;
                            _ticksSemJogar = 0;
                            LimparMao();
                            CarregarMaoVisual();
                            DesenharTabuleiroComDinos();
                            AtualizarHistorico();
                            AtualizarInfoPartida();
                        }
                        else
                        {
                            _ticksSemJogar = 0;
                            lblStatusJogada.Text = $"❌ Timeout Rio falhou: {retornoRio}";
                            lblStatusJogada.ForeColor = Color.Red;
                        }
                        return;
                    }
                }

                // É minha vez!
                lblStatusJogada.Text = $"🎯 Turno {turnoAtual} | Dado: {faceDado} | É minha vez!";
                lblStatusJogada.ForeColor = Color.DarkBlue;

                jogouEsteturno = true;
                _ticksSemJogar = 0;
                FazerJogadaInteligente(faceDado);
            }
            finally
            {
                _processandoTick = false; // sempre libera ao final
            }
        }

        private void FazerJogadaInteligente(string faceDado)
        {
            // Sempre recarrega do servidor antes de jogar
            LimparMao();
            CarregarMaoVisual();

            if (_maoSiglas.Count == 0)
            {
                lblStatusJogada.Text = "❌ Sem dinossauros na mão!";
                lblStatusJogada.ForeColor = Color.Red;
                jogouEsteturno = false;
                return;
            }

            // Pega apenas O PRIMEIRO dino válido — nunca mais de 1 por turno
            List<string> cercadosPermitidos = ObterCercadosPermitidos(faceDado);

            string melhorDino = null;
            string melhorCercado = null;
            int melhorPontuacao = -1;

            // Faz uma cópia da mão para não modificar durante o loop
            var maoAtual = new List<string>(_maoSiglas);

            foreach (string dino in maoAtual)
            {
                foreach (string cercado in cercadosPermitidos)
                {
                    int pontos = EstimarPontos(dino, cercado);
                    if (pontos > melhorPontuacao)
                    {
                        melhorPontuacao = pontos;
                        melhorDino = dino;
                        melhorCercado = cercado;
                    }
                }
                break; // ← avalia só o primeiro dino diferente encontrado
                       // remove este break se quiser avaliar todos
            }

            // Remove o break acima e mantém o loop completo para avaliar todos os dinos
            // O break estava errado — precisa avaliar todos, mas jogar só 1
            melhorDino = null;
            melhorCercado = null;
            melhorPontuacao = -1;

            foreach (string dino in maoAtual)
            {
                foreach (string cercado in cercadosPermitidos)
                {
                    int pontos = EstimarPontos(dino, cercado);
                    if (pontos > melhorPontuacao)
                    {
                        melhorPontuacao = pontos;
                        melhorDino = dino;
                        melhorCercado = cercado;
                    }
                }
            }

            if (melhorDino == null || melhorPontuacao < 0)
            {
                melhorDino = maoAtual[0];
                melhorCercado = "RI";
                lblStatusJogada.Text = $"🌊 Sem jogada válida → Rio: {melhorDino}";
                lblStatusJogada.ForeColor = Color.CadetBlue;
            }
            else
            {
                lblStatusJogada.Text = $"🧠 Jogando {melhorDino} em {melhorCercado} (~{melhorPontuacao} pts)";
                lblStatusJogada.ForeColor = Color.DarkBlue;
            }

            this.Refresh();

            // Joga APENAS 1 dino e para
            string dinoApi = CapitalizarSigla(melhorDino);
            string retorno = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dinoApi, melhorCercado);

            if (!string.IsNullOrWhiteSpace(retorno) && !retorno.StartsWith("ERRO"))
            {
                lblStatusJogada.Text = $"✅ {dinoApi} → {melhorCercado} | {retorno}";
                lblStatusJogada.ForeColor = Color.Green;
                // Recarrega mão do servidor após jogar
                LimparMao();
                CarregarMaoVisual();
                DesenharTabuleiroComDinos();
                AtualizarHistorico();
                AtualizarInfoPartida();
                return; // ← sai imediatamente, joga só 1
            }

            // Falhou → tenta Rio
            lblStatusJogada.Text = $"⚠️ Falhou ({retorno}) → tentando Rio...";
            lblStatusJogada.ForeColor = Color.OrangeRed;
            this.Refresh();

            // Recarrega mão antes do fallback
            LimparMao();
            CarregarMaoVisual();

            if (_maoSiglas.Count == 0)
            {
                lblStatusJogada.Text = "❌ Sem dinos para fallback!";
                jogouEsteturno = false;
                return;
            }

            string dinoFallback = CapitalizarSigla(_maoSiglas[0]);
            string retornoRio = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dinoFallback, "RI");

            if (!string.IsNullOrWhiteSpace(retornoRio) && !retornoRio.StartsWith("ERRO"))
            {
                lblStatusJogada.Text = $"🌊 Fallback Rio: {dinoFallback} | {retornoRio}";
                lblStatusJogada.ForeColor = Color.SteelBlue;
                LimparMao();
                CarregarMaoVisual();
                DesenharTabuleiroComDinos();
                AtualizarHistorico();
                AtualizarInfoPartida();
            }
            else
            {
                lblStatusJogada.Text = $"❌ Rio também falhou: {retornoRio}";
                lblStatusJogada.ForeColor = Color.Red;
                jogouEsteturno = false;
            }
        }

        /// <summary>
        /// Retorna os cercados permitidos pelo dado, conforme o manual.
        /// </summary>
        private List<string> ObterCercadosPermitidos(string faceDado)
        {
            // Mapeamento do lado do tabuleiro por cercado
            // FL = Floresta (FI), PR = Pradaria (PA), WC = Banheiros (IS/CD lado banheiro),
            // AL = Alimentação (CD/FI lado alimentação), VZ = Cercado Vazio, TI = sem T-Rex
            switch (faceDado)
            {
                case "FL": // Floresta: FI e MT (lado florestal)
                    return new List<string> { "FI", "MT" };

                case "PR": // Pradaria: PA e MT (lado pradaria)
                    return new List<string> { "PA", "MT" };

                case "WC": // Banheiros: IS e CD (lado banheiros)
                    return new List<string> { "IS", "CD" };

                case "AL": // Alimentação: FI e CD (lado alimentação)
                    return new List<string> { "FI", "CD" };

                case "VZ": // Cercado Vazio: qualquer cercado que esteja vazio
                    return ObterCercadosVazios();

                case "TI": // Cuidado T-Rex: qualquer cercado sem T-Rex
                    return ObterCercadosSemTRex();

                default:
                    return new List<string> { "CD", "FI", "MT", "PA", "RS", "IS" };
            }
        }

        private List<string> ObterCercadosVazios()
        {
            var vazios = new List<string>();
            string tabuleiro = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);
            var ocupados = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(tabuleiro) && !tabuleiro.StartsWith("ERRO"))
            {
                foreach (string linha in tabuleiro.Replace("\r", "").Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;
                    string[] p = linha.Split(',');
                    if (p.Length >= 3 && int.TryParse(p[2].Trim(), out int qtd) && qtd > 0)
                        ocupados.Add(p[0].Trim());
                }
            }

            foreach (string c in new[] { "CD", "FI", "MT", "PA", "RS", "IS" })
                if (!ocupados.Contains(c))
                    vazios.Add(c);

            return vazios.Count > 0 ? vazios : new List<string> { "RI" };
        }

        private List<string> ObterCercadosSemTRex()
        {
            var semTRex = new List<string> { "CD", "FI", "MT", "PA", "RS", "IS" };
            string tabuleiro = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);

            if (!string.IsNullOrWhiteSpace(tabuleiro) && !tabuleiro.StartsWith("ERRO"))
            {
                foreach (string linha in tabuleiro.Replace("\r", "").Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;
                    string[] p = linha.Split(',');
                    if (p.Length >= 2 && p[1].Trim() == "Ti")
                        semTRex.Remove(p[0].Trim());
                }
            }

            return semTRex.Count > 0 ? semTRex : new List<string> { "RI" };
        }

        /// <summary>
        /// Estima pontos de colocar um dino em um cercado, seguindo as regras do manual.
        /// </summary>
        private int EstimarPontos(string dino, string cercado)
        {
            string tabuleiro = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);
            var dinosPorCercado = new Dictionary<string, List<string>>();

            if (!string.IsNullOrWhiteSpace(tabuleiro) && !tabuleiro.StartsWith("ERRO"))
            {
                foreach (string linha in tabuleiro.Replace("\r", "").Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;
                    string[] p = linha.Split(',');
                    if (p.Length < 3) continue;
                    string c = p[0].Trim();
                    string d = p[1].Trim();
                    if (!int.TryParse(p[2].Trim(), out int qtd)) continue;

                    if (!dinosPorCercado.ContainsKey(c))
                        dinosPorCercado[c] = new List<string>();
                    for (int i = 0; i < qtd; i++)
                        dinosPorCercado[c].Add(d);
                }
            }

            var dinosNoCercado = dinosPorCercado.ContainsKey(cercado)
                ? dinosPorCercado[cercado]
                : new List<string>();

            string dinoApi = CapitalizarSigla(dino);

            switch (cercado)
            {
                case "FI": // Floresta da Igualdade: só mesma espécie
                    if (dinosNoCercado.Count == 0) return 3; // começa bem
                    if (dinosNoCercado[0] == dinoApi) return 4 + dinosNoCercado.Count; // mesma espécie = ótimo
                    return -1; // espécie diferente = inválido

                case "CD": // Campina da Diferença: só espécies diferentes
                    if (dinosNoCercado.Contains(dinoApi)) return -1; // já tem essa espécie = inválido
                    return 3 + dinosNoCercado.Count;

                case "PA": // Pradaria do Amor: 5 pts por casal
                    int casais = dinosNoCercado.Count(d => d == dinoApi) / 2;
                    bool vaiFazarCasal = dinosNoCercado.Count(d => d == dinoApi) % 2 == 1;
                    return vaiFazarCasal ? 8 : 2; // forma casal = excelente

                case "MT": // Mata Tripla: 7 pts se tiver exatamente 3
                    if (dinosNoCercado.Count == 2) return 9; // vai completar os 3 = ótimo
                    if (dinosNoCercado.Count == 0) return 3;
                    if (dinosNoCercado.Count == 1) return 5;
                    return -1; // já tem 3 = cheio

                case "RS": // Rei da Selva: 1 dino, 7 pts se tiver mais dessa espécie que outros
                    if (dinosNoCercado.Count > 0) return -1; // já ocupado
                    if (dinoApi == "Ti" || dinoApi == "Tr") return 6; // dinos raros são melhores no RS
                    return 4;

                case "IS": // Ilha Solitária: 1 dino, 7 pts se for único dessa espécie no zoo
                    if (dinosNoCercado.Count > 0) return -1; // já ocupado
                    return 5;

                case "RI": // Rio: 1 pt por dino
                    return 1;

                default:
                    return 1;
            }
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

        private void CarregarJogadoresPartida()
        {
            lstJogadoresPartida.Items.Clear();
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO")) return;

            retorno = retorno.Replace("\r", "");
            foreach (string linha in retorno.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(linha))
                    lstJogadoresPartida.Items.Add(linha.Trim());
            }
        }

        private void AdicionarDinoMao(string sigla)
        {
            int index = _maoPictureBoxes.Count;
            Image img = ObterImagemDinossauro(sigla);
            if (img == null) return;

            var pb = new PictureBox
            {
                Image = img,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = MAO_SIZE,
                Height = MAO_SIZE,
                Left = picboxTabuleiro.Left + picboxTabuleiro.Width + MAO_GAP,
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
                if (d.Length >= 2 && int.TryParse(d[1].Trim(), out int turno))
                {
                    lblTurno.Text = $"Turno: {turno}";
                }
            }

            AtualizarInfoPartida();
            DesenharTabuleiroComDinos();
            CarregarMaoVisual();
            AtualizarHistorico();
        }

        private void btnValidarTurno_Click(object sender, EventArgs e) => ValidarTurno();

        private void btnVerificarTabuleiro_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIDJogadorTabuleiro.Text, out int idJogador))
            { MessageBox.Show("Informe um ID válido."); return; }

            string retorno = Jogo.ExibirTabuleiro(idJogador, SenhaJogadorPrincipal);
            if (string.IsNullOrWhiteSpace(retorno) || retorno.StartsWith("ERRO"))
            { MessageBox.Show(string.IsNullOrWhiteSpace(retorno) ? "Erro ao exibir tabuleiro." : retorno); return; }

            lstTabuleiro.Items.Clear();
            foreach (string linha in retorno.Replace("\r", "").Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha))
                    lstTabuleiro.Items.Add(linha.Trim());
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

        private void btn_auto_Click(object sender, EventArgs e)
        {
            IniciarAutomacao();
        }
    }
}