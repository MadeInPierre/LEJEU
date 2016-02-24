using Microsoft.Xna.Framework;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LevelEditor
{
    partial class MainForm
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
            Application.Exit();
            base.Dispose(disposing);
        }

        //private SettingsForm settingsForm = new SettingsForm();
        



        #region Windows Form Designer generated code





        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainSplit = new System.Windows.Forms.SplitContainer();
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabLevel = new System.Windows.Forms.TabPage();
            this.XNAWindow = new System.Windows.Forms.PictureBox();
            this.TabXML = new System.Windows.Forms.TabPage();
            this.XMLText = new System.Windows.Forms.TextBox();
            this.PropertiesSplit = new System.Windows.Forms.SplitContainer();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.btnParentProperty = new System.Windows.Forms.Button();
            this.btnOpenProperty = new System.Windows.Forms.Button();
            this.SliderTime = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.CollisionsModeButton = new System.Windows.Forms.ToolStripButton();
            this.SectorsModeButton = new System.Windows.Forms.ToolStripButton();
            this.SpawnsModeButton = new System.Windows.Forms.ToolStripButton();
            this.TCCModeButton = new System.Windows.Forms.ToolStripButton();
            this.DictionaryDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.collisionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryEnnemies = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryDecoration = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryItems = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryTraps = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryMain = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryPlayerspawn = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryEnd = new System.Windows.Forms.ToolStripMenuItem();
            this.DictionaryGoal = new System.Windows.Forms.ToolStripMenuItem();
            this.interactiveObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNoState = new System.Windows.Forms.ToolStripButton();
            this.btnEditVertices = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.BtnPolygon = new System.Windows.Forms.ToolStripButton();
            this.BtnRectangle = new System.Windows.Forms.ToolStripButton();
            this.BtnCircle = new System.Windows.Forms.ToolStripButton();
            this.BtnEdge = new System.Windows.Forms.ToolStripButton();
            this.btnSpawn = new System.Windows.Forms.ToolStripButton();
            this.LblCurrentYear = new System.Windows.Forms.ToolStripLabel();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLayers = new System.Windows.Forms.ToolStripButton();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.FileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.FileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.FileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.FileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.EditSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsCreateEnnemy = new System.Windows.Forms.ToolStripMenuItem();
            this.TCCTools = new System.Windows.Forms.ToolStripMenuItem();
            this.effectsPickerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.RunReleaseMode = new System.Windows.Forms.ToolStripMenuItem();
            this.RunStop = new System.Windows.Forms.ToolStripMenuItem();
            this.PosLabel = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.currentStateLabel = new System.Windows.Forms.Label();
            this.UpDownScenario = new System.Windows.Forms.NumericUpDown();
            this.currentModeLabel = new System.Windows.Forms.Label();
            this.rBtnPrecision = new System.Windows.Forms.RadioButton();
            this.rBtnScenario = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
            this.MainSplit.Panel1.SuspendLayout();
            this.MainSplit.Panel2.SuspendLayout();
            this.MainSplit.SuspendLayout();
            this.TabControl1.SuspendLayout();
            this.TabLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.XNAWindow)).BeginInit();
            this.TabXML.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplit)).BeginInit();
            this.PropertiesSplit.Panel1.SuspendLayout();
            this.PropertiesSplit.Panel2.SuspendLayout();
            this.PropertiesSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SliderTime)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownScenario)).BeginInit();
            this.SuspendLayout();
            // 
            // MainSplit
            // 
            this.MainSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainSplit.Location = new System.Drawing.Point(18, 171);
            this.MainSplit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MainSplit.Name = "MainSplit";
            // 
            // MainSplit.Panel1
            // 
            this.MainSplit.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.MainSplit.Panel1.Controls.Add(this.TabControl1);
            // 
            // MainSplit.Panel2
            // 
            this.MainSplit.Panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MainSplit.Panel2.Controls.Add(this.PropertiesSplit);
            this.MainSplit.Size = new System.Drawing.Size(1120, 500);
            this.MainSplit.SplitterDistance = 902;
            this.MainSplit.SplitterWidth = 6;
            this.MainSplit.TabIndex = 11;
            // 
            // TabControl1
            // 
            this.TabControl1.Controls.Add(this.TabLevel);
            this.TabControl1.Controls.Add(this.TabXML);
            this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl1.Location = new System.Drawing.Point(0, 0);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(902, 500);
            this.TabControl1.TabIndex = 0;
            // 
            // TabLevel
            // 
            this.TabLevel.Controls.Add(this.XNAWindow);
            this.TabLevel.Location = new System.Drawing.Point(4, 29);
            this.TabLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabLevel.Name = "TabLevel";
            this.TabLevel.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabLevel.Size = new System.Drawing.Size(894, 467);
            this.TabLevel.TabIndex = 0;
            this.TabLevel.Text = "Level";
            this.TabLevel.UseVisualStyleBackColor = true;
            // 
            // XNAWindow
            // 
            this.XNAWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XNAWindow.Location = new System.Drawing.Point(4, 5);
            this.XNAWindow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.XNAWindow.Name = "XNAWindow";
            this.XNAWindow.Size = new System.Drawing.Size(886, 457);
            this.XNAWindow.TabIndex = 0;
            this.XNAWindow.TabStop = false;
            this.XNAWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.XNAWindow_MouseDown);
            this.XNAWindow.MouseEnter += new System.EventHandler(this.XNAWindow_MouseEnter);
            this.XNAWindow.MouseLeave += new System.EventHandler(this.XNAWindow_MouseLeave);
            this.XNAWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.XNAWindow_MouseMove);
            this.XNAWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.XNAWindow_MouseUp);
            // 
            // TabXML
            // 
            this.TabXML.Controls.Add(this.XMLText);
            this.TabXML.Location = new System.Drawing.Point(4, 29);
            this.TabXML.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabXML.Name = "TabXML";
            this.TabXML.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabXML.Size = new System.Drawing.Size(894, 467);
            this.TabXML.TabIndex = 1;
            this.TabXML.Text = "XML";
            this.TabXML.UseVisualStyleBackColor = true;
            this.TabXML.Enter += new System.EventHandler(this.TabXML_Click);
            // 
            // XMLText
            // 
            this.XMLText.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.XMLText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XMLText.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.XMLText.Location = new System.Drawing.Point(4, 5);
            this.XMLText.Multiline = true;
            this.XMLText.Name = "XMLText";
            this.XMLText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.XMLText.Size = new System.Drawing.Size(886, 457);
            this.XMLText.TabIndex = 1;
            // 
            // PropertiesSplit
            // 
            this.PropertiesSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertiesSplit.Location = new System.Drawing.Point(0, 0);
            this.PropertiesSplit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PropertiesSplit.Name = "PropertiesSplit";
            this.PropertiesSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // PropertiesSplit.Panel1
            // 
            this.PropertiesSplit.Panel1.Controls.Add(this.PropertyGrid);
            // 
            // PropertiesSplit.Panel2
            // 
            this.PropertiesSplit.Panel2.Controls.Add(this.TreeView);
            this.PropertiesSplit.Size = new System.Drawing.Size(212, 500);
            this.PropertiesSplit.SplitterDistance = 203;
            this.PropertiesSplit.SplitterWidth = 6;
            this.PropertiesSplit.TabIndex = 0;
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyGrid.HelpVisible = false;
            this.PropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(212, 203);
            this.PropertyGrid.TabIndex = 0;
            this.PropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PropertyGrid_SelectedGridItemChanged);
            this.PropertyGrid.SelectedObjectsChanged += new System.EventHandler(this.PropertyGrid_SelectedObjectsChanged);
            this.PropertyGrid.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PropertyGrid_Scroll);
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView.Indent = 20;
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(212, 291);
            this.TreeView.TabIndex = 0;
            // 
            // btnParentProperty
            // 
            this.btnParentProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParentProperty.Location = new System.Drawing.Point(988, 12);
            this.btnParentProperty.Name = "btnParentProperty";
            this.btnParentProperty.Size = new System.Drawing.Size(75, 31);
            this.btnParentProperty.TabIndex = 15;
            this.btnParentProperty.Text = "Parent";
            this.btnParentProperty.UseVisualStyleBackColor = true;
            this.btnParentProperty.Click += new System.EventHandler(this.btnParentProperty_Click);
            // 
            // btnOpenProperty
            // 
            this.btnOpenProperty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenProperty.Location = new System.Drawing.Point(1070, 12);
            this.btnOpenProperty.Name = "btnOpenProperty";
            this.btnOpenProperty.Size = new System.Drawing.Size(75, 32);
            this.btnOpenProperty.TabIndex = 14;
            this.btnOpenProperty.Text = "Open";
            this.btnOpenProperty.UseVisualStyleBackColor = true;
            this.btnOpenProperty.Click += new System.EventHandler(this.btnOpenProperty_Click);
            // 
            // SliderTime
            // 
            this.SliderTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SliderTime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SliderTime.Location = new System.Drawing.Point(190, 96);
            this.SliderTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SliderTime.Maximum = 1000;
            this.SliderTime.Name = "SliderTime";
            this.SliderTime.Size = new System.Drawing.Size(966, 69);
            this.SliderTime.TabIndex = 10;
            this.SliderTime.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SliderTime.Scroll += new System.EventHandler(this.SliderTime_Scroll);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CollisionsModeButton,
            this.SectorsModeButton,
            this.SpawnsModeButton,
            this.TCCModeButton,
            this.DictionaryDropDown,
            this.ToolStripSeparator1,
            this.btnNoState,
            this.btnEditVertices,
            this.toolStripSeparator3,
            this.BtnPolygon,
            this.BtnRectangle,
            this.BtnCircle,
            this.BtnEdge,
            this.btnSpawn,
            this.LblCurrentYear,
            this.ToolStripSeparator2,
            this.btnRun,
            this.btnStop,
            this.toolStripSeparator4,
            this.btnLayers});
            this.toolStrip1.Location = new System.Drawing.Point(0, 47);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(1156, 44);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // CollisionsModeButton
            // 
            this.CollisionsModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CollisionsModeButton.Image = ((System.Drawing.Image)(resources.GetObject("CollisionsModeButton.Image")));
            this.CollisionsModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CollisionsModeButton.Name = "CollisionsModeButton";
            this.CollisionsModeButton.Size = new System.Drawing.Size(28, 41);
            this.CollisionsModeButton.Text = "Collisions";
            this.CollisionsModeButton.Click += new System.EventHandler(this.CollisionsModeButton_Click);
            // 
            // SectorsModeButton
            // 
            this.SectorsModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SectorsModeButton.Image = ((System.Drawing.Image)(resources.GetObject("SectorsModeButton.Image")));
            this.SectorsModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SectorsModeButton.Name = "SectorsModeButton";
            this.SectorsModeButton.Size = new System.Drawing.Size(28, 41);
            this.SectorsModeButton.Text = "Sectors";
            this.SectorsModeButton.Click += new System.EventHandler(this.SectorsModeButton_Click);
            // 
            // SpawnsModeButton
            // 
            this.SpawnsModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SpawnsModeButton.Image = ((System.Drawing.Image)(resources.GetObject("SpawnsModeButton.Image")));
            this.SpawnsModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SpawnsModeButton.Name = "SpawnsModeButton";
            this.SpawnsModeButton.Size = new System.Drawing.Size(28, 41);
            this.SpawnsModeButton.Text = "Spawns";
            this.SpawnsModeButton.Click += new System.EventHandler(this.SpawnsModeButton_Click);
            // 
            // TCCModeButton
            // 
            this.TCCModeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TCCModeButton.Image = ((System.Drawing.Image)(resources.GetObject("TCCModeButton.Image")));
            this.TCCModeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TCCModeButton.Name = "TCCModeButton";
            this.TCCModeButton.Size = new System.Drawing.Size(28, 41);
            this.TCCModeButton.Text = "Changing Collisions";
            this.TCCModeButton.Click += new System.EventHandler(this.TCCModeButton_Click);
            // 
            // DictionaryDropDown
            // 
            this.DictionaryDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DictionaryDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.collisionsToolStripMenuItem,
            this.sectorsToolStripMenuItem,
            this.DictionaryEnnemies,
            this.DictionaryDecoration,
            this.DictionaryItems,
            this.DictionaryTraps,
            this.DictionaryMain,
            this.interactiveObjectsToolStripMenuItem});
            this.DictionaryDropDown.Image = ((System.Drawing.Image)(resources.GetObject("DictionaryDropDown.Image")));
            this.DictionaryDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DictionaryDropDown.Name = "DictionaryDropDown";
            this.DictionaryDropDown.Size = new System.Drawing.Size(156, 41);
            this.DictionaryDropDown.Text = "Dictionary";
            // 
            // collisionsToolStripMenuItem
            // 
            this.collisionsToolStripMenuItem.Name = "collisionsToolStripMenuItem";
            this.collisionsToolStripMenuItem.Size = new System.Drawing.Size(319, 42);
            this.collisionsToolStripMenuItem.Text = "Collisions";
            this.collisionsToolStripMenuItem.Click += new System.EventHandler(this.collisionsToolStripMenuItem_Click);
            // 
            // sectorsToolStripMenuItem
            // 
            this.sectorsToolStripMenuItem.Name = "sectorsToolStripMenuItem";
            this.sectorsToolStripMenuItem.Size = new System.Drawing.Size(319, 42);
            this.sectorsToolStripMenuItem.Text = "Sectors";
            // 
            // DictionaryEnnemies
            // 
            this.DictionaryEnnemies.Name = "DictionaryEnnemies";
            this.DictionaryEnnemies.Size = new System.Drawing.Size(319, 42);
            this.DictionaryEnnemies.Text = "Enemies";
            this.DictionaryEnnemies.Click += new System.EventHandler(this.DictionaryEnnemies_Click);
            // 
            // DictionaryDecoration
            // 
            this.DictionaryDecoration.Name = "DictionaryDecoration";
            this.DictionaryDecoration.Size = new System.Drawing.Size(319, 42);
            this.DictionaryDecoration.Text = "Decoration";
            this.DictionaryDecoration.Click += new System.EventHandler(this.DictionaryDecoration_Click);
            // 
            // DictionaryItems
            // 
            this.DictionaryItems.Name = "DictionaryItems";
            this.DictionaryItems.Size = new System.Drawing.Size(319, 42);
            this.DictionaryItems.Text = "Items";
            this.DictionaryItems.Click += new System.EventHandler(this.DictionaryItems_Click);
            // 
            // DictionaryTraps
            // 
            this.DictionaryTraps.Name = "DictionaryTraps";
            this.DictionaryTraps.Size = new System.Drawing.Size(319, 42);
            this.DictionaryTraps.Text = "Traps";
            this.DictionaryTraps.Click += new System.EventHandler(this.DictionaryTraps_Click);
            // 
            // DictionaryMain
            // 
            this.DictionaryMain.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DictionaryPlayerspawn,
            this.DictionaryEnd,
            this.DictionaryGoal});
            this.DictionaryMain.Name = "DictionaryMain";
            this.DictionaryMain.Size = new System.Drawing.Size(319, 42);
            this.DictionaryMain.Text = "Main";
            // 
            // DictionaryPlayerspawn
            // 
            this.DictionaryPlayerspawn.Name = "DictionaryPlayerspawn";
            this.DictionaryPlayerspawn.Size = new System.Drawing.Size(258, 42);
            this.DictionaryPlayerspawn.Text = "PlayerSpawn";
            this.DictionaryPlayerspawn.Click += new System.EventHandler(this.DictionaryPlayerspawn_Click);
            // 
            // DictionaryEnd
            // 
            this.DictionaryEnd.Name = "DictionaryEnd";
            this.DictionaryEnd.Size = new System.Drawing.Size(258, 42);
            this.DictionaryEnd.Text = "End";
            this.DictionaryEnd.Click += new System.EventHandler(this.DictionaryEnd_Click);
            // 
            // DictionaryGoal
            // 
            this.DictionaryGoal.Name = "DictionaryGoal";
            this.DictionaryGoal.Size = new System.Drawing.Size(258, 42);
            this.DictionaryGoal.Text = "Goal";
            this.DictionaryGoal.Click += new System.EventHandler(this.DictionaryGoal_Click);
            // 
            // interactiveObjectsToolStripMenuItem
            // 
            this.interactiveObjectsToolStripMenuItem.Name = "interactiveObjectsToolStripMenuItem";
            this.interactiveObjectsToolStripMenuItem.Size = new System.Drawing.Size(319, 42);
            this.interactiveObjectsToolStripMenuItem.Text = "InteractiveObjects";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 44);
            // 
            // btnNoState
            // 
            this.btnNoState.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNoState.Image = ((System.Drawing.Image)(resources.GetObject("btnNoState.Image")));
            this.btnNoState.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNoState.Name = "btnNoState";
            this.btnNoState.Size = new System.Drawing.Size(28, 41);
            this.btnNoState.Text = "None";
            this.btnNoState.Click += new System.EventHandler(this.btnNoState_Click);
            // 
            // btnEditVertices
            // 
            this.btnEditVertices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditVertices.Image = ((System.Drawing.Image)(resources.GetObject("btnEditVertices.Image")));
            this.btnEditVertices.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditVertices.Name = "btnEditVertices";
            this.btnEditVertices.Size = new System.Drawing.Size(28, 41);
            this.btnEditVertices.Text = "Select an object in the the PropertyGrid first to edit it\'s vertices.";
            this.btnEditVertices.Click += new System.EventHandler(this.btnEditVertices_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 44);
            // 
            // BtnPolygon
            // 
            this.BtnPolygon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnPolygon.Image = ((System.Drawing.Image)(resources.GetObject("BtnPolygon.Image")));
            this.BtnPolygon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPolygon.Name = "BtnPolygon";
            this.BtnPolygon.Size = new System.Drawing.Size(28, 41);
            this.BtnPolygon.Text = "Polygon";
            this.BtnPolygon.Click += new System.EventHandler(this.BtnPolygon_Click);
            // 
            // BtnRectangle
            // 
            this.BtnRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnRectangle.Image = ((System.Drawing.Image)(resources.GetObject("BtnRectangle.Image")));
            this.BtnRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnRectangle.Name = "BtnRectangle";
            this.BtnRectangle.Size = new System.Drawing.Size(28, 41);
            this.BtnRectangle.Text = "Rectangle";
            this.BtnRectangle.Click += new System.EventHandler(this.BtnRectangle_Click);
            // 
            // BtnCircle
            // 
            this.BtnCircle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnCircle.Image = ((System.Drawing.Image)(resources.GetObject("BtnCircle.Image")));
            this.BtnCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnCircle.Name = "BtnCircle";
            this.BtnCircle.Size = new System.Drawing.Size(28, 41);
            this.BtnCircle.Text = "Circle";
            this.BtnCircle.Click += new System.EventHandler(this.BtnCircle_Click);
            // 
            // BtnEdge
            // 
            this.BtnEdge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnEdge.Image = ((System.Drawing.Image)(resources.GetObject("BtnEdge.Image")));
            this.BtnEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnEdge.Name = "BtnEdge";
            this.BtnEdge.Size = new System.Drawing.Size(28, 41);
            this.BtnEdge.Text = "Edge";
            this.BtnEdge.Click += new System.EventHandler(this.BtnEdge_Click);
            // 
            // btnSpawn
            // 
            this.btnSpawn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSpawn.Image = ((System.Drawing.Image)(resources.GetObject("btnSpawn.Image")));
            this.btnSpawn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSpawn.Name = "btnSpawn";
            this.btnSpawn.Size = new System.Drawing.Size(28, 41);
            this.btnSpawn.Text = "Spawn";
            this.btnSpawn.Click += new System.EventHandler(this.btnSpawn_Click);
            // 
            // LblCurrentYear
            // 
            this.LblCurrentYear.Name = "LblCurrentYear";
            this.LblCurrentYear.Size = new System.Drawing.Size(199, 41);
            this.LblCurrentYear.Text = "Current Year : 0";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 44);
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(28, 41);
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(28, 41);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 44);
            // 
            // btnLayers
            // 
            this.btnLayers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLayers.Image = ((System.Drawing.Image)(resources.GetObject("btnLayers.Image")));
            this.btnLayers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLayers.Name = "btnLayers";
            this.btnLayers.Size = new System.Drawing.Size(28, 41);
            this.btnLayers.Text = "btnLayers";
            this.btnLayers.Click += new System.EventHandler(this.btnLayers_Click);
            // 
            // Menu
            // 
            this.Menu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuEdit,
            this.MenuTools,
            this.MenuRun});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.Menu.Size = new System.Drawing.Size(1156, 47);
            this.Menu.TabIndex = 8;
            this.Menu.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileNew,
            this.FileOpen,
            this.FileSave,
            this.FileClose});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(70, 41);
            this.MenuFile.Text = "File";
            // 
            // FileNew
            // 
            this.FileNew.Name = "FileNew";
            this.FileNew.Size = new System.Drawing.Size(172, 42);
            this.FileNew.Text = "New";
            this.FileNew.Click += new System.EventHandler(this.FileNew_Click);
            // 
            // FileOpen
            // 
            this.FileOpen.Name = "FileOpen";
            this.FileOpen.Size = new System.Drawing.Size(172, 42);
            this.FileOpen.Text = "Open";
            this.FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
            // 
            // FileSave
            // 
            this.FileSave.Name = "FileSave";
            this.FileSave.Size = new System.Drawing.Size(172, 42);
            this.FileSave.Text = "Save";
            this.FileSave.Click += new System.EventHandler(this.FileSave_Click);
            // 
            // FileClose
            // 
            this.FileClose.Name = "FileClose";
            this.FileClose.Size = new System.Drawing.Size(172, 42);
            this.FileClose.Text = "Close";
            // 
            // MenuEdit
            // 
            this.MenuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditSettings});
            this.MenuEdit.Name = "MenuEdit";
            this.MenuEdit.Size = new System.Drawing.Size(75, 41);
            this.MenuEdit.Text = "Edit";
            // 
            // EditSettings
            // 
            this.EditSettings.Name = "EditSettings";
            this.EditSettings.Size = new System.Drawing.Size(202, 42);
            this.EditSettings.Text = "Settings";
            this.EditSettings.Click += new System.EventHandler(this.EditSettings_Click);
            // 
            // MenuTools
            // 
            this.MenuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolsCreateEnnemy,
            this.TCCTools,
            this.effectsPickerToolStripMenuItem});
            this.MenuTools.Name = "MenuTools";
            this.MenuTools.Size = new System.Drawing.Size(93, 41);
            this.MenuTools.Text = "Tools";
            // 
            // ToolsCreateEnnemy
            // 
            this.ToolsCreateEnnemy.Name = "ToolsCreateEnnemy";
            this.ToolsCreateEnnemy.Size = new System.Drawing.Size(468, 42);
            this.ToolsCreateEnnemy.Text = "Create Ennemy";
            // 
            // TCCTools
            // 
            this.TCCTools.Name = "TCCTools";
            this.TCCTools.Size = new System.Drawing.Size(468, 42);
            this.TCCTools.Text = "Time Changing Collisions Tool";
            this.TCCTools.Click += new System.EventHandler(this.TCCTools_Click);
            // 
            // effectsPickerToolStripMenuItem
            // 
            this.effectsPickerToolStripMenuItem.Name = "effectsPickerToolStripMenuItem";
            this.effectsPickerToolStripMenuItem.Size = new System.Drawing.Size(468, 42);
            this.effectsPickerToolStripMenuItem.Text = "Effects Picker";
            this.effectsPickerToolStripMenuItem.Click += new System.EventHandler(this.effectsPickerToolStripMenuItem_Click);
            // 
            // MenuRun
            // 
            this.MenuRun.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunReleaseMode,
            this.RunStop});
            this.MenuRun.Name = "MenuRun";
            this.MenuRun.Size = new System.Drawing.Size(75, 41);
            this.MenuRun.Text = "Run";
            // 
            // RunReleaseMode
            // 
            this.RunReleaseMode.Name = "RunReleaseMode";
            this.RunReleaseMode.Size = new System.Drawing.Size(162, 42);
            this.RunReleaseMode.Text = "Run";
            // 
            // RunStop
            // 
            this.RunStop.Name = "RunStop";
            this.RunStop.Size = new System.Drawing.Size(162, 42);
            this.RunStop.Text = "Stop";
            // 
            // PosLabel
            // 
            this.PosLabel.AutoSize = true;
            this.PosLabel.Location = new System.Drawing.Point(14, 142);
            this.PosLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PosLabel.Name = "PosLabel";
            this.PosLabel.Size = new System.Drawing.Size(126, 20);
            this.PosLabel.TabIndex = 12;
            this.PosLabel.Text = "Pos : { X: 0 Y: 0 }";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // currentStateLabel
            // 
            this.currentStateLabel.AutoSize = true;
            this.currentStateLabel.Location = new System.Drawing.Point(318, 142);
            this.currentStateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentStateLabel.Name = "currentStateLabel";
            this.currentStateLabel.Size = new System.Drawing.Size(148, 20);
            this.currentStateLabel.TabIndex = 13;
            this.currentStateLabel.Text = "currentState : None";
            // 
            // UpDownScenario
            // 
            this.UpDownScenario.Enabled = false;
            this.UpDownScenario.Location = new System.Drawing.Point(113, 96);
            this.UpDownScenario.Name = "UpDownScenario";
            this.UpDownScenario.Size = new System.Drawing.Size(70, 26);
            this.UpDownScenario.TabIndex = 14;
            this.UpDownScenario.ValueChanged += new System.EventHandler(this.UpDownScenario_ValueChanged);
            // 
            // currentModeLabel
            // 
            this.currentModeLabel.AutoSize = true;
            this.currentModeLabel.Location = new System.Drawing.Point(630, 142);
            this.currentModeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentModeLabel.Name = "currentModeLabel";
            this.currentModeLabel.Size = new System.Drawing.Size(180, 20);
            this.currentModeLabel.TabIndex = 16;
            this.currentModeLabel.Text = "CurrentMode : Collisions";
            // 
            // rBtnPrecision
            // 
            this.rBtnPrecision.AutoSize = true;
            this.rBtnPrecision.Checked = true;
            this.rBtnPrecision.Location = new System.Drawing.Point(12, 96);
            this.rBtnPrecision.Name = "rBtnPrecision";
            this.rBtnPrecision.Size = new System.Drawing.Size(44, 24);
            this.rBtnPrecision.TabIndex = 17;
            this.rBtnPrecision.TabStop = true;
            this.rBtnPrecision.Text = "P";
            this.rBtnPrecision.UseVisualStyleBackColor = true;
            this.rBtnPrecision.CheckedChanged += new System.EventHandler(this.rBtnPrecision_CheckedChanged);
            // 
            // rBtnScenario
            // 
            this.rBtnScenario.AutoSize = true;
            this.rBtnScenario.Location = new System.Drawing.Point(62, 96);
            this.rBtnScenario.Name = "rBtnScenario";
            this.rBtnScenario.Size = new System.Drawing.Size(45, 24);
            this.rBtnScenario.TabIndex = 18;
            this.rBtnScenario.Text = "S";
            this.rBtnScenario.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 683);
            this.Controls.Add(this.rBtnScenario);
            this.Controls.Add(this.rBtnPrecision);
            this.Controls.Add(this.currentModeLabel);
            this.Controls.Add(this.btnParentProperty);
            this.Controls.Add(this.btnOpenProperty);
            this.Controls.Add(this.UpDownScenario);
            this.Controls.Add(this.currentStateLabel);
            this.Controls.Add(this.PosLabel);
            this.Controls.Add(this.MainSplit);
            this.Controls.Add(this.SliderTime);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Menu);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "LEJEU Level Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.MainSplit.Panel1.ResumeLayout(false);
            this.MainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
            this.MainSplit.ResumeLayout(false);
            this.TabControl1.ResumeLayout(false);
            this.TabLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.XNAWindow)).EndInit();
            this.TabXML.ResumeLayout(false);
            this.TabXML.PerformLayout();
            this.PropertiesSplit.Panel1.ResumeLayout(false);
            this.PropertiesSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PropertiesSplit)).EndInit();
            this.PropertiesSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SliderTime)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownScenario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



        public IntPtr getDrawSurface()
        {
            return XNAWindow.Handle;
        }
        public Vector2 XNAWindowDimensions
        {
            get { return new Vector2(XNAWindow.Size.Width, XNAWindow.Size.Height); }
        }
        public object SelectedProperty
        {
            get { return PropertyGrid.SelectedObject; }
        }

        internal SplitContainer MainSplit;
        internal TabControl TabControl1;
        internal TabPage TabLevel;
        public PictureBox XNAWindow;
        internal TabPage TabXML;
        internal SplitContainer PropertiesSplit;
        internal PropertyGrid PropertyGrid;
        internal TrackBar SliderTime;
        private ToolStrip toolStrip1;
        private ToolStripButton CollisionsModeButton;
        private ToolStripButton SectorsModeButton;
        private ToolStripButton SpawnsModeButton;
        private ToolStripButton TCCModeButton;
        internal ToolStripDropDownButton DictionaryDropDown;
        internal ToolStripMenuItem DictionaryEnnemies;
        internal ToolStripMenuItem DictionaryDecoration;
        internal ToolStripMenuItem DictionaryItems;
        internal ToolStripMenuItem DictionaryTraps;
        internal ToolStripMenuItem DictionaryMain;
        internal ToolStripMenuItem DictionaryPlayerspawn;
        internal ToolStripMenuItem DictionaryEnd;
        internal ToolStripMenuItem DictionaryGoal;
        internal ToolStripSeparator ToolStripSeparator1;
        internal ToolStripButton BtnPolygon;
        internal ToolStripButton BtnRectangle;
        internal ToolStripButton BtnCircle;
        internal ToolStripButton BtnEdge;
        internal ToolStripLabel LblCurrentYear;
        internal ToolStripSeparator ToolStripSeparator2;
        private MenuStrip Menu;
        private ToolStripMenuItem MenuFile;
        private ToolStripMenuItem FileNew;
        private ToolStripMenuItem FileOpen;
        private ToolStripMenuItem FileSave;
        private ToolStripMenuItem FileClose;
        private ToolStripMenuItem MenuEdit;
        private ToolStripMenuItem EditSettings;
        private ToolStripMenuItem MenuTools;
        private ToolStripMenuItem ToolsCreateEnnemy;
        private ToolStripMenuItem MenuRun;
        private ToolStripMenuItem RunReleaseMode;
        private ToolStripMenuItem RunStop;
        private SettingsForm settingsForm;
        public static TCCTool TCCToolform;
        private EffectsForm effectsForm;
        private ToolStripMenuItem collisionsToolStripMenuItem;
        private ToolStripMenuItem sectorsToolStripMenuItem;
        private Label PosLabel;
        internal ToolStripButton btnNoState;
        internal ToolStripButton btnSpawn;
        private TreeView TreeView;
        private BindingSource bindingSource1;
        private Timer timer1;
        private ToolStripButton btnEditVertices;
        private ToolStripSeparator toolStripSeparator3;
        private Label currentStateLabel;
        private ToolStripMenuItem TCCTools;
        private Button btnOpenProperty;
        private Button btnParentProperty;
        private ToolStripMenuItem effectsPickerToolStripMenuItem;
        private NumericUpDown UpDownScenario;
        private Label currentModeLabel;
        private ToolStripButton btnRun;
        private ToolStripButton btnStop;
        private ToolStripMenuItem interactiveObjectsToolStripMenuItem;
        private RadioButton rBtnScenario;
        private RadioButton rBtnPrecision;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton btnLayers;
        private TextBox XMLText;
    }
}