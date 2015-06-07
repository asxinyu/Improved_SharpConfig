namespace Sample
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.pnlTxtBox = new System.Windows.Forms.Panel();
            this.mainSplitter = new System.Windows.Forms.Splitter();
            this.trViewCfg = new System.Windows.Forms.TreeView();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.picBoxLogo = new System.Windows.Forms.PictureBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnOpenConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveConfig = new System.Windows.Forms.ToolStripButton();
            this.txtCommentChars = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.pnlTxtBox);
            this.mainPanel.Controls.Add(this.mainSplitter);
            this.mainPanel.Controls.Add(this.trViewCfg);
            this.mainPanel.Location = new System.Drawing.Point(15, 34);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(923, 472);
            this.mainPanel.TabIndex = 2;
            // 
            // pnlTxtBox
            // 
            this.pnlTxtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTxtBox.Location = new System.Drawing.Point(0, 0);
            this.pnlTxtBox.Name = "pnlTxtBox";
            this.pnlTxtBox.Size = new System.Drawing.Size(578, 472);
            this.pnlTxtBox.TabIndex = 4;
            // 
            // mainSplitter
            // 
            this.mainSplitter.BackColor = System.Drawing.Color.Silver;
            this.mainSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.mainSplitter.Location = new System.Drawing.Point(578, 0);
            this.mainSplitter.Name = "mainSplitter";
            this.mainSplitter.Size = new System.Drawing.Size(6, 472);
            this.mainSplitter.TabIndex = 2;
            this.mainSplitter.TabStop = false;
            // 
            // trViewCfg
            // 
            this.trViewCfg.BackColor = System.Drawing.Color.White;
            this.trViewCfg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.trViewCfg.Dock = System.Windows.Forms.DockStyle.Right;
            this.trViewCfg.Location = new System.Drawing.Point(584, 0);
            this.trViewCfg.Name = "trViewCfg";
            this.trViewCfg.Size = new System.Drawing.Size(339, 472);
            this.trViewCfg.TabIndex = 3;
            this.trViewCfg.TabStop = false;
            // 
            // mainTimer
            // 
            this.mainTimer.Interval = 500;
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // picBoxLogo
            // 
            this.picBoxLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picBoxLogo.BackgroundImage = global::Sample.Properties.Resources.sharpconfig_logo;
            this.picBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBoxLogo.Location = new System.Drawing.Point(682, 512);
            this.picBoxLogo.Name = "picBoxLogo";
            this.picBoxLogo.Size = new System.Drawing.Size(256, 48);
            this.picBoxLogo.TabIndex = 6;
            this.picBoxLogo.TabStop = false;
            this.picBoxLogo.Click += new System.EventHandler(this.picBoxLogo_Click);
            this.picBoxLogo.MouseEnter += new System.EventHandler(this.picBoxLogo_MouseEnter);
            this.picBoxLogo.MouseLeave += new System.EventHandler(this.picBoxLogo_MouseLeave);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenConfig,
            this.toolStripSeparator1,
            this.btnSaveConfig});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(950, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Image = global::Sample.Properties.Resources.open;
            this.btnOpenConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(84, 22);
            this.btnOpenConfig.Text = "Open a file";
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveConfig.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveConfig.Image")));
            this.btnSaveConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(77, 22);
            this.btnSaveConfig.Text = "Save to a file";
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // txtCommentChars
            // 
            this.txtCommentChars.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtCommentChars.Location = new System.Drawing.Point(96, 3);
            this.txtCommentChars.Name = "txtCommentChars";
            this.txtCommentChars.Size = new System.Drawing.Size(103, 21);
            this.txtCommentChars.TabIndex = 7;
            this.txtCommentChars.TabStop = false;
            this.txtCommentChars.TextChanged += new System.EventHandler(this.OnConfigChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Comment Chars:";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 315F));
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.txtCommentChars, 1, 0);
            this.tableLayoutPanel.Location = new System.Drawing.Point(15, 512);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(405, 48);
            this.tableLayoutPanel.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(950, 572);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.picBoxLogo);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 480);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SharpConfig Sample";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxLogo)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TreeView trViewCfg;
        private System.Windows.Forms.Splitter mainSplitter;
        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.Panel pnlTxtBox;
        private System.Windows.Forms.PictureBox picBoxLogo;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnOpenConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSaveConfig;
        private System.Windows.Forms.TextBox txtCommentChars;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}

