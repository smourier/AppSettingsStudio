namespace AppSettingsStudio;

partial class AboutForm
{
    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ComponentResourceManager resources = new ComponentResourceManager(typeof(AboutForm));
        tableLayoutPanelMain = new TableLayoutPanel();
        buttonOk = new Button();
        pictureBoxIcon = new PictureBox();
        labelText = new Label();
        tableLayoutPanelMain.SuspendLayout();
        ((ISupportInitialize)pictureBoxIcon).BeginInit();
        SuspendLayout();
        // 
        // tableLayoutPanelMain
        // 
        resources.ApplyResources(tableLayoutPanelMain, "tableLayoutPanelMain");
        tableLayoutPanelMain.Controls.Add(buttonOk, 1, 1);
        tableLayoutPanelMain.Controls.Add(pictureBoxIcon, 0, 0);
        tableLayoutPanelMain.Controls.Add(labelText, 1, 0);
        tableLayoutPanelMain.Name = "tableLayoutPanelMain";
        // 
        // buttonOk
        // 
        resources.ApplyResources(buttonOk, "buttonOk");
        buttonOk.DialogResult = DialogResult.OK;
        buttonOk.Name = "buttonOk";
        buttonOk.UseVisualStyleBackColor = true;
        // 
        // pictureBoxIcon
        // 
        resources.ApplyResources(pictureBoxIcon, "pictureBoxIcon");
        pictureBoxIcon.Name = "pictureBoxIcon";
        pictureBoxIcon.TabStop = false;
        // 
        // labelText
        // 
        resources.ApplyResources(labelText, "labelText");
        labelText.Name = "labelText";
        // 
        // AboutForm
        // 
        AcceptButton = buttonOk;
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = buttonOk;
        Controls.Add(tableLayoutPanelMain);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        tableLayoutPanelMain.ResumeLayout(false);
        tableLayoutPanelMain.PerformLayout();
        ((ISupportInitialize)pictureBoxIcon).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.PictureBox pictureBoxIcon;
    private System.Windows.Forms.Label labelText;
}