using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class SettingsForm : Form
    {
        #region Load
        private enum TreeViewContent { None, Zones, Scenarios, Backs }
        private TreeViewContent treeViewContent = TreeViewContent.None;
        private bool FormLoaded = false;
        public SettingsForm()
        {
            InitializeComponent();

            timer1.Interval = 1000;
            timer1.Start();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        { //when the form is created
            //change de dropdowns to the current LevelProperties values.
            NBacksUpDown.Value = LevelProperties.Backgrounds.Count - 1;
            NZonesUpDown.Value = LevelProperties.NZones - 1;
            scenariosUpDown.Value = LevelProperties.Scenarios.Count - 1;
            NYearsUpDown.Value = LevelProperties.NYears;
            BackSpeedUpDown.Value = (decimal)LevelProperties.BackSpeed;
            timeSpeedUpDown.Value = LevelProperties.TTS;
            WeatherDropDown.Text = LevelProperties.Weather;
            txtSoundtrackPath.Text = LevelProperties.SoundtrackPath;

            txtLevelPath.Text = LevelProperties.LevelBasePath;
            checkBoxDevMode.Checked = EditorVariables.DevMode;

            FormLoaded = true;
        }
        #endregion

        #region DropDowns
        private void NZonesUpDown_ValueChanged(object sender, EventArgs e)
        { //when we change the NZones value
            //change the global NZones value
            LevelProperties.NZones = (int)NZonesUpDown.Value + 1;
            

            for (int i = 0; i <= NZonesUpDown.Value; i++)
            {
                if (i >= LevelProperties.NZones) //if the zone does not exist yet
                {
                    foreach (ScenarioProperties scenario in LevelProperties.Scenarios)
                    {
                        scenario.Zones.Add(new ZoneProperties());
                        //{
                        //    platformPath = "null",
                        //    foregroundPath = "null",
                        //    PassiveTraps = new List<PassiveTrapsProperties>()
                        //});
                    }
                }
            }

            if(NZonesUpDown.Value < LevelProperties.NZones - 1)
            {
                foreach (ScenarioProperties scenario in LevelProperties.Scenarios)
                {
                    scenario.Zones.RemoveRange((int)NZonesUpDown.Value, (LevelProperties.NZones - 1) - (int)NZonesUpDown.Value);
                }
            }

            loadZonesTreeView();
            foreach (ScenarioProperties sc in LevelProperties.Scenarios) sc.UpdateZonesCount();
            //PropertyGridSettings.SelectedObject = LevelProperties.Zones.Last();

        }

        private void NBacksUpDown_ValueChanged_1(object sender, EventArgs e)
        { //when we change the NBacks value
            //change the global NBacks value
            LevelProperties.NBacks = (int)NBacksUpDown.Value;
            
            for (int i = 0; i <= NBacksUpDown.Value; i++)
            {
                if (i >= LevelProperties.Backgrounds.Count) //if the zone does not exist yet
                {
                    LevelProperties.Backgrounds.Add("null");
                }
            }

            if (NBacksUpDown.Value < LevelProperties.Backgrounds.Count - 1)
            {
                LevelProperties.Backgrounds.RemoveRange((int)NBacksUpDown.Value, (LevelProperties.Backgrounds.Count - 1) - (int)NBacksUpDown.Value);
            }

            loadBacksTreeView();
            PropertyGridSettings.SelectedObject = LevelProperties.Backgrounds.Last();
        }

        private void NYearsUpDown_ValueChanged(object sender, EventArgs e)
        { //when we change the NYears value
            //change the global NYears value
            LevelProperties.NYears = (int)NYearsUpDown.Value;
            //change the mainform slider capacity to NYears DONE IN MAINFORM
        }

        private void WeatherDropDown_SelectedIndexChanged(object sender, EventArgs e)
        { //When we change the Weather
          //change the global weather enum/string
          //weather = (ComboBox)WeatherDropDown).Text;
        }

        private void timeSpeedUpDown_ValueChanged(object sender, EventArgs e)
        {
            LevelProperties.TTS = (int)timeSpeedUpDown.Value;
        }
        #endregion

        #region Labels
        private void NZonesLabel_Click(object sender, EventArgs e)
        {
            loadZonesTreeView();

            //PropertyGridSettings.SelectedObject = LevelProperties.Zones[0];

        }
        #endregion

        #region Helpers
        private void TreeViewSettings_AfterSelect(object sender, TreeViewEventArgs e)
        { // when we want to change the element in the propertyGrid
            if (treeViewContent == TreeViewContent.Zones)
                ;//PropertyGridSettings.SelectedObject = LevelProperties.Zones[Convert.ToInt32(TreeViewSettings.SelectedNode.Text)];
            else if (treeViewContent == TreeViewContent.Scenarios)
                PropertyGridSettings.SelectedObject = LevelProperties.Scenarios[Convert.ToInt32(TreeViewSettings.SelectedNode.Text)];
            else if (treeViewContent == TreeViewContent.Backs)
                PropertyGridSettings.SelectedObject = LevelProperties.Backgrounds[Convert.ToInt32(TreeViewSettings.SelectedNode.Text)];
        }
        
        private void loadZonesTreeView()
        {
            treeViewContent = TreeViewContent.Zones;
            TreeViewSettings.Nodes.Clear();
            
            for (int i = 0; i < LevelProperties.NZones; i++)
            {
                TreeNode node = new TreeNode(i.ToString());
                TreeViewSettings.Nodes.Add(node);
            }
        }

        private void loadBacksTreeView()
        {
            treeViewContent = TreeViewContent.Backs;
            TreeViewSettings.Nodes.Clear();
            int count = 0;
            foreach (string back in LevelProperties.Backgrounds)
            {
                TreeNode node = new TreeNode(count.ToString());
                TreeViewSettings.Nodes.Add(node);
                count++;
            }
        }

        private void loadScenariosTreeView()
        {
            treeViewContent = TreeViewContent.Scenarios;
            TreeViewSettings.Nodes.Clear();
            int count = 0;
            foreach (ScenarioProperties sc in LevelProperties.Scenarios)
            {
                TreeNode node = new TreeNode(count.ToString());
                TreeViewSettings.Nodes.Add(node);
                count++;
            }
        }
        #endregion

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Editor.reloadImages = true;
            
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {//IMPROVE Load Image button useless ?
            if (PropertyGridSettings.SelectedGridItem != null)
            {
                if (PropertyGridSettings.SelectedGridItem.Label.ToString() == "platformPath")
                {
                    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {
                        string path = openFileDialog1.FileName;
                        //((ZoneProperties)PropertyGridSettings.SelectedObject).platformPath = path;
                    }
                }
                if (PropertyGridSettings.SelectedGridItem.Label.ToString() == "foregroundPath")
                {
                    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                    if (result == DialogResult.OK) // Test result.
                    {
                        string path = openFileDialog1.FileName;
                        //((ZoneProperties)PropertyGridSettings.SelectedObject).foregroundPath = path;
                    }
                }
            }

        }

        #region Scenarios (useless for now)
        private void lblScenarios_Click(object sender, EventArgs e)
        {
            loadScenariosTreeView();
        }
        private void scenariosUpDown_ValueChanged(object sender, EventArgs e)
        {

            for (int i = 0; i <= scenariosUpDown.Value; i++)
            {
                if (i >= LevelProperties.Scenarios.Count) //if the zone does not exist yet
                    LevelProperties.Scenarios.Add(new ScenarioProperties());
            }

            if (scenariosUpDown.Value < LevelProperties.Scenarios.Count - 1)
                LevelProperties.Scenarios.RemoveRange((int)scenariosUpDown.Value, (LevelProperties.Scenarios.Count - 1) - (int)scenariosUpDown.Value);

            loadScenariosTreeView();

            PropertyGridSettings.SelectedObject = LevelProperties.Scenarios.Last();

        }
        #endregion

        private void btnAutoDetect_Click(object sender, EventArgs e)
        { 
            //When clicking this button, the user will choose the Root Directory of a level.
            // this method will automatically detect how many zones are in this level (based on the images) and fill the LevelProperties informations.

            string levelRelativePath = GetFolderToInspect();
            ScanImages(levelRelativePath);
        }

        public static void ScanImages(string pathToInspect)
        {
            if (string.IsNullOrEmpty(pathToInspect)) return; //if we couldn't get the folder, cancel the action
             //"Maps/Level1.1"
            
            string levelFullPath = EditorVariables.ContentBasePath + pathToInspect;
            // "C:/Users/_Jack_-_Daxter_0/Google Drive/LEJEU/CODE/LEJEU/LEJEU.Content/Maps/Level1.1/"
            Console.WriteLine("Full path to scan : " + levelFullPath);

            
            try
            {
                int numberOfZones;
                string[] platformsImgNames = Directory.GetFiles(levelFullPath + "Zones/Platforms", "*.*", SearchOption.TopDirectoryOnly)
                                                                .Select(path => Path.GetFileName(path))
                                                                .ToArray(); //gets only the file names of the files in the selected dir ([level]/Zones/Platforms)

                numberOfZones = platformsImgNames.Length;                       // 
                for (int i = 0; i <= platformsImgNames.Length; i++)              // Determine the number of zones in the level.
                {                                                               // the number of zones is normally the size of the array,
                    if (!platformsImgNames.Contains("zone" + i + ".png"))       // but the for loop is here to ignore/prevent junk files (e.g. "Desktop.ini")
                    { numberOfZones = i; break; }                               //
                }                                                               //
                Console.WriteLine("Found {0} valid zones.", numberOfZones);

                ///////////////////////////////BACKGROUNDS COUNTING
                int numberOfBacks;
                string[] backImgNames = Directory.GetFiles(levelFullPath + "Backgrounds", "*.*", SearchOption.TopDirectoryOnly)
                                                                .Select(path => Path.GetFileName(path))
                                                                .ToArray(); //gets only the file names of the files in the selected dir ([level]/Zones/Platforms)

                numberOfBacks = backImgNames.Length;                            // 
                for (int i = 0; i <= backImgNames.Length; i++)                   // Determine the number of backgrounds in the level.
                {                                                               // the number of backs is normally the size of the array,
                    if (!backImgNames.Contains("back" + i + ".png"))            // but the for loop is here to ignore/prevent junk files (e.g. "Desktop.ini")
                    { numberOfBacks = i; break; }                               //
                }                                                               //
                Console.WriteLine("Found {0} valid backgrounds.", numberOfBacks);



                
                LevelProperties.LevelBasePath = pathToInspect;
                LevelProperties.NZones = numberOfZones;
                LevelProperties.NBacks = numberOfBacks;
                
                    foreach (ScenarioProperties sc in LevelProperties.Scenarios) sc.UpdateZonesCount();
            }
            catch { MessageBox.Show("Could not import all the images. Did you point to a Level's root folder, and is it full of images already ?", 
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private string GetFolderToInspect()
        { // Select a folder and get the path relative to the LEJEU.Content folder. if it is invalid, return null.
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.SelectedPath = new Uri(EditorVariables.ContentBasePath).ToString() + "Maps/";
            dialog.ShowDialog();

            Uri fileAbsoluteLocation = new Uri(dialog.SelectedPath);

            string s = Helpers.GetRelativePathFromContent(EditorVariables.ContentBasePath, fileAbsoluteLocation.ToString());
            //s = s.Substring(s.IndexOf(@"/") + 1); // take of the unwanted "LEJEU.Content/" at the beginning

            if (string.IsNullOrEmpty(s))
            {
                MessageBox.Show("Folder is invalid. Try again with a folder inside LEJEU.Content/", "Invalid Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
            {
                if (MessageBox.Show("Loading " + s + ". Continue ?", "Folder found", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return null;
            }
            s += "/";
            Console.WriteLine("new Level Base Path : " + s);
            LevelProperties.LevelBasePath = s; //this is where the Base Level Folder is, set it in the LP.
            return s;
        }

        private void txtLevelPath_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ScanImages(txtLevelPath.Text);
                LevelProperties.LevelBasePath = txtLevelPath.Text;
            }
        }

        private void checkBoxDevMode_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                if (checkBoxDevMode.Checked)
                {
                    DialogResult result = MessageBox.Show("Are you sure the Level Base Path is updated ? Make sure the number of zone is correct before proceeding.",
                                                          "Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (result)
                    {
                        case DialogResult.Cancel:
                            checkBoxDevMode.Checked = false;
                            return; //get outta here
                        case DialogResult.Yes:
                            EditorVariables.DevMode = true;
                            break;
                        case DialogResult.No:
                            string levelRelativePath = GetFolderToInspect();
                            ScanImages(levelRelativePath);
                            EditorVariables.DevMode = true;
                            break;
                    }


                }
                else EditorVariables.DevMode = false;
            }
        }

        private void txtSoundtrackPath_TextChanged(object sender, EventArgs e)
        {
            LevelProperties.SoundtrackPath = txtSoundtrackPath.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(!txtLevelPath.Focused)
                txtLevelPath.Text = LevelProperties.LevelBasePath;
            NZonesUpDown.Value = LevelProperties.NZones - 1;
            NBacksUpDown.Value = LevelProperties.NBacks;
            if(!txtSoundtrackPath.Focused)
                txtSoundtrackPath.Text = LevelProperties.SoundtrackPath;
        }
    }

}