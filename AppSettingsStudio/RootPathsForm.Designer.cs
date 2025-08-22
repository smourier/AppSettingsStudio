namespace AppSettingsStudio
{
    partial class RootPathsForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(RootPathsForm));
            tableLayoutMain = new TableLayoutPanel();
            panelButtons = new Panel();
            buttonBrowse = new Button();
            buttonRemove = new Button();
            buttonAdd = new Button();
            buttonOk = new Button();
            buttonCancel = new Button();
            listViewPaths = new ListView();
            columnHeaderPath = new ColumnHeader();
            tableLayoutMain.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutMain
            // 
            resources.ApplyResources(tableLayoutMain, "tableLayoutMain");
            tableLayoutMain.Controls.Add(panelButtons, 0, 1);
            tableLayoutMain.Controls.Add(listViewPaths, 0, 0);
            tableLayoutMain.Name = "tableLayoutMain";
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonBrowse);
            panelButtons.Controls.Add(buttonRemove);
            panelButtons.Controls.Add(buttonAdd);
            panelButtons.Controls.Add(buttonOk);
            panelButtons.Controls.Add(buttonCancel);
            resources.ApplyResources(panelButtons, "panelButtons");
            panelButtons.Name = "panelButtons";
            // 
            // buttonBrowse
            // 
            resources.ApplyResources(buttonBrowse, "buttonBrowse");
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // buttonRemove
            // 
            resources.ApplyResources(buttonRemove, "buttonRemove");
            buttonRemove.Name = "buttonRemove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += ButtonRemove_Click;
            // 
            // buttonAdd
            // 
            resources.ApplyResources(buttonAdd, "buttonAdd");
            buttonAdd.Name = "buttonAdd";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += ButtonAdd_Click;
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
            // listViewPaths
            // 
            listViewPaths.Columns.AddRange(new ColumnHeader[] { columnHeaderPath });
            resources.ApplyResources(listViewPaths, "listViewPaths");
            listViewPaths.FullRowSelect = true;
            listViewPaths.MultiSelect = false;
            listViewPaths.Name = "listViewPaths";
            listViewPaths.UseCompatibleStateImageBehavior = false;
            listViewPaths.View = View.Details;
            listViewPaths.SelectedIndexChanged += ListViewPaths_SelectedIndexChanged;
            // 
            // columnHeaderPath
            // 
            resources.ApplyResources(columnHeaderPath, "columnHeaderPath");
            // 
            // RootPathsForm
            // 
            AcceptButton = buttonOk;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            Controls.Add(tableLayoutMain);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RootPathsForm";
            tableLayoutMain.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutMain;
        private Panel panelButtons;
        private Button buttonOk;
        private Button buttonCancel;
        private ListView listViewPaths;
        private ColumnHeader columnHeaderPath;
        private Button buttonBrowse;
        private Button buttonRemove;
        private Button buttonAdd;
    }
}