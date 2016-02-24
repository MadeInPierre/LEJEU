namespace LevelEditor
{
    partial class TCCTool
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
            this.components = new System.ComponentModel.Container();
            this.btnAddTCC = new System.Windows.Forms.Button();
            this.btnRemoveTCC = new System.Windows.Forms.Button();
            this.TCCsTreeView = new System.Windows.Forms.TreeView();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.btnAddState = new System.Windows.Forms.Button();
            this.StatesTreeView = new System.Windows.Forms.TreeView();
            this.btnRemoveState = new System.Windows.Forms.Button();
            this.lblCurrentYear = new System.Windows.Forms.Label();
            this.btnRenameTCC = new System.Windows.Forms.Button();
            this.btnRemoveImg = new System.Windows.Forms.Button();
            this.btnAddImg = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkSF = new System.Windows.Forms.CheckBox();
            this.checkActiveSF = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnAddTCC
            // 
            this.btnAddTCC.Location = new System.Drawing.Point(349, 12);
            this.btnAddTCC.Name = "btnAddTCC";
            this.btnAddTCC.Size = new System.Drawing.Size(117, 38);
            this.btnAddTCC.TabIndex = 0;
            this.btnAddTCC.Text = "Add";
            this.btnAddTCC.UseVisualStyleBackColor = true;
            this.btnAddTCC.Click += new System.EventHandler(this.btnAddTCC_Click);
            // 
            // btnRemoveTCC
            // 
            this.btnRemoveTCC.Location = new System.Drawing.Point(349, 170);
            this.btnRemoveTCC.Name = "btnRemoveTCC";
            this.btnRemoveTCC.Size = new System.Drawing.Size(117, 38);
            this.btnRemoveTCC.TabIndex = 1;
            this.btnRemoveTCC.Text = "Remove";
            this.btnRemoveTCC.UseVisualStyleBackColor = true;
            this.btnRemoveTCC.Click += new System.EventHandler(this.btnRemoveTCC_Click);
            // 
            // TCCsTreeView
            // 
            this.TCCsTreeView.Location = new System.Drawing.Point(12, 12);
            this.TCCsTreeView.Name = "TCCsTreeView";
            this.TCCsTreeView.Size = new System.Drawing.Size(331, 196);
            this.TCCsTreeView.TabIndex = 2;
            this.TCCsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TCCsTreeView_AfterSelect);
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PropertyGrid.Location = new System.Drawing.Point(12, 227);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(454, 414);
            this.PropertyGrid.TabIndex = 3;
            this.PropertyGrid.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.PropertyGrid_Scroll);
            // 
            // btnAddState
            // 
            this.btnAddState.Location = new System.Drawing.Point(12, 647);
            this.btnAddState.Name = "btnAddState";
            this.btnAddState.Size = new System.Drawing.Size(60, 38);
            this.btnAddState.TabIndex = 4;
            this.btnAddState.Text = "+";
            this.btnAddState.UseVisualStyleBackColor = true;
            this.btnAddState.Click += new System.EventHandler(this.btnAddState_Click);
            // 
            // StatesTreeView
            // 
            this.StatesTreeView.Location = new System.Drawing.Point(288, 648);
            this.StatesTreeView.Name = "StatesTreeView";
            this.StatesTreeView.Size = new System.Drawing.Size(178, 125);
            this.StatesTreeView.TabIndex = 8;
            this.StatesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.StatesTreeView_AfterSelect);
            // 
            // btnRemoveState
            // 
            this.btnRemoveState.Location = new System.Drawing.Point(78, 648);
            this.btnRemoveState.Name = "btnRemoveState";
            this.btnRemoveState.Size = new System.Drawing.Size(60, 38);
            this.btnRemoveState.TabIndex = 9;
            this.btnRemoveState.Text = "-";
            this.btnRemoveState.UseVisualStyleBackColor = true;
            this.btnRemoveState.Click += new System.EventHandler(this.btnRemoveState_Click);
            // 
            // lblCurrentYear
            // 
            this.lblCurrentYear.AutoSize = true;
            this.lblCurrentYear.Location = new System.Drawing.Point(144, 709);
            this.lblCurrentYear.Name = "lblCurrentYear";
            this.lblCurrentYear.Size = new System.Drawing.Size(18, 20);
            this.lblCurrentYear.TabIndex = 11;
            this.lblCurrentYear.Text = "0";
            // 
            // btnRenameTCC
            // 
            this.btnRenameTCC.Location = new System.Drawing.Point(349, 57);
            this.btnRenameTCC.Name = "btnRenameTCC";
            this.btnRenameTCC.Size = new System.Drawing.Size(117, 38);
            this.btnRenameTCC.TabIndex = 12;
            this.btnRenameTCC.Text = "Rename";
            this.btnRenameTCC.UseVisualStyleBackColor = true;
            this.btnRenameTCC.Click += new System.EventHandler(this.btnRenameTCC_Click);
            // 
            // btnRemoveImg
            // 
            this.btnRemoveImg.Location = new System.Drawing.Point(12, 735);
            this.btnRemoveImg.Name = "btnRemoveImg";
            this.btnRemoveImg.Size = new System.Drawing.Size(126, 38);
            this.btnRemoveImg.TabIndex = 10;
            this.btnRemoveImg.Text = "- Image";
            this.btnRemoveImg.UseVisualStyleBackColor = true;
            this.btnRemoveImg.Click += new System.EventHandler(this.btnRemoveImg_Click);
            // 
            // btnAddImg
            // 
            this.btnAddImg.Location = new System.Drawing.Point(12, 691);
            this.btnAddImg.Name = "btnAddImg";
            this.btnAddImg.Size = new System.Drawing.Size(126, 38);
            this.btnAddImg.TabIndex = 5;
            this.btnAddImg.Text = "+ Image";
            this.btnAddImg.UseVisualStyleBackColor = true;
            this.btnAddImg.Click += new System.EventHandler(this.btnAddImg_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(144, 735);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 38);
            this.button1.TabIndex = 13;
            this.button1.Text = "- Object";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkSF
            // 
            this.checkSF.AutoSize = true;
            this.checkSF.Location = new System.Drawing.Point(349, 101);
            this.checkSF.Name = "checkSF";
            this.checkSF.Size = new System.Drawing.Size(106, 24);
            this.checkSF.TabIndex = 14;
            this.checkSF.Text = "SafeZone";
            this.checkSF.UseVisualStyleBackColor = true;
            this.checkSF.CheckedChanged += new System.EventHandler(this.checkSF_CheckedChanged);
            // 
            // checkActiveSF
            // 
            this.checkActiveSF.AutoSize = true;
            this.checkActiveSF.Location = new System.Drawing.Point(144, 660);
            this.checkActiveSF.Name = "checkActiveSF";
            this.checkActiveSF.Size = new System.Drawing.Size(138, 24);
            this.checkActiveSF.TabIndex = 15;
            this.checkActiveSF.Text = "SFActiveState";
            this.checkActiveSF.UseVisualStyleBackColor = true;
            this.checkActiveSF.CheckedChanged += new System.EventHandler(this.checkActiveSF_CheckedChanged);
            // 
            // TCCTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 785);
            this.Controls.Add(this.StatesTreeView);
            this.Controls.Add(this.checkActiveSF);
            this.Controls.Add(this.checkSF);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRenameTCC);
            this.Controls.Add(this.lblCurrentYear);
            this.Controls.Add(this.btnRemoveImg);
            this.Controls.Add(this.btnRemoveState);
            this.Controls.Add(this.btnAddImg);
            this.Controls.Add(this.btnAddState);
            this.Controls.Add(this.PropertyGrid);
            this.Controls.Add(this.TCCsTreeView);
            this.Controls.Add(this.btnRemoveTCC);
            this.Controls.Add(this.btnAddTCC);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TCCTool";
            this.Text = "TCCTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCCTool_FormClosing);
            this.Load += new System.EventHandler(this.TTCTool_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TCCTool_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TCCTool_KeyUp);
        }

        #endregion

        private System.Windows.Forms.Button btnAddTCC;
        private System.Windows.Forms.Button btnRemoveTCC;
        private System.Windows.Forms.TreeView TCCsTreeView;
        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.Button btnAddState;
        private System.Windows.Forms.TreeView StatesTreeView;
        private System.Windows.Forms.Button btnRemoveState;
        private System.Windows.Forms.Label lblCurrentYear;
        private System.Windows.Forms.Button btnRenameTCC;
        private System.Windows.Forms.Button btnRemoveImg;
        private System.Windows.Forms.Button btnAddImg;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkSF;
        private System.Windows.Forms.CheckBox checkActiveSF;
    }
}