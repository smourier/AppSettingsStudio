namespace AppSettingsStudio
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Main));
            menuStripMain = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exportToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            rootPathsToolStripMenuItem = new ToolStripMenuItem();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            variablesToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            openRootDirectoryPathToolStripMenuItem = new ToolStripMenuItem();
            treeToolStripMenuItem = new ToolStripMenuItem();
            flatToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            hierarchical1LevelsToolStripMenuItem = new ToolStripMenuItem();
            hierarchical2LevelsToolStripMenuItem = new ToolStripMenuItem();
            hierarchical3LevelsToolStripMenuItem = new ToolStripMenuItem();
            hierarchicalToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            aboutSettingsStudioToolStripMenuItem = new ToolStripMenuItem();
            statusStripMain = new StatusStrip();
            toolStripStatusLabelLeft = new ToolStripStatusLabel();
            splitContainerMain = new SplitContainer();
            splitContainerTree = new SplitContainer();
            treeViewSettings = new TreeView();
            propertyGridMain = new PropertyGrid();
            contextMenuStripInstance = new ContextMenuStrip(components);
            addVirtualSettingsToolStripMenuItem = new ToolStripMenuItem();
            linkVirtualSettingsToolStripMenuItem = new ToolStripMenuItem();
            importVirtualSettingsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            runToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStripAppSettings = new ContextMenuStrip(components);
            makeActiveToolStripMenuItem = new ToolStripMenuItem();
            openWithDefaultEditorToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            deleteAppSettingToolStripMenuItem = new ToolStripMenuItem();
            menuStripMain.SuspendLayout();
            statusStripMain.SuspendLayout();
            ((ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((ISupportInitialize)splitContainerTree).BeginInit();
            splitContainerTree.Panel1.SuspendLayout();
            splitContainerTree.Panel2.SuspendLayout();
            splitContainerTree.SuspendLayout();
            contextMenuStripInstance.SuspendLayout();
            contextMenuStripAppSettings.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, viewToolStripMenuItem, aboutToolStripMenuItem });
            resources.ApplyResources(menuStripMain, "menuStripMain");
            menuStripMain.Name = "menuStripMain";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, exportToolStripMenuItem, toolStripSeparator4, rootPathsToolStripMenuItem, preferencesToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(fileToolStripMenuItem, "fileToolStripMenuItem");
            fileToolStripMenuItem.DropDownOpening += FileToolStripMenuItem_DropDownOpening;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            resources.ApplyResources(saveToolStripMenuItem, "saveToolStripMenuItem");
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            resources.ApplyResources(exportToolStripMenuItem, "exportToolStripMenuItem");
            exportToolStripMenuItem.Click += ExportToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(toolStripSeparator4, "toolStripSeparator4");
            // 
            // rootPathsToolStripMenuItem
            // 
            rootPathsToolStripMenuItem.Name = "rootPathsToolStripMenuItem";
            resources.ApplyResources(rootPathsToolStripMenuItem, "rootPathsToolStripMenuItem");
            rootPathsToolStripMenuItem.Click += RootPathsToolStripMenuItem_Click;
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            resources.ApplyResources(preferencesToolStripMenuItem, "preferencesToolStripMenuItem");
            preferencesToolStripMenuItem.Click += PreferencesToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { variablesToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // variablesToolStripMenuItem
            // 
            variablesToolStripMenuItem.Name = "variablesToolStripMenuItem";
            resources.ApplyResources(variablesToolStripMenuItem, "variablesToolStripMenuItem");
            variablesToolStripMenuItem.Click += VariablesToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { refreshToolStripMenuItem, toolStripSeparator6, openRootDirectoryPathToolStripMenuItem, treeToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            resources.ApplyResources(viewToolStripMenuItem, "viewToolStripMenuItem");
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            resources.ApplyResources(refreshToolStripMenuItem, "refreshToolStripMenuItem");
            refreshToolStripMenuItem.Click += RefreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(toolStripSeparator6, "toolStripSeparator6");
            // 
            // openRootDirectoryPathToolStripMenuItem
            // 
            openRootDirectoryPathToolStripMenuItem.Name = "openRootDirectoryPathToolStripMenuItem";
            resources.ApplyResources(openRootDirectoryPathToolStripMenuItem, "openRootDirectoryPathToolStripMenuItem");
            openRootDirectoryPathToolStripMenuItem.Click += OpenRootDirectoryPathToolStripMenuItem_Click;
            // 
            // treeToolStripMenuItem
            // 
            treeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { flatToolStripMenuItem, toolStripSeparator7, hierarchical1LevelsToolStripMenuItem, hierarchical2LevelsToolStripMenuItem, hierarchical3LevelsToolStripMenuItem, hierarchicalToolStripMenuItem });
            treeToolStripMenuItem.Name = "treeToolStripMenuItem";
            resources.ApplyResources(treeToolStripMenuItem, "treeToolStripMenuItem");
            treeToolStripMenuItem.DropDownOpening += TreeToolStripMenuItem_DropDownOpening;
            // 
            // flatToolStripMenuItem
            // 
            flatToolStripMenuItem.CheckOnClick = true;
            flatToolStripMenuItem.Name = "flatToolStripMenuItem";
            resources.ApplyResources(flatToolStripMenuItem, "flatToolStripMenuItem");
            flatToolStripMenuItem.Click += FlatToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(toolStripSeparator7, "toolStripSeparator7");
            // 
            // hierarchical1LevelsToolStripMenuItem
            // 
            hierarchical1LevelsToolStripMenuItem.Name = "hierarchical1LevelsToolStripMenuItem";
            resources.ApplyResources(hierarchical1LevelsToolStripMenuItem, "hierarchical1LevelsToolStripMenuItem");
            hierarchical1LevelsToolStripMenuItem.Click += Hierarchical1LevelsToolStripMenuItem_Click;
            // 
            // hierarchical2LevelsToolStripMenuItem
            // 
            hierarchical2LevelsToolStripMenuItem.Name = "hierarchical2LevelsToolStripMenuItem";
            resources.ApplyResources(hierarchical2LevelsToolStripMenuItem, "hierarchical2LevelsToolStripMenuItem");
            hierarchical2LevelsToolStripMenuItem.Click += Hierarchical2LevelsToolStripMenuItem_Click;
            // 
            // hierarchical3LevelsToolStripMenuItem
            // 
            hierarchical3LevelsToolStripMenuItem.Name = "hierarchical3LevelsToolStripMenuItem";
            resources.ApplyResources(hierarchical3LevelsToolStripMenuItem, "hierarchical3LevelsToolStripMenuItem");
            hierarchical3LevelsToolStripMenuItem.Click += Hierarchical3LevelsToolStripMenuItem_Click;
            // 
            // hierarchicalToolStripMenuItem
            // 
            hierarchicalToolStripMenuItem.Name = "hierarchicalToolStripMenuItem";
            resources.ApplyResources(hierarchicalToolStripMenuItem, "hierarchicalToolStripMenuItem");
            hierarchicalToolStripMenuItem.Click += HierarchicalToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutSettingsStudioToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(aboutToolStripMenuItem, "aboutToolStripMenuItem");
            // 
            // aboutSettingsStudioToolStripMenuItem
            // 
            aboutSettingsStudioToolStripMenuItem.Name = "aboutSettingsStudioToolStripMenuItem";
            resources.ApplyResources(aboutSettingsStudioToolStripMenuItem, "aboutSettingsStudioToolStripMenuItem");
            aboutSettingsStudioToolStripMenuItem.Click += AboutSettingsStudioToolStripMenuItem_Click;
            // 
            // statusStripMain
            // 
            statusStripMain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelLeft });
            resources.ApplyResources(statusStripMain, "statusStripMain");
            statusStripMain.Name = "statusStripMain";
            // 
            // toolStripStatusLabelLeft
            // 
            toolStripStatusLabelLeft.Name = "toolStripStatusLabelLeft";
            resources.ApplyResources(toolStripStatusLabelLeft, "toolStripStatusLabelLeft");
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(splitContainerMain, "splitContainerMain");
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(splitContainerTree);
            // 
            // splitContainerTree
            // 
            resources.ApplyResources(splitContainerTree, "splitContainerTree");
            splitContainerTree.Name = "splitContainerTree";
            // 
            // splitContainerTree.Panel1
            // 
            splitContainerTree.Panel1.Controls.Add(treeViewSettings);
            // 
            // splitContainerTree.Panel2
            // 
            splitContainerTree.Panel2.Controls.Add(propertyGridMain);
            // 
            // treeViewSettings
            // 
            resources.ApplyResources(treeViewSettings, "treeViewSettings");
            treeViewSettings.FullRowSelect = true;
            treeViewSettings.HideSelection = false;
            treeViewSettings.LabelEdit = true;
            treeViewSettings.Name = "treeViewSettings";
            treeViewSettings.ShowRootLines = false;
            treeViewSettings.BeforeLabelEdit += TreeViewSettings_BeforeLabelEdit;
            treeViewSettings.AfterLabelEdit += TreeViewSettings_AfterLabelEdit;
            treeViewSettings.BeforeSelect += TreeViewSettings_BeforeSelect;
            treeViewSettings.AfterSelect += TreeViewSettings_AfterSelect;
            treeViewSettings.MouseClick += TreeViewSettings_MouseClick;
            // 
            // propertyGridMain
            // 
            resources.ApplyResources(propertyGridMain, "propertyGridMain");
            propertyGridMain.Name = "propertyGridMain";
            propertyGridMain.PropertySort = PropertySort.NoSort;
            propertyGridMain.ToolbarVisible = false;
            // 
            // contextMenuStripInstance
            // 
            contextMenuStripInstance.Items.AddRange(new ToolStripItem[] { addVirtualSettingsToolStripMenuItem, linkVirtualSettingsToolStripMenuItem, importVirtualSettingsToolStripMenuItem, toolStripSeparator5, runToolStripMenuItem, toolStripSeparator2, deleteToolStripMenuItem });
            contextMenuStripInstance.Name = "contextMenuStripTree";
            resources.ApplyResources(contextMenuStripInstance, "contextMenuStripInstance");
            // 
            // addVirtualSettingsToolStripMenuItem
            // 
            addVirtualSettingsToolStripMenuItem.Name = "addVirtualSettingsToolStripMenuItem";
            resources.ApplyResources(addVirtualSettingsToolStripMenuItem, "addVirtualSettingsToolStripMenuItem");
            addVirtualSettingsToolStripMenuItem.Click += AddVirtualSettingsToolStripMenuItem_Click;
            // 
            // linkVirtualSettingsToolStripMenuItem
            // 
            linkVirtualSettingsToolStripMenuItem.Name = "linkVirtualSettingsToolStripMenuItem";
            resources.ApplyResources(linkVirtualSettingsToolStripMenuItem, "linkVirtualSettingsToolStripMenuItem");
            linkVirtualSettingsToolStripMenuItem.Click += LinkVirtualSettingsToolStripMenuItem_Click;
            // 
            // importVirtualSettingsToolStripMenuItem
            // 
            importVirtualSettingsToolStripMenuItem.Name = "importVirtualSettingsToolStripMenuItem";
            resources.ApplyResources(importVirtualSettingsToolStripMenuItem, "importVirtualSettingsToolStripMenuItem");
            importVirtualSettingsToolStripMenuItem.Click += ImportVirtualSettingsToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(toolStripSeparator5, "toolStripSeparator5");
            // 
            // runToolStripMenuItem
            // 
            runToolStripMenuItem.Name = "runToolStripMenuItem";
            resources.ApplyResources(runToolStripMenuItem, "runToolStripMenuItem");
            runToolStripMenuItem.Click += RunToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(deleteToolStripMenuItem, "deleteToolStripMenuItem");
            deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // contextMenuStripAppSettings
            // 
            contextMenuStripAppSettings.Items.AddRange(new ToolStripItem[] { makeActiveToolStripMenuItem, openWithDefaultEditorToolStripMenuItem, toolStripSeparator3, deleteAppSettingToolStripMenuItem });
            contextMenuStripAppSettings.Name = "contextMenuStripAppSettings";
            resources.ApplyResources(contextMenuStripAppSettings, "contextMenuStripAppSettings");
            contextMenuStripAppSettings.Opening += ContextMenuStripAppSettings_Opening;
            // 
            // makeActiveToolStripMenuItem
            // 
            makeActiveToolStripMenuItem.Name = "makeActiveToolStripMenuItem";
            resources.ApplyResources(makeActiveToolStripMenuItem, "makeActiveToolStripMenuItem");
            makeActiveToolStripMenuItem.Click += MakeActiveToolStripMenuItem_Click;
            // 
            // openWithDefaultEditorToolStripMenuItem
            // 
            openWithDefaultEditorToolStripMenuItem.Name = "openWithDefaultEditorToolStripMenuItem";
            resources.ApplyResources(openWithDefaultEditorToolStripMenuItem, "openWithDefaultEditorToolStripMenuItem");
            openWithDefaultEditorToolStripMenuItem.Click += OpenWithDefaultEditorToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(toolStripSeparator3, "toolStripSeparator3");
            // 
            // deleteAppSettingToolStripMenuItem
            // 
            deleteAppSettingToolStripMenuItem.Name = "deleteAppSettingToolStripMenuItem";
            resources.ApplyResources(deleteAppSettingToolStripMenuItem, "deleteAppSettingToolStripMenuItem");
            deleteAppSettingToolStripMenuItem.Click += DeleteAppSettingToolStripMenuItem_Click;
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Controls.Add(statusStripMain);
            Controls.Add(menuStripMain);
            KeyPreview = true;
            MainMenuStrip = menuStripMain;
            Name = "Main";
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            ((ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerTree.Panel1.ResumeLayout(false);
            splitContainerTree.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainerTree).EndInit();
            splitContainerTree.ResumeLayout(false);
            contextMenuStripInstance.ResumeLayout(false);
            contextMenuStripAppSettings.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLeft;
        private System.Windows.Forms.SplitContainer splitContainerTree;
        private System.Windows.Forms.TreeView treeViewSettings;
        private System.Windows.Forms.PropertyGrid propertyGridMain;
        private ContextMenuStrip contextMenuStripInstance;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutSettingsStudioToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem variablesToolStripMenuItem;
        private ToolStripMenuItem addVirtualSettingsToolStripMenuItem;
        private ToolStripMenuItem importVirtualSettingsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ContextMenuStrip contextMenuStripAppSettings;
        private ToolStripMenuItem makeActiveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem deleteAppSettingToolStripMenuItem;
        private ToolStripMenuItem linkVirtualSettingsToolStripMenuItem;
        private ToolStripMenuItem openWithDefaultEditorToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem runToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem openRootDirectoryPathToolStripMenuItem;
        internal SplitContainer splitContainerMain;
        private ToolStripMenuItem rootPathsToolStripMenuItem;
        private ToolStripMenuItem treeToolStripMenuItem;
        private ToolStripMenuItem flatToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem hierarchical1LevelsToolStripMenuItem;
        private ToolStripMenuItem hierarchical2LevelsToolStripMenuItem;
        private ToolStripMenuItem hierarchical3LevelsToolStripMenuItem;
        private ToolStripMenuItem hierarchicalToolStripMenuItem;
    }
}
