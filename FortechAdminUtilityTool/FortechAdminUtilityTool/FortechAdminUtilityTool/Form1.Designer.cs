namespace FortechAdminUtilityTool
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.UsernameText = new System.Windows.Forms.Label();
            this.SelectUserbutton = new System.Windows.Forms.Button();
            this.PasswordText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // UsernameText
            // 
            this.UsernameText.AutoSize = true;
            this.UsernameText.Font = new System.Drawing.Font("Open Sans Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameText.Location = new System.Drawing.Point(12, 9);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(157, 38);
            this.UsernameText.TabIndex = 0;
            this.UsernameText.Text = "Username";
            // 
            // SelectUserbutton
            // 
            this.SelectUserbutton.Font = new System.Drawing.Font("Open Sans", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectUserbutton.Location = new System.Drawing.Point(13, 392);
            this.SelectUserbutton.Name = "SelectUserbutton";
            this.SelectUserbutton.Size = new System.Drawing.Size(106, 38);
            this.SelectUserbutton.TabIndex = 1;
            this.SelectUserbutton.Text = "select  User";
            this.SelectUserbutton.UseVisualStyleBackColor = true;
            this.SelectUserbutton.Click += new System.EventHandler(this.SelectUserbutton_Click);
            // 
            // PasswordText
            // 
            this.PasswordText.AutoSize = true;
            this.PasswordText.Font = new System.Drawing.Font("Open Sans", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasswordText.Location = new System.Drawing.Point(15, 47);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Size = new System.Drawing.Size(85, 23);
            this.PasswordText.TabIndex = 2;
            this.PasswordText.Text = "password";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.SelectUserbutton);
            this.Controls.Add(this.UsernameText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UsernameText;
        private System.Windows.Forms.Button SelectUserbutton;
        private System.Windows.Forms.Label PasswordText;
    }
}

