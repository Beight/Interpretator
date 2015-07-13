namespace Intepretator
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
            this.loadFile = new System.Windows.Forms.Button();
            this.tbfile = new System.Windows.Forms.RichTextBox();
            this.tbconverted = new System.Windows.Forms.RichTextBox();
            this.Convert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loadFile
            // 
            this.loadFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.loadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadFile.Location = new System.Drawing.Point(292, 385);
            this.loadFile.Name = "loadFile";
            this.loadFile.Size = new System.Drawing.Size(87, 25);
            this.loadFile.TabIndex = 0;
            this.loadFile.Text = "Load File";
            this.loadFile.UseVisualStyleBackColor = true;
            this.loadFile.Click += new System.EventHandler(this.loadFile_Click);
            // 
            // tbfile
            // 
            this.tbfile.Location = new System.Drawing.Point(12, 12);
            this.tbfile.Name = "tbfile";
            this.tbfile.ReadOnly = true;
            this.tbfile.Size = new System.Drawing.Size(249, 329);
            this.tbfile.TabIndex = 1;
            this.tbfile.Text = "";
            this.tbfile.TextChanged += new System.EventHandler(this.tbfile_TextChanged);
            // 
            // tbconverted
            // 
            this.tbconverted.Location = new System.Drawing.Point(414, 12);
            this.tbconverted.Name = "tbconverted";
            this.tbconverted.ReadOnly = true;
            this.tbconverted.Size = new System.Drawing.Size(249, 329);
            this.tbconverted.TabIndex = 2;
            this.tbconverted.Text = "";
            this.tbconverted.TextChanged += new System.EventHandler(this.tbconverted_TextChanged);
            // 
            // Convert
            // 
            this.Convert.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Convert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Convert.Location = new System.Drawing.Point(292, 426);
            this.Convert.Name = "Convert";
            this.Convert.Size = new System.Drawing.Size(87, 25);
            this.Convert.TabIndex = 3;
            this.Convert.Text = "Execute";
            this.Convert.UseVisualStyleBackColor = true;
            this.Convert.Click += new System.EventHandler(this.Convert_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 498);
            this.Controls.Add(this.Convert);
            this.Controls.Add(this.tbconverted);
            this.Controls.Add(this.tbfile);
            this.Controls.Add(this.loadFile);
            this.Name = "Form1";
            this.Text = "Interpretator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadFile;
        private System.Windows.Forms.RichTextBox tbfile;
        private System.Windows.Forms.RichTextBox tbconverted;
        private System.Windows.Forms.Button Convert;

    }
}

