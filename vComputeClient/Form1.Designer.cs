namespace vComputeClient
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.assemblyCodeParams = new System.Windows.Forms.RichTextBox();
            this.assemblyCode = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.execute = new System.Windows.Forms.Button();
            this.notifList = new System.Windows.Forms.TextBox();
            this.assemblyList = new System.Windows.Forms.ListBox();
            this.assemblyParams = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(694, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 21);
            this.button1.TabIndex = 0;
            this.button1.Text = "OFF";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 375);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.assemblyCodeParams);
            this.tabPage1.Controls.Add(this.assemblyCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 349);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Code";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // assemblyCodeParams
            // 
            this.assemblyCodeParams.Location = new System.Drawing.Point(542, 0);
            this.assemblyCodeParams.Name = "assemblyCodeParams";
            this.assemblyCodeParams.Size = new System.Drawing.Size(223, 349);
            this.assemblyCodeParams.TabIndex = 1;
            this.assemblyCodeParams.Text = "";
            // 
            // assemblyCode
            // 
            this.assemblyCode.Location = new System.Drawing.Point(0, 0);
            this.assemblyCode.Name = "assemblyCode";
            this.assemblyCode.Size = new System.Drawing.Size(541, 349);
            this.assemblyCode.TabIndex = 0;
            this.assemblyCode.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.assemblyParams);
            this.tabPage2.Controls.Add(this.assemblyList);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 349);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Saved Assembly";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // execute
            // 
            this.execute.Location = new System.Drawing.Point(12, 406);
            this.execute.Name = "execute";
            this.execute.Size = new System.Drawing.Size(92, 30);
            this.execute.TabIndex = 2;
            this.execute.Text = "Execute";
            this.execute.UseVisualStyleBackColor = true;
            this.execute.Click += new System.EventHandler(this.execute_Click);
            // 
            // notifList
            // 
            this.notifList.Location = new System.Drawing.Point(131, 406);
            this.notifList.Multiline = true;
            this.notifList.Name = "notifList";
            this.notifList.Size = new System.Drawing.Size(653, 118);
            this.notifList.TabIndex = 3;
            // 
            // assemblyList
            // 
            this.assemblyList.FormattingEnabled = true;
            this.assemblyList.Location = new System.Drawing.Point(0, 3);
            this.assemblyList.Name = "assemblyList";
            this.assemblyList.Size = new System.Drawing.Size(256, 342);
            this.assemblyList.TabIndex = 0;
            // 
            // assemblyParams
            // 
            this.assemblyParams.Location = new System.Drawing.Point(262, 3);
            this.assemblyParams.Name = "assemblyParams";
            this.assemblyParams.Size = new System.Drawing.Size(500, 342);
            this.assemblyParams.TabIndex = 1;
            this.assemblyParams.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 524);
            this.Controls.Add(this.notifList);
            this.Controls.Add(this.execute);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "vCompute Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox assemblyCodeParams;
        private System.Windows.Forms.RichTextBox assemblyCode;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button execute;
        private System.Windows.Forms.TextBox notifList;
        private System.Windows.Forms.RichTextBox assemblyParams;
        private System.Windows.Forms.ListBox assemblyList;
    }
}

