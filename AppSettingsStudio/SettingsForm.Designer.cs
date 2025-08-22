using System.Windows.Forms;
using System.Xml.Linq;

namespace AppSettingsStudio;

partial class SettingsForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
        tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
        panelButtons = new System.Windows.Forms.Panel();
        buttonOk = new System.Windows.Forms.Button();
        buttonCancel = new System.Windows.Forms.Button();
        propertyGridSettings = new System.Windows.Forms.PropertyGrid();
        tableLayoutPanelMain.SuspendLayout();
        panelButtons.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanelMain
        // 
        resources.ApplyResources(tableLayoutPanelMain, "tableLayoutPanelMain");
        tableLayoutPanelMain.Controls.Add(panelButtons, 0, 1);
        tableLayoutPanelMain.Controls.Add(propertyGridSettings, 0, 0);
        tableLayoutPanelMain.Name = "tableLayoutPanelMain";
        // 
        // panelButtons
        // 
        resources.ApplyResources(panelButtons, "panelButtons");
        panelButtons.Controls.Add(buttonOk);
        panelButtons.Controls.Add(buttonCancel);
        panelButtons.Name = "panelButtons";
        // 
        // buttonOk
        // 
        resources.ApplyResources(buttonOk, "buttonOk");
        buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
        buttonOk.Name = "buttonOk";
        buttonOk.UseVisualStyleBackColor = true;
        // 
        // buttonCancel
        // 
        resources.ApplyResources(buttonCancel, "buttonCancel");
        buttonCancel.CausesValidation = false;
        buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        buttonCancel.Name = "buttonCancel";
        buttonCancel.UseVisualStyleBackColor = true;
        // 
        // propertyGridSettings
        // 
        resources.ApplyResources(propertyGridSettings, "propertyGridSettings");
        propertyGridSettings.Name = "propertyGridSettings";
        propertyGridSettings.ToolbarVisible = false;
        // 
        // SettingsForm
        // 
        AcceptButton = buttonOk;
        resources.ApplyResources(this, "$this");
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = buttonCancel;
        Controls.Add(tableLayoutPanelMain);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SettingsForm";
        tableLayoutPanelMain.ResumeLayout(false);
        panelButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
    private System.Windows.Forms.Panel panelButtons;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.PropertyGrid propertyGridSettings;
}