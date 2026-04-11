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
            this.lblPartida = new System.Windows.Forms.Label();
            this.lblKeyJogadorPrincipal = new System.Windows.Forms.Label();
            this.lblIDJogadorPrincipal = new System.Windows.Forms.Label();
            this.btnIniciarPartida = new System.Windows.Forms.Button();
            this.lblRodada = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnEntrarJogadorPrincipal = new System.Windows.Forms.Button();
            this.txtNomeJogadorPrincipal = new System.Windows.Forms.TextBox();
            this.txtSenhaPartida = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblStatusJogadorPrincipal = new System.Windows.Forms.Label();
            this.lblStatusPartida = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
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
            // lblPartida
            // 
            this.lblPartida.AutoSize = true;
            this.lblPartida.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPartida.Location = new System.Drawing.Point(229, 11);
            this.lblPartida.Name = "lblPartida";
            this.lblPartida.Size = new System.Drawing.Size(159, 13);
            this.lblPartida.TabIndex = 1;
            this.lblPartida.Text = "Selecione uma partida no Lobby";
            // 
            // lblKeyJogadorPrincipal
            // 
            this.lblKeyJogadorPrincipal.AutoSize = true;
            this.lblKeyJogadorPrincipal.Location = new System.Drawing.Point(243, 59);
            this.lblKeyJogadorPrincipal.Name = "lblKeyJogadorPrincipal";
            this.lblKeyJogadorPrincipal.Size = new System.Drawing.Size(10, 13);
            this.lblKeyJogadorPrincipal.TabIndex = 2;
            this.lblKeyJogadorPrincipal.Text = "-";
            // 
            // lblIDJogadorPrincipal
            // 
            this.lblIDJogadorPrincipal.AutoSize = true;
            this.lblIDJogadorPrincipal.Location = new System.Drawing.Point(229, 34);
            this.lblIDJogadorPrincipal.Name = "lblIDJogadorPrincipal";
            this.lblIDJogadorPrincipal.Size = new System.Drawing.Size(10, 13);
            this.lblIDJogadorPrincipal.TabIndex = 3;
            this.lblIDJogadorPrincipal.Text = "-";
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
            // lblRodada
            // 
            this.lblRodada.AutoSize = true;
            this.lblRodada.Location = new System.Drawing.Point(422, 162);
            this.lblRodada.Name = "lblRodada";
            this.lblRodada.Size = new System.Drawing.Size(10, 13);
            this.lblRodada.TabIndex = 7;
            this.lblRodada.Text = "-";
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
            this.groupBox1.Controls.Add(this.lblIDJogadorPrincipal);
            this.groupBox1.Controls.Add(this.lblKeyJogadorPrincipal);
            this.groupBox1.Controls.Add(this.lblPartida);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 82);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // btnEntrarJogadorPrincipal
            // 
            this.btnEntrarJogadorPrincipal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEntrarJogadorPrincipal.Location = new System.Drawing.Point(260, 16);
            this.btnEntrarJogadorPrincipal.Name = "btnEntrarJogadorPrincipal";
            this.btnEntrarJogadorPrincipal.Size = new System.Drawing.Size(119, 54);
            this.btnEntrarJogadorPrincipal.TabIndex = 25;
            this.btnEntrarJogadorPrincipal.Text = "Adicionar Jogador Principal";
            this.btnEntrarJogadorPrincipal.UseVisualStyleBackColor = true;
            this.btnEntrarJogadorPrincipal.Click += new System.EventHandler(this.btnEntrarJogadorPrincipal_Click);
            // 
            // txtNomeJogadorPrincipal
            // 
            this.txtNomeJogadorPrincipal.Location = new System.Drawing.Point(144, 16);
            this.txtNomeJogadorPrincipal.Name = "txtNomeJogadorPrincipal";
            this.txtNomeJogadorPrincipal.Size = new System.Drawing.Size(100, 20);
            this.txtNomeJogadorPrincipal.TabIndex = 26;
            // 
            // txtSenhaPartida
            // 
            this.txtSenhaPartida.Location = new System.Drawing.Point(144, 50);
            this.txtSenhaPartida.Name = "txtSenhaPartida";
            this.txtSenhaPartida.Size = new System.Drawing.Size(100, 20);
            this.txtSenhaPartida.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Nome do jogador principal:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Senha da partida:";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.txtSenhaPartida);
            this.groupBox6.Controls.Add(this.txtNomeJogadorPrincipal);
            this.groupBox6.Controls.Add(this.btnEntrarJogadorPrincipal);
            this.groupBox6.Location = new System.Drawing.Point(221, 8);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(387, 79);
            this.groupBox6.TabIndex = 30;
            this.groupBox6.TabStop = false;
            // 
            // lblStatusJogadorPrincipal
            // 
            this.lblStatusJogadorPrincipal.AutoSize = true;
            this.lblStatusJogadorPrincipal.Location = new System.Drawing.Point(614, 45);
            this.lblStatusJogadorPrincipal.Name = "lblStatusJogadorPrincipal";
            this.lblStatusJogadorPrincipal.Size = new System.Drawing.Size(10, 13);
            this.lblStatusJogadorPrincipal.TabIndex = 31;
            this.lblStatusJogadorPrincipal.Text = "-";
            // 
            // lblStatusPartida
            // 
            this.lblStatusPartida.AutoSize = true;
            this.lblStatusPartida.Location = new System.Drawing.Point(422, 130);
            this.lblStatusPartida.Name = "lblStatusPartida";
            this.lblStatusPartida.Size = new System.Drawing.Size(10, 13);
            this.lblStatusPartida.TabIndex = 32;
            this.lblStatusPartida.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(753, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "label8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(724, 162);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "label9";
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 187);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblStatusPartida);
            this.Controls.Add(this.lblStatusJogadorPrincipal);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblRodada);
            this.Controls.Add(this.button1);
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.Load += new System.EventHandler(this.Lobby_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblPartida;
        private System.Windows.Forms.Label lblKeyJogadorPrincipal;
        private System.Windows.Forms.Label lblIDJogadorPrincipal;
        private System.Windows.Forms.Button btnIniciarPartida;
        private System.Windows.Forms.Label lblRodada;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEntrarJogadorPrincipal;
        private System.Windows.Forms.TextBox txtNomeJogadorPrincipal;
        private System.Windows.Forms.TextBox txtSenhaPartida;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblStatusJogadorPrincipal;
        private System.Windows.Forms.Label lblStatusPartida;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}