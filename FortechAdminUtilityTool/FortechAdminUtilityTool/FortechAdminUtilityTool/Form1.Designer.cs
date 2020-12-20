namespace FortechAdminUtilityTool
{
    partial class mainWindow
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
            this.UserIDtext = new System.Windows.Forms.Label();
            this.UserPasswordtext = new System.Windows.Forms.Label();
            this.ConnectionStateText = new System.Windows.Forms.Label();
            this.UserStatusText = new System.Windows.Forms.Label();
            this.UserBanbutton = new System.Windows.Forms.Button();
            this.SaveFileDataTextBox = new System.Windows.Forms.TextBox();
            this.SavefileDataText = new System.Windows.Forms.Label();
            this.UpdateSaveFileButton = new System.Windows.Forms.Button();
            this.clearSaveFileButton = new System.Windows.Forms.Button();
            this.SaveFileTimeDateText = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UsernameText
            // 
            this.UsernameText.AutoSize = true;
            this.UsernameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameText.Location = new System.Drawing.Point(12, 9);
            this.UsernameText.Name = "UsernameText";
            this.UsernameText.Size = new System.Drawing.Size(121, 26);
            this.UsernameText.TabIndex = 0;
            this.UsernameText.Text = "Username";
            this.UsernameText.Click += new System.EventHandler(this.UsernameText_Click);
            // 
            // SelectUserbutton
            // 
            this.SelectUserbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectUserbutton.Location = new System.Drawing.Point(8, 352);
            this.SelectUserbutton.Name = "SelectUserbutton";
            this.SelectUserbutton.Size = new System.Drawing.Size(112, 43);
            this.SelectUserbutton.TabIndex = 1;
            this.SelectUserbutton.Text = "select  User";
            this.SelectUserbutton.UseVisualStyleBackColor = true;
            this.SelectUserbutton.Click += new System.EventHandler(this.SelectUserbutton_Click);
            // 
            // UserIDtext
            // 
            this.UserIDtext.AutoSize = true;
            this.UserIDtext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserIDtext.Location = new System.Drawing.Point(15, 52);
            this.UserIDtext.Name = "UserIDtext";
            this.UserIDtext.Size = new System.Drawing.Size(63, 17);
            this.UserIDtext.TabIndex = 2;
            this.UserIDtext.Text = "User ID: ";
            // 
            // UserPasswordtext
            // 
            this.UserPasswordtext.AutoSize = true;
            this.UserPasswordtext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserPasswordtext.Location = new System.Drawing.Point(15, 78);
            this.UserPasswordtext.Name = "UserPasswordtext";
            this.UserPasswordtext.Size = new System.Drawing.Size(77, 17);
            this.UserPasswordtext.TabIndex = 3;
            this.UserPasswordtext.Text = "Password: ";
            // 
            // ConnectionStateText
            // 
            this.ConnectionStateText.AutoSize = true;
            this.ConnectionStateText.BackColor = System.Drawing.Color.Transparent;
            this.ConnectionStateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionStateText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ConnectionStateText.Location = new System.Drawing.Point(445, 372);
            this.ConnectionStateText.Name = "ConnectionStateText";
            this.ConnectionStateText.Size = new System.Drawing.Size(242, 26);
            this.ConnectionStateText.TabIndex = 4;
            this.ConnectionStateText.Text = "Database connecting ";
            // 
            // UserStatusText
            // 
            this.UserStatusText.AutoSize = true;
            this.UserStatusText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserStatusText.Location = new System.Drawing.Point(15, 104);
            this.UserStatusText.Name = "UserStatusText";
            this.UserStatusText.Size = new System.Drawing.Size(56, 17);
            this.UserStatusText.TabIndex = 5;
            this.UserStatusText.Text = "Status: ";
            // 
            // UserBanbutton
            // 
            this.UserBanbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserBanbutton.Location = new System.Drawing.Point(126, 352);
            this.UserBanbutton.Name = "UserBanbutton";
            this.UserBanbutton.Size = new System.Drawing.Size(118, 43);
            this.UserBanbutton.TabIndex = 6;
            this.UserBanbutton.Text = "Ban  User";
            this.UserBanbutton.UseVisualStyleBackColor = true;
            this.UserBanbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SaveFileDataTextBox
            // 
            this.SaveFileDataTextBox.Location = new System.Drawing.Point(254, 52);
            this.SaveFileDataTextBox.Multiline = true;
            this.SaveFileDataTextBox.Name = "SaveFileDataTextBox";
            this.SaveFileDataTextBox.Size = new System.Drawing.Size(433, 317);
            this.SaveFileDataTextBox.TabIndex = 7;
            // 
            // SavefileDataText
            // 
            this.SavefileDataText.AutoSize = true;
            this.SavefileDataText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SavefileDataText.Location = new System.Drawing.Point(247, 9);
            this.SavefileDataText.Name = "SavefileDataText";
            this.SavefileDataText.Size = new System.Drawing.Size(162, 26);
            this.SavefileDataText.TabIndex = 8;
            this.SavefileDataText.Text = "SaveFile Data";
            // 
            // UpdateSaveFileButton
            // 
            this.UpdateSaveFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateSaveFileButton.Location = new System.Drawing.Point(8, 235);
            this.UpdateSaveFileButton.Name = "UpdateSaveFileButton";
            this.UpdateSaveFileButton.Size = new System.Drawing.Size(236, 43);
            this.UpdateSaveFileButton.TabIndex = 9;
            this.UpdateSaveFileButton.Text = "update SaveFile";
            this.UpdateSaveFileButton.UseVisualStyleBackColor = true;
            this.UpdateSaveFileButton.Click += new System.EventHandler(this.UpdateSaveFileButton_Click);
            // 
            // clearSaveFileButton
            // 
            this.clearSaveFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearSaveFileButton.Location = new System.Drawing.Point(8, 284);
            this.clearSaveFileButton.Name = "clearSaveFileButton";
            this.clearSaveFileButton.Size = new System.Drawing.Size(236, 43);
            this.clearSaveFileButton.TabIndex = 10;
            this.clearSaveFileButton.Text = "clear SaveFile";
            this.clearSaveFileButton.UseVisualStyleBackColor = true;
            this.clearSaveFileButton.Click += new System.EventHandler(this.clearSaveFileButton_Click);
            // 
            // SaveFileTimeDateText
            // 
            this.SaveFileTimeDateText.AutoSize = true;
            this.SaveFileTimeDateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveFileTimeDateText.Location = new System.Drawing.Point(15, 131);
            this.SaveFileTimeDateText.Name = "SaveFileTimeDateText";
            this.SaveFileTimeDateText.Size = new System.Drawing.Size(77, 17);
            this.SaveFileTimeDateText.TabIndex = 11;
            this.SaveFileTimeDateText.Text = "TimeDate :";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(8, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(236, 43);
            this.button1.TabIndex = 12;
            this.button1.Text = "set TimeDate to now";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // mainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 407);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SaveFileTimeDateText);
            this.Controls.Add(this.clearSaveFileButton);
            this.Controls.Add(this.UpdateSaveFileButton);
            this.Controls.Add(this.SavefileDataText);
            this.Controls.Add(this.SaveFileDataTextBox);
            this.Controls.Add(this.UserBanbutton);
            this.Controls.Add(this.UserStatusText);
            this.Controls.Add(this.ConnectionStateText);
            this.Controls.Add(this.UserPasswordtext);
            this.Controls.Add(this.UserIDtext);
            this.Controls.Add(this.SelectUserbutton);
            this.Controls.Add(this.UsernameText);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "mainWindow";
            this.ShowIcon = false;
            this.Text = "Fortech Admin Utility Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UsernameText;
        private System.Windows.Forms.Button SelectUserbutton;
        private System.Windows.Forms.Label UserIDtext;
        private System.Windows.Forms.Label UserPasswordtext;
        private System.Windows.Forms.Label ConnectionStateText;
        private System.Windows.Forms.Label UserStatusText;
        private System.Windows.Forms.Button UserBanbutton;
        private System.Windows.Forms.TextBox SaveFileDataTextBox;
        private System.Windows.Forms.Label SavefileDataText;
        private System.Windows.Forms.Button UpdateSaveFileButton;
        private System.Windows.Forms.Button clearSaveFileButton;
        private System.Windows.Forms.Label SaveFileTimeDateText;
        private System.Windows.Forms.Button button1;
    }
}

