namespace Mouse_Magic
{
    partial class MouseMagic
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
            this.Run = new System.Windows.Forms.Button();
            this.Script = new System.Windows.Forms.RichTextBox();
            this.currentLine = new System.Windows.Forms.TextBox();
            this.startLine = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(396, 962);
            this.Run.Margin = new System.Windows.Forms.Padding(2);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(77, 27);
            this.Run.TabIndex = 6;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // Script
            // 
            this.Script.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Script.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Script.ForeColor = System.Drawing.Color.White;
            this.Script.Location = new System.Drawing.Point(7, 6);
            this.Script.Margin = new System.Windows.Forms.Padding(2);
            this.Script.Name = "Script";
            this.Script.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Script.Size = new System.Drawing.Size(466, 952);
            this.Script.TabIndex = 11;
            this.Script.Text = "";
            this.Script.Click += new System.EventHandler(this.Script_Click);
            this.Script.TextChanged += new System.EventHandler(this.Script_TextChanged);
            // 
            // currentLine
            // 
            this.currentLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLine.Location = new System.Drawing.Point(10, 962);
            this.currentLine.Margin = new System.Windows.Forms.Padding(2);
            this.currentLine.Name = "currentLine";
            this.currentLine.ReadOnly = true;
            this.currentLine.Size = new System.Drawing.Size(327, 32);
            this.currentLine.TabIndex = 12;
            this.currentLine.Text = "not running";
            this.currentLine.TextChanged += new System.EventHandler(this.currentLine_TextChanged);
            // 
            // startLine
            // 
            this.startLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLine.Location = new System.Drawing.Point(338, 962);
            this.startLine.Margin = new System.Windows.Forms.Padding(2);
            this.startLine.Name = "startLine";
            this.startLine.Size = new System.Drawing.Size(56, 32);
            this.startLine.TabIndex = 13;
            this.startLine.Text = "0";
            this.startLine.TextChanged += new System.EventHandler(this.startLine_TextChanged);
            // 
            // MouseMagic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 1000);
            this.Controls.Add(this.startLine);
            this.Controls.Add(this.currentLine);
            this.Controls.Add(this.Script);
            this.Controls.Add(this.Run);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MouseMagic";
            this.Text = " MouseMagic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MouseMagic_FormClosing);
            this.Load += new System.EventHandler(this.MouseMagic_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.RichTextBox Script;
        private System.Windows.Forms.TextBox currentLine;
        private System.Windows.Forms.TextBox startLine;
    }
}

