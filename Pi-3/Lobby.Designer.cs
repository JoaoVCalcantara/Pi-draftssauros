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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lobby));
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(8, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(207, 75);
            this.button1.TabIndex = 0;
            this.button1.Text = "Lobby";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbPartida
            // 
            this.lbPartida.AutoSize = true;
            this.lbPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPartida.Location = new System.Drawing.Point(222, 7);
            this.lbPartida.Name = "lbPartida";
            this.lbPartida.Size = new System.Drawing.Size(176, 15);
            this.lbPartida.TabIndex = 1;
            this.lbPartida.Text = "Nenhuma Partida Selecionada";
            // 
            // lbSenha
            // 
            this.lbSenha.AutoSize = true;
            this.lbSenha.Location = new System.Drawing.Point(243, 59);
            this.lbSenha.Name = "lbSenha";
            this.lbSenha.Size = new System.Drawing.Size(10, 13);
            this.lbSenha.TabIndex = 2;
            this.lbSenha.Text = "-";
            // 
            // lbId
            // 
            this.lbId.AutoSize = true;
            this.lbId.Location = new System.Drawing.Point(229, 34);
            this.lbId.Name = "lbId";
            this.lbId.Size = new System.Drawing.Size(10, 13);
            this.lbId.TabIndex = 3;
            this.lbId.Text = "-";
            // 
            // btnIniciarPartida
            // 
            this.btnIniciarPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarPartida.Location = new System.Drawing.Point(2, 11);
            this.btnIniciarPartida.Name = "btnIniciarPartida";
            this.btnIniciarPartida.Size = new System.Drawing.Size(130, 65);
            this.btnIniciarPartida.TabIndex = 4;
            this.btnIniciarPartida.Text = "Iniciar Partida";
            this.btnIniciarPartida.UseVisualStyleBackColor = true;
            this.btnIniciarPartida.Click += new System.EventHandler(this.btnIniciarPartida_Click);
            // 
            // btnListar
            // 
            this.btnListar.Location = new System.Drawing.Point(10, 12);
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
            this.lstDinossauros.Location = new System.Drawing.Point(10, 50);
            this.lstDinossauros.Name = "lstDinossauros";
            this.lstDinossauros.Size = new System.Drawing.Size(112, 95);
            this.lstDinossauros.TabIndex = 6;
            // 
            // lblRodada
            // 
            this.lblRodada.AutoSize = true;
            this.lblRodada.Location = new System.Drawing.Point(544, 162);
            this.lblRodada.Name = "lblRodada";
            this.lblRodada.Size = new System.Drawing.Size(0, 13);
            this.lblRodada.TabIndex = 7;
            // 
            // lstVerficarTurno
            // 
            this.lstVerficarTurno.FormattingEnabled = true;
            this.lstVerficarTurno.Location = new System.Drawing.Point(6, 38);
            this.lstVerficarTurno.Name = "lstVerficarTurno";
            this.lstVerficarTurno.Size = new System.Drawing.Size(120, 95);
            this.lstVerficarTurno.TabIndex = 8;
            // 
            // btnJogar
            // 
            this.btnJogar.Location = new System.Drawing.Point(120, 30);
            this.btnJogar.Name = "btnJogar";
            this.btnJogar.Size = new System.Drawing.Size(130, 67);
            this.btnJogar.TabIndex = 9;
            this.btnJogar.Text = "Fazer Jogada";
            this.btnJogar.UseVisualStyleBackColor = true;
            this.btnJogar.Click += new System.EventHandler(this.btnJogar_Click);
            // 
            // txtDinossauro
            // 
            this.txtDinossauro.Location = new System.Drawing.Point(6, 30);
            this.txtDinossauro.Name = "txtDinossauro";
            this.txtDinossauro.Size = new System.Drawing.Size(100, 20);
            this.txtDinossauro.TabIndex = 10;
            // 
            // txtCercado
            // 
            this.txtCercado.Location = new System.Drawing.Point(7, 77);
            this.txtCercado.Name = "txtCercado";
            this.txtCercado.Size = new System.Drawing.Size(100, 20);
            this.txtCercado.TabIndex = 11;
            // 
            // lblDinossauro
            // 
            this.lblDinossauro.AutoSize = true;
            this.lblDinossauro.Location = new System.Drawing.Point(4, 14);
            this.lblDinossauro.Name = "lblDinossauro";
            this.lblDinossauro.Size = new System.Drawing.Size(63, 13);
            this.lblDinossauro.TabIndex = 12;
            this.lblDinossauro.Text = "Dinossauro:";
            // 
            // lblCercado
            // 
            this.lblCercado.AutoSize = true;
            this.lblCercado.Location = new System.Drawing.Point(4, 61);
            this.lblCercado.Name = "lblCercado";
            this.lblCercado.Size = new System.Drawing.Size(47, 13);
            this.lblCercado.TabIndex = 13;
            this.lblCercado.Text = "Cercado";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Estado do Tabuleiro";
            // 
            // lstTabuleiro
            // 
            this.lstTabuleiro.FormattingEnabled = true;
            this.lstTabuleiro.Location = new System.Drawing.Point(6, 38);
            this.lstTabuleiro.Name = "lstTabuleiro";
            this.lstTabuleiro.Size = new System.Drawing.Size(120, 95);
            this.lstTabuleiro.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(138, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "ID da partida:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Key do Jogador:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(138, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "ID do jogador:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnIniciarPartida);
            this.groupBox1.Controls.Add(this.lbId);
            this.groupBox1.Controls.Add(this.lbSenha);
            this.groupBox1.Controls.Add(this.lbPartida);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 82);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstDinossauros);
            this.groupBox2.Controls.Add(this.btnListar);
            this.groupBox2.Location = new System.Drawing.Point(12, 181);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(132, 155);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblCercado);
            this.groupBox3.Controls.Add(this.lblDinossauro);
            this.groupBox3.Controls.Add(this.txtCercado);
            this.groupBox3.Controls.Add(this.txtDinossauro);
            this.groupBox3.Controls.Add(this.btnJogar);
            this.groupBox3.Location = new System.Drawing.Point(150, 181);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 108);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lstTabuleiro);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(150, 289);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(132, 139);
            this.groupBox4.TabIndex = 22;
            this.groupBox4.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.lstVerficarTurno);
            this.groupBox5.Location = new System.Drawing.Point(284, 289);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(132, 139);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Verificar Turno";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(422, 187);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(241, 241);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblRodada);
            this.Controls.Add(this.button1);
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.Load += new System.EventHandler(this.Lobby_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}