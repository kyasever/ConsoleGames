namespace Destroy.Winform
{
    partial class MainForm 
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
            this.groupBoxGameObjects = new System.Windows.Forms.GroupBox();
            this.treeViewLeft = new System.Windows.Forms.TreeView();
            this.panelGame = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBoxComponent = new System.Windows.Forms.GroupBox();
            this.treeViewRight = new System.Windows.Forms.TreeView();
            this.groupBoxMessage = new System.Windows.Forms.GroupBox();
            this.listBoxMessage = new System.Windows.Forms.ListBox();
            this.groupBoxOther = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panelMain = new System.Windows.Forms.Panel();
            this.groupBoxGameObjects.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBoxComponent.SuspendLayout();
            this.groupBoxMessage.SuspendLayout();
            this.groupBoxOther.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGameObjects
            // 
            this.groupBoxGameObjects.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxGameObjects.BackColor = System.Drawing.Color.White;
            this.groupBoxGameObjects.Controls.Add(this.treeViewLeft);
            this.groupBoxGameObjects.Location = new System.Drawing.Point(0, 0);
            this.groupBoxGameObjects.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxGameObjects.Name = "groupBoxGameObjects";
            this.groupBoxGameObjects.Size = new System.Drawing.Size(150, 406);
            this.groupBoxGameObjects.TabIndex = 4;
            this.groupBoxGameObjects.TabStop = false;
            this.groupBoxGameObjects.Text = "GameObjects:";
            // 
            // treeViewLeft
            // 
            this.treeViewLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLeft.Location = new System.Drawing.Point(3, 17);
            this.treeViewLeft.Margin = new System.Windows.Forms.Padding(0);
            this.treeViewLeft.Name = "treeViewLeft";
            this.treeViewLeft.Size = new System.Drawing.Size(144, 386);
            this.treeViewLeft.TabIndex = 0;
            this.treeViewLeft.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panelGame
            // 
            this.panelGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelGame.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelGame.Location = new System.Drawing.Point(150, 0);
            this.panelGame.Margin = new System.Windows.Forms.Padding(0);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(502, 262);
            this.panelGame.TabIndex = 7;
            this.panelGame.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelGame_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.视图ToolStripMenuItem,
            this.编辑ToolStripMenuItem,
            this.操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(830, 25);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.视图ToolStripMenuItem.Text = "视图";
            // 
            // 编辑ToolStripMenuItem
            // 
            this.编辑ToolStripMenuItem.Name = "编辑ToolStripMenuItem";
            this.编辑ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.编辑ToolStripMenuItem.Text = "编辑";
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.操作ToolStripMenuItem.Text = "操作";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(830, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // groupBoxComponent
            // 
            this.groupBoxComponent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxComponent.BackColor = System.Drawing.Color.White;
            this.groupBoxComponent.Controls.Add(this.treeViewRight);
            this.groupBoxComponent.Location = new System.Drawing.Point(652, 0);
            this.groupBoxComponent.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxComponent.Name = "groupBoxComponent";
            this.groupBoxComponent.Size = new System.Drawing.Size(166, 265);
            this.groupBoxComponent.TabIndex = 11;
            this.groupBoxComponent.TabStop = false;
            this.groupBoxComponent.Text = "Component";
            // 
            // treeViewRight
            // 
            this.treeViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewRight.Location = new System.Drawing.Point(3, 17);
            this.treeViewRight.Margin = new System.Windows.Forms.Padding(0);
            this.treeViewRight.Name = "treeViewRight";
            this.treeViewRight.Size = new System.Drawing.Size(160, 245);
            this.treeViewRight.TabIndex = 0;
            // 
            // groupBoxMessage
            // 
            this.groupBoxMessage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxMessage.BackColor = System.Drawing.Color.White;
            this.groupBoxMessage.Controls.Add(this.listBoxMessage);
            this.groupBoxMessage.Location = new System.Drawing.Point(150, 262);
            this.groupBoxMessage.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxMessage.Name = "groupBoxMessage";
            this.groupBoxMessage.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxMessage.Size = new System.Drawing.Size(353, 144);
            this.groupBoxMessage.TabIndex = 12;
            this.groupBoxMessage.TabStop = false;
            this.groupBoxMessage.Text = "Message";
            // 
            // listBoxMessage
            // 
            this.listBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMessage.FormattingEnabled = true;
            this.listBoxMessage.ItemHeight = 12;
            this.listBoxMessage.Location = new System.Drawing.Point(0, 14);
            this.listBoxMessage.Name = "listBoxMessage";
            this.listBoxMessage.Size = new System.Drawing.Size(353, 130);
            this.listBoxMessage.TabIndex = 0;
            // 
            // groupBoxOther
            // 
            this.groupBoxOther.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBoxOther.BackColor = System.Drawing.Color.White;
            this.groupBoxOther.Controls.Add(this.textBox1);
            this.groupBoxOther.Location = new System.Drawing.Point(503, 265);
            this.groupBoxOther.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxOther.Name = "groupBoxOther";
            this.groupBoxOther.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxOther.Size = new System.Drawing.Size(315, 141);
            this.groupBoxOther.TabIndex = 13;
            this.groupBoxOther.TabStop = false;
            this.groupBoxOther.Text = "Others";
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 14);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(315, 127);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "aad \\n 激发时间";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.groupBoxGameObjects);
            this.panelMain.Controls.Add(this.groupBoxMessage);
            this.panelMain.Controls.Add(this.groupBoxOther);
            this.panelMain.Controls.Add(this.groupBoxComponent);
            this.panelMain.Controls.Add(this.panelGame);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(830, 417);
            this.panelMain.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 464);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DestroyEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBoxGameObjects.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxComponent.ResumeLayout(false);
            this.groupBoxMessage.ResumeLayout(false);
            this.groupBoxOther.ResumeLayout(false);
            this.groupBoxOther.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxGameObjects;
        private System.Windows.Forms.TreeView treeViewLeft;
        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBoxComponent;
        private System.Windows.Forms.TreeView treeViewRight;
        private System.Windows.Forms.GroupBox groupBoxMessage;
        private System.Windows.Forms.GroupBox groupBoxOther;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ListBox listBoxMessage;
        private System.Windows.Forms.TextBox textBox1;
    }
}