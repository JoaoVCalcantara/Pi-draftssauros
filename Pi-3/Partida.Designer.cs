namespace Pi_3
{
    partial class Partida
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
            this.PartidaNome = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPartida = new System.Windows.Forms.TextBox();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.btnPartida = new System.Windows.Forms.Button();
            this.txtGrupo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // PartidaNome
            // 
            this.PartidaNome.AutoSize = true;
            this.PartidaNome.Location = new System.Drawing.Point(12, 50);
            this.PartidaNome.Name = "PartidaNome";
            this.PartidaNome.Size = new System.Drawing.Size(85, 13);
            this.PartidaNome.TabIndex = 0;
            this.PartidaNome.Text = "Nome da partida";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Senha";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(246, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Grupo";
            // 
            // txtPartida
            // 
            this.txtPartida.Location = new System.Drawing.Point(12, 66);
            this.txtPartida.Name = "txtPartida";
            this.txtPartida.Size = new System.Drawing.Size(100, 20);
            this.txtPartida.TabIndex = 3;
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(127, 66);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(100, 20);
            this.txtSenha.TabIndex = 4;
            // 
            // btnPartida
            // 
            this.btnPartida.Location = new System.Drawing.Point(335, 141);
            this.btnPartida.Name = "btnPartida";
            this.btnPartida.Size = new System.Drawing.Size(75, 23);
            this.btnPartida.TabIndex = 6;
            this.btnPartida.Text = "Criar Partida";
            this.btnPartida.UseVisualStyleBackColor = true;
            this.btnPartida.Click += new System.EventHandler(this.btnPartida_Click);
            // 
            // txtGrupo
            // 
            this.txtGrupo.BackColor = System.Drawing.SystemColors.Window;
            this.txtGrupo.Enabled = false;
            this.txtGrupo.Location = new System.Drawing.Point(249, 66);
            this.txtGrupo.Name = "txtGrupo";
            this.txtGrupo.Size = new System.Drawing.Size(100, 20);
            this.txtGrupo.TabIndex = 5;
            this.txtGrupo.Text = "Jurássicos";
            // 
            // Partida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 167);
            this.Controls.Add(this.btnPartida);
            this.Controls.Add(this.txtGrupo);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.txtPartida);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PartidaNome);
            this.Name = "Partida";
            this.Text = "Partida";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PartidaNome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPartida;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Button btnPartida;
        private System.Windows.Forms.TextBox txtGrupo;
    }
}