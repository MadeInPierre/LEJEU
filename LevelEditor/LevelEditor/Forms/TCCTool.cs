using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public partial class TCCTool : Form
    {
        public bool FormClosed;
        private bool ShiftPressed;
        private bool CtrlPressed;

        public TCCProperties CurrentTCC
        {
            get; set;
            //get
            //{ //search for the TCC that contains the PropertyGrid's state
            //    if (PropertyGrid.SelectedObject != null && CurrentState != null)
            //    {
            //        foreach (TCCProperties TCC in LevelProperties.TCCs)
            //        {
            //            foreach (TCCState s in TCC.States)
            //            {
            //                if (s.Equals(CurrentState)) return TCC;
            //            }
            //        }
            //    }
            //    return null;
            //}
        }
        public TCCState CurrentState
        {
            get; set;
            //get
            //{
            //    if (PropertyGrid.SelectedObject != null)
            //        return (TCCState)PropertyGrid.SelectedObject;
            //    else return null;
            //}
        }

        public int SelectedTCCIndex
        {
            get
            {
                if (TCCsTreeView.SelectedNode != null)
                    return TCCsTreeView.SelectedNode.Index;
                else return -1;
            }
        }
        public int SelectedStateIndex
        {
            get
            {
                if (StatesTreeView.SelectedNode != null)
                    return StatesTreeView.SelectedNode.Index;
                return -1;
            }
        }

        public TCCTool()
        {
            InitializeComponent();
            FormClosed = false;
            timer1.Interval = 1000;
            timer1.Start();
            checkSF.Enabled = false;
            checkActiveSF.Enabled = false;
        }

        private void TTCTool_Load(object sender, EventArgs e)
        {
            this.TopMost = true; // always on top window
            RefreshData();
        }

        private void TCCTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1_Tick(null, null);
            FormClosed = true;
        }

        public void RefreshData()
        {
            lblCurrentYear.Text = EditorVariables.CurrentYear.ToString();

            TCCsTreeView.Nodes.Clear();
            int counter = 0;
            foreach (TCCProperties TCC in LevelProperties.TCCs)
            {
                counter++;
                // IF IT EXIST IN CURRENTYEAR
                TreeNode node = new TreeNode(TCC.TCCName);

                TCCsTreeView.Nodes.Add(node);
            }

            RefreshPropertyGrid();
        }
        public void RefreshPropertyGrid()
        {
            PropertyGrid.Refresh();
        }

        private void TCCsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        { //when selecting a new TCC, load its states
            if (TCCsTreeView.SelectedNode != null)
            {
                CurrentTCC = LevelProperties.TCCs[TCCsTreeView.SelectedNode.Index];
                checkSF.Enabled = true;
                checkSF.Checked = CurrentTCC.IsSafeZone;
                if (CurrentTCC.IsSafeZone && CurrentState != null)
                {
                    checkActiveSF.Enabled = true;
                    checkActiveSF.Checked = CurrentState.IsActiveState;
                }
                else
                {
                    checkActiveSF.Enabled = false;
                    checkActiveSF.Checked = false;
                }
            }
            else
            {
                CurrentTCC = null;
                checkSF.Checked = false;
                checkSF.Enabled = false;
            }
            StatesTreeView.Nodes.Clear();
            int counter = 0;
            foreach (TCCState state in CurrentTCC.States)
            {
                counter++;
                TreeNode node = new TreeNode("State " + counter.ToString());

                StatesTreeView.Nodes.Add(node);
            }
        }

        private void StatesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (StatesTreeView.SelectedNode != null)
            {
                CurrentState = CurrentTCC?.States[StatesTreeView.SelectedNode.Index];
                if (CurrentTCC.IsSafeZone)
                {
                    checkActiveSF.Enabled = true;
                    checkActiveSF.Checked = CurrentState.IsActiveState;
                }
                else
                {
                    checkActiveSF.Enabled = false;
                    checkActiveSF.Checked = false;
                }
            }
            else
            {
                CurrentState = null;
                checkActiveSF.Enabled = false;
                checkActiveSF.Checked = false;
            }

            PropertyGrid.SelectedObject = CurrentState;
            //get the selected state of the selected TCC
        }

        private void btnRenameTCC_Click(object sender, EventArgs e)
        { // rename a TCC
            if (TCCsTreeView.SelectedNode != null)
            {
                string newName = Prompt.ShowDialog("Rename");
                if (newName != null && newName != "")
                    CurrentTCC.TCCName = newName;
                RefreshData();
            }
        }

        private void btnRemoveTCC_Click(object sender, EventArgs e)
        {
            if (TCCsTreeView.SelectedNode != null)
            {
                LevelProperties.TCCs.RemoveAt(TCCsTreeView.SelectedNode.Index);
                TCCsTreeView.Nodes.RemoveAt(TCCsTreeView.SelectedNode.Index);
            }
        }

        private void btnAddTCC_Click(object sender, EventArgs e)
        {
            TreeNode node = new TreeNode("New Node");
            TCCsTreeView.Nodes.Add(node);

            LevelProperties.TCCs.Add(new TCCProperties() { TCCName = "New Node" });
        }

        private void btnAddState_Click(object sender, EventArgs e)
        {
            if (TCCsTreeView.SelectedNode != null)
            {
                CurrentTCC.States.Add(new TCCState());
                PropertyGrid.SelectedObject = CurrentTCC?.States.Last();

                string name = "State " + CurrentTCC?.States.Count().ToString();
                TreeNode node = new TreeNode(name);
                StatesTreeView.Nodes.Add(node);
            }
        }

        private void btnRemoveState_Click(object sender, EventArgs e)
        {
            if (StatesTreeView.SelectedNode != null && CurrentTCC != null)
            {
                CurrentTCC.States.RemoveAt(StatesTreeView.SelectedNode.Index);
                StatesTreeView.Nodes.RemoveAt(StatesTreeView.SelectedNode.Index);
            }
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string path = openFileDialog1.FileName;
                Console.WriteLine(path);

                CurrentState.ImagePath = Helpers.GetRelativePathFromContent(LevelProperties.LevelBasePath, path);
                FileStream fs = new FileStream(EditorVariables.ContentBasePath + CurrentState.ImagePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                CurrentState.Image = Texture2D.FromStream(Game1.graphicsDevice, fs); fs.Flush();
                PropertyGrid.Refresh();
            }
        }
        private void btnRemoveImg_Click(object sender, EventArgs e)
        {
            if (CurrentState != null)
            {
                CurrentState.ImagePath = "";
                CurrentState.Image.Dispose();
                CurrentState.Image = null;
            }
            PropertyGrid.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CurrentState != null)
            {
                CurrentState.CollisionObject?.Delete();
                CurrentState.CollisionObject = null;
                Refresh();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        begin:
            foreach (TCCProperties t in LevelProperties.TCCs)
            {
                //foreach (State s in t.States)
                //{
                //    if (s.CollisionObject != null && !s.CollisionObject.Alive) { t.States.Remove(s); goto begin; } //if we deleted in the XNAView the CollisionObject,
                //}                                                                                                   // take out the whole state (don't remember why i did it)
                for (int i = 0; i < t.States.Count; i++)
                {
                    if (t.States[i].CollisionObject != null && !t.States[i].CollisionObject.Alive) t.States[i].CollisionObject = null;
                }
            }
        }

        private void checkSF_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentTCC != null)
            {
                CurrentTCC.IsSafeZone = checkSF.Checked;
                if (checkSF.Checked && CurrentState != null)
                {
                    checkActiveSF.Enabled = true;
                    checkActiveSF.Checked = CurrentState.IsActiveState;
                }
                else { checkActiveSF.Enabled = false; checkActiveSF.Checked = false; }
            }
            else { checkSF.Enabled = false; checkSF.Checked = false; }

        }
        private void checkActiveSF_CheckedChanged(object sender, EventArgs e)
        {
            if(CurrentTCC != null)
                CurrentState.IsActiveState = checkActiveSF.Checked;
        }

        private void PropertyGrid_Scroll(object sender, MouseEventArgs e)
        {
            float d = e.Delta / 120 * 0.1f; // get the mouse wheel direction (1 for up and -1 for down). the more the wheel goes fast, the bigger the value gets.
            if (ShiftPressed) d /= 2; // if Shift is pressed, move slower.
            if (PropertyGrid.SelectedObject != null)
            { //IMPROVE Update all of this : all vector2's X and Y properties don't want to show up anymore
                if (PropertyGrid.SelectedGridItem != null)
                {
                    if (PropertyGrid.SelectedGridItem.Label.ToString() == "ImagePosition")
                        if (CtrlPressed) CurrentState.ImagePosition += new Vector2(0, d);
                        else CurrentState.ImagePosition += new Vector2(d, 0);
                }
                RefreshPropertyGrid();
            }
        }

        private void TCCTool_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift) ShiftPressed = true;
            else ShiftPressed = false;

            if (e.Control) CtrlPressed = true;
            else CtrlPressed = false;
        }

        private void TCCTool_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift) ShiftPressed = true;
            else ShiftPressed = false;

            if (e.Control) CtrlPressed = true;
            else CtrlPressed = false;
        }
    }
}