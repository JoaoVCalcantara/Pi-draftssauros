namespace Pi_3
{
    partial class Lobby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.lbPartida = new System.Windows.Forms.Label();
            this.lbSenha = new System.Windows.Forms.Label();
            this.lbId = new System.Windows.Forms.Label();
            this.btnIniciarPartida = new System.Windows.Forms.Button();
            this.btnListar = new System.Windows.Forms.Button();
            this.lstDinossauros = new System.Windows.Forms.ListBox();
            this.lblRodada = new System.Windows.Forms.Label();
            this.lstVerficarTurno = new System.Windows.Forms.ListBox();
            this.btnJogar = new System.Windows.Forms.Button();
            this.txtDinossauro = new System.Windows.Forms.TextBox();
            this.txtCercado = new System.Windows.Forms.TextBox();
            this.lblDinossauro = new System.Windows.Forms.Label();
            this.lblCercado = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTabuleiro = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(260, 94);
            this.button1.TabIndex = 0;
            this.button1.Text = "Lobby";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbPartida
            // 
            this.lbPartida.AutoSize = true;
            this.lbPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPartida.Location = new System.Drawing.Point(12, 132);
            this.lbPartida.Name = "lbPartida";
            this.lbPartida.Size = new System.Drawing.Size(266, 24);
            this.lbPartida.TabIndex = 1;
            this.lbPartida.Text = "Nenhuma Partida Selecionada";
            // 
            // lbSenha
            // 
            this.lbSenha.AutoSize = true;
            this.lbSenha.Location = new System.Drawing.Point(324, 39);
            this.lbSenha.Name = "lbSenha";
            this.lbSenha.Size = new System.Drawing.Size(38, 13);
            this.lbSenha.TabIndex = 2;
            this.lbSenha.Text = "Senha";
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(324, 12);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(18, 13);
            this.lbId.TabIndex = 3;
            this.lbId.Text = "ID";
            // 
            // btnIniciarPartida
            // 
            this.btnIniciarPartida.Location = new System.Drawing.Point(547, 12);
            this.btnIniciarPartida.Name = "btnIniciarPartida";
            this.btnIniciarPartida.Size = new System.Drawing.Size(112, 24);
            this.btnIniciarPartida.TabIndex = 4;
            this.btnIniciarPartida.Text = "Iniciar Partida";
            this.btnIniciarPartida.UseVisualStyleBackColor = true;
            this.btnIniciarPartida.Click += new System.EventHandler(this.btnIniciarPartida_Click);
            // 
            // btnListar
            // 
            this.btnListar.Location = new System.Drawing.Point(547, 49);
            this.btnListar.Name = "btnListar";
            this.btnListar.Size = new System.Drawing.Size(112, 24);
            this.btnListar.TabIndex = 5;
            this.btnListar.Text = "Listar Dinossauros";
            this.btnListar.UseVisualStyleBackColor = true;
            this.btnListar.Click += new System.EventHandler(this.btnListar_Click);
            // 
            // lstDinossauros
            // 
            this.lstDinossauros.FormattingEnabled = true;
            this.lstDinossauros.Location = new System.Drawing.Point(547, 98);
            this.lstDinossauros.Name = "lstDinossauros";
            this.lstDinossauros.Size = new System.Drawing.Size(112, 95);
            this.lstDinossauros.TabIndex = 6;
            // 
            // lblRodada
            // 
            this.lblRodada.AutoSize = true;
            this.lblRodada.Location = new System.Drawing.Point(544, 82);
            this.lblRodada.Name = "lblRodada";
            this.lblRodada.Size = new System.Drawing.Size(0, 13);
            this.lblRodada.TabIndex = 7;
            // 
            // lstVerficarTurno
            // 
            this.lstVerficarTurno.FormattingEnabled = true;
            this.lstVerficarTurno.Location = new System.Drawing.Point(327, 98);
            this.lstVerficarTurno.Name = "lstVerficarTurno";
            this.lstVerficarTurno.Size = new System.Drawing.Size(120, 95);
            this.lstVerficarTurno.TabIndex = 8;
            // 
            // btnJogar
            // 
            this.btnJogar.Location = new System.Drawing.Point(440, 267);
            this.btnJogar.Name = "btnJogar";
            this.btnJogar.Size = new System.Drawing.Size(120, 38);
            this.btnJogar.TabIndex = 9;
            this.btnJogar.Text = "Fazer Jogada";
            this.btnJogar.UseVisualStyleBackColor = true;
            this.btnJogar.Click += new System.EventHandler(this.btnJogar_Click);
            // 
            // txtDinossauro
            // 
            this.txtDinossauro.Location = new System.Drawing.Point(386, 226);
            this.txtDinossauro.Name = "txtDinossauro";
            this.txtDinossauro.Size = new System.Drawing.Size(100, 20);
            this.txtDinossauro.TabIndex = 10;
            // 
            // txtCercado
            // 
            this.txtCercado.Location = new System.Drawing.Point(516, 226);
            this.txtCercado.Name = "txtCercado";
            this.txtCercado.Size = new System.Drawing.Size(100, 20);
            this.txtCercado.TabIndex = 11;
            // 
            // lblDinossauro
            // 
            this.lblDinossauro.AutoSize = true;
            this.lblDinossauro.Location = new System.Drawing.Point(383, 210);
            this.lblDinossauro.Name = "lblDinossauro";
            this.lblDinossauro.Size = new System.Drawing.Size(63, 13);
            this.lblDinossauro.TabIndex = 12;
            this.lblDinossauro.Text = "Dinossauro:";
            // 
            // lblCercado
            // 
            this.lblCercado.AutoSize = true;
            this.lblCercado.Location = new System.Drawing.Point(513, 210);
            this.lblCercado.Name = "lblCercado";
            this.lblCercado.Size = new System.Drawing.Size(47, 13);
            this.lblCercado.TabIndex = 13;
            this.lblCercado.Text = "Cercado";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Estado do Tabuleiro";
            // 
            // lstTabuleiro
            // 
            this.lstTabuleiro.FormattingEnabled = true;
            this.lstTabuleiro.Location = new System.Drawing.Point(12, 343);
            this.lstTabuleiro.Name = "lstTabuleiro";
            this.lstTabuleiro.Size = new System.Drawing.Size(120, 95);
            this.lstTabuleiro.TabIndex = 15;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstTabuleiro);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblCercado);
            this.Controls.Add(this.lblDinossauro);
            this.Controls.Add(this.txtCercado);
            this.Controls.Add(this.txtDinossauro);
            this.Controls.Add(this.btnJogar);
            this.Controls.Add(this.lstVerficarTurno);
            this.Controls.Add(this.lblRodada);
            this.Controls.Add(this.lstDinossauros);
            this.Controls.Add(this.btnListar);
            this.Controls.Add(this.btnIniciarPartida);
            this.Controls.Add(this.lbId);
            this.Controls.Add(this.lbSenha);
            this.Controls.Add(this.lbPartida);
            this.Controls.Add(this.button1);
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbPartida;
        private System.Windows.Forms.Label lbSenha;
        private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.Button btnIniciarPartida;
        private System.Windows.Forms.Button btnListar;
        private System.Windows.Forms.ListBox lstDinossauros;
        private System.Windows.Forms.Label lblRodada;
        private System.Windows.Forms.ListBox lstVerficarTurno;
        private System.Windows.Forms.Button btnJogar;
        private System.Windows.Forms.TextBox txtDinossauro;
        private System.Windows.Forms.TextBox txtCercado;
        private System.Windows.Forms.Label lblDinossauro;
        private System.Windows.Forms.Label lblCercado;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstTabuleiro;
    }
}