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
        public string NomeJogadorPrincipal { get; set; }
        public string InfoRodada { get; set; }

        private readonly List<PictureBox> _dinosNoTabuleiro = new List<PictureBox>();
        private readonly List<PictureBox> _maoPictureBoxes = new List<PictureBox>();
        private readonly List<string> _siglasDinosNaMao = new List<string>();
        private PictureBox _picDado;

        private Timer _timerAutomacao;
        private int _ultimoTurnoProcessado = -1;
        private bool _jaJogouNesteTurno = false;
        private bool _processandoTick = false;

        private const double PESO_NEGACAO_OPONENTE = 0.35;
        private const double PESO_FUTURO_PROPRIO = 0.25;
        private const double PESO_IS_PENALTY_CEDO = 4.0;
        private const double PESO_FECHAMENTO_TURNO_FINAL = 1.5;
        private const double PESO_ESCASSEZ_UNICO = 1.35;
        private const double PENALTY_ABRIR_TARDE = 3.0;
        private const double BONUS_FECHAR_COMBO_R2 = 4.0;
        private const double PESO_ANTI_DOACAO = 0.5;
        private const int TOTAL_TURNOS = 12;
        private static readonly double[] PESOS_NEGACAO_POR_DISTANCIA = { 1.0, 0.5, 0.25, 0.12, 0.06 };

        private static readonly string[] TodasAsEspecies = { "Br", "Ep", "Et", "Pa", "Ti", "Tr" };

        private static readonly int[] PONTOS_FI = { 0, 2, 4, 8, 12, 18, 24 };
        private static readonly int[] PONTOS_CD = { 0, 1, 3, 6, 10, 15, 21 };
        private const int PONTOS_POR_CASAL = 5;
        private const int PONTOS_MT_COMPLETA = 7;
        private const int PONTOS_RS_VITORIA = 7;
        private const int PONTOS_IS_VITORIA = 7;
        private const int PONTOS_POR_DINO_RIO = 1;
        private const int BONUS_TREX_POR_CERCADO = 1;

        private static readonly Dictionary<string, Point> PosicaoDeCadaCercado = new Dictionary<string, Point>
        {
            { "CD", new Point(485, 265) },
            { "FI", new Point(190, 90) },
            { "MT", new Point(240, 240) },
            { "PA", new Point(275, 400) },
            { "RS", new Point(550, 70) },
            { "IS", new Point(615, 380) },
            { "RI", new Point(0, 0) },
        };

        private static readonly Dictionary<string, int> CapacidadeDeCadaCercado = new Dictionary<string, int>
        {
            { "CD", 6 }, { "FI", 6 }, { "MT", 3 },
            { "PA", 12 }, { "RS", 1 }, { "IS", 1 }, { "RI", 12 },
        };

        private static readonly Dictionary<string, LayoutInfo> LayoutCercado = new Dictionary<string, LayoutInfo>
        {
            { "FI", new LayoutInfo(32, 0, 6, 35, 40) },
            { "CD", new LayoutInfo(32, 0, 6, 35, 40) },
            { "MT", new LayoutInfo(50, 0, 3, 45, 52) },
            { "PA", new LayoutInfo(32, 36, 6, 32, 38) },
            { "RS", new LayoutInfo(0, 0, 1, 55, 60) },
            { "IS", new LayoutInfo(0, 0, 1, 55, 60) },
            { "RI", new LayoutInfo(32, 36, 6, 32, 38) },
        };

        private class LayoutInfo
        {
            public int SpacingX, SpacingY, PerRow, SizeNormal, SizeGrande;
            public LayoutInfo(int sx, int sy, int pr, int sn, int sg)
            { SpacingX = sx; SpacingY = sy; PerRow = pr; SizeNormal = sn; SizeGrande = sg; }
        }

        private class ContextoJogo
        {
            public int TurnoAtual;
            public int TurnosRestantes;
            public int TurnosRestantesNaRodada;
            public bool Rodada2;
            public int NumeroJogadores;
            public List<int> IdsOponentesEmOrdem;
            public Dictionary<int, Dictionary<string, List<string>>> TabuleirosOponentes;
            public Dictionary<string, int> MaoDinos;
        }

        public TelaJogo()
        {
            InitializeComponent();
            this.Load += TelaJogo_Load;
        }

        private void TelaJogo_Load(object sender, EventArgs e)
        {
            lblIDJogadorPrincipal.Text = IdJogadorPrincipal.ToString();
            lblKeyJogadorPrincipal.Text = SenhaJogadorPrincipal;
            AtualizarTurnoAtual();
            AtualizarTela();
        }

        private void AtualizarTurnoAtual()
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (RespostaInvalida(statusPartida)) return;
            string[] campos = statusPartida.Split(',');
            if (campos.Length >= 2 && int.TryParse(campos[1].Trim(), out int turno))
                lblTurno.Text = turno.ToString();
        }

        private void AtualizarTela()
        {
            CarregarJogadoresNaLista();
            AtualizarInfoDadoAtual();
            DesenharTabuleiroComDinos();
            CarregarMaoDaAPI();
            AtualizarHistorico();
        }

        private void btn_auto_Click(object sender, EventArgs e) => IniciarAutomacao();
        private void btnListar_Click(object sender, EventArgs e) => CarregarMaoDaAPI();
        private void btnAtualizarHistorico_Click(object sender, EventArgs e) => AtualizarTela();
        private void btnValidarTurno_Click(object sender, EventArgs e) => ExibirStatusTurnoAtual();

        private void btnJogar_Click(object sender, EventArgs e)
        {
            string dinossauro = CapitalizarSigla(txtDinossauro.Text.Trim());
            string cercado = txtCercado.Text.Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(dinossauro) || string.IsNullOrWhiteSpace(cercado))
            {
                MessageBox.Show("Informe o dinossauro e o cercado.");
                return;
            }
            string retorno = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dinossauro, cercado);
            if (RespostaInvalida(retorno))
            {
                MessageBox.Show(string.IsNullOrWhiteSpace(retorno) ? "Sem resposta do servidor." : retorno);
                return;
            }
            txtDinossauro.Text = "";
            AtualizarTela();
            ExibirStatusAposJogadaManual();
        }

        private void IniciarAutomacao()
        {
            if (_timerAutomacao != null && _timerAutomacao.Enabled)
            {
                MessageBox.Show("Automação já está em andamento!");
                return;
            }
            _ultimoTurnoProcessado = -1;
            _jaJogouNesteTurno = false;
            _timerAutomacao = new Timer { Interval = 3000 };
            _timerAutomacao.Tick += TimerAutomacao_Tick;
            _timerAutomacao.Start();
            ExibirStatus("Automação iniciada!", Color.Blue);
        }

        private void TimerAutomacao_Tick(object sender, EventArgs e)
        {
            if (_processandoTick) return;
            _processandoTick = true;
            try { ProcessarTick(); }
            finally { _processandoTick = false; }
        }

        private void ProcessarTick()
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (RespostaInvalida(statusPartida))
            {
                ExibirStatus("Erro ao verificar partida.", Color.Red);
                return;
            }
            string[] campos = statusPartida.Split(',');
            if (campos.Length < 5) return;

            string statusGeral = campos[0].Trim();
            if (!int.TryParse(campos[1].Trim(), out int turnoAtual)) return;
            string idJogadorComDado = campos[3].Trim();
            string faceDado = campos[4].Trim();

            lblTurno.Text = turnoAtual.ToString();

            if (statusGeral == "E")
            {
                _timerAutomacao.Stop();
                ExibirStatus("Partida encerrada!", Color.DarkGreen);
                AtualizarHistorico();
                MostrarPontuacaoFinal();
                return;
            }

            bool ehNovoTurno = _ultimoTurnoProcessado != turnoAtual;
            if (ehNovoTurno)
            {
                _ultimoTurnoProcessado = turnoAtual;
                _jaJogouNesteTurno = false;
            }

            if (_jaJogouNesteTurno)
            {
                ExibirStatus($"Turno {turnoAtual} | Aguardando próximo turno...", Color.Gray);
                return;
            }

            bool euRoleiODado = idJogadorComDado == IdJogadorPrincipal.ToString();
            string restricaoDado = euRoleiODado ? "livre" : faceDado;

            ExibirStatus($"Turno {turnoAtual} | Dado: {faceDado} | Calculando jogada...", Color.DarkBlue);
            ExecutarJogadaAutomatica(restricaoDado, turnoAtual);
        }

        private void ExecutarJogadaAutomatica(string restricaoDado, int turnoAtual)
        {
            CarregarMaoDaAPI();
            if (_siglasDinosNaMao.Count == 0)
            {
                ExibirStatus("Sem dinossauros na mão!", Color.Red);
                return;
            }

            var meuTabuleiro = ObterEstadoDoTabuleiro();
            var cercadosPermitidos = ObterCercadosPermitidosPeloDado(restricaoDado, meuTabuleiro);
            var contexto = ConstruirContexto(turnoAtual);

            var (melhorDino, melhorCercado) = EscolherMelhorJogada(cercadosPermitidos, meuTabuleiro, contexto);
            RealizarJogada(melhorDino, melhorCercado, cercadosPermitidos);
        }

        private ContextoJogo ConstruirContexto(int turnoAtual)
        {
            var idsOponentes = ObterIdsOponentesEmOrdem();
            var tabuleirosOpo = new Dictionary<int, Dictionary<string, List<string>>>();
            foreach (int id in idsOponentes)
                tabuleirosOpo[id] = ObterEstadoTabuleiroOponente(id);

            var maoDinos = new Dictionary<string, int>();
            foreach (string s in _siglasDinosNaMao)
            {
                string key = CapitalizarSigla(s);
                maoDinos[key] = maoDinos.ContainsKey(key) ? maoDinos[key] + 1 : 1;
            }

            int turnosNaRodada = turnoAtual <= 6 ? 6 - turnoAtual + 1 : 12 - turnoAtual + 1;

            return new ContextoJogo
            {
                TurnoAtual = turnoAtual,
                TurnosRestantes = Math.Max(0, TOTAL_TURNOS - turnoAtual + 1),
                TurnosRestantesNaRodada = Math.Max(0, turnosNaRodada),
                Rodada2 = turnoAtual > 6,
                NumeroJogadores = idsOponentes.Count + 1,
                IdsOponentesEmOrdem = idsOponentes,
                TabuleirosOponentes = tabuleirosOpo,
                MaoDinos = maoDinos,
            };
        }

        private List<int> ObterIdsOponentesEmOrdem()
        {
            var ordem = new List<int>();
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (RespostaInvalida(retorno)) return ordem;
            var ids = SplitLinhas(retorno)
                .Select(l => l.Split(','))
                .Where(p => p.Length >= 1 && int.TryParse(p[0].Trim(), out _))
                .Select(p => int.Parse(p[0].Trim()))
                .ToList();
            int minhaPos = ids.IndexOf(IdJogadorPrincipal);
            if (minhaPos < 0 || ids.Count <= 1) return ordem;
            for (int i = 1; i < ids.Count; i++)
                ordem.Add(ids[(minhaPos + i) % ids.Count]);
            return ordem;
        }

        private (string dino, string cercado) EscolherMelhorJogada(
            List<string> cercadosPermitidos,
            Dictionary<string, List<string>> meuTabuleiro,
            ContextoJogo contexto)
        {
            string melhorDino = null;
            string melhorCercado = "RI";
            double melhorPontuacaoTotal = double.MinValue;

            var todasAsOpcoes = cercadosPermitidos.Union(new[] { "RI" }).ToList();
            var siglasUnicas = _siglasDinosNaMao.Distinct().ToList();

            double multFechamento = MultiplicadorFechamento(contexto.TurnosRestantes);

            foreach (string dino in siglasUnicas)
            {
                double ganhoCedidoOponentes = CalcularNegacaoMultiOponente(dino, contexto);
                double penaltyDoacao = CalcularPenaltyDoacao(dino, contexto);
                double fatorEscassez = FatorEscassezNaMao(dino, contexto);

                foreach (string cercado in todasAsOpcoes)
                {
                    double meuGanhoImediato = GanhoMarginalReal(dino, cercado, meuTabuleiro, contexto);
                    if (meuGanhoImediato < 0) continue;

                    double potencialFuturo = EstimarPotencialFuturo(dino, cercado, meuTabuleiro, contexto);
                    double bonusContinuidade = BonusContinuidade(dino, cercado, meuTabuleiro);
                    double penaltyIS = PenaltyISCedo(cercado, contexto);
                    double penaltyAbrirTarde = PenaltyAbrirComboTarde(dino, cercado, meuTabuleiro, contexto);
                    double bonusFecharR2 = BonusFecharComboR2(dino, cercado, meuTabuleiro, contexto);

                    double ganhoEfetivo = meuGanhoImediato * multFechamento * fatorEscassez;

                    double pontuacaoTotal = ganhoEfetivo
                                          + PESO_FUTURO_PROPRIO * potencialFuturo
                                          + bonusContinuidade
                                          + bonusFecharR2
                                          - penaltyIS
                                          - penaltyAbrirTarde
                                          - PESO_NEGACAO_OPONENTE * ganhoCedidoOponentes
                                          - PESO_ANTI_DOACAO * penaltyDoacao;

                    if (pontuacaoTotal > melhorPontuacaoTotal)
                    {
                        melhorPontuacaoTotal = pontuacaoTotal;
                        melhorDino = CapitalizarSigla(dino);
                        melhorCercado = cercado;
                    }
                }
            }

            if (melhorCercado == "RI" && cercadosPermitidos.Count > 0)
            {
                string especieIS = ObterEspecieNaIlhaSolitaria(meuTabuleiro);
                foreach (string cercado in new[] { "CD", "MT", "FI", "PA", "RS", "IS" }.Where(cercadosPermitidos.Contains))
                {
                    foreach (string dino in siglasUnicas)
                    {
                        string dinoApi = CapitalizarSigla(dino);
                        if (especieIS != null && dinoApi == especieIS && cercado != "IS") continue;
                        return (dinoApi, cercado);
                    }
                }
            }

            if (melhorDino == null)
                melhorDino = CapitalizarSigla(_siglasDinosNaMao[0]);

            return (melhorDino, melhorCercado);
        }

        private double MultiplicadorFechamento(int turnosRestantes)
        {
            if (turnosRestantes >= 6) return 1.0;
            double progresso = (6 - turnosRestantes) / 6.0;
            return 1.0 + (PESO_FECHAMENTO_TURNO_FINAL - 1.0) * progresso;
        }

        private double PenaltyISCedo(string cercado, ContextoJogo contexto)
        {
            if (cercado != "IS") return 0;
            double turnosRestantes = contexto.TurnosRestantes;
            if (turnosRestantes <= 3) return 0;
            return PESO_IS_PENALTY_CEDO * (turnosRestantes / 12.0);
        }

        // Dino único na mão = boost (não vai voltar). Várias cópias = sem pressa. Escala com nº jogadores.
        private double FatorEscassezNaMao(string dino, ContextoJogo contexto)
        {
            string dinoApi = CapitalizarSigla(dino);
            if (contexto?.MaoDinos == null || !contexto.MaoDinos.TryGetValue(dinoApi, out int qtd) || qtd <= 0)
                return 1.0;

            double fatorJogadores = 1.0 + (contexto.NumeroJogadores - 2) * 0.1;
            if (qtd == 1) return PESO_ESCASSEZ_UNICO * fatorJogadores;
            if (qtd == 2) return 1.0;
            return 0.85;
        }

        // Penalty pra abrir FI/CD com 1 dino quando faltam poucos turnos na rodada (não vai fechar).
        private double PenaltyAbrirComboTarde(string dino, string cercado, Dictionary<string, List<string>> tabuleiro, ContextoJogo contexto)
        {
            if (cercado != "FI" && cercado != "CD") return 0;
            var atual = tabuleiro.ContainsKey(cercado) ? tabuleiro[cercado] : new List<string>();
            if (atual.Count > 0) return 0;
            int turnosNaRodada = contexto.TurnosRestantesNaRodada;
            if (turnosNaRodada >= 5) return 0;
            if (turnosNaRodada >= 3) return PENALTY_ABRIR_TARDE * 0.4;
            return PENALTY_ABRIR_TARDE;
        }

        // Em R2, bônus extra pra fechar/avançar combos do R1. Mais peso conforme combo já está cheio.
        private double BonusFecharComboR2(string dino, string cercado, Dictionary<string, List<string>> tabuleiro, ContextoJogo contexto)
        {
            if (!contexto.Rodada2) return 0;
            string dinoApi = CapitalizarSigla(dino);
            var atual = tabuleiro.ContainsKey(cercado) ? tabuleiro[cercado] : new List<string>();

            if (cercado == "FI" && atual.Count >= 3 && atual[0] == dinoApi)
                return BONUS_FECHAR_COMBO_R2 * (atual.Count / 6.0);
            if (cercado == "CD" && atual.Count >= 3 && !atual.Contains(dinoApi))
                return BONUS_FECHAR_COMBO_R2 * (atual.Count / 6.0);
            if (cercado == "MT" && atual.Count == 2)
                return BONUS_FECHAR_COMBO_R2;
            if (cercado == "PA" && atual.Count > 0 && atual.Contains(dinoApi))
                return BONUS_FECHAR_COMBO_R2 * 0.6;
            return 0;
        }

        // Detecta "doação" indireta: passar dinos que o próximo oponente vai usar muito bem.
        private double CalcularPenaltyDoacao(string dinoAJogar, ContextoJogo contexto)
        {
            if (contexto?.IdsOponentesEmOrdem == null || contexto.IdsOponentesEmOrdem.Count == 0) return 0;
            if (contexto.MaoDinos == null) return 0;

            int idProximo = contexto.IdsOponentesEmOrdem[0];
            if (!contexto.TabuleirosOponentes.TryGetValue(idProximo, out var tabProx)) return 0;

            double doacaoTotal = 0;
            foreach (var kv in contexto.MaoDinos)
            {
                string dinoNaMao = kv.Key;
                int qtdRestante = kv.Value - (dinoNaMao == CapitalizarSigla(dinoAJogar) ? 1 : 0);
                if (qtdRestante <= 0) continue;
                double valorParaOponente = MelhorValorOponenteExtrai(dinoNaMao, tabProx, contexto);
                doacaoTotal += qtdRestante * valorParaOponente * 0.3;
            }
            return doacaoTotal;
        }

        private double BonusContinuidade(string dino, string cercado, Dictionary<string, List<string>> tabuleiro)
        {
            string dinoApi = CapitalizarSigla(dino);
            if (cercado == "FI" && tabuleiro.ContainsKey("FI") && tabuleiro["FI"].Count > 0 && tabuleiro["FI"][0] == dinoApi)
                return 2.0;
            if (cercado == "PA" && tabuleiro.ContainsKey("PA"))
            {
                int casaisExistentes = tabuleiro["PA"].GroupBy(d => d).Sum(g => g.Count() / 2);
                int countDessa = tabuleiro["PA"].Count(d => d == dinoApi);
                if (countDessa % 2 == 1) return 3.0;
                if (countDessa >= 2) return 1.0;
                if (casaisExistentes > 0) return 0.5;
            }
            return 0;
        }

        // Ganho marginal real considerando RS/IS/T-Rex com contexto multi-jogador.
        private double GanhoMarginalReal(string dino, string cercado, Dictionary<string, List<string>> tabuleiro, ContextoJogo contexto)
        {
            string dinoApi = CapitalizarSigla(dino);

            if (!PodeColocar(dinoApi, cercado, tabuleiro)) return -1;

            int pontosAntes = PontuarTabuleiroAfetadoPorCercado(cercado, tabuleiro, contexto);
            var copia = ClonarTabuleiro(tabuleiro);
            if (!copia.ContainsKey(cercado)) copia[cercado] = new List<string>();
            copia[cercado].Add(dinoApi);
            int pontosDepois = PontuarTabuleiroAfetadoPorCercado(cercado, copia, contexto);

            double bonusTrex = 0;
            if (dinoApi == "Ti" && cercado != "RI")
            {
                bool tinhaTrex = tabuleiro.ContainsKey(cercado) && tabuleiro[cercado].Contains("Ti");
                if (!tinhaTrex) bonusTrex = BONUS_TREX_POR_CERCADO;
                else bonusTrex = -0.5;
            }

            return Math.Max(0, pontosDepois - pontosAntes) + bonusTrex;
        }

        // Soma pontos do cercado + impacto colateral (RS pode mudar quando coloco em outro cercado).
        private int PontuarTabuleiroAfetadoPorCercado(string cercadoAfetado, Dictionary<string, List<string>> tab, ContextoJogo contexto)
        {
            int total = PontuarCercadoComContexto(cercadoAfetado, tab, contexto);
            if (cercadoAfetado != "RS" && tab.ContainsKey("RS") && tab["RS"].Count > 0)
                total += PontuarCercadoComContexto("RS", tab, contexto);
            if (cercadoAfetado != "IS" && tab.ContainsKey("IS") && tab["IS"].Count > 0)
                total += PontuarCercadoComContexto("IS", tab, contexto);
            return total;
        }

        // Versão completa: RS/IS olham os oponentes pra saber se vale 7 pts ou 0.
        private int PontuarCercadoComContexto(string cercado, Dictionary<string, List<string>> tabuleiro, ContextoJogo contexto)
        {
            if (cercado == "RS")
            {
                if (!tabuleiro.ContainsKey("RS") || tabuleiro["RS"].Count == 0) return 0;
                string especie = tabuleiro["RS"][0];
                int minhaQtd = ContarEspecieNoTabuleiro(tabuleiro, especie);
                int maxOpoQtd = 0;
                if (contexto?.TabuleirosOponentes != null)
                    foreach (var t in contexto.TabuleirosOponentes.Values)
                        maxOpoQtd = Math.Max(maxOpoQtd, ContarEspecieNoTabuleiro(t, especie));
                return minhaQtd >= maxOpoQtd ? PONTOS_RS_VITORIA : 0;
            }
            if (cercado == "IS")
            {
                if (!tabuleiro.ContainsKey("IS") || tabuleiro["IS"].Count == 0) return 0;
                string especie = tabuleiro["IS"][0];
                return EspecieExisteForaDaIlhaSolitaria(especie, tabuleiro) ? 0 : PONTOS_IS_VITORIA;
            }
            return PontuarCercadoLocal(cercado, tabuleiro);
        }

        // Heurística do quanto a jogada "prepara" pontuação futura no mesmo cercado.
        private double EstimarPotencialFuturo(string dino, string cercado, Dictionary<string, List<string>> tabuleiro, ContextoJogo contexto)
        {
            string dinoApi = CapitalizarSigla(dino);
            var atual = tabuleiro.ContainsKey(cercado) ? tabuleiro[cercado] : new List<string>();
            double escalaTurno = contexto.TurnosRestantes <= 2 ? 0.3 : 1.0;

            switch (cercado)
            {
                case "FI":
                    if (atual.Count == 0) return contexto.Rodada2 ? 1.5 : 3;
                    if (atual[0] == dinoApi)
                        return (PONTOS_FI[Math.Min(atual.Count + 2, 6)] - PONTOS_FI[atual.Count + 1]) * escalaTurno;
                    return 0;
                case "CD":
                    if (atual.Count == 0) return contexto.Rodada2 ? 1.0 : 2;
                    if (!atual.Contains(dinoApi))
                        return (PONTOS_CD[Math.Min(atual.Count + 2, 6)] - PONTOS_CD[atual.Count + 1]) * escalaTurno;
                    return 0;
                case "MT":
                    if (atual.Count < 3) return (atual.Count == 2 ? 4 : 1) * escalaTurno;
                    return 0;
                case "PA":
                    if (atual.Count > 0 && atual[atual.Count - 1] == dinoApi) return 0;
                    return (atual.Count % 2 == 0 ? 1.5 : 0) * escalaTurno;
                default:
                    return 0;
            }
        }

        private bool PodeColocar(string dinoApi, string cercado, Dictionary<string, List<string>> tabuleiro)
        {
            var dinosNoCercado = tabuleiro.ContainsKey(cercado) ? tabuleiro[cercado] : new List<string>();
            int capacidade = CapacidadeDeCadaCercado.ContainsKey(cercado) ? CapacidadeDeCadaCercado[cercado] : 6;

            if (cercado != "RI" && dinosNoCercado.Count >= capacidade) return false;

            if (cercado == "IS")
            {
                if (dinosNoCercado.Count > 0) return false;
                if (EspecieExisteForaDaIlhaSolitaria(dinoApi, tabuleiro)) return false;
            }

            string especieIS = ObterEspecieNaIlhaSolitaria(tabuleiro);
            if (especieIS != null && dinoApi == especieIS && cercado != "IS") return false;

            if (cercado == "FI" && dinosNoCercado.Count > 0 && dinosNoCercado[0] != dinoApi) return false;
            if (cercado == "CD" && dinosNoCercado.Contains(dinoApi)) return false;

            return true;
        }

        // Pontos de um cercado isolado (sem bônus T-Rex; RS/IS contam otimistas).
        private int PontuarCercadoLocal(string cercado, Dictionary<string, List<string>> tabuleiro)
        {
            if (!tabuleiro.ContainsKey(cercado)) return 0;
            var dinos = tabuleiro[cercado];
            if (dinos.Count == 0) return 0;

            switch (cercado)
            {
                case "FI":
                    return PONTOS_FI[Math.Min(dinos.Count, 6)];
                case "CD":
                    return PONTOS_CD[Math.Min(dinos.Count, 6)];
                case "PA":
                    var contagem = dinos.GroupBy(d => d).Select(g => g.Count() / 2).Sum();
                    return contagem * PONTOS_POR_CASAL;
                case "MT":
                    return dinos.Count == 3 ? PONTOS_MT_COMPLETA : 0;
                case "RS":
                    return dinos.Count > 0 ? PONTOS_RS_VITORIA : 0;
                case "IS":
                    return dinos.Count > 0 ? PONTOS_IS_VITORIA : 0;
                case "RI":
                    return dinos.Count * PONTOS_POR_DINO_RIO;
                default:
                    return 0;
            }
        }

        // Negação ponderada: cada oponente da rodada recebe peso decrescente conforme distância da minha vez.
        private double CalcularNegacaoMultiOponente(string dinoQueSereiJogado, ContextoJogo contexto)
        {
            if (contexto?.IdsOponentesEmOrdem == null || contexto.IdsOponentesEmOrdem.Count == 0) return 0;
            var maoCedida = MaoSemUmDino(dinoQueSereiJogado);
            if (maoCedida.Count == 0) return 0;

            double total = 0;
            for (int i = 0; i < contexto.IdsOponentesEmOrdem.Count; i++)
            {
                int idOpo = contexto.IdsOponentesEmOrdem[i];
                if (!contexto.TabuleirosOponentes.TryGetValue(idOpo, out var tabOpo) || tabOpo.Count == 0) continue;
                double peso = i < PESOS_NEGACAO_POR_DISTANCIA.Length ? PESOS_NEGACAO_POR_DISTANCIA[i] : 0.03;
                double melhorDoOpo = maoCedida.Max(dino => MelhorValorOponenteExtrai(dino, tabOpo, contexto));
                total += peso * melhorDoOpo;
            }
            return total;
        }

        private double MelhorValorOponenteExtrai(string dino, Dictionary<string, List<string>> tabuleiroOponente, ContextoJogo contexto)
        {
            var todosCercados = new[] { "CD", "FI", "MT", "PA", "RS", "IS", "RI" };
            return todosCercados.Max(cercado => Math.Max(0, GanhoMarginalReal(dino, cercado, tabuleiroOponente, contexto)));
        }

        private Dictionary<string, List<string>> ClonarTabuleiro(Dictionary<string, List<string>> origem)
        {
            var copia = new Dictionary<string, List<string>>();
            foreach (var kv in origem) copia[kv.Key] = new List<string>(kv.Value);
            return copia;
        }

        private void RealizarJogada(string dino, string cercado, List<string> cercadosPermitidos)
        {
            ExibirStatus($"Jogando {dino} em {cercado}", Color.DarkBlue);
            this.Refresh();

            string retorno = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dino, cercado);
            if (!RespostaInvalida(retorno))
            {
                _jaJogouNesteTurno = true;
                ExibirStatus($"{dino} → {cercado} | OK", Color.Green);
                AtualizarTela();
                return;
            }

            foreach (string alternativa in cercadosPermitidos.Where(c => c != cercado))
            {
                string retAlt = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dino, alternativa);
                if (!RespostaInvalida(retAlt))
                {
                    _jaJogouNesteTurno = true;
                    ExibirStatus($"{dino} → {alternativa} (alternativa) | OK", Color.Orange);
                    AtualizarTela();
                    return;
                }
            }

            ExibirStatus("Todos falharam, tentando Rio...", Color.OrangeRed);
            this.Refresh();

            string retornoRio = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dino, "RI");
            if (!RespostaInvalida(retornoRio))
            {
                _jaJogouNesteTurno = true;
                ExibirStatus($"{dino} → RI (último recurso) | OK", Color.SteelBlue);
                AtualizarTela();
            }
            else
            {
                ExibirStatus($"Rio também falhou: {retornoRio}", Color.Red);
            }
        }

        private List<string> ObterCercadosPermitidosPeloDado(string restricaoDado, Dictionary<string, List<string>> meuTabuleiro)
        {
            switch (restricaoDado)
            {
                case "livre": return new List<string> { "CD", "FI", "MT", "PA", "RS", "IS" };
                case "FL": return new List<string> { "FI", "MT" };
                case "PR": return new List<string> { "PA", "CD" };
                case "WC": return new List<string> { "IS", "CD" };
                case "AL": return new List<string> { "FI", "CD" };
                case "VZ": return ObterCercadosVazios(meuTabuleiro);
                case "TI": return ObterCercadosSemTRex(meuTabuleiro);
                default: return new List<string> { "CD", "FI", "MT", "PA", "RS", "IS" };
            }
        }

        private List<string> ObterCercadosVazios(Dictionary<string, List<string>> meuTabuleiro)
        {
            var cercadosOcupados = meuTabuleiro.Keys.Where(c => meuTabuleiro[c].Count > 0).ToHashSet();
            return new[] { "CD", "FI", "MT", "PA", "RS", "IS" }
                .Where(c => !cercadosOcupados.Contains(c))
                .ToList();
        }

        private List<string> ObterCercadosSemTRex(Dictionary<string, List<string>> meuTabuleiro)
        {
            var cercadosComTRex = meuTabuleiro
                .Where(par => par.Value.Contains("Ti"))
                .Select(par => par.Key)
                .ToHashSet();
            return new[] { "CD", "FI", "MT", "PA", "RS", "IS" }
                .Where(c => !cercadosComTRex.Contains(c))
                .ToList();
        }

        private Dictionary<string, List<string>> ObterEstadoTabuleiroDe(int idJogador, string senha)
        {
            var estado = new Dictionary<string, List<string>>();
            string tabuleiro = Jogo.ExibirTabuleiro(idJogador, senha);
            if (RespostaInvalida(tabuleiro)) return estado;
            foreach (string linha in SplitLinhas(tabuleiro))
            {
                string[] partes = linha.Split(',');
                if (partes.Length < 3) continue;
                string c = partes[0].Trim(); string d = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int q)) continue;
                if (!estado.ContainsKey(c)) estado[c] = new List<string>();
                for (int i = 0; i < q; i++) estado[c].Add(d);
            }
            return estado;
        }

        private Dictionary<string, List<string>> ObterEstadoTabuleiroOponente(int idOponente)
            => ObterEstadoTabuleiroDe(idOponente, string.Empty);

        private Dictionary<string, List<string>> ObterEstadoDoTabuleiro()
            => ObterEstadoTabuleiroDe(IdJogadorPrincipal, SenhaJogadorPrincipal);

        private string ObterEspecieNaIlhaSolitaria(Dictionary<string, List<string>> tabuleiro)
        {
            if (!tabuleiro.ContainsKey("IS") || tabuleiro["IS"].Count == 0) return null;
            return tabuleiro["IS"][0];
        }

        private bool EspecieExisteForaDaIlhaSolitaria(string dino, Dictionary<string, List<string>> tabuleiro)
        {
            return tabuleiro.Where(par => par.Key != "IS").Any(par => par.Value.Contains(dino));
        }

        private void DesenharTabuleiroComDinos()
        {
            LimparDinosDoTabuleiro();
            string retorno = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);
            if (RespostaInvalida(retorno)) return;
            var contadorPorCercado = new Dictionary<string, int>();
            foreach (string linha in SplitLinhas(retorno))
            {
                string[] partes = linha.Split(',');
                if (partes.Length < 3) continue;
                string cercado = partes[0].Trim(); string dino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int quantidade)) continue;
                if (!PosicaoDeCadaCercado.ContainsKey(cercado)) continue;
                Image imagem = ObterImagemDinossauro(dino);
                if (imagem == null) continue;
                if (!contadorPorCercado.ContainsKey(cercado)) contadorPorCercado[cercado] = 0;
                int capacidade = CapacidadeDeCadaCercado.ContainsKey(cercado) ? CapacidadeDeCadaCercado[cercado] : 6;
                for (int i = 0; i < quantidade; i++)
                {
                    if (contadorPorCercado[cercado] >= capacidade) break;
                    AdicionarDinoNoTabuleiro(cercado, dino, RotacionarEsquerda(imagem), contadorPorCercado[cercado]);
                    contadorPorCercado[cercado]++;
                }
            }
        }

        private void AdicionarDinoNoTabuleiro(string cercado, string sigla, Image imagem, int slot)
        {
            Point posicaoBase = PosicaoDeCadaCercado[cercado];
            LayoutInfo layout = LayoutCercado.ContainsKey(cercado) ? LayoutCercado[cercado] : new LayoutInfo(30, 0, 6, 40, 50);

            bool dinoGrande = (sigla == "Ti" || sigla == "Tr");
            int tamanho = dinoGrande ? layout.SizeGrande : layout.SizeNormal;

            int coluna = layout.PerRow > 0 ? slot % layout.PerRow : 0;
            int linha = layout.PerRow > 0 ? slot / layout.PerRow : 0;

            int offsetY = dinoGrande && layout.SpacingY == 0 ? -5 : 0;

            var pb = new PictureBox
            {
                Image = imagem,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Width = tamanho,
                Height = tamanho,
                Left = posicaoBase.X - picboxTabuleiro.Left + coluna * layout.SpacingX,
                Top = posicaoBase.Y - picboxTabuleiro.Top + linha * layout.SpacingY + offsetY,
            };
            picboxTabuleiro.Controls.Add(pb);
            pb.BringToFront();
            _dinosNoTabuleiro.Add(pb);
        }

        private void CarregarMaoDaAPI()
        {
            LimparMao();
            string retorno = Jogo.ExibirMao(IdJogadorPrincipal, SenhaJogadorPrincipal);
            if (RespostaInvalida(retorno)) return;
            bool pulaPrimeiraLinha = true;
            foreach (string linha in SplitLinhas(retorno))
            {
                if (pulaPrimeiraLinha) { pulaPrimeiraLinha = false; continue; }
                string[] partes = linha.Split(',');
                if (partes.Length < 2) continue;
                string sigla = partes[0].Trim();
                if (!int.TryParse(partes[1].Trim(), out int quantidade)) continue;
                for (int i = 0; i < quantidade; i++) AdicionarDinoNaMao(sigla);
            }
            lstDinossauros.Items.Clear();
            foreach (string sigla in _siglasDinosNaMao) lstDinossauros.Items.Add(sigla);
        }

        private void AdicionarDinoNaMao(string sigla)
        {
            Image imagem = ObterImagemDinossauro(sigla);
            if (imagem == null) return;
            int posicaoVertical = _maoPictureBoxes.Count;
            var pb = new PictureBox
            {
                Image = imagem,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 50,
                Height = 50,
                Left = picboxTabuleiro.Left + picboxTabuleiro.Width + 12,
                Top = picboxTabuleiro.Top + posicaoVertical * 58,
                Tag = sigla,
            };
            pb.Click += (s, ev) => txtDinossauro.Text = ((PictureBox)s).Tag.ToString();
            this.Controls.Add(pb);
            pb.BringToFront();
            _maoPictureBoxes.Add(pb);
            _siglasDinosNaMao.Add(sigla);
        }

        private void CarregarJogadoresNaLista()
        {
            lstJogadoresPartida.Items.Clear();
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (RespostaInvalida(retorno)) return;
            foreach (string linha in SplitLinhas(retorno)) lstJogadoresPartida.Items.Add(linha);
        }

        private void AtualizarInfoDadoAtual()
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (RespostaInvalida(statusPartida)) return;
            string[] campos = statusPartida.Split(',');
            if (campos.Length < 5) return;
            string faceDado = campos[4].Trim();
            if (_picDado == null)
            {
                _picDado = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Width = 50,
                    Height = 50,
                    Left = this.ClientSize.Width - 62,
                    Top = this.ClientSize.Height - 62,
                };
                this.Controls.Add(_picDado);
                _picDado.BringToFront();
            }
            _picDado.Image = ObterImagemDado(faceDado);
        }

        private void AtualizarHistorico()
        {
            string historico = Jogo.ListarHistorico(IdPartida);
            if (RespostaInvalida(historico)) return;
            lstHistorico.Items.Clear();
            foreach (string linha in SplitLinhas(historico)) lstHistorico.Items.Add(linha);
            if (lstHistorico.Items.Count > 0) lstHistorico.TopIndex = lstHistorico.Items.Count - 1;
        }

        private void ExibirStatusTurnoAtual()
        {
            string statusTurno = Jogo.VerificarTurno(IdPartida);
            if (RespostaInvalida(statusTurno))
            {
                MessageBox.Show(string.IsNullOrWhiteSpace(statusTurno) ? "Erro ao verificar turno." : statusTurno);
                return;
            }
            string textoFormatado = statusTurno.Trim();
            foreach (string face in new[] { "AL", "FL", "PR", "TI", "VZ", "WC" })
            {
                int pos = textoFormatado.IndexOf(face);
                if (pos != -1 && pos + face.Length < textoFormatado.Length)
                {
                    textoFormatado = textoFormatado.Insert(pos + face.Length, "\n");
                    break;
                }
            }
            lstVerficarTurno.Items.Clear();
            foreach (string linha in textoFormatado.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha)) lstVerficarTurno.Items.Add(linha.Trim());
        }

        private void ExibirStatusAposJogadaManual()
        {
            string statusPartida = Jogo.VerificarPartida(IdPartida);
            if (RespostaInvalida(statusPartida)) return;
            string[] campos = statusPartida.Split(',');
            if (campos.Length < 3) return;
            if (campos[2].Trim() == "F")
                ExibirStatus("Turno finalizado!", Color.Green);
            else
                ExibirStatus("Jogada registrada. Aguardando outros jogadores...", Color.DarkOrange);
        }

        private void btnExibirPontuacao_Click(object sender, EventArgs e)
        {
            PreencherListaPontuacao();
        }

        private void PreencherListaPontuacao()
        {
            lstPontuacao.Items.Clear();

            bool partidaEncerrada = PartidaEstaEncerrada();
            lstPontuacao.Items.Add(partidaEncerrada
                ? "=== PONTUAÇÃO FINAL (oficial) ==="
                : "=== PONTUAÇÃO PARCIAL (estimada) ===");
            lstPontuacao.Items.Add("");

            var jogadores = ObterListaJogadores();
            if (jogadores.Count == 0)
            {
                lstPontuacao.Items.Add("Sem jogadores na partida.");
                return;
            }

            if (partidaEncerrada)
            {
                var rankingOficial = jogadores
                    .OrderByDescending(j => j.pontos)
                    .ToList();

                int pos = 1;
                foreach (var j in rankingOficial)
                {
                    string nomeMostrar = string.IsNullOrWhiteSpace(j.nome) ? $"ID {j.id}" : j.nome;
                    lstPontuacao.Items.Add($"{pos}º {nomeMostrar,-15} {j.pontos,3} pts");
                    pos++;
                }
                return;
            }

            var tabuleiros = new Dictionary<int, Dictionary<string, List<string>>>();
            foreach (var j in jogadores)
            {
                tabuleiros[j.id] = j.id == IdJogadorPrincipal
                    ? ObterEstadoTabuleiroDe(j.id, SenhaJogadorPrincipal)
                    : ObterEstadoTabuleiroOponente(j.id);
            }

            var ranking = new List<(int id, string nome, int pontos, Dictionary<string, int> detalhe)>();
            foreach (var j in jogadores)
            {
                var detalhe = CalcularPontuacaoDetalhada(tabuleiros[j.id], j.id, tabuleiros);
                int total = detalhe.Values.Sum();
                ranking.Add((j.id, j.nome, total, detalhe));
            }

            ranking = ranking.OrderByDescending(r => r.pontos).ToList();

            int posicao = 1;
            foreach (var r in ranking)
            {
                string nomeMostrar = string.IsNullOrWhiteSpace(r.nome) ? $"ID {r.id}" : r.nome;
                lstPontuacao.Items.Add($"{posicao}º {nomeMostrar,-15} {r.pontos,3} pts");
                lstPontuacao.Items.Add($"   FI:{r.detalhe["FI"]} CD:{r.detalhe["CD"]} PA:{r.detalhe["PA"]}");
                lstPontuacao.Items.Add($"   MT:{r.detalhe["MT"]} RS:{r.detalhe["RS"]} IS:{r.detalhe["IS"]}");
                lstPontuacao.Items.Add($"   RIO:{r.detalhe["RI"]} TREX:{r.detalhe["TREX"]}");
                lstPontuacao.Items.Add("");
                posicao++;
            }
        }

        private bool PartidaEstaEncerrada()
        {
            string status = Jogo.VerificarPartida(IdPartida);
            if (RespostaInvalida(status)) return false;
            var campos = status.Split(',');
            return campos.Length > 0 && campos[0].Trim() == "E";
        }

        private void MostrarPontuacaoFinal()
        {
            PreencherListaPontuacao();
            lstPontuacao.Items.Insert(1, "*** PARTIDA ENCERRADA ***");
        }

        private List<(int id, string nome, int pontos)> ObterListaJogadores()
        {
            var resultado = new List<(int, string, int)>();
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (RespostaInvalida(retorno)) return resultado;
            foreach (string linha in SplitLinhas(retorno))
            {
                var partes = linha.Split(',');
                if (partes.Length < 1) continue;
                if (!int.TryParse(partes[0].Trim(), out int id)) continue;
                string nome = partes.Length >= 2 ? partes[1].Trim() : $"Jogador {id}";
                int pontos = 0;
                if (partes.Length >= 3) int.TryParse(partes[2].Trim(), out pontos);
                resultado.Add((id, nome, pontos));
            }
            return resultado;
        }

        private Dictionary<string, int> CalcularPontuacaoDetalhada(
            Dictionary<string, List<string>> meuTab,
            int meuId,
            Dictionary<int, Dictionary<string, List<string>>> todosTabuleiros)
        {
            var pontos = new Dictionary<string, int>
            {
                { "FI", 0 }, { "CD", 0 }, { "PA", 0 }, { "MT", 0 },
                { "RS", 0 }, { "IS", 0 }, { "RI", 0 }, { "TREX", 0 }
            };

            if (meuTab.ContainsKey("FI"))
                pontos["FI"] = PONTOS_FI[Math.Min(meuTab["FI"].Count, 6)];

            if (meuTab.ContainsKey("CD"))
                pontos["CD"] = PONTOS_CD[Math.Min(meuTab["CD"].Count, 6)];

            if (meuTab.ContainsKey("PA"))
            {
                int casais = meuTab["PA"].GroupBy(d => d).Select(g => g.Count() / 2).Sum();
                pontos["PA"] = casais * PONTOS_POR_CASAL;
            }

            if (meuTab.ContainsKey("MT") && meuTab["MT"].Count == 3)
                pontos["MT"] = PONTOS_MT_COMPLETA;

            if (meuTab.ContainsKey("RS") && meuTab["RS"].Count > 0)
            {
                string especieRS = meuTab["RS"][0];
                int minhaQtd = ContarEspecieNoTabuleiro(meuTab, especieRS);
                bool venci = todosTabuleiros
                    .Where(kv => kv.Key != meuId)
                    .All(kv => ContarEspecieNoTabuleiro(kv.Value, especieRS) <= minhaQtd);
                if (venci) pontos["RS"] = PONTOS_RS_VITORIA;
            }

            if (meuTab.ContainsKey("IS") && meuTab["IS"].Count > 0)
            {
                string especieIS = meuTab["IS"][0];
                bool unica = !EspecieExisteForaDaIlhaSolitaria(especieIS, meuTab);
                if (unica) pontos["IS"] = PONTOS_IS_VITORIA;
            }

            if (meuTab.ContainsKey("RI"))
                pontos["RI"] = meuTab["RI"].Count * PONTOS_POR_DINO_RIO;

            int bonusTrex = 0;
            foreach (var kv in meuTab)
            {
                if (kv.Key == "RI") continue;
                if (kv.Value.Contains("Ti")) bonusTrex += BONUS_TREX_POR_CERCADO;
            }
            pontos["TREX"] = bonusTrex;

            return pontos;
        }

        private int ContarEspecieNoTabuleiro(Dictionary<string, List<string>> tab, string especie)
        {
            return tab.Values.Sum(lista => lista.Count(d => d == especie));
        }

        private List<string> MaoSemUmDino(string dinoARemover)
        {
            var resultado = new List<string>(_siglasDinosNaMao);
            int indice = resultado.IndexOf(dinoARemover);
            if (indice >= 0) resultado.RemoveAt(indice);
            return resultado;
        }

        private void LimparDinosDoTabuleiro()
        {
            foreach (var pb in _dinosNoTabuleiro) { picboxTabuleiro.Controls.Remove(pb); pb.Dispose(); }
            _dinosNoTabuleiro.Clear();
        }

        private void LimparMao()
        {
            foreach (var pb in _maoPictureBoxes) { this.Controls.Remove(pb); pb.Dispose(); }
            _maoPictureBoxes.Clear();
            _siglasDinosNaMao.Clear();
        }

        private void ExibirStatus(string mensagem, Color cor)
        {
            lblStatusJogada.Text = mensagem;
            lblStatusJogada.ForeColor = cor;
        }

        private bool RespostaInvalida(string resposta) =>
            string.IsNullOrWhiteSpace(resposta) || resposta.StartsWith("ERRO");

        private IEnumerable<string> SplitLinhas(string texto) =>
            texto.Replace("\r", "").Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).Select(l => l.Trim());

        private string CapitalizarSigla(string sigla)
        {
            if (string.IsNullOrEmpty(sigla)) return sigla;
            return char.ToUpper(sigla[0]) + (sigla.Length > 1 ? sigla.Substring(1).ToLower() : "");
        }

        private Image RotacionarEsquerda(Image imagem)
        {
            Bitmap bitmap = new Bitmap(imagem);
            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
            return bitmap;
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
