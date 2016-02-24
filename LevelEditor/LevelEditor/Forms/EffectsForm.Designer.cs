namespace LevelEditor
{
    partial class EffectsForm
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
            this.EffectsTreeView = new System.Windows.Forms.TreeView();
            this.EffectsDictTreeView = new System.Windows.Forms.TreeView();
            this.btnRenameTCC = new System.Windows.Forms.Button();
            this.btnRemoveTCC = new System.Windows.Forms.Button();
            this.btnAddTCC = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EffectsTreeView
            // 
            this.EffectsTreeView.Location = new System.Drawing.Point(12, 12);
            this.EffectsTreeView.Name = "EffectsTreeView";
            this.EffectsTreeView.Size = new System.Drawing.Size(423, 167);
            this.EffectsTreeView.TabIndex = 0;
            // 
            // EffectsDictTreeView
            // 
            this.EffectsDictTreeView.Location = new System.Drawing.Point(12, 185);
            this.EffectsDictTreeView.Name = "EffectsDictTreeView";
            this.EffectsDictTreeView.Size = new System.Drawing.Size(544, 342);
            this.EffectsDictTreeView.TabIndex = 1;
            // 
            // btnRenameTCC
            // 
            this.btnRenameTCC.Location = new System.Drawing.Point(441, 57);
            this.btnRenameTCC.Name = "btnRenameTCC";
            this.btnRenameTCC.Size = new System.Drawing.Size(117, 38);
            this.btnRenameTCC.TabIndex = 15;
            this.btnRenameTCC.Text = "Rename";
            this.btnRenameTCC.UseVisualStyleBackColor = true;
            this.btnRenameTCC.Click += new System.EventHandler(this.btnRenameTCC_Click);
            // 
            // btnRemoveTCC
            // 
            this.btnRemoveTCC.Location = new System.Drawing.Point(441, 101);
            this.btnRemoveTCC.Name = "btnRemoveTCC";
            this.btnRemoveTCC.Size = new System.Drawing.Size(117, 38);
            this.btnRemoveTCC.TabIndex = 14;
            this.btnRemoveTCC.Text = "Remove";
            this.btnRemoveTCC.UseVisualStyleBackColor = true;
            // 
            // btnAddTCC
            // 
            this.btnAddTCC.Location = new System.Drawing.Point(441, 12);
            this.btnAddTCC.Name = "btnAddTCC";
            this.btnAddTCC.Size = new System.Drawing.Size(117, 38);
            this.btnAddTCC.TabIndex = 13;
            this.btnAddTCC.Text = "Add";
            this.btnAddTCC.UseVisualStyleBackColor = true;
            this.btnAddTCC.Click += new System.EventHandler(this.btnAddTCC_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(441, 141);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(117, 38);
            this.btnDone.TabIndex = 16;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            // 
            // EffectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 539);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnRenameTCC);
            this.Controls.Add(this.btnRemoveTCC);
            this.Controls.Add(this.btnAddTCC);
            this.Controls.Add(this.EffectsDictTreeView);
            this.Controls.Add(this.EffectsTreeView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EffectsForm";
            this.Text = "EffectsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EffectsForm_FormClosed);
            this.Load += new System.EventHandler(this.EffectsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView EffectsTreeView;
        private System.Windows.Forms.TreeView EffectsDictTreeView;
        private System.Windows.Forms.Button btnRenameTCC;
        private System.Windows.Forms.Button btnRemoveTCC;
        private System.Windows.Forms.Button btnAddTCC;
        public System.Windows.Forms.Button btnDone;
    }
}