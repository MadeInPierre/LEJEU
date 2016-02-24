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
    public partial class LayersForm : Form
    {
        private bool FormLoaded = false;

        public LayersForm()
        {
            InitializeComponent();

            cPlatforms.Checked = LayersDisplay.Platforms;
            cLadder.Checked = LayersDisplay.Ladder;
            cOthers.Checked = LayersDisplay.Others;
            if (cPlatforms.Checked || cLadder.Checked || cOthers.Checked)
                cCollisions.Checked = true;
            if(!cCollisions.Checked)
            {
                cPlatforms.Enabled = false;
                cOthers.Enabled = false;
                cLadder.Enabled = false;
            }

            cSectors.Checked = LayersDisplay.Sectors;
            cTCCs.Checked = LayersDisplay.TCCs;

            cEnemies.Checked = LayersDisplay.Enemies;
            cTraps.Checked = LayersDisplay.Traps;
            cDeco.Checked = LayersDisplay.Decoration;
            cItems.Checked = LayersDisplay.Items;
            if (cEnemies.Checked || cTraps.Checked || cDeco.Checked || cItems.Checked)
                cSpawns.Checked = true;
            if(!cSpawns.Checked)
            {
                cEnemies.Enabled = false;
                cTraps.Enabled = false;
                cDeco.Enabled = false;
                cItems.Enabled = false;
            }

            FormLoaded = true;
        }

        private void cCollisions_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                cPlatforms.Enabled = true;
                cOthers.Enabled = true;
                cLadder.Enabled = true;

                if (cCollisions.Checked)
                {
                    cPlatforms.Checked = true;
                    cOthers.Checked = true;
                    cLadder.Checked = true;
                }
                else
                {
                    cPlatforms.Checked = false;
                    cOthers.Checked = false;
                    cLadder.Checked = false;

                    cPlatforms.Enabled = false;
                    cOthers.Enabled = false;
                    cLadder.Enabled = false;
                }
            }
        }

        private void cPlatforms_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Platforms = cPlatforms.Checked;
            if (cPlatforms.Checked) ShowCollisions("Platforms");
            else HideCollisions("Platforms");
        }

        private void cOthers_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Others = cOthers.Checked;
            if (cOthers.Checked) ShowCollisions("Others");
            else ShowCollisions("Others");
        }

        private void cLadder_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Ladder = cLadder.Checked;
            if (cLadder.Checked) ShowCollisions("Ladder");
            else ShowCollisions("Ladder");
        }



        private void cSectors_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Sectors = cSectors.Checked;
            if (cSectors.Checked)
                MainForm.UpdateObjectsVisibility();
            else
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (Sector s in sc.Sectors)
                        s.Hide();
        }

        private void cTCCs_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.TCCs = cTCCs.Checked;
            if (cTCCs.Checked)
                MainForm.UpdateObjectsVisibility();
            else
            {
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (ZoneProperties z in sc.Zones)
                        foreach (ItemProperties i in z.Items)
                            i.Hide();
            }
        }


        private void cSpawns_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                cEnemies.Enabled = true;
                cTraps.Enabled = true;
                cDeco.Enabled = true;
                cItems.Enabled = true;

                if (cSpawns.Checked)
                {
                    cEnemies.Checked = true;
                    cTraps.Checked = true;
                    cDeco.Checked = true;
                    cItems.Checked = true;
                }
                else
                {
                    cEnemies.Checked = false;
                    cTraps.Checked = false;
                    cDeco.Checked = false;
                    cItems.Checked = false;

                    cEnemies.Enabled = false;
                    cTraps.Enabled = false;
                    cDeco.Enabled = false;
                    cItems.Enabled = false;
                }
            }
        }

        private void cEnemies_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Enemies = cEnemies.Checked;
            if (cEnemies.Checked)
                MainForm.UpdateObjectsVisibility();
            else
            {
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (EnemyProperties en in sc.Enemies)
                        en.Hide();
                foreach (EnemyProperties en in LevelProperties.TravelEnemies)
                    en.Hide();
            }
        }

        private void cTraps_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Traps = cTraps.Checked;
            if (cTraps.Checked)
                MainForm.UpdateObjectsVisibility();
            else
            {
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (ZoneProperties z in sc.Zones)
                        foreach (PassiveTrapsProperties pt in z.PassiveTraps)
                            pt.Hide();
                foreach (PassiveTrapsProperties pt in LevelProperties.TravelTraps)
                    pt.Hide();
            }
        }

        private void cDeco_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Decoration = cDeco.Checked;
            if (cDeco.Checked)
                MainForm.UpdateObjectsVisibility();
            else
            {
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (ZoneProperties z in sc.Zones)
                        foreach (DecorationProperties d in z.Decoration)
                            d.Hide();
                foreach (DecorationProperties d in LevelProperties.TravelDecos)
                    d.Hide();
            }
        }

        private void cItems_CheckedChanged(object sender, EventArgs e)
        {
            LayersDisplay.Items = cItems.Checked;
            if (cItems.Checked)
                MainForm.UpdateObjectsVisibility();
            else
            {
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    foreach (ZoneProperties z in sc.Zones)
                        foreach (ItemProperties i in z.Items)
                            i.Hide();
            }
        }

        public static void HideCollisions(string param)
        { //param can be Platforms, Ladder, Others
            if(param == "Platforms")
            {
                HideCollisions("Solid");
                HideCollisions("Semi");
                return;
            }
            if(param == "Others")
            {
                //When changing the collisions list, add also a line with "HideCollisions("[new UserData name]");"
                HideCollisions("Water");
                HideCollisions("Sand");
                return;
            }

            foreach (ObjCircle c in LevelProperties.Collisions.CircleList)
                if (c.UserData == param)
                    c.Hide();
            foreach (ObjRectangle r in LevelProperties.Collisions.RectList)
                if (r.UserData == param)
                    r.Hide();
            foreach (ObjPolygon p in LevelProperties.Collisions.PolyList)
                if (p.UserData == param)
                    p.Hide();
            foreach (ObjEdge e in LevelProperties.Collisions.EdgeList)
                if (e.UserData == param)
                    e.Hide();
            foreach (ObjEdgeChain e in LevelProperties.Collisions.EdgeChainList)
                if (e.UserData == param)
                    e.Hide();
        }
        public static void ShowCollisions(string param)
        { //param can be Platforms, Ladder, Others
            if (param == "Platforms")
            {
                ShowCollisions("Solid");
                ShowCollisions("Semi");
                return;
            }
            if (param == "Others")
            {
                //When changing the collisions list, add also a line with "ShowCollisions("[new UserData name]");"
                ShowCollisions("Water");
                ShowCollisions("Sand");
                return;
            }

            foreach (ObjCircle c in LevelProperties.Collisions.CircleList)
                if (c.UserData == param)
                    c.Show();
            foreach (ObjRectangle r in LevelProperties.Collisions.RectList)
                if (r.UserData == param)
                    r.Show();
            foreach (ObjPolygon p in LevelProperties.Collisions.PolyList)
                if (p.UserData == param)
                    p.Show();
            foreach (ObjEdge e in LevelProperties.Collisions.EdgeList)
                if (e.UserData == param)
                    e.Show();
            foreach (ObjEdgeChain e in LevelProperties.Collisions.EdgeChainList)
                if (e.UserData == param)
                    e.Show();
        }

        
    }

    public static class LayersDisplay
    {
        //Determines if a kind of object should be shown or not.
        public static bool Platforms            { get; set; }
        public static bool Others               { get; set; }
        public static bool Ladder               { get; set; }

        public static bool Sectors              { get; set; }
        public static bool TCCs                 { get; set; }

        public static bool Enemies              { get; set; }
        public static bool Traps                { get; set; }
        public static bool Decoration           { get; set; }
        public static bool Items                { get; set; }

        public static void Initialize()
        {
            Platforms = true;
            Others = true;
            Ladder = true;

            Sectors = true;
            TCCs = true;

            Enemies = true;
            Traps = true;
            Decoration = true;
            Items = true;
        }
    }
}
