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
        components = new System.ComponentModel.Container();
        toolStrip = new ClickThroughToolStrip();
        leftLabel = new ToolStripLabel();
        leftButton = new ToolStripButton();
        rightLabel = new ToolStripLabel();
        rightButton = new ToolStripButton();
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
        toolStrip.Items.AddRange(new ToolStripItem[] { leftLabel, leftButton, rightLabel, rightButton, toolStripSeparator1, optSideBySide, optIgnoreWhitespace, optWordWrap, optShowWhitespace, toolStripSeparator2, refreshButton });
        toolStrip.Location = new Point(0, 0);
        toolStrip.Name = "toolStrip";
        toolStrip.Size = new Size(1000, 25);
        toolStrip.TabIndex = 0;
        //
        // leftLabel
        //
        leftLabel.Name = "leftLabel";
        leftLabel.Text = "Left:";
        //
        // leftButton
        //
        leftButton.Name = "leftButton";
        leftButton.ToolTipText = "Choose the left side";
        leftButton.Click += OnLeftClick;
        //
        // rightLabel
        //
        rightLabel.Name = "rightLabel";
        rightLabel.Text = "Right:";
        //
        // rightButton
        //
        rightButton.Name = "rightButton";
        rightButton.ToolTipText = "Choose the right side";
        rightButton.Click += OnRightClick;
        //
        // toolStripSeparator1
        //
        toolStripSeparator1.Name = "toolStripSeparator1";
        //
        // optSideBySide
        //
        optSideBySide.CheckOnClick = true;
        optSideBySide.Name = "optSideBySide";
        optSideBySide.Text = "Side by side";
        optSideBySide.Click += OnOptionChanged;
        //
        // optIgnoreWhitespace
        //
        optIgnoreWhitespace.CheckOnClick = true;
        optIgnoreWhitespace.Name = "optIgnoreWhitespace";
        optIgnoreWhitespace.Text = "Ignore whitespace";
        optIgnoreWhitespace.Click += OnOptionChanged;
        //
        // optWordWrap
        //
        optWordWrap.CheckOnClick = true;
        optWordWrap.Name = "optWordWrap";
        optWordWrap.Text = "Word wrap";
        optWordWrap.Click += OnOptionChanged;
        //
        // optShowWhitespace
        //
        optShowWhitespace.CheckOnClick = true;
        optShowWhitespace.Name = "optShowWhitespace";
        optShowWhitespace.Text = "Show whitespace";
        optShowWhitespace.Click += OnOptionChanged;
        //
        // toolStripSeparator2
        //
        toolStripSeparator2.Name = "toolStripSeparator2";
        //
        // refreshButton
        //
        refreshButton.Name = "refreshButton";
        refreshButton.Text = "Refresh";
        refreshButton.Click += OnRefreshClick;
        //
        // statusStrip
        //
        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 678);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1000, 22);
        statusStrip.TabIndex = 1;
        //
        // statusLabel
        //
        statusLabel.Name = "statusLabel";
        statusLabel.Spring = true;
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // webView
        //
        webView.CreationProperties = null;
        webView.Dock = DockStyle.Fill;
        webView.Location = new Point(0, 25);
        webView.Name = "webView";
        webView.Size = new Size(1000, 653);
        webView.TabIndex = 2;
        webView.ZoomFactor = 1D;
        //
        // DiffForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 700);
        Controls.Add(webView);
        Controls.Add(toolStrip);
        Controls.Add(statusStrip);
        Name = "DiffForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Compare";
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
