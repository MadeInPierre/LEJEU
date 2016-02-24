namespace LevelEditor
{
    partial class LayersForm
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
            this.cCollisions = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.cSectors = new System.Windows.Forms.CheckBox();
            this.cSpawns = new System.Windows.Forms.CheckBox();
            this.cEnemies = new System.Windows.Forms.CheckBox();
            this.cTraps = new System.Windows.Forms.CheckBox();
            this.cItems = new System.Windows.Forms.CheckBox();
            this.cDeco = new System.Windows.Forms.CheckBox();
            this.cPlatforms = new System.Windows.Forms.CheckBox();
            this.cOthers = new System.Windows.Forms.CheckBox();
            this.cLadder = new System.Windows.Forms.CheckBox();
            this.cTCCs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cCollisions
            // 
            this.cCollisions.AutoSize = true;
            this.cCollisions.Location = new System.Drawing.Point(12, 12);
            this.cCollisions.Name = "cCollisions";
            this.cCollisions.Size = new System.Drawing.Size(101, 24);
            this.cCollisions.TabIndex = 0;
            this.cCollisions.Text = "Collisions";
            this.cCollisions.UseVisualStyleBackColor = true;
            this.cCollisions.CheckedChanged += new System.EventHandler(this.cCollisions_CheckedChanged);
            // 
            // cSectors
            // 
            this.cSectors.AutoSize = true;
            this.cSectors.Location = new System.Drawing.Point(12, 132);
            this.cSectors.Name = "cSectors";
            this.cSectors.Size = new System.Drawing.Size(90, 24);
            this.cSectors.TabIndex = 1;
            this.cSectors.Text = "Sectors";
            this.cSectors.UseVisualStyleBackColor = true;
            this.cSectors.CheckedChanged += new System.EventHandler(this.cSectors_CheckedChanged);
            // 
            // cSpawns
            // 
            this.cSpawns.AutoSize = true;
            this.cSpawns.Location = new System.Drawing.Point(12, 192);
            this.cSpawns.Name = "cSpawns";
            this.cSpawns.Size = new System.Drawing.Size(92, 24);
            this.cSpawns.TabIndex = 2;
            this.cSpawns.Text = "Spawns";
            this.cSpawns.UseVisualStyleBackColor = true;
            this.cSpawns.CheckedChanged += new System.EventHandler(this.cSpawns_CheckedChanged);
            // 
            // cEnemies
            // 
            this.cEnemies.AutoSize = true;
            this.cEnemies.Location = new System.Drawing.Point(34, 222);
            this.cEnemies.Name = "cEnemies";
            this.cEnemies.Size = new System.Drawing.Size(97, 24);
            this.cEnemies.TabIndex = 3;
            this.cEnemies.Text = "Enemies";
            this.cEnemies.UseVisualStyleBackColor = true;
            this.cEnemies.CheckedChanged += new System.EventHandler(this.cEnemies_CheckedChanged);
            // 
            // cTraps
            // 
            this.cTraps.AutoSize = true;
            this.cTraps.Location = new System.Drawing.Point(34, 252);
            this.cTraps.Name = "cTraps";
            this.cTraps.Size = new System.Drawing.Size(75, 24);
            this.cTraps.TabIndex = 4;
            this.cTraps.Text = "Traps";
            this.cTraps.UseVisualStyleBackColor = true;
            this.cTraps.CheckedChanged += new System.EventHandler(this.cTraps_CheckedChanged);
            // 
            // cItems
            // 
            this.cItems.AutoSize = true;
            this.cItems.Location = new System.Drawing.Point(34, 312);
            this.cItems.Name = "cItems";
            this.cItems.Size = new System.Drawing.Size(75, 24);
            this.cItems.TabIndex = 5;
            this.cItems.Text = "Items";
            this.cItems.UseVisualStyleBackColor = true;
            this.cItems.CheckedChanged += new System.EventHandler(this.cItems_CheckedChanged);
            // 
            // cDeco
            // 
            this.cDeco.AutoSize = true;
            this.cDeco.Location = new System.Drawing.Point(34, 282);
            this.cDeco.Name = "cDeco";
            this.cDeco.Size = new System.Drawing.Size(113, 24);
            this.cDeco.TabIndex = 6;
            this.cDeco.Text = "Decoration";
            this.cDeco.UseVisualStyleBackColor = true;
            this.cDeco.CheckedChanged += new System.EventHandler(this.cDeco_CheckedChanged);
            // 
            // cPlatforms
            // 
            this.cPlatforms.AutoSize = true;
            this.cPlatforms.Location = new System.Drawing.Point(29, 42);
            this.cPlatforms.Name = "cPlatforms";
            this.cPlatforms.Size = new System.Drawing.Size(102, 24);
            this.cPlatforms.TabIndex = 7;
            this.cPlatforms.Text = "Platforms";
            this.cPlatforms.UseVisualStyleBackColor = true;
            this.cPlatforms.CheckedChanged += new System.EventHandler(this.cPlatforms_CheckedChanged);
            // 
            // cOthers
            // 
            this.cOthers.AutoSize = true;
            this.cOthers.Location = new System.Drawing.Point(29, 102);
            this.cOthers.Name = "cOthers";
            this.cOthers.Size = new System.Drawing.Size(83, 24);
            this.cOthers.TabIndex = 8;
            this.cOthers.Text = "Others";
            this.cOthers.UseVisualStyleBackColor = true;
            this.cOthers.CheckedChanged += new System.EventHandler(this.cOthers_CheckedChanged);
            // 
            // cLadder
            // 
            this.cLadder.AutoSize = true;
            this.cLadder.Location = new System.Drawing.Point(29, 72);
            this.cLadder.Name = "cLadder";
            this.cLadder.Size = new System.Drawing.Size(85, 24);
            this.cLadder.TabIndex = 9;
            this.cLadder.Text = "Ladder";
            this.cLadder.UseVisualStyleBackColor = true;
            this.cLadder.CheckedChanged += new System.EventHandler(this.cLadder_CheckedChanged);
            // 
            // cTCCs
            // 
            this.cTCCs.AutoSize = true;
            this.cTCCs.Location = new System.Drawing.Point(12, 162);
            this.cTCCs.Name = "cTCCs";
            this.cTCCs.Size = new System.Drawing.Size(74, 24);
            this.cTCCs.TabIndex = 10;
            this.cTCCs.Text = "TCCs";
            this.cTCCs.UseVisualStyleBackColor = true;
            this.cTCCs.CheckedChanged += new System.EventHandler(this.cTCCs_CheckedChanged);
            // 
            // LayersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 375);
            this.Controls.Add(this.cTCCs);
            this.Controls.Add(this.cLadder);
            this.Controls.Add(this.cOthers);
            this.Controls.Add(this.cPlatforms);
            this.Controls.Add(this.cDeco);
            this.Controls.Add(this.cItems);
            this.Controls.Add(this.cTraps);
            this.Controls.Add(this.cEnemies);
            this.Controls.Add(this.cSpawns);
            this.Controls.Add(this.cSectors);
            this.Controls.Add(this.cCollisions);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayersForm";
            this.Text = "LayersForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cCollisions;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox cSectors;
        private System.Windows.Forms.CheckBox cSpawns;
        private System.Windows.Forms.CheckBox cEnemies;
        private System.Windows.Forms.CheckBox cTraps;
        private System.Windows.Forms.CheckBox cItems;
        private System.Windows.Forms.CheckBox cDeco;
        private System.Windows.Forms.CheckBox cPlatforms;
        private System.Windows.Forms.CheckBox cOthers;
        private System.Windows.Forms.CheckBox cLadder;
        private System.Windows.Forms.CheckBox cTCCs;
    }
}