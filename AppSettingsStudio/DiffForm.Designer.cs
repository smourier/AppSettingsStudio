namespace AppSettingsStudio;

partial class DiffForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        ComponentResourceManager resources = new ComponentResourceManager(typeof(DiffForm));
        toolStrip = new ClickThroughToolStrip();
        leftLabel = new ToolStripLabel();
        leftButton = new ToolStripButton();
        rightLabel = new ToolStripLabel();
        rightButton = new ToolStripButton();
        swapButton = new ToolStripButton();
        toolStripSeparator1 = new ToolStripSeparator();
        optSideBySide = new ToolStripButton();
        optIgnoreWhitespace = new ToolStripButton();
        optWordWrap = new ToolStripButton();
        optShowWhitespace = new ToolStripButton();
        toolStripSeparator2 = new ToolStripSeparator();
        refreshButton = new ToolStripButton();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        webView = new WebView2();
        toolStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        ((ISupportInitialize)webView).BeginInit();
        SuspendLayout();
        // 
        // toolStrip
        // 
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.Items.AddRange(new ToolStripItem[] { leftLabel, leftButton, rightLabel, rightButton, swapButton, toolStripSeparator1, optSideBySide, optIgnoreWhitespace, optWordWrap, optShowWhitespace, toolStripSeparator2, refreshButton });
        resources.ApplyResources(toolStrip, "toolStrip");
        toolStrip.Name = "toolStrip";
        // 
        // leftLabel
        // 
        leftLabel.Name = "leftLabel";
        resources.ApplyResources(leftLabel, "leftLabel");
        // 
        // leftButton
        // 
        leftButton.Name = "leftButton";
        resources.ApplyResources(leftButton, "leftButton");
        leftButton.Click += OnLeftClick;
        // 
        // rightLabel
        // 
        rightLabel.Name = "rightLabel";
        resources.ApplyResources(rightLabel, "rightLabel");
        // 
        // rightButton
        // 
        rightButton.Name = "rightButton";
        resources.ApplyResources(rightButton, "rightButton");
        rightButton.Click += OnRightClick;
        //
        // swapButton
        //
        swapButton.Name = "swapButton";
        resources.ApplyResources(swapButton, "swapButton");
        swapButton.Click += OnSwapClick;
        //
        // toolStripSeparator1
        //
        toolStripSeparator1.Name = "toolStripSeparator1";
        resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
        // 
        // optSideBySide
        // 
        optSideBySide.CheckOnClick = true;
        optSideBySide.Name = "optSideBySide";
        resources.ApplyResources(optSideBySide, "optSideBySide");
        optSideBySide.Click += OnOptionChanged;
        // 
        // optIgnoreWhitespace
        // 
        optIgnoreWhitespace.CheckOnClick = true;
        optIgnoreWhitespace.Name = "optIgnoreWhitespace";
        resources.ApplyResources(optIgnoreWhitespace, "optIgnoreWhitespace");
        optIgnoreWhitespace.Click += OnOptionChanged;
        // 
        // optWordWrap
        // 
        optWordWrap.CheckOnClick = true;
        optWordWrap.Name = "optWordWrap";
        resources.ApplyResources(optWordWrap, "optWordWrap");
        optWordWrap.Click += OnOptionChanged;
        // 
        // optShowWhitespace
        // 
        optShowWhitespace.CheckOnClick = true;
        optShowWhitespace.Name = "optShowWhitespace";
        resources.ApplyResources(optShowWhitespace, "optShowWhitespace");
        optShowWhitespace.Click += OnOptionChanged;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
        // 
        // refreshButton
        // 
        refreshButton.Name = "refreshButton";
        resources.ApplyResources(refreshButton, "refreshButton");
        refreshButton.Click += OnRefreshClick;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        resources.ApplyResources(statusStrip, "statusStrip");
        statusStrip.Name = "statusStrip";
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        resources.ApplyResources(statusLabel, "statusLabel");
        statusLabel.Spring = true;
        // 
        // webView
        // 
        webView.AllowExternalDrop = true;
        webView.CreationProperties = null;
        webView.DefaultBackgroundColor = Color.White;
        resources.ApplyResources(webView, "webView");
        webView.Name = "webView";
        webView.ZoomFactor = 1D;
        // 
        // DiffForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(webView);
        Controls.Add(toolStrip);
        Controls.Add(statusStrip);
        MinimizeBox = false;
        Name = "DiffForm";
        toolStrip.ResumeLayout(false);
        toolStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ((ISupportInitialize)webView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private ClickThroughToolStrip toolStrip;
    private ToolStripLabel leftLabel;
    private ToolStripButton leftButton;
    private ToolStripLabel rightLabel;
    private ToolStripButton rightButton;
    private ToolStripButton swapButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton optSideBySide;
    private ToolStripButton optIgnoreWhitespace;
    private ToolStripButton optWordWrap;
    private ToolStripButton optShowWhitespace;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripButton refreshButton;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private WebView2 webView;
}
