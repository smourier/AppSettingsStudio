namespace AppSettingsStudio
{
    partial class BrowserForm
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
            tableLayoutPanelMain = new TableLayoutPanel();
            panelMain = new Panel();
            treeViewSettings = new TreeView();
            panelButtons = new Panel();
            buttonOk = new Button();
            buttonCancel = new Button();
            tableLayoutPanelMain.SuspendLayout();
            panelMain.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 1;
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanelMain.Controls.Add(panelMain, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelButtons, 0, 1);
            tableLayoutPanelMain.Dock = DockStyle.Fill;
            tableLayoutPanelMain.Location = new Point(0, 0);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 2;
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanelMain.Size = new Size(800, 450);
            tableLayoutPanelMain.TabIndex = 2;
            // 
            // panelMain
            // 
            panelMain.Controls.Add(treeViewSettings);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(3, 3);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(794, 414);
            panelMain.TabIndex = 0;
            // 
            // treeViewSettings
            // 
            treeViewSettings.Dock = DockStyle.Fill;
            treeViewSettings.FullRowSelect = true;
            treeViewSettings.HideSelection = false;
            treeViewSettings.Location = new Point(0, 0);
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.ShowRootLines = false;
            treeViewSettings.Size = new Size(794, 414);
            treeViewSettings.TabIndex = 4;
            treeViewSettings.AfterSelect += TreeViewSettings_AfterSelect;
            treeViewSettings.MouseDoubleClick += TreeViewSettings_MouseDoubleClick;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonOk);
            panelButtons.Controls.Add(buttonCancel);
            panelButtons.Dock = DockStyle.Fill;
            panelButtons.Location = new Point(0, 420);
            panelButtons.Margin = new Padding(0);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(800, 30);
            panelButtons.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Anchor = AnchorStyles.Right;
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.ImeMode = ImeMode.NoControl;
            buttonOk.Location = new Point(612, 0);
            buttonOk.Margin = new Padding(4, 3, 4, 3);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(88, 27);
            buttonOk.TabIndex = 0;
            buttonOk.Text = "&OK";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = AnchorStyles.Right;
            buttonCancel.CausesValidation = false;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.ImeMode = ImeMode.NoControl;
            buttonCancel.Location = new Point(708, 0);
            buttonCancel.Margin = new Padding(4, 3, 4, 3);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(88, 27);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "&Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // BrowserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanelMain);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BrowserForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Link to an existing Virtual Settings";
            tableLayoutPanelMain.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private Panel panelMain;
        private Panel panelButtons;
        private Button buttonOk;
        private Button buttonCancel;
        private TreeView treeViewSettings;
    }
}