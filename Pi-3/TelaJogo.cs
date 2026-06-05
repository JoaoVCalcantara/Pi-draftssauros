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

        private const double PESO_NEGACAO_OPONENTE = 0.4;
        private const int TAMANHO_DINO_MAO = 50;
        private const int TAMANHO_DINO_TABULEIRO = 45;
        private const int TAMANHO_DINO_GRANDE_TABULEIRO = 60;
        private const int ESPACAMENTO_MAO = 58;
        private const int MARGEM_MAO = 12;

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
            { "PA", 6 }, { "RS", 1 }, { "IS", 1 }, { "RI", 6 },
        };

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

        private void btnVerificarTabuleiro_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtIDJogadorTabuleiro.Text, out int idJogador))
            {
                MessageBox.Show("Informe um ID válido.");
                return;
            }

            string retorno = Jogo.ExibirTabuleiro(idJogador, SenhaJogadorPrincipal);

            if (RespostaInvalida(retorno))
            {
                MessageBox.Show(string.IsNullOrWhiteSpace(retorno) ? "Erro ao exibir tabuleiro." : retorno);
                return;
            }

            lstTabuleiro.Items.Clear();
            foreach (string linha in SplitLinhas(retorno))
                lstTabuleiro.Items.Add(linha);
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
                ExibirStatus($"Turno {turnoAtual} | Jogada feita, aguardando próximo turno...", Color.Gray);
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

            Dictionary<string, List<string>> tabuleiroProximoJogador = null;

            if (turnoAtual > 1)
            {
                int idProximo = ObterIdProximoJogador();
                if (idProximo > 0)
                    tabuleiroProximoJogador = ObterEstadoTabuleiroOponente(idProximo);
            }

            var (melhorDino, melhorCercado) = EscolherMelhorJogada(cercadosPermitidos, meuTabuleiro, tabuleiroProximoJogador);
            RealizarJogada(melhorDino, melhorCercado);
        }

        private (string dino, string cercado) EscolherMelhorJogada(
            List<string> cercadosPermitidos,
            Dictionary<string, List<string>> meuTabuleiro,
            Dictionary<string, List<string>> tabuleiroProximoJogador)
        {
            string melhorDino = null;
            string melhorCercado = "RI";
            double melhorPontuacaoTotal = double.MinValue;

            var todasAsOpcoes = cercadosPermitidos.Union(new[] { "RI" }).ToList();

            foreach (string dino in _siglasDinosNaMao.Distinct())
            {
                double ganhoProximoSePassarMaoSemEsseDino = CalcularGanhoProximoComMaoSemEsseDino(
                    dino, tabuleiroProximoJogador);

                foreach (string cercado in todasAsOpcoes)
                {
                    int meuGanho = EstimarPontuacaoDaJogada(dino, cercado, meuTabuleiro);
                    if (meuGanho < 0) continue;

                    double pontuacaoTotal = meuGanho - PESO_NEGACAO_OPONENTE * ganhoProximoSePassarMaoSemEsseDino;

                    if (pontuacaoTotal > melhorPontuacaoTotal)
                    {
                        melhorPontuacaoTotal = pontuacaoTotal;
                        melhorDino = CapitalizarSigla(dino);
                        melhorCercado = cercado;
                    }
                }
            }

            if (melhorDino == null)
                melhorDino = CapitalizarSigla(_siglasDinosNaMao[0]);

            return (melhorDino, melhorCercado);
        }

        private double CalcularGanhoProximoComMaoSemEsseDino(
            string dinoQueSereiJogado,
            Dictionary<string, List<string>> tabuleiroProximoJogador)
        {
            if (tabuleiroProximoJogador == null || tabuleiroProximoJogador.Count == 0)
                return 0;

            var maoCedidaAoProximo = MaoSemUmDino(dinoQueSereiJogado);
            return maoCedidaAoProximo.Sum(dino => EstimarMelhorValorQueOponenteExtrai(dino, tabuleiroProximoJogador));
        }

        private int EstimarMelhorValorQueOponenteExtrai(string dino, Dictionary<string, List<string>> tabuleiroOponente)
        {
            var todosCercados = new[] { "CD", "FI", "MT", "PA", "RS", "IS", "RI" };
            return todosCercados.Max(cercado => Math.Max(0, EstimarPontuacaoDaJogada(dino, cercado, tabuleiroOponente)));
        }

        private void RealizarJogada(string dino, string cercado)
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

            ExibirStatus($"Falhou ({retorno}), tentando Rio...", Color.OrangeRed);
            this.Refresh();

            string retornoRio = Jogo.Jogar(IdJogadorPrincipal, SenhaJogadorPrincipal, dino, "RI");

            if (!RespostaInvalida(retornoRio))
            {
                _jaJogouNesteTurno = true;
                ExibirStatus($"{dino} → RI (erro no cercado original) | OK", Color.SteelBlue);
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
                case "PR": return new List<string> { "PA", "MT" };
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

        private string ObterEspecieNaIlhaSolitaria(Dictionary<string, List<string>> tabuleiro)
        {
            if (!tabuleiro.ContainsKey("IS") || tabuleiro["IS"].Count == 0)
                return null;
            return tabuleiro["IS"][0];
        }

        private bool EspecieExisteForaDaIlhaSolitaria(string dino, Dictionary<string, List<string>> tabuleiro)
        {
            return tabuleiro
                .Where(par => par.Key != "IS")
                .Any(par => par.Value.Contains(dino));
        }

        private int EstimarPontuacaoDaJogada(string dino, string cercado, Dictionary<string, List<string>> tabuleiro)
        {
            string dinoApi = CapitalizarSigla(dino);
            var dinosNoCercado = tabuleiro.ContainsKey(cercado) ? tabuleiro[cercado] : new List<string>();

            string especieNaIlhaSolitaria = ObterEspecieNaIlhaSolitaria(tabuleiro);
            bool quebrariaIlhaSolitaria = especieNaIlhaSolitaria != null
                && dinoApi == especieNaIlhaSolitaria
                && cercado != "IS";
            if (quebrariaIlhaSolitaria) return -1;

            switch (cercado)
            {
                case "FI":
                    if (dinosNoCercado.Count >= 6) return -1;
                    if (dinosNoCercado.Count == 0) return 3;
                    return dinosNoCercado[0] == dinoApi ? 4 + dinosNoCercado.Count : -1;

                case "CD":
                    if (dinosNoCercado.Count >= 6) return -1;
                    return dinosNoCercado.Contains(dinoApi) ? -1 : 3 + dinosNoCercado.Count;

                case "PA":
                    if (dinosNoCercado.Count >= 6) return -1;
                    bool formaCarsal = dinosNoCercado.Count(d => d == dinoApi) % 2 == 1;
                    return formaCarsal ? 8 : 2;

                case "MT":
                    if (dinosNoCercado.Count >= 3) return -1;
                    if (dinosNoCercado.Count == 0) return 3;
                    if (dinosNoCercado.Count == 1) return 5;
                    return 9;

                case "RS":
                    if (dinosNoCercado.Count > 0) return -1;
                    return (dinoApi == "Ti" || dinoApi == "Tr") ? 6 : 4;

                case "IS":
                    if (dinosNoCercado.Count > 0) return -1;
                    if (EspecieExisteForaDaIlhaSolitaria(dinoApi, tabuleiro)) return -1;
                    return 7;

                case "RI":
                    return 1;

                default:
                    return 1;
            }
        }

        private int ObterIdProximoJogador()
        {
            string retorno = Jogo.ListarJogadores(IdPartida);
            if (RespostaInvalida(retorno)) return -1;

            var ids = SplitLinhas(retorno)
                .Select(l => l.Split(','))
                .Where(p => p.Length >= 1 && int.TryParse(p[0].Trim(), out _))
                .Select(p => int.Parse(p[0].Trim()))
                .ToList();

            int minhaPos = ids.IndexOf(IdJogadorPrincipal);
            if (minhaPos < 0 || ids.Count <= 1) return -1;

            return ids[(minhaPos + 1) % ids.Count];
        }

        private Dictionary<string, List<string>> ObterEstadoTabuleiroOponente(int idOponente)
        {
            var estado = new Dictionary<string, List<string>>();
            string tabuleiro = Jogo.ExibirTabuleiro(idOponente, string.Empty);
            if (RespostaInvalida(tabuleiro)) return estado;

            foreach (string linha in SplitLinhas(tabuleiro))
            {
                string[] partes = linha.Split(',');
                if (partes.Length < 3) continue;

                string cercado = partes[0].Trim();
                string dino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int quantidade)) continue;

                if (!estado.ContainsKey(cercado))
                    estado[cercado] = new List<string>();

                for (int i = 0; i < quantidade; i++)
                    estado[cercado].Add(dino);
            }

            return estado;
        }

        private Dictionary<string, List<string>> ObterEstadoDoTabuleiro()
        {
            var estado = new Dictionary<string, List<string>>();
            string tabuleiro = Jogo.ExibirTabuleiro(IdJogadorPrincipal, SenhaJogadorPrincipal);
            if (RespostaInvalida(tabuleiro)) return estado;

            foreach (string linha in SplitLinhas(tabuleiro))
            {
                string[] partes = linha.Split(',');
                if (partes.Length < 3) continue;

                string cercado = partes[0].Trim();
                string dino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int quantidade)) continue;

                if (!estado.ContainsKey(cercado))
                    estado[cercado] = new List<string>();

                for (int i = 0; i < quantidade; i++)
                    estado[cercado].Add(dino);
            }

            return estado;
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

                string cercado = partes[0].Trim();
                string dino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int quantidade)) continue;
                if (!PosicaoDeCadaCercado.ContainsKey(cercado)) continue;

                Image imagem = ObterImagemDinossauro(dino);
                if (imagem == null) continue;

                if (!contadorPorCercado.ContainsKey(cercado))
                    contadorPorCercado[cercado] = 0;

                int capacidade = CapacidadeDeCadaCercado.ContainsKey(cercado) ? CapacidadeDeCadaCercado[cercado] : 6;
                int tamanho = (dino == "Ti" || dino == "Tr") ? TAMANHO_DINO_GRANDE_TABULEIRO : TAMANHO_DINO_TABULEIRO;

                for (int i = 0; i < quantidade; i++)
                {
                    if (contadorPorCercado[cercado] >= capacidade) break;
                    AdicionarDinoNoTabuleiro(cercado, RotacionarEsquerda(imagem), tamanho, contadorPorCercado[cercado]);
                    contadorPorCercado[cercado]++;
                }
            }
        }

        private void AdicionarDinoNoTabuleiro(string cercado, Image imagem, int tamanho, int slot)
        {
            Point posicao = PosicaoDeCadaCercado[cercado];
            int espacamento = cercado == "MT" ? 22 : 30;

            var pb = new PictureBox
            {
                Image = imagem,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Width = tamanho,
                Height = tamanho,
                Left = posicao.X - picboxTabuleiro.Left + slot * espacamento,
                Top = posicao.Y - picboxTabuleiro.Top,
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

                for (int i = 0; i < quantidade; i++)
                    AdicionarDinoNaMao(sigla);
            }

            lstDinossauros.Items.Clear();
            foreach (string sigla in _siglasDinosNaMao)
                lstDinossauros.Items.Add(sigla);
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
                Width = TAMANHO_DINO_MAO,
                Height = TAMANHO_DINO_MAO,
                Left = picboxTabuleiro.Left + picboxTabuleiro.Width + MARGEM_MAO,
                Top = picboxTabuleiro.Top + posicaoVertical * ESPACAMENTO_MAO,
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

            foreach (string linha in SplitLinhas(retorno))
                lstJogadoresPartida.Items.Add(linha);
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
            foreach (string linha in SplitLinhas(historico))
                lstHistorico.Items.Add(linha);

            if (lstHistorico.Items.Count > 0)
                lstHistorico.TopIndex = lstHistorico.Items.Count - 1;
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
                int posicao = textoFormatado.IndexOf(face);
                if (posicao != -1 && posicao + face.Length < textoFormatado.Length)
                {
                    textoFormatado = textoFormatado.Insert(posicao + face.Length, "\n");
                    break;
                }
            }

            lstVerficarTurno.Items.Clear();
            foreach (string linha in textoFormatado.Split('\n'))
                if (!string.IsNullOrWhiteSpace(linha))
                    lstVerficarTurno.Items.Add(linha.Trim());
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

        private List<string> MaoSemUmDino(string dinoARemover)
        {
            var maoResultante = new List<string>(_siglasDinosNaMao);
            int indice = maoResultante.IndexOf(dinoARemover);
            if (indice >= 0) maoResultante.RemoveAt(indice);
            return maoResultante;
        }

        private void LimparDinosDoTabuleiro()
        {
            foreach (var pb in _dinosNoTabuleiro)
            {
                picboxTabuleiro.Controls.Remove(pb);
                pb.Dispose();
            }
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