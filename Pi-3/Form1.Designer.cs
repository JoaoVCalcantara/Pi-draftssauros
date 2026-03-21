namespace Pi_3
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_Test = new System.Windows.Forms.TextBox();
            this.lb_Jurassicos = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_criar_partida = new System.Windows.Forms.Button();
            this.btnJogadores = new System.Windows.Forms.Button();
            this.btnSelecionarPartida = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAtualizarPartidas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_Test
            // 
            this.txt_Test.Location = new System.Drawing.Point(12, 51);
            this.txt_Test.Multiline = true;
            this.txt_Test.Name = "txt_Test";
            this.txt_Test.Size = new System.Drawing.Size(209, 316);
            this.txt_Test.TabIndex = 1;
            // 
            // lb_Jurassicos
            // 
            this.lb_Jurassicos.AutoSize = true;
            this.lb_Jurassicos.Location = new System.Drawing.Point(691, 405);
            this.lb_Jurassicos.Name = "lb_Jurassicos";
            this.lb_Jurassicos.Size = new System.Drawing.Size(0, 13);
            this.lb_Jurassicos.TabIndex = 2;
            this.lb_Jurassicos.Click += new System.EventHandler(this.lb_Jurassicos_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(270, 51);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(209, 316);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(501, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(501, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(501, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(564, 51);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(209, 316);
            this.listBox2.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 405);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "label4";
            // 
            // btn_criar_partida
            // 
            this.btn_criar_partida.Location = new System.Drawing.Point(12, 12);
            this.btn_criar_partida.Name = "btn_criar_partida";
            this.btn_criar_partida.Size = new System.Drawing.Size(137, 33);
            this.btn_criar_partida.TabIndex = 9;
            this.btn_criar_partida.Text = "Criar partida";
            this.btn_criar_partida.UseVisualStyleBackColor = true;
            this.btn_criar_partida.Click += new System.EventHandler(this.btn_criar_partida_Click);
            // 
            // btnJogadores
            // 
            this.btnJogadores.Location = new System.Drawing.Point(302, 12);
            this.btnJogadores.Name = "btnJogadores";
            this.btnJogadores.Size = new System.Drawing.Size(137, 33);
            this.btnJogadores.TabIndex = 10;
            this.btnJogadores.Text = "Adicionar Jogadores";
            this.btnJogadores.UseVisualStyleBackColor = true;
            this.btnJogadores.Click += new System.EventHandler(this.btnJogadores_Click);
            // 
            // btnSelecionarPartida
            // 
            this.btnSelecionarPartida.Location = new System.Drawing.Point(507, 12);
            this.btnSelecionarPartida.Name = "btnSelecionarPartida";
            this.btnSelecionarPartida.Size = new System.Drawing.Size(137, 33);
            this.btnSelecionarPartida.TabIndex = 11;
            this.btnSelecionarPartida.Text = "Selecionar Partida";
            this.btnSelecionarPartida.UseVisualStyleBackColor = true;
            this.btnSelecionarPartida.Click += new System.EventHandler(this.btnSelecionarPartida_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(650, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(137, 33);
            this.btnCancelar.TabIndex = 12;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(697, 405);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "label5";
            // 
            // btnAtualizarPartidas
            // 
            this.btnAtualizarPartidas.Location = new System.Drawing.Point(155, 12);
            this.btnAtualizarPartidas.Name = "btnAtualizarPartidas";
            this.btnAtualizarPartidas.Size = new System.Drawing.Size(141, 33);
            this.btnAtualizarPartidas.TabIndex = 14;
            this.btnAtualizarPartidas.Text = "Atualizar partidas";
            this.btnAtualizarPartidas.UseVisualStyleBackColor = true;
            this.btnAtualizarPartidas.Click += new System.EventHandler(this.btnAtualizarPartidas_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAtualizarPartidas);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSelecionarPartida);
            this.Controls.Add(this.btnJogadores);
            this.Controls.Add(this.btn_criar_partida);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lb_Jurassicos);
            this.Controls.Add(this.txt_Test);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_Test;
        private System.Windows.Forms.Label lb_Jurassicos;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_criar_partida;
        private System.Windows.Forms.Button btnJogadores;
        private System.Windows.Forms.Button btnSelecionarPartida;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAtualizarPartidas;
    }
}

