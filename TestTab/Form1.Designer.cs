namespace TestTab {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainTabs1 = new CloseAbleTab.MainTabs();
            this.SuspendLayout();
            // 
            // mainTabs1
            // 
            this.mainTabs1.BackColor = System.Drawing.SystemColors.Control;
            this.mainTabs1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainTabs1.DefaultTextHeaderTab = "New Tab";
            this.mainTabs1.Location = new System.Drawing.Point(12, 12);
            this.mainTabs1.Name = "mainTabs1";
            this.mainTabs1.Size = new System.Drawing.Size(283, 160);
            this.mainTabs1.TabIndex = 0;
            this.mainTabs1.TabSelectedIndex = -1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 262);
            this.Controls.Add(this.mainTabs1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private CloseAbleTab.MainTabs mainTabs1;
    }
}

