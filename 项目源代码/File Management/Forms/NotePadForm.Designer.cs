namespace File_Management
{
    partial class NotePadForm
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
            this.TextArea = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextArea
            // 
            this.TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextArea.Location = new System.Drawing.Point(0, 0);
            this.TextArea.Multiline = true;
            this.TextArea.Name = "TextArea";
            this.TextArea.Size = new System.Drawing.Size(535, 450);
            this.TextArea.TabIndex = 0;
            this.TextArea.UseWaitCursor = true;
            this.TextArea.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // NotePadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(535, 450);
            this.Controls.Add(this.TextArea);
            this.Name = "NotePadForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.NotePadForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextArea;
    }
}