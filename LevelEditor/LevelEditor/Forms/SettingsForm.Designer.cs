namespace LevelEditor
{
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
            this.components = new System.ComponentModel.Container();
            this.NZonesLabel = new System.Windows.Forms.Label();
            this.NZonesUpDown = new System.Windows.Forms.NumericUpDown();
            this.NBacksUpDown = new System.Windows.Forms.NumericUpDown();
            this.NBacksLabel = new System.Windows.Forms.Label();
            this.NYearsUpDown = new System.Windows.Forms.NumericUpDown();
            this.NYearsLabel = new System.Windows.Forms.Label();
            this.BackSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.BackSpeedLabel = new System.Windows.Forms.Label();
            this.WeatherLabel = new System.Windows.Forms.Label();
            this.WeatherDropDown = new System.Windows.Forms.ComboBox();
            this.TreeViewSettings = new System.Windows.Forms.TreeView();
            this.PropertyGridSettings = new System.Windows.Forms.PropertyGrid();
            this.timeSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.timeSpeedLabel = new System.Windows.Forms.Label();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblScenarios = new System.Windows.Forms.Label();
            this.btnAutoDetect = new System.Windows.Forms.Button();
            this.scenariosUpDown = new System.Windows.Forms.NumericUpDown();
            this.txtLevelPath = new System.Windows.Forms.TextBox();
            this.lblLevelPath = new System.Windows.Forms.Label();
            this.checkBoxDevMode = new System.Windows.Forms.CheckBox();
            this.lblSoundtrackPath = new System.Windows.Forms.Label();
            this.txtSoundtrackPath = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.NZonesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBacksUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NYearsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenariosUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // NZonesLabel
            // 
            this.NZonesLabel.AutoSize = true;
            this.NZonesLabel.Location = new System.Drawing.Point(20, 94);
            this.NZonesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NZonesLabel.Name = "NZonesLabel";
            this.NZonesLabel.Size = new System.Drawing.Size(65, 20);
            this.NZonesLabel.TabIndex = 0;
            this.NZonesLabel.Text = "NZones";
            this.NZonesLabel.Click += new System.EventHandler(this.NZonesLabel_Click);
            // 
            // NZonesUpDown
            // 
            this.NZonesUpDown.Location = new System.Drawing.Point(225, 91);
            this.NZonesUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NZonesUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NZonesUpDown.Name = "NZonesUpDown";
            this.NZonesUpDown.Size = new System.Drawing.Size(180, 26);
            this.NZonesUpDown.TabIndex = 1;
            this.NZonesUpDown.ValueChanged += new System.EventHandler(this.NZonesUpDown_ValueChanged);
            // 
            // NBacksUpDown
            // 
            this.NBacksUpDown.Location = new System.Drawing.Point(226, 131);
            this.NBacksUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NBacksUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NBacksUpDown.Name = "NBacksUpDown";
            this.NBacksUpDown.Size = new System.Drawing.Size(180, 26);
            this.NBacksUpDown.TabIndex = 3;
            this.NBacksUpDown.ValueChanged += new System.EventHandler(this.NBacksUpDown_ValueChanged_1);
            // 
            // NBacksLabel
            // 
            this.NBacksLabel.AutoSize = true;
            this.NBacksLabel.Location = new System.Drawing.Point(20, 134);
            this.NBacksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NBacksLabel.Name = "NBacksLabel";
            this.NBacksLabel.Size = new System.Drawing.Size(64, 20);
            this.NBacksLabel.TabIndex = 2;
            this.NBacksLabel.Text = "NBacks";
            // 
            // NYearsUpDown
            // 
            this.NYearsUpDown.Location = new System.Drawing.Point(224, 11);
            this.NYearsUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NYearsUpDown.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.NYearsUpDown.Name = "NYearsUpDown";
            this.NYearsUpDown.Size = new System.Drawing.Size(180, 26);
            this.NYearsUpDown.TabIndex = 6;
            this.NYearsUpDown.ValueChanged += new System.EventHandler(this.NYearsUpDown_ValueChanged);
            // 
            // NYearsLabel
            // 
            this.NYearsLabel.AutoSize = true;
            this.NYearsLabel.Location = new System.Drawing.Point(18, 14);
            this.NYearsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NYearsLabel.Name = "NYearsLabel";
            this.NYearsLabel.Size = new System.Drawing.Size(62, 20);
            this.NYearsLabel.TabIndex = 5;
            this.NYearsLabel.Text = "NYears";
            // 
            // BackSpeedUpDown
            // 
            this.BackSpeedUpDown.DecimalPlaces = 1;
            this.BackSpeedUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BackSpeedUpDown.Location = new System.Drawing.Point(305, 168);
            this.BackSpeedUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BackSpeedUpDown.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.BackSpeedUpDown.Name = "BackSpeedUpDown";
            this.BackSpeedUpDown.Size = new System.Drawing.Size(100, 26);
            this.BackSpeedUpDown.TabIndex = 8;
            // 
            // BackSpeedLabel
            // 
            this.BackSpeedLabel.AutoSize = true;
            this.BackSpeedLabel.Location = new System.Drawing.Point(54, 174);
            this.BackSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BackSpeedLabel.Name = "BackSpeedLabel";
            this.BackSpeedLabel.Size = new System.Drawing.Size(92, 20);
            this.BackSpeedLabel.TabIndex = 7;
            this.BackSpeedLabel.Text = "BackSpeed";
            // 
            // WeatherLabel
            // 
            this.WeatherLabel.AutoSize = true;
            this.WeatherLabel.Location = new System.Drawing.Point(24, 212);
            this.WeatherLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WeatherLabel.Name = "WeatherLabel";
            this.WeatherLabel.Size = new System.Drawing.Size(70, 20);
            this.WeatherLabel.TabIndex = 9;
            this.WeatherLabel.Text = "Weather";
            // 
            // WeatherDropDown
            // 
            this.WeatherDropDown.FormattingEnabled = true;
            this.WeatherDropDown.Items.AddRange(new object[] {
            "Sunny",
            "Cloudy",
            "Deep Rain",
            "Light Rain",
            "Snow"});
            this.WeatherDropDown.Location = new System.Drawing.Point(226, 211);
            this.WeatherDropDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WeatherDropDown.Name = "WeatherDropDown";
            this.WeatherDropDown.Size = new System.Drawing.Size(180, 28);
            this.WeatherDropDown.TabIndex = 10;
            this.WeatherDropDown.Text = "Choose Weather";
            this.WeatherDropDown.SelectedIndexChanged += new System.EventHandler(this.WeatherDropDown_SelectedIndexChanged);
            // 
            // TreeViewSettings
            // 
            this.TreeViewSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeViewSettings.Location = new System.Drawing.Point(414, 11);
            this.TreeViewSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TreeViewSettings.Name = "TreeViewSettings";
            this.TreeViewSettings.Size = new System.Drawing.Size(282, 304);
            this.TreeViewSettings.TabIndex = 0;
            this.TreeViewSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeViewSettings_AfterSelect);
            // 
            // PropertyGridSettings
            // 
            this.PropertyGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGridSettings.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.PropertyGridSettings.HelpVisible = false;
            this.PropertyGridSettings.Location = new System.Drawing.Point(18, 355);
            this.PropertyGridSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PropertyGridSettings.Name = "PropertyGridSettings";
            this.PropertyGridSettings.Size = new System.Drawing.Size(680, 392);
            this.PropertyGridSettings.TabIndex = 4;
            // 
            // timeSpeedUpDown
            // 
            this.timeSpeedUpDown.Location = new System.Drawing.Point(302, 51);
            this.timeSpeedUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timeSpeedUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.timeSpeedUpDown.Name = "timeSpeedUpDown";
            this.timeSpeedUpDown.Size = new System.Drawing.Size(100, 26);
            this.timeSpeedUpDown.TabIndex = 12;
            this.timeSpeedUpDown.ValueChanged += new System.EventHandler(this.timeSpeedUpDown_ValueChanged);
            // 
            // timeSpeedLabel
            // 
            this.timeSpeedLabel.AutoSize = true;
            this.timeSpeedLabel.Location = new System.Drawing.Point(51, 54);
            this.timeSpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeSpeedLabel.Name = "timeSpeedLabel";
            this.timeSpeedLabel.Size = new System.Drawing.Size(90, 20);
            this.timeSpeedLabel.TabIndex = 11;
            this.timeSpeedLabel.Text = "TimeSpeed";
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadImage.Enabled = false;
            this.btnLoadImage.Location = new System.Drawing.Point(18, 707);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(108, 40);
            this.btnLoadImage.TabIndex = 13;
            this.btnLoadImage.Text = "Load Image";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "S";
            // 
            // lblScenarios
            // 
            this.lblScenarios.AutoSize = true;
            this.lblScenarios.Location = new System.Drawing.Point(24, 253);
            this.lblScenarios.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScenarios.Name = "lblScenarios";
            this.lblScenarios.Size = new System.Drawing.Size(80, 20);
            this.lblScenarios.TabIndex = 14;
            this.lblScenarios.Text = "Scenarios";
            this.lblScenarios.Click += new System.EventHandler(this.lblScenarios_Click);
            // 
            // btnAutoDetect
            // 
            this.btnAutoDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoDetect.Location = new System.Drawing.Point(590, 707);
            this.btnAutoDetect.Name = "btnAutoDetect";
            this.btnAutoDetect.Size = new System.Drawing.Size(108, 40);
            this.btnAutoDetect.TabIndex = 16;
            this.btnAutoDetect.Text = "Auto Detect";
            this.btnAutoDetect.UseVisualStyleBackColor = true;
            this.btnAutoDetect.Click += new System.EventHandler(this.btnAutoDetect_Click);
            // 
            // scenariosUpDown
            // 
            this.scenariosUpDown.Location = new System.Drawing.Point(226, 251);
            this.scenariosUpDown.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.scenariosUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.scenariosUpDown.Name = "scenariosUpDown";
            this.scenariosUpDown.Size = new System.Drawing.Size(180, 26);
            this.scenariosUpDown.TabIndex = 15;
            this.scenariosUpDown.ValueChanged += new System.EventHandler(this.scenariosUpDown_ValueChanged);
            // 
            // txtLevelPath
            // 
            this.txtLevelPath.Location = new System.Drawing.Point(114, 285);
            this.txtLevelPath.Name = "txtLevelPath";
            this.txtLevelPath.Size = new System.Drawing.Size(292, 26);
            this.txtLevelPath.TabIndex = 17;
            this.txtLevelPath.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtLevelPath_KeyUp);
            // 
            // lblLevelPath
            // 
            this.lblLevelPath.AutoSize = true;
            this.lblLevelPath.Location = new System.Drawing.Point(24, 288);
            this.lblLevelPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLevelPath.Name = "lblLevelPath";
            this.lblLevelPath.Size = new System.Drawing.Size(83, 20);
            this.lblLevelPath.TabIndex = 18;
            this.lblLevelPath.Text = "Level Path";
            // 
            // checkBoxDevMode
            // 
            this.checkBoxDevMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDevMode.AutoSize = true;
            this.checkBoxDevMode.Location = new System.Drawing.Point(531, 323);
            this.checkBoxDevMode.Name = "checkBoxDevMode";
            this.checkBoxDevMode.Size = new System.Drawing.Size(173, 24);
            this.checkBoxDevMode.TabIndex = 19;
            this.checkBoxDevMode.Text = "Development Mode";
            this.checkBoxDevMode.UseVisualStyleBackColor = true;
            this.checkBoxDevMode.CheckedChanged += new System.EventHandler(this.checkBoxDevMode_CheckedChanged);
            // 
            // lblSoundtrackPath
            // 
            this.lblSoundtrackPath.AutoSize = true;
            this.lblSoundtrackPath.Location = new System.Drawing.Point(20, 323);
            this.lblSoundtrackPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSoundtrackPath.Name = "lblSoundtrackPath";
            this.lblSoundtrackPath.Size = new System.Drawing.Size(87, 20);
            this.lblSoundtrackPath.TabIndex = 21;
            this.lblSoundtrackPath.Text = "Music Path";
            // 
            // txtSoundtrackPath
            // 
            this.txtSoundtrackPath.Location = new System.Drawing.Point(114, 320);
            this.txtSoundtrackPath.Name = "txtSoundtrackPath";
            this.txtSoundtrackPath.Size = new System.Drawing.Size(292, 26);
            this.txtSoundtrackPath.TabIndex = 20;
            this.txtSoundtrackPath.TextChanged += new System.EventHandler(this.txtSoundtrackPath_TextChanged);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 766);
            this.Controls.Add(this.lblSoundtrackPath);
            this.Controls.Add(this.txtSoundtrackPath);
            this.Controls.Add(this.checkBoxDevMode);
            this.Controls.Add(this.lblLevelPath);
            this.Controls.Add(this.txtLevelPath);
            this.Controls.Add(this.btnAutoDetect);
            this.Controls.Add(this.scenariosUpDown);
            this.Controls.Add(this.lblScenarios);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.timeSpeedUpDown);
            this.Controls.Add(this.timeSpeedLabel);
            this.Controls.Add(this.PropertyGridSettings);
            this.Controls.Add(this.TreeViewSettings);
            this.Controls.Add(this.WeatherDropDown);
            this.Controls.Add(this.WeatherLabel);
            this.Controls.Add(this.BackSpeedUpDown);
            this.Controls.Add(this.BackSpeedLabel);
            this.Controls.Add(this.NYearsUpDown);
            this.Controls.Add(this.NYearsLabel);
            this.Controls.Add(this.NBacksUpDown);
            this.Controls.Add(this.NBacksLabel);
            this.Controls.Add(this.NZonesUpDown);
            this.Controls.Add(this.NZonesLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NZonesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NBacksUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NYearsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scenariosUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NZonesLabel;
        private System.Windows.Forms.NumericUpDown NZonesUpDown;
        private System.Windows.Forms.NumericUpDown NBacksUpDown;
        private System.Windows.Forms.Label NBacksLabel;
        private System.Windows.Forms.NumericUpDown NYearsUpDown;
        private System.Windows.Forms.Label NYearsLabel;
        private System.Windows.Forms.NumericUpDown BackSpeedUpDown;
        private System.Windows.Forms.Label BackSpeedLabel;
        private System.Windows.Forms.Label WeatherLabel;
        private System.Windows.Forms.ComboBox WeatherDropDown;
        private System.Windows.Forms.TreeView TreeViewSettings;
        private System.Windows.Forms.PropertyGrid PropertyGridSettings;
        private System.Windows.Forms.NumericUpDown timeSpeedUpDown;
        private System.Windows.Forms.Label timeSpeedLabel;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblScenarios;
        private System.Windows.Forms.Button btnAutoDetect;
        private System.Windows.Forms.NumericUpDown scenariosUpDown;
        private System.Windows.Forms.TextBox txtLevelPath;
        private System.Windows.Forms.Label lblLevelPath;
        private System.Windows.Forms.CheckBox checkBoxDevMode;
        private System.Windows.Forms.Label lblSoundtrackPath;
        private System.Windows.Forms.TextBox txtSoundtrackPath;
        private System.Windows.Forms.Timer timer1;
    }
}