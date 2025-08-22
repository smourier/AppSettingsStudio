namespace AppSettingsStudio.Monaco;

partial class MonacoEditorControl
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        WebView = new Microsoft.Web.WebView2.WinForms.WebView2();
        ((System.ComponentModel.ISupportInitialize)WebView).BeginInit();
        SuspendLayout();
        // 
        // WebView
        // 
        WebView.AllowExternalDrop = false;
        WebView.CreationProperties = null;
        WebView.DefaultBackgroundColor = System.Drawing.Color.White;
        WebView.Name = "WebView";
        WebView.Size = new System.Drawing.Size(0, 0);
        WebView.TabIndex = 0;
        WebView.ZoomFactor = 1D;
        ((System.ComponentModel.ISupportInitialize)WebView).EndInit();
        ResumeLayout(false);
    }

    #endregion

    public Microsoft.Web.WebView2.WinForms.WebView2 WebView;
}
