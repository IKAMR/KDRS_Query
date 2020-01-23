namespace KDRS_Query
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
            this.btnInFile = new System.Windows.Forms.Button();
            this.btnTrgtFold = new System.Windows.Forms.Button();
            this.btnRunQ = new System.Windows.Forms.Button();
            this.txtInFile = new System.Windows.Forms.TextBox();
            this.txtQFile = new System.Windows.Forms.TextBox();
            this.txtTrgtPath = new System.Windows.Forms.TextBox();
            this.btnQFile = new System.Windows.Forms.Button();
            this.txtLogbox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChooseReport = new System.Windows.Forms.Button();
            this.txtReportFile = new System.Windows.Forms.TextBox();
            this.btnWriteReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInFile
            // 
            this.btnInFile.Location = new System.Drawing.Point(13, 33);
            this.btnInFile.Name = "btnInFile";
            this.btnInFile.Size = new System.Drawing.Size(144, 23);
            this.btnInFile.TabIndex = 0;
            this.btnInFile.Text = "Choose input file";
            this.btnInFile.UseVisualStyleBackColor = true;
            this.btnInFile.Click += new System.EventHandler(this.btnInFile_Click);
            // 
            // btnTrgtFold
            // 
            this.btnTrgtFold.Location = new System.Drawing.Point(12, 62);
            this.btnTrgtFold.Name = "btnTrgtFold";
            this.btnTrgtFold.Size = new System.Drawing.Size(145, 23);
            this.btnTrgtFold.TabIndex = 2;
            this.btnTrgtFold.Text = "Choose target folder";
            this.btnTrgtFold.UseVisualStyleBackColor = true;
            this.btnTrgtFold.Click += new System.EventHandler(this.btnTrgtFold_Click);
            // 
            // btnRunQ
            // 
            this.btnRunQ.Location = new System.Drawing.Point(12, 155);
            this.btnRunQ.Name = "btnRunQ";
            this.btnRunQ.Size = new System.Drawing.Size(107, 40);
            this.btnRunQ.TabIndex = 6;
            this.btnRunQ.Text = "Run Query";
            this.btnRunQ.UseVisualStyleBackColor = true;
            this.btnRunQ.Click += new System.EventHandler(this.btnRunQ_Click);
            // 
            // txtInFile
            // 
            this.txtInFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInFile.Location = new System.Drawing.Point(163, 35);
            this.txtInFile.Name = "txtInFile";
            this.txtInFile.Size = new System.Drawing.Size(531, 20);
            this.txtInFile.TabIndex = 1;
            // 
            // txtQFile
            // 
            this.txtQFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQFile.Location = new System.Drawing.Point(163, 93);
            this.txtQFile.Name = "txtQFile";
            this.txtQFile.Size = new System.Drawing.Size(531, 20);
            this.txtQFile.TabIndex = 5;
            // 
            // txtTrgtPath
            // 
            this.txtTrgtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTrgtPath.Location = new System.Drawing.Point(163, 64);
            this.txtTrgtPath.Name = "txtTrgtPath";
            this.txtTrgtPath.Size = new System.Drawing.Size(531, 20);
            this.txtTrgtPath.TabIndex = 3;
            // 
            // btnQFile
            // 
            this.btnQFile.Location = new System.Drawing.Point(14, 91);
            this.btnQFile.Name = "btnQFile";
            this.btnQFile.Size = new System.Drawing.Size(143, 23);
            this.btnQFile.TabIndex = 4;
            this.btnQFile.Text = "Choose query file";
            this.btnQFile.UseVisualStyleBackColor = true;
            this.btnQFile.Click += new System.EventHandler(this.btnQFile_Click);
            // 
            // txtLogbox
            // 
            this.txtLogbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogbox.Location = new System.Drawing.Point(12, 201);
            this.txtLogbox.Multiline = true;
            this.txtLogbox.Name = "txtLogbox";
            this.txtLogbox.ReadOnly = true;
            this.txtLogbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogbox.Size = new System.Drawing.Size(682, 148);
            this.txtLogbox.TabIndex = 7;
            this.txtLogbox.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnChooseReport
            // 
            this.btnChooseReport.Location = new System.Drawing.Point(14, 364);
            this.btnChooseReport.Name = "btnChooseReport";
            this.btnChooseReport.Size = new System.Drawing.Size(143, 23);
            this.btnChooseReport.TabIndex = 8;
            this.btnChooseReport.Text = "Choose report file";
            this.btnChooseReport.UseVisualStyleBackColor = true;
            // 
            // txtReportFile
            // 
            this.txtReportFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReportFile.Location = new System.Drawing.Point(163, 366);
            this.txtReportFile.Name = "txtReportFile";
            this.txtReportFile.Size = new System.Drawing.Size(531, 20);
            this.txtReportFile.TabIndex = 9;
            // 
            // btnWriteReport
            // 
            this.btnWriteReport.Location = new System.Drawing.Point(14, 393);
            this.btnWriteReport.Name = "btnWriteReport";
            this.btnWriteReport.Size = new System.Drawing.Size(107, 40);
            this.btnWriteReport.TabIndex = 10;
            this.btnWriteReport.Text = "Write report";
            this.btnWriteReport.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 450);
            this.Controls.Add(this.btnWriteReport);
            this.Controls.Add(this.btnChooseReport);
            this.Controls.Add(this.txtReportFile);
            this.Controls.Add(this.txtLogbox);
            this.Controls.Add(this.btnQFile);
            this.Controls.Add(this.txtTrgtPath);
            this.Controls.Add(this.txtQFile);
            this.Controls.Add(this.txtInFile);
            this.Controls.Add(this.btnRunQ);
            this.Controls.Add(this.btnTrgtFold);
            this.Controls.Add(this.btnInFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInFile;
        private System.Windows.Forms.Button btnTrgtFold;
        private System.Windows.Forms.Button btnRunQ;
        private System.Windows.Forms.TextBox txtInFile;
        private System.Windows.Forms.TextBox txtQFile;
        private System.Windows.Forms.TextBox txtTrgtPath;
        private System.Windows.Forms.Button btnQFile;
        private System.Windows.Forms.TextBox txtLogbox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnChooseReport;
        private System.Windows.Forms.TextBox txtReportFile;
        private System.Windows.Forms.Button btnWriteReport;
    }
}

