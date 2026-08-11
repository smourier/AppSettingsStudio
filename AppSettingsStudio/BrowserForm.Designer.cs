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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(BrowserForm));
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
            resources.ApplyResources(tableLayoutPanelMain, "tableLayoutPanelMain");
            tableLayoutPanelMain.Controls.Add(panelMain, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelButtons, 0, 1);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            // 
            // panelMain
            // 
            panelMain.Controls.Add(treeViewSettings);
            resources.ApplyResources(panelMain, "panelMain");
            panelMain.Name = "panelMain";
            // 
            // treeViewSettings
            // 
            resources.ApplyResources(treeViewSettings, "treeViewSettings");
            treeViewSettings.FullRowSelect = true;
            treeViewSettings.HideSelection = false;
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.ShowRootLines = false;
            treeViewSettings.AfterSelect += TreeViewSettings_AfterSelect;
            treeViewSettings.MouseDoubleClick += TreeViewSettings_MouseDoubleClick;
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonOk);
            panelButtons.Controls.Add(buttonCancel);
            resources.ApplyResources(panelButtons, "panelButtons");
            panelButtons.Name = "panelButtons";
            // 
            // buttonOk
            // 
            resources.ApplyResources(buttonOk, "buttonOk");
            buttonOk.DialogResult = DialogResult.OK;
            buttonOk.Name = "buttonOk";
            buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(buttonCancel, "buttonCancel");
            buttonCancel.CausesValidation = false;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // BrowserForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelMain);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BrowserForm";
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