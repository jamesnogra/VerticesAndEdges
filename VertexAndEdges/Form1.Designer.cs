namespace VertexAndEdges
{
    partial class VertexAndEdges
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blindSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPicBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.connectVertexButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.toVertexComboBox = new System.Windows.Forms.ComboBox();
            this.fromVertexComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.showAdjacencyMatrixButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.blindSearchToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(884, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.openGraphToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save Graph";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // openGraphToolStripMenuItem
            // 
            this.openGraphToolStripMenuItem.Name = "openGraphToolStripMenuItem";
            this.openGraphToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openGraphToolStripMenuItem.Text = "Open Graph";
            this.openGraphToolStripMenuItem.Click += new System.EventHandler(this.openGraphToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // blindSearchToolStripMenuItem
            // 
            this.blindSearchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bFSToolStripMenuItem,
            this.dFSToolStripMenuItem});
            this.blindSearchToolStripMenuItem.Name = "blindSearchToolStripMenuItem";
            this.blindSearchToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.blindSearchToolStripMenuItem.Text = "Blind Search";
            // 
            // bFSToolStripMenuItem
            // 
            this.bFSToolStripMenuItem.Name = "bFSToolStripMenuItem";
            this.bFSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.bFSToolStripMenuItem.Text = "Breadth-First Search";
            // 
            // dFSToolStripMenuItem
            // 
            this.dFSToolStripMenuItem.Name = "dFSToolStripMenuItem";
            this.dFSToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dFSToolStripMenuItem.Text = "Depth-First Search";
            // 
            // mainPicBox
            // 
            this.mainPicBox.BackColor = System.Drawing.Color.White;
            this.mainPicBox.Location = new System.Drawing.Point(12, 27);
            this.mainPicBox.Name = "mainPicBox";
            this.mainPicBox.Size = new System.Drawing.Size(690, 622);
            this.mainPicBox.TabIndex = 1;
            this.mainPicBox.TabStop = false;
            this.mainPicBox.Click += new System.EventHandler(this.mainPicBox_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connectVertexButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.toVertexComboBox);
            this.groupBox1.Controls.Add(this.fromVertexComboBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(708, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 112);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connect Vertex (Edge)";
            // 
            // connectVertexButton
            // 
            this.connectVertexButton.Location = new System.Drawing.Point(45, 79);
            this.connectVertexButton.Name = "connectVertexButton";
            this.connectVertexButton.Size = new System.Drawing.Size(75, 23);
            this.connectVertexButton.TabIndex = 6;
            this.connectVertexButton.Text = "Connect";
            this.connectVertexButton.UseVisualStyleBackColor = true;
            this.connectVertexButton.Click += new System.EventHandler(this.connectVertexButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To:";
            // 
            // toVertexComboBox
            // 
            this.toVertexComboBox.FormattingEnabled = true;
            this.toVertexComboBox.Location = new System.Drawing.Point(45, 52);
            this.toVertexComboBox.Name = "toVertexComboBox";
            this.toVertexComboBox.Size = new System.Drawing.Size(113, 21);
            this.toVertexComboBox.TabIndex = 4;
            // 
            // fromVertexComboBox
            // 
            this.fromVertexComboBox.FormattingEnabled = true;
            this.fromVertexComboBox.Location = new System.Drawing.Point(45, 24);
            this.fromVertexComboBox.Name = "fromVertexComboBox";
            this.fromVertexComboBox.Size = new System.Drawing.Size(113, 21);
            this.fromVertexComboBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // showAdjacencyMatrixButton
            // 
            this.showAdjacencyMatrixButton.Location = new System.Drawing.Point(709, 163);
            this.showAdjacencyMatrixButton.Name = "showAdjacencyMatrixButton";
            this.showAdjacencyMatrixButton.Size = new System.Drawing.Size(163, 23);
            this.showAdjacencyMatrixButton.TabIndex = 3;
            this.showAdjacencyMatrixButton.Text = "Show Adjacency Matrix";
            this.showAdjacencyMatrixButton.UseVisualStyleBackColor = true;
            this.showAdjacencyMatrixButton.Click += new System.EventHandler(this.showAdjacencyMatrixButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // VertexAndEdges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.showAdjacencyMatrixButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mainPicBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "VertexAndEdges";
            this.Text = "Vertex and Edges";
            this.Load += new System.EventHandler(this.VertexAndEdges_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPicBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blindSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bFSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dFSToolStripMenuItem;
        private System.Windows.Forms.PictureBox mainPicBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox fromVertexComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox toVertexComboBox;
        private System.Windows.Forms.Button connectVertexButton;
        private System.Windows.Forms.Button showAdjacencyMatrixButton;
        private System.Windows.Forms.ToolStripMenuItem openGraphToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

