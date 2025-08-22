namespace AppSettingsStudio
{
    partial class VariablesForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(VariablesForm));
            tableLayoutPanelMain = new TableLayoutPanel();
            listViewVariables = new ListView();
            columnHeaderName = new ColumnHeader();
            columnHeaderValue = new ColumnHeader();
            panelButtons = new Panel();
            buttonRemove = new Button();
            buttonModify = new Button();
            buttonCancel = new Button();
            buttonAdd = new Button();
            tableLayoutPanelMain.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            resources.ApplyResources(tableLayoutPanelMain, "tableLayoutPanelMain");
            tableLayoutPanelMain.Controls.Add(listViewVariables, 0, 0);
            tableLayoutPanelMain.Controls.Add(panelButtons, 0, 1);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            // 
            // listViewVariables
            // 
            listViewVariables.Columns.AddRange(new ColumnHeader[] { columnHeaderName, columnHeaderValue });
            resources.ApplyResources(listViewVariables, "listViewVariables");
            listViewVariables.FullRowSelect = true;
            listViewVariables.MultiSelect = false;
            listViewVariables.Name = "listViewVariables";
            listViewVariables.UseCompatibleStateImageBehavior = false;
            listViewVariables.View = View.Details;
            listViewVariables.SelectedIndexChanged += ListViewVariables_SelectedIndexChanged;
            listViewVariables.MouseDoubleClick += ListViewVariables_MouseDoubleClick;
            // 
            // columnHeaderName
            // 
            resources.ApplyResources(columnHeaderName, "columnHeaderName");
            // 
            // columnHeaderValue
            // 
            resources.ApplyResources(columnHeaderValue, "columnHeaderValue");
            // 
            // panelButtons
            // 
            panelButtons.Controls.Add(buttonRemove);
            panelButtons.Controls.Add(buttonModify);
            panelButtons.Controls.Add(buttonCancel);
            panelButtons.Controls.Add(buttonAdd);
            resources.ApplyResources(panelButtons, "panelButtons");
            panelButtons.Name = "panelButtons";
            // 
            // buttonRemove
            // 
            resources.ApplyResources(buttonRemove, "buttonRemove");
            buttonRemove.CausesValidation = false;
            buttonRemove.Name = "buttonRemove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += ButtonRemove_Click;
            // 
            // buttonModify
            // 
            resources.ApplyResources(buttonModify, "buttonModify");
            buttonModify.CausesValidation = false;
            buttonModify.Name = "buttonModify";
            buttonModify.UseVisualStyleBackColor = true;
            buttonModify.Click += ButtonModify_Click;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(buttonCancel, "buttonCancel");
            buttonCancel.CausesValidation = false;
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Name = "buttonCancel";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            resources.ApplyResources(buttonAdd, "buttonAdd");
            buttonAdd.CausesValidation = false;
            buttonAdd.Name = "buttonAdd";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += ButtonAdd_Click;
            // 
            // VariablesForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanelMain);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "VariablesForm";
            tableLayoutPanelMain.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private ListView listViewVariables;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderValue;
        private Panel panelButtons;
        private Button buttonCancel;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonModify;
    }
}