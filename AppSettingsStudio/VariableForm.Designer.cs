namespace AppSettingsStudio
{
    partial class VariableForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(VariableForm));
            tableLayoutPanelMain = new TableLayoutPanel();
            panelMain = new Panel();
            textBoxValue = new TextBox();
            label2 = new Label();
            textBoxName = new TextBox();
            label1 = new Label();
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
            panelMain.Controls.Add(textBoxValue);
            panelMain.Controls.Add(label2);
            panelMain.Controls.Add(textBoxName);
            panelMain.Controls.Add(label1);
            resources.ApplyResources(panelMain, "panelMain");
            panelMain.Name = "panelMain";
            // 
            // textBoxValue
            // 
            resources.ApplyResources(textBoxValue, "textBoxValue");
            textBoxValue.Name = "textBoxValue";
            textBoxValue.TextChanged += TextBoxValue_TextChanged;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // textBoxName
            // 
            resources.ApplyResources(textBoxName, "textBoxName");
            textBoxName.Name = "textBoxName";
            textBoxName.TextChanged += TextBoxName_TextChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
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
            // VariableForm
            // 
            AcceptButton = buttonOk;
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            Controls.Add(tableLayoutPanelMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "VariableForm";
            tableLayoutPanelMain.ResumeLayout(false);
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanelMain;
        private Panel panelMain;
        private Label label2;
        private Label label1;
        private Panel panelButtons;
        private Button buttonOk;
        private Button buttonCancel;
        internal TextBox textBoxName;
        internal TextBox textBoxValue;
    }
}