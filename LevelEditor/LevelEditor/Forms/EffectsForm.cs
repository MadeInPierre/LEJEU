using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
    public partial class EffectsForm : Form
    {
        public List<string> FinalEffects;
        public bool Closed;

        public EffectsForm(List<string> SelectedObject)
        {
            InitializeComponent();
            if (SelectedObject != null)
                FinalEffects = SelectedObject;
            else FinalEffects = new List<string>();

            if (SelectedObject != null)
            { // get the current effects already in the sector
                foreach (string s in SelectedObject)
                {
                    TreeNode node = new TreeNode(s);
                    EffectsTreeView.Nodes.Add(node);
                }
            }

            Closed = false;
        }

        private void EffectsForm_Load(object sender, EventArgs e)
        {
            //Manually put all the existing effects.. hardcoded ?
            //IMPROVE create an effects xml ? (needed ?)
            EffectsDictTreeView.Nodes.Add(new TreeNode("DAMAGE [int amount]"));
            EffectsDictTreeView.Nodes.Add(new TreeNode("KILL"));
            EffectsDictTreeView.Nodes.Add(new TreeNode("DAMAGE [int amount] [int damage/sec]"));
        }

        private void btnAddTCC_Click(object sender, EventArgs e)
        {
            if (EffectsDictTreeView.SelectedNode != null)
            {
                string[] s = EffectsDictTreeView.SelectedNode.Text.Split('[');

                string finalString = s[0]; //get the ID of the effect
                if(s[0].ToCharArray()[s[0].Length - 1] == ' ')
                    finalString = s[0].Remove(s[0].Length - 1); // take off the unwanted space at the end if there is one

                for (int i = 1; i < s.Length; i++)
                {
                    string newArg = Prompt.ShowDialog(s[i]);
                    finalString += ' ' + newArg;
                }

                TreeNode node = new TreeNode(finalString);
                EffectsTreeView.Nodes.Add(node);

                RefreshFinalEffects();
                //((Sector)((MainForm)this.ParentForm).SelectedProperty).Effects.Add(finalString);
            }
        }

        private void btnRenameTCC_Click(object sender, EventArgs e)
        {
            if (EffectsTreeView.SelectedNode != null)
            {
                EffectsTreeView.SelectedNode.Text = Prompt.ShowDialog(EffectsTreeView.SelectedNode.Text);
                RefreshFinalEffects();
            }
        }



        private void RefreshFinalEffects()
        {
            List<string> s = new List<string>();
            foreach (TreeNode node in EffectsTreeView.Nodes)
            {
                s.Add(node.Text);
            }
            FinalEffects = s;
        }

        private void EffectsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshFinalEffects();
            Closed = true;
        }
    }
}
