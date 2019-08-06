namespace Destroy.Winform
{
    partial class FormEditor
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
            this.顶部信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMousePosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelCom = new System.Windows.Forms.Panel();
            this.treeViewCom = new System.Windows.Forms.TreeView();
            this.panelObj = new System.Windows.Forms.Panel();
            this.treeViewObj = new System.Windows.Forms.TreeView();
            this.panelGame = new System.Windows.Forms.Panel();
            this.panelMsg = new System.Windows.Forms.Panel();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.panelGameWindow = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.panelCom.SuspendLayout();
            this.panelObj.SuspendLayout();
            this.panelGame.SuspendLayout();
            this.panelMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.顶部信息ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(720, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 顶部信息ToolStripMenuItem
            // 
            this.顶部信息ToolStripMenuItem.Name = "顶部信息ToolStripMenuItem";
            this.顶部信息ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.顶部信息ToolStripMenuItem.Text = "顶部信息";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelMousePosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 371);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(720, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel1.Text = "底部信息";
            // 
            // toolStripStatusLabelMousePosition
            // 
            this.toolStripStatusLabelMousePosition.Name = "toolStripStatusLabelMousePosition";
            this.toolStripStatusLabelMousePosition.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabelMousePosition.Text = "toolStripStatusLabel2";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelEditor);
            this.panelMain.Controls.Add(this.panelGame);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(720, 346);
            this.panelMain.TabIndex = 2;
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.splitter1);
            this.panelEditor.Controls.Add(this.panelCom);
            this.panelEditor.Controls.Add(this.panelObj);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(342, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(378, 346);
            this.panelEditor.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 346);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // panelCom
            // 
            this.panelCom.Controls.Add(this.treeViewCom);
            this.panelCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCom.Location = new System.Drawing.Point(200, 0);
            this.panelCom.Name = "panelCom";
            this.panelCom.Size = new System.Drawing.Size(178, 346);
            this.panelCom.TabIndex = 1;
            // 
            // treeViewCom
            // 
            this.treeViewCom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCom.Location = new System.Drawing.Point(0, 0);
            this.treeViewCom.Name = "treeViewCom";
            this.treeViewCom.Size = new System.Drawing.Size(178, 346);
            this.treeViewCom.TabIndex = 0;
            // 
            // panelObj
            // 
            this.panelObj.Controls.Add(this.treeViewObj);
            this.panelObj.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelObj.Location = new System.Drawing.Point(0, 0);
            this.panelObj.Name = "panelObj";
            this.panelObj.Size = new System.Drawing.Size(200, 346);
            this.panelObj.TabIndex = 0;
            // 
            // treeViewObj
            // 
            this.treeViewObj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewObj.Location = new System.Drawing.Point(0, 0);
            this.treeViewObj.Name = "treeViewObj";
            this.treeViewObj.Size = new System.Drawing.Size(200, 346);
            this.treeViewObj.TabIndex = 0;
            this.treeViewObj.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewObj_AfterSelect);
            // 
            // panelGame
            // 
            this.panelGame.Controls.Add(this.panelMsg);
            this.panelGame.Controls.Add(this.panelGameWindow);
            this.panelGame.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelGame.Location = new System.Drawing.Point(0, 0);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(342, 346);
            this.panelGame.TabIndex = 0;
            // 
            // panelMsg
            // 
            this.panelMsg.Controls.Add(this.listBoxMsg);
            this.panelMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMsg.Location = new System.Drawing.Point(0, 191);
            this.panelMsg.Name = "panelMsg";
            this.panelMsg.Size = new System.Drawing.Size(342, 155);
            this.panelMsg.TabIndex = 1;
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 12;
            this.listBoxMsg.Location = new System.Drawing.Point(0, 0);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(342, 155);
            this.listBoxMsg.TabIndex = 0;
            // 
            // panelGameWindow
            // 
            this.panelGameWindow.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGameWindow.Location = new System.Drawing.Point(0, 0);
            this.panelGameWindow.Name = "panelGameWindow";
            this.panelGameWindow.Size = new System.Drawing.Size(342, 191);
            this.panelGameWindow.TabIndex = 0;
            this.panelGameWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelGameWindow_MouseMove);
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 393);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormEditor";
            this.Text = "Destroy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditor_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEditor_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelEditor.ResumeLayout(false);
            this.panelCom.ResumeLayout(false);
            this.panelObj.ResumeLayout(false);
            this.panelGame.ResumeLayout(false);
            this.panelMsg.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStripMenuItem 顶部信息ToolStripMenuItem;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.Panel panelMsg;
        private System.Windows.Forms.Panel panelGameWindow;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panelCom;
        private System.Windows.Forms.TreeView treeViewCom;
        private System.Windows.Forms.Panel panelObj;
        private System.Windows.Forms.TreeView treeViewObj;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMousePosition;
    }
}