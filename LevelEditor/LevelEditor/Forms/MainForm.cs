#region usings
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.DebugView;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using LEJEU.Entities;
using LEJEU.Entities.Enemies;
using Microsoft.Xna.Framework.Content;
using System.Xml.Linq;
using System.Xml;
#endregion
/* NOTES
 * All images (enemies, traps, items etc) have to be pointing to the left when designed.
 *  Enemies thumbnails must not be compiled to xnb fils, but stay at .pngs.
 */


/* .BUG REPORT ([priority] description) 
 * [normal]         when deleting and enemy, if we are touching both the enemy and his IAAux it crashes (because it tries to remove the IAAux when the enemy is already dead)
 * [normal]         When deleting and recreating objects in the TCC mode, the bodies are not sotred in the TCC's state anymore..
 * [low]            When resizing the MainForm, it completely bugs (weird)
 * 
 * 
 * 
 * [high : DONE]    The IAAux module is veeeery buggy everywhere.
 * [high : DONE]    fix the objects destination when they are created (they're currently all spawning in EditorVariables.RectList eg)
 * [high : DONE]    clicking with both left and right button in currentState.None will crash the app (cause : mouseFarseer removed twice)
 * [normal : DONE   When creating a sector, let the user be able to doubleclick the CollisionObject to have the Body's properties (and not the sector's)
 * [normal : DONE]  moving a polygon in NoState state will change the BodyPosition and not each vertice's position
 * [normal : DONE]  if we move the mouse clicking when the EnemyDict or TrapsDict is activated, it crashes.
 * [normal : DONE]  create a bool "creating" activated when we are currently creating an object. this is to prevent crashes in MouseMove
 * [normal : DONE]  PlayerSpawn and goalSpawn are not working anymore
 * [low : DONE]     Create an Effects Form for when we will want to edit the effects of a sector
 * [low : DONE]     When touching the World Origin cross in currentState.None (normal mode), it crashes in the MouseMove event. 
 * [low : DONE]     when using the scroll feature in the PropertyGrid, we can accidentaly set the size of an object below 0. ==> crash
 * 
 ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
 * IMPROVEMENTS
 * [normal]         Add moving platforms ==> create a new group : GeneralObjects/InteractiveObjects ? or put it in enemies as they are the same (moving platforms, trampoline) 
 * [normal]         Implement the decoration + SafeZones
 * [normal]         Implement the EditVertices feature for EdgeChains
 * [low]            only allow to delete/move an object if we are in the same Mode/State as the concerned object (to avoid us deleting the objects we didn't want to delete)
 * [low]            Add the possibility to show only the kinds of objects we want.
 * [low]            change the ennemy's thumbnail according to the age
 * [low]            Add the functions Save and Load + Run and Stop.
 * 
 * 
 
 * [high : DONE]    Adapt the resources paths relative to the Root Directory (eg. C:/User/GoogleDrive/RES/.../test.png ==> RES/.../test.png)
 * [normal : DONE]  Remove the images from the dictionaries xml and find it automatically (we can deduce it)
 * [normal : DONE]  Make the EditVertices feature
 * [normal : DONE]  Add slider ticks (epoques) REVIEW
 * [normal : DONE]  Add the possibility to choose different images at the background (for each scenario)
 * [normal : DONE]  Only let us create a polygon (starting putting vertices) if we actualy can (instead of making everything disapear if we actually couldn't)
 * [normal : DONE]  change the trap's point color to orange and not red
 * [normal : DONE]  when we move an object, the result only appears at the end and not while we're moving (cause unknown)
 * [normal : DONE]  change the EdgeList way of things : let it have its own list of ObjEdges
 * [low : DONE]     remove the cancel bool in the MouseDown Spawn and replace it with a goto
 * [low : DONE]     fix the color of the selected button in currentState (e.g. the grey is not at the good place when we click on currentMode Spawn)
 */





namespace LevelEditor
{
    public partial class MainForm : Form
    {

        #region Variables/Constructor

        Body mouseFarseer;
        public bool MouseInXNA; // Determines if the mouse is hovering on the XNAWindow surface. This is made to filter the keyboard inputs (text input / shortcuts)
        private int currentZone = 0; // in which zone is the mouse ? in currentZone. (used for spawns, mainly traps)
        private bool spawning = false; //determines if we are creating a spawn during a click/move/release (to avoid errors)
        private bool saveSucceeded = false;

        private bool ShiftPressed; // Stores if the shift key is currently pressed or not (used for shortcuts)

        public enum CurrentMode
        {
            None,
            Collisions,
            Sectors,
            Spawns,
            TCC,
            IAAux
        };
        public enum CurrentState
        {
            None,
            Polygon,
            Rectangle,
            Circle,
            Edge,
            EdgeChain,

            PlayerSpawn,
            EndSpawn,
            Spawn,
            GoalSpawn,

            EditVertices // change polygons' and edgeChains' vertices
        };

        public enum CurrentDict
        {
            None,
            Collisions,
            Sectors,
            Ennemies,
            Decoration,
            Items,
            Traps
        }

        public enum CurrentView
        {
            Scenario,
            Precision // year per year view
        }

        CurrentMode currentMode = CurrentMode.None;
        CurrentState currentState = CurrentState.None;
        CurrentDict currentDict = CurrentDict.None;


        public ScenarioProperties CurrentScenario
        {
            get
            {
                if (currentView == CurrentView.Scenario)
                    return LevelProperties.Scenarios[(int)UpDownScenario.Value];
                else return null;
            }
        }
        public int CurrentScenarioIndex
        {
            get { return (int)UpDownScenario.Value; }
        }

        public static CurrentView currentView = CurrentView.Precision;

        Vector2 clickPos = new Vector2(); // the position of the mouse when we CLICK on the XNAWindow
        //public ObjMouse mouseObject; // ATTENTION : A OPTIMISER, SOURCE D'ERREURS !

        private int nbrPointPol = 0; // made for building polygons

        public MainForm(string LoadPath)
        {
            InitializeComponent();

            #region Selecting Collisions Mode by Default
            ResetModeButtonsColor();
            CollisionsModeButton.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);

            //change mode to collisions
            currentMode = CurrentMode.Collisions;
            currentState = CurrentState.Polygon;

            BtnPolygon.Enabled = true;
            BtnRectangle.Enabled = true;
            BtnCircle.Enabled = true;
            BtnEdge.Enabled = true;
            btnSpawn.Enabled = false;
            #endregion
            #region Selecting No State mode by Default
            ResetStateButtonsColor();
            btnNoState.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.None;
            btnEditVertices.Enabled = false;
            #endregion
            loadCollisionsTree();
            TreeView.ExpandAll();

            LayersDisplay.Initialize();

            if(LoadPath != string.Empty)
            {
                LevelLoader.LoadLevel(LoadPath);
            }
        }

        public void Initialize()
        {
            //LoadScenarioView();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }
        #endregion

        #region Other Stuff
        private void SliderTime_Scroll(object sender, EventArgs e)
        { //When we change the slider's position/time
          //if(currentView == CurrentView.Precision)
            LblCurrentYear.Text = "Current Year : " + ((TrackBar)sender).Value.ToString();
            //else LblCurrentYear.Text = "Current Scenario : " + ((TrackBar)sender).Value.ToString();
            //change the current year

            UpdateObjectsVisibility();

            EditorVariables.CurrentYear = SliderTime.Value;
        }

        private void BtnScenarioView_Click(object sender, EventArgs e)
        { //Zoom In Button Click
            //LoadScenarioView(); //TODO put the Scenario system back ?
        }

        private void BtnPrecisionView_Click(object sender, EventArgs e)
        { // Zoom Out Button CLick
            //if (currentView == CurrentView.Scenario)
            //    LoadPrecisionView(SliderTime.Value);
            //else LoadPrecisionView((int)ScenarioLeft.Value);
        }

        private void btnRun_Click(object sender, EventArgs e)
        { // Launch the game, with the current level's path of the properties.xml
            //save the game
            DialogResult result = MessageBox.Show("Save the game first ?", "Run", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (result)
            {
                case DialogResult.Cancel:
                    break;
                case DialogResult.Yes:
                    LevelLoader.SaveLevel();

                    //launch the game
                    //Process.Start(game .exe path, levelPath(argument))
                    break;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            //stop the process.
        }

        private void TabXML_Click(object sender, EventArgs e)
        { //when we click on the XML tab
            //load and display the XML file.
            try
            {

                XmlDocument doc = new XmlDocument();
                doc.Load(EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "properties.xml"); //load the file
                string xmlText = doc.InnerXml; //take the string

                XDocument xDocument = XDocument.Parse(xmlText); //indent it
                XMLText.Text = xDocument.ToString(); // show it.
            }
            catch
            {
                MessageBox.Show("File " + EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "properties.xml" + " not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        { // Show a message box before closing in case we forgot to save
            string result = MessageBox.Show("Do you want to save before leaving ?", "Save and Exit",
                                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question).ToString();

            if (result == "Yes")
            { //save and leave
                e.Cancel = false;
                FileSave_Click(null, null);
            }
            else if (result == "No")
            { //leave
                e.Cancel = false;
            }
            else if (result == "Cancel")
            { //cancel closing
                e.Cancel = true;
            }

        }

        private void PropertyGrid_Scroll(object sender, MouseEventArgs e)
        { // if we scroll in the propertygrid, increase or decrease the value of the selected property (useless but fun)
            //HARDCODED : can't set the PropertyGrid.SelectedGridItem.Value, so i had to go through each case one by one. try to improve by setting the value directly.
            float d = e.Delta / 120 * 0.1f; // get the mouse wheel direction (1 for up and -1 for down). the more the wheel goes fast, the bigger the value gets.
            if (ShiftPressed) d /= 2; // if Shift is pressed, move slower.
            if (PropertyGrid.SelectedObject != null)
            { //IMPROVE Update all of this : all vector2's X and Y properties don't want to show up anymore
                if (PropertyGrid.SelectedGridItem != null)
                {
                    //if (PropertyGrid.SelectedGridItem.Value.GetType() == typeof(float))
                    if (PropertyGrid.SelectedObject.GetType() == typeof(ObjRectangle))
                    { //Rectangle
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "BodyPosition")
                            if (CtrlPressed) ((ObjRectangle)PropertyGrid.SelectedObject).BodyPosition += new Vector2(0, d);
                            else ((ObjRectangle)PropertyGrid.SelectedObject).BodyPosition             += new Vector2(d, 0);

                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Size")
                            if (CtrlPressed) ((ObjRectangle)PropertyGrid.SelectedObject).Size += new Vector2(0, d);
                            else ((ObjRectangle)PropertyGrid.SelectedObject).Size             += new Vector2(d, 0);
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(ObjCircle))
                    { // Circle
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "BodyPosition")
                            if (CtrlPressed) ((ObjCircle)PropertyGrid.SelectedObject).BodyPosition += new Vector2(0, d);
                            else ((ObjCircle)PropertyGrid.SelectedObject).BodyPosition += new Vector2(d, 0);

                        else if (PropertyGrid.SelectedGridItem.Label.ToString() == "Radius")
                        {
                            float r = MathHelper.Clamp(((ObjCircle)PropertyGrid.SelectedObject).Radius + d, 0.1f, 100f); // protection against negative value
                            ((ObjCircle)PropertyGrid.SelectedObject).Radius = r;
                        }
                    }
                    else if(PropertyGrid.SelectedObject.GetType() == typeof(IAAuxiliary.Radius))
                    {
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "radius")
                        {
                            float r = MathHelper.Clamp(((IAAuxiliary.Radius)PropertyGrid.SelectedObject).radius + d, 0.1f, 100f); // protection against negative value
                            ((IAAuxiliary.Radius)PropertyGrid.SelectedObject).radius = r;
                        }
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(ObjPolygon))
                    { // Polygon
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "BodyPosition")
                        { //change the poly's position
                            if (!CtrlPressed)
                            {
                                List<Vector2> verts = new List<Vector2>();
                                for (int i = 0; i < ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList.Count; i++)
                                {
                                    verts.Add(((ObjPolygon)PropertyGrid.SelectedObject).VerticesList[i] + new Vector2(d, 0));
                                }
                                ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList = verts;
                            }
                            else
                            {
                                List<Vector2> verts = new List<Vector2>();
                                for (int i = 0; i < ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList.Count; i++)
                                {
                                    verts.Add(((ObjPolygon)PropertyGrid.SelectedObject).VerticesList[i] + new Vector2(0, d));
                                }
                                ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList = verts;
                            }
                        }
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(ObjEdge))
                    { // Circle
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Vert1Pos")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((ObjEdge)PropertyGrid.SelectedObject).Vert1Pos += new Vector2(d, 0);
                            else
                                ((ObjEdge)PropertyGrid.SelectedObject).Vert1Pos += new Vector2(0, d);
                        }
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Vert2Pos")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((ObjEdge)PropertyGrid.SelectedObject).Vert2Pos += new Vector2(d, 0);
                            else
                                ((ObjEdge)PropertyGrid.SelectedObject).Vert2Pos += new Vector2(0, d);
                        }
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(EnemyProperties))
                    { // Ennemy
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Position")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((EnemyProperties)PropertyGrid.SelectedObject).Position += new Vector2(d, 0);
                            else
                                ((EnemyProperties)PropertyGrid.SelectedObject).Position += new Vector2(0, d);
                        }
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(PassiveTrapsProperties))
                    { // Ennemy
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Position")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((PassiveTrapsProperties)PropertyGrid.SelectedObject).Position += new Vector2(d, 0);
                            else
                                ((PassiveTrapsProperties)PropertyGrid.SelectedObject).Position += new Vector2(0, d);
                        }
                        else if (PropertyGrid.SelectedGridItem.Label.ToString() == "Angle")
                            ((PassiveTrapsProperties)PropertyGrid.SelectedObject).Angle += d / 2;
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(DecorationProperties))
                    { // Ennemy
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Position")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((DecorationProperties)PropertyGrid.SelectedObject).Position += new Vector2(d, 0);
                            else
                                ((DecorationProperties)PropertyGrid.SelectedObject).Position += new Vector2(0, d);
                        }
                        else if (PropertyGrid.SelectedGridItem.Label.ToString() == "Angle")
                            ((DecorationProperties)PropertyGrid.SelectedObject).Angle += d / 2;
                    }
                    else if (PropertyGrid.SelectedObject.GetType() == typeof(ItemProperties))
                    { // Ennemy
                        if (PropertyGrid.SelectedGridItem.Label.ToString() == "Position")
                        { //change the Rect's position
                            if (!CtrlPressed)
                                ((ItemProperties)PropertyGrid.SelectedObject).Position += new Vector2(d, 0);
                            else
                                ((ItemProperties)PropertyGrid.SelectedObject).Position += new Vector2(0, d);
                        }
                    }
                }
                //TODO add deco, items, 

                PropertyGrid.Refresh();
            }
        }

        private void PropertyGrid_SelectedObjectsChanged(object sender, EventArgs e)
        { // if the new object is a polygon, Edge or edgeChain, activate the EditVertices button.
            if (PropertyGrid.SelectedObject != null)
            {
                string type = PropertyGrid.SelectedObject.GetType().Name.ToString();
                switch (type)
                {
                    case "ObjPolygon":
                    case "ObjEdgeChain":
                        btnEditVertices.Enabled = true;
                        break;
                    case "ActivationZone":
                        if (((IAAuxiliary.ActivationZone)PropertyGrid.SelectedObject).ActivationBody.GetType() == typeof(ObjPolygon))
                            btnEditVertices.Enabled = true; //if we are editing an ennemy's IAAux and if it is a polygon, let us modify the vertices
                        if (((IAAuxiliary.ActivationZone)PropertyGrid.SelectedObject).ActivationBody.GetType() == typeof(ObjEdgeChain))
                            btnEditVertices.Enabled = true; //if we are editing an ennemy's IAAux and if it is a polygon, let us modify the vertices
                        break;
                    default:
                        btnEditVertices.Enabled = false;
                        break;
                }
            }
            else btnEditVertices.Enabled = false;
            if (currentMode == CurrentMode.IAAux) CollisionsModeButton_Click(null, null); // when getting out of the ennemy's IAAux edition mode, go back to normal
        }

        private void btnOpenProperty_Click(object sender, EventArgs e)
        {
            if (PropertyGrid.SelectedGridItem != null)
            {
                if (PropertyGrid.SelectedGridItem.Label.ToString() == "CollisionObject")
                { //if a sector is selected and we doubleClick on the CollisionObject, open it
                    PropertyGrid.SelectedObject = ((Sector)PropertyGrid.SelectedObject).CollisionObject;
                }
                
                //we can open the ennemy's DualWaypoints (IAAux) in order to modify its properties
                if(PropertyGrid.SelectedGridItem.Label == "WP1")
                    PropertyGrid.SelectedObject = ((IAAuxiliary.DualWaypoints)PropertyGrid.SelectedObject).WP1;
                if(PropertyGrid.SelectedGridItem.Label == "WP2")
                    PropertyGrid.SelectedObject = ((IAAuxiliary.DualWaypoints)PropertyGrid.SelectedObject).WP2;

                if (PropertyGrid.SelectedGridItem.Label == "PathObject")
                    PropertyGrid.SelectedObject = ((IAAuxiliary.IAPath)PropertyGrid.SelectedObject).PathObject;

                //else if (PropertyGrid.SelectedGridItem.Label.ToString() == "spawnRect")
                //{ // go to an ennemy/trap's rect properties (useless ?)
                //    if (PropertyGrid.SelectedGridItem.GetType() == typeof(EnemyProperties))
                //        PropertyGrid.SelectedObject = ((EnemyProperties)PropertyGrid.SelectedObject).spawnRect;
                //    if (PropertyGrid.SelectedGridItem.GetType() == typeof(PassiveTrapsProperties))
                //        PropertyGrid.SelectedObject = ((PassiveTrapsProperties)PropertyGrid.SelectedObject).spawnRect;
                //}
                else if (PropertyGrid.SelectedGridItem.Label.ToString() == "Effects")
                { //if a sector is selected and we doubleClick on the CollisionObject, open it
                    showEffectsPicker(((Sector)PropertyGrid.SelectedObject).Effects);
                }
            }
        }

        private void btnParentProperty_Click(object sender, EventArgs e)
        {
            // if a Farseer object is selected and if it has a parent, open it.
            if (PropertyGrid.SelectedObject != null)
            {
                if (Helpers.ClassInheritsFrom(PropertyGrid.SelectedObject.GetType(), typeof(ObjectClass)) // A LISIBILIFIER ?
                && ((ObjectClass)PropertyGrid.SelectedObject).Parent != null)
                {
                    PropertyGrid.SelectedObject = ((ObjectClass)PropertyGrid.SelectedObject).Parent;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        //clean the LevelProperties lists (check if each element is at false)
            rs:
            foreach (ObjRectangle r in LevelProperties.Collisions.RectList)
                if (!r.Alive) { LevelProperties.Collisions.RectList.Remove(r); goto rs; }

            cs:
            foreach (ObjCircle c in LevelProperties.Collisions.CircleList)
                if (!c.Alive) { LevelProperties.Collisions.CircleList.Remove(c); goto cs; }

            ps:
            foreach (ObjPolygon p in LevelProperties.Collisions.PolyList)
                if (!p.Alive) { LevelProperties.Collisions.PolyList.Remove(p); goto ps; }

            ////////////


            eps:
            foreach (EnemyProperties ep in LevelProperties.TravelEnemies)
            {
                if (!ep.spawnRect.Alive)
                {
                    ep.Delete();
                    LevelProperties.TravelEnemies.Remove(ep);
                    goto eps;
                }
            }

            tts:
            foreach (PassiveTrapsProperties tt in LevelProperties.TravelTraps)
            {
                if (!tt.spawnRect.Alive)
                {
                    tt.Delete();
                    LevelProperties.TravelTraps.Remove(tt);
                    goto tts;
                }
            }

            tds:
            foreach (DecorationProperties td in LevelProperties.TravelDecos)
            {
                if (!td.spawnRect.Alive)
                {
                    LevelProperties.TravelDecos.Remove(td);
                    goto tds;
                }
            }



            foreach (ScenarioProperties scenario in LevelProperties.Scenarios)
            {
                zs:
                foreach (ZoneProperties zone in scenario.Zones)
                {
                    foreach (PassiveTrapsProperties trap in zone.PassiveTraps)
                        if (!trap.spawnRect.Alive) { trap.Delete(); zone.PassiveTraps.Remove(trap); goto zs; }
                    foreach (DecorationProperties deco in zone.Decoration)
                        if (!deco.spawnRect.Alive) { zone.Decoration.Remove(deco); goto zs; }
                    foreach (ItemProperties item in zone.Items)
                        if (!item.spawnRect.Alive) { zone.Items.Remove(item); goto zs; }
                }

                sec:
                foreach (Sector s in scenario.Sectors)
                    if (s.CollisionObject != null && !s.CollisionObject.Alive)
                    {
                        scenario.Sectors.Remove(s);
                        if (effectsForm != null && !effectsForm.Closed) effectsForm.Close();
                        goto sec;
                    }

                ess:
                foreach (EnemyProperties ens in scenario.Enemies)
                {
                    if (!ens.spawnRect.Alive)
                    {
                        ens.Delete();
                        scenario.Enemies.Remove(ens);
                        goto ess;
                    }
                }
            }

            DrawScenariosGuidelines();
            saveSucceeded = false;

            // set the curentState label to the current currentState.
            currentStateLabel.Text = "CurrentState : " + currentState.ToString();
            currentModeLabel.Text  = "CurrentMode : "  + currentMode.ToString();
        }
        #endregion

        #region MenuBar
        private void EditSettings_Click(object sender, EventArgs e)
        { //launch the settings window
            showSettingsForm();
        }

        private void FileNew_Click(object sender, EventArgs e)
        { //When we create a new file

            //create a level
            LevelLoader.ClearLevel();
            Editor.reloadImages = true; // reset the images
            //open the settings
            showSettingsForm();
        }

        private void settingsForm_Closed(object sender, EventArgs e)
        { //when we finished using the settings form and closed it, update the concerned elements.
            SliderTime.Maximum = LevelProperties.NYears;
            UpDownScenario.Maximum = LevelProperties.Scenarios.Count - 1;
        }

        void showSettingsForm()
        { // resets and shows the settings form (creates a new window)
            settingsForm = new SettingsForm();
            settingsForm.FormClosed += new FormClosedEventHandler(settingsForm_Closed);
            settingsForm.StartPosition = FormStartPosition.CenterScreen;
            settingsForm.Show();
        }

        private void DictionaryPlayerspawn_Click(object sender, EventArgs e)
        {
            currentState = CurrentState.PlayerSpawn;
        }

        private void DictionaryEnd_Click(object sender, EventArgs e)
        {
            currentState = CurrentState.EndSpawn;
        }

        private void DictionaryGoal_Click(object sender, EventArgs e)
        {
            currentState = CurrentState.GoalSpawn;
        }
        #endregion

        #region XNAMouseHandling

        private void XNAWindow_MouseDown(object sender, MouseEventArgs e)
        { // When we CLICK on the XNA Window      

            Vector2 mousePos = CameraEditor.ScreenToWorld(new Vector2(e.X, e.Y));
            clickPos = mousePos; //BUG bug ?
            currentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);

            switch (e.Button.ToString())
            { // depending on which button we press
                case "Left":
                    //if (currentState == CurrentState.Move)
                    //{
                    //    clickPos.X = e.X;
                    //    clickPos.Y = e.Y;
                    //}
                    if (currentState == CurrentState.Circle)
                    {
                        clickPos = mousePos;
                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                    LevelProperties.Collisions.CircleList.Add(new ObjCircle(ConvertUnits.ToSimUnits(mousePos),
                                        0.1f, 0.0f, TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) });
                                break;
                            case CurrentMode.Sectors:
                                if(currentView == CurrentView.Scenario) {
                                    CurrentScenario.Sectors.Add(new Sector(new ObjCircle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.0f), null));
                                    CurrentScenario.Sectors.Last().CollisionObject.Parent = CurrentScenario.Sectors.Last();
                                }
                                break;
                            case CurrentMode.TCC:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions && TCCToolform.CurrentState.CollisionObject == null)
                                    TCCToolform.CurrentState.CollisionObject = new ObjCircle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.0f, TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                break;
                            case CurrentMode.IAAux:
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, create the circle in its IAAuxNeeded (ActivationPath)
                                    if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone
                                        && ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() != typeof(ObjCircle))
                                    { //if the ennemy needs an ActivationZone and if the old body isn't a rectangle, replace the old one with a rectangle.
                                        ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.Delete();
                                        ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody =
                                            new ObjCircle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.0f) { Color = IAAuxiliary.IAAux.IAAuxColor,
                                                Parent = ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded };
                                    }
                                }
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, create the circle in its IAAuxNeeded (ActivationPath)
                                    if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone
                                        && ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() != typeof(ObjCircle))
                                    { //if the ennemy needs an ActivationZone and if the old body isn't a circle, replace the old one with a circle.
                                        ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.Delete();
                                        ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody =
                                            new ObjCircle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.0f)
                                            {
                                                Color = IAAuxiliary.IAAux.IAAuxColor,
                                                Parent = ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded
                                            };
                                    }
                                }
                                break;
                        }


                    }
                    if (currentState == CurrentState.Rectangle)
                    { 
                        clickPos = mousePos;
                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                    LevelProperties.Collisions.RectList.Add(new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                        0.1f, 0.1f, 0f, TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) });
                                break;
                            case CurrentMode.Sectors:
                                if (currentView == CurrentView.Scenario)
                                {
                                    CurrentScenario.Sectors.Add(new Sector(new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.1f, 0f), null));
                                    CurrentScenario.Sectors.Last().CollisionObject.Parent = CurrentScenario.Sectors.Last(); }
                                break;
                            case CurrentMode.TCC:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions && TCCToolform.CurrentState.CollisionObject == null)
                                    TCCToolform.CurrentState.CollisionObject = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                        0.1f, 0.1f, 0f, TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                break;
                            case CurrentMode.IAAux:
                                if(PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, create the rectangle in its IAAuxNeeded (ActivationPath)
                                    if(((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone
                                        && ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() != typeof(ObjRectangle))
                                    { //if the ennemy needs an ActivationZone and if the old body isn't a rectangle, replace the old one with a rectangle.
                                        ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.Delete();
                                        ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody =
                                            new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.1f, 0f) { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                    }
                                }
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, create the rectangle in its IAAuxNeeded (ActivationPath)
                                    if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone
                                        && ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() != typeof(ObjRectangle))
                                    { //if the ennemy needs an ActivationZone and if the old body isn't a rectangle, replace the old one with a rectangle.
                                        ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.Delete();
                                        ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody =
                                            new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.1f, 0f) { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                    }
                                }
                                break;
                        }

                    }
                    if (currentState == CurrentState.Edge)
                    {
                        clickPos = mousePos;

                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                {
                                    LevelProperties.Collisions.EdgeList.Add(new ObjEdge(ConvertUnits.ToSimUnits(mousePos),
                                    ConvertUnits.ToSimUnits(new Vector2(mousePos.X + 1, mousePos.Y)), TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) });
                                }
                                break;
                            case CurrentMode.Sectors:
                                if (currentView == CurrentView.Scenario)
                                {
                                    CurrentScenario.Sectors.Add(new Sector(new ObjEdge(ConvertUnits.ToSimUnits(mousePos),
                                                                            ConvertUnits.ToSimUnits(new Vector2(mousePos.X + 1, mousePos.Y))), null));
                                    CurrentScenario.Sectors.Last().CollisionObject.Parent = CurrentScenario.Sectors.Last();
                                }
                                else MessageBox.Show("Sectors Only allowed on Scenarios.");
                                break;
                            case CurrentMode.TCC:
                                if (TCCToolform.CurrentState?.CollisionObject == null && TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                    TCCToolform.CurrentState.CollisionObject = new ObjEdge(ConvertUnits.ToSimUnits(mousePos),
                                    ConvertUnits.ToSimUnits(new Vector2(mousePos.X + 1, mousePos.Y)), TreeView.SelectedNode.Text)
                                    { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                break;
                        }

                    }
                    if ((currentState == CurrentState.Spawn && currentDict != CurrentDict.Collisions) || currentState == CurrentState.PlayerSpawn || currentState == CurrentState.EndSpawn || currentState == CurrentState.GoalSpawn)
                    {
                        clickPos.X = mousePos.X;
                        clickPos.Y = mousePos.Y;
                        string userData = "";
                        Color color = new Color(255, 0, 255);
                        switch (currentState)
                        {
                            case CurrentState.Spawn:
                                if (TreeView.SelectedNode != null && TreeView.SelectedNode.Level != 0)
                                {
                                    if (currentDict == CurrentDict.Ennemies)  color = Color.Red;
                                    if (currentDict == CurrentDict.Traps)     color = Color.Orange;
                                    if (currentDict == CurrentDict.Items)     color = Color.GreenYellow;
                                    if (currentDict == CurrentDict.Decoration) color = Color.AliceBlue;
                                    userData = TreeView.SelectedNode.Text; 
                                }
                                else goto skip; // if no Node is selected, skip everything. (avoid crashes as we don't know what to spawn)
                                break;
                            case CurrentState.PlayerSpawn:
                                if (LevelProperties.Spawns.playerSpawn == null)
                                {
                                    userData = "Player";
                                    color = Color.GreenYellow;
                                    LevelProperties.Spawns.playerSpawn = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.5f, 0.5f, 0f) { Color = color, UserData = userData };
                                }
                                else LevelProperties.Spawns.playerSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                goto skip;
                            case CurrentState.EndSpawn:
                                if (LevelProperties.Spawns.EndSpawn == null)
                                {
                                    userData = "End";
                                    color = Color.GreenYellow;
                                    LevelProperties.Spawns.EndSpawn = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.5f, 0.5f, 0f) { Color = color, UserData = userData };
                                }
                                else LevelProperties.Spawns.EndSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                goto skip;
                            case CurrentState.GoalSpawn:
                                if (LevelProperties.Spawns.GoalSpawn == null)
                                {
                                    userData = "Goal";
                                    color = Color.GreenYellow;
                                    LevelProperties.Spawns.GoalSpawn = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos), 0.5f, 0.5f, 0f) { Color = color, UserData = userData };
                                }
                                else LevelProperties.Spawns.GoalSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                goto skip;
                        }


                        switch (currentDict)
                        {
                            case CurrentDict.Ennemies:
                                ObjRectangle rectEnemy = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                ConvertUnits.ToSimUnits(EnnemiesDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                ConvertUnits.ToSimUnits(EnnemiesDict.images[TreeView.SelectedNode.ImageIndex].Height), 0f);

                                if (currentView == CurrentView.Scenario)
                                {
                                    CurrentScenario.Enemies.Add(new EnemyProperties(userData, ConvertUnits.ToSimUnits(mousePos), SliderTime.Value, rectEnemy)
                                    {
                                        thumbnail = Helpers.ImageToTexture2D(EnnemiesDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                    });
                                    CurrentScenario.Enemies.Last().spawnRect.Color = color;
                                }
                                if(currentView == CurrentView.Precision)
                                {
                                    LevelProperties.TravelEnemies.Add(new EnemyProperties(userData, ConvertUnits.ToSimUnits(mousePos), SliderTime.Value, rectEnemy)
                                    {
                                        thumbnail = Helpers.ImageToTexture2D(EnnemiesDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                    });
                                    LevelProperties.TravelEnemies.Last().spawnRect.Color = color;
                                }

                                spawning = true;
                                break;
                            case CurrentDict.Items:
                                if (currentView == CurrentView.Scenario)
                                {
                                    if (CurrentScenario.Zones.Count > currentZone)
                                    {
                                        CurrentScenario.Zones[currentZone].Items.Add(new ItemProperties(TreeView.SelectedNode.Text, ConvertUnits.ToSimUnits(new Vector2(mousePos.X, mousePos.Y)),
                                            ConvertUnits.ToSimUnits(ItemsDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                            ConvertUnits.ToSimUnits(ItemsDict.images[TreeView.SelectedNode.ImageIndex].Height), Color.GreenYellow));

                                        if (TreeView.SelectedNode.Text == "Checkpoint") //if we are putting a checkpoint, put the parent for when we will want to view it's properties on the propertygrid.
                                            CurrentScenario.Zones[currentZone].Items.Last().spawnRect.Parent = CurrentScenario.Zones[currentZone].Items.Last();
                                        CurrentScenario.Zones[currentZone].Items.Last().thumbnail = Helpers.ImageToTexture2D(ItemsDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice);
                                        spawning = true;
                                    }
                                }
                                break;
                            case CurrentDict.Traps:
                                if (currentView == CurrentView.Scenario)
                                {
                                    if (CurrentScenario.Zones.Count > currentZone)
                                    {
                                        ObjRectangle rectTrap = new ObjRectangle(ConvertUnits.ToSimUnits(new Vector2(mousePos.X, mousePos.Y)),
                                            ConvertUnits.ToSimUnits(TrapsDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                            ConvertUnits.ToSimUnits(TrapsDict.images[TreeView.SelectedNode.ImageIndex].Height), 0);

                                        CurrentScenario.Zones[currentZone].PassiveTraps.Add(new PassiveTrapsProperties(userData, ConvertUnits.ToSimUnits(mousePos), SliderTime.Value, rectTrap)
                                        {
                                            thumbnail = Helpers.ImageToTexture2D(TrapsDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                        });
                                        CurrentScenario.Zones[currentZone].PassiveTraps.Last().spawnRect.Color = color;
                                        spawning = true;
                                    }
                                }
                                else if(currentView == CurrentView.Precision)
                                {
                                    ObjRectangle rectTrap = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                            ConvertUnits.ToSimUnits(TrapsDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                            ConvertUnits.ToSimUnits(TrapsDict.images[TreeView.SelectedNode.ImageIndex].Height), 0);

                                    LevelProperties.TravelTraps.Add(new PassiveTrapsProperties(userData, ConvertUnits.ToSimUnits(mousePos), SliderTime.Value, rectTrap)
                                    {
                                        thumbnail = Helpers.ImageToTexture2D(TrapsDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                    });
                                    LevelProperties.TravelTraps.Last().spawnRect.Color = color;

                                    spawning = true;
                                }
                                break;
                            case CurrentDict.Decoration:
                                if (currentView == CurrentView.Scenario)
                                {
                                    if (currentZone < CurrentScenario.Zones.Count)
                                    {
                                        ObjRectangle rectDeco = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                            ConvertUnits.ToSimUnits(DecoDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                            ConvertUnits.ToSimUnits(DecoDict.images[TreeView.SelectedNode.ImageIndex].Height), 0);

                                        CurrentScenario.Zones[currentZone].Decoration.Add(new DecorationProperties(userData, ConvertUnits.ToSimUnits(mousePos), rectDeco, SliderTime.Value)
                                        {
                                            thumbnail = Helpers.ImageToTexture2D(DecoDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                        });
                                        CurrentScenario.Zones[currentZone].Decoration.Last().spawnRect.Color = color;
                                        spawning = true;
                                    }
                                }
                                else if (currentView == CurrentView.Precision)
                                {
                                    ObjRectangle rectDeco = new ObjRectangle(ConvertUnits.ToSimUnits(mousePos),
                                            ConvertUnits.ToSimUnits(DecoDict.images[TreeView.SelectedNode.ImageIndex].Width),
                                            ConvertUnits.ToSimUnits(DecoDict.images[TreeView.SelectedNode.ImageIndex].Height), 0);

                                    LevelProperties.TravelDecos.Add(new DecorationProperties(userData, ConvertUnits.ToSimUnits(mousePos), rectDeco, SliderTime.Value)
                                    {
                                        thumbnail = Helpers.ImageToTexture2D(DecoDict.images[TreeView.SelectedNode.ImageIndex], Game1.graphicsDevice)
                                    });
                                    LevelProperties.TravelDecos.Last().spawnRect.Color = color;

                                    spawning = true;
                                }
                                break;
                        }
                    }
                skip: //goto for the previous if (get out of the if to avoid an error)
                    if (currentState == CurrentState.None)
                    {
                        if (mouseFarseer == null)
                        {
                            clickPos = mousePos;
                            mouseFarseer = BodyFactory.CreateCircle(EditorVariables.world, 0.1f, 10f);
                            mouseFarseer.Position = ConvertUnits.ToSimUnits(mousePos);
                            mouseFarseer.BodyType = BodyType.Dynamic;
                            mouseFarseer.IsSensor = true;
                        }
                    }
                    if (currentState == CurrentState.EditVertices)
                    {
                        if (mouseFarseer == null)
                        { // create a mouse
                            clickPos = mousePos;
                            mouseFarseer = BodyFactory.CreateCircle(EditorVariables.world, 0.1f, 10f);
                            mouseFarseer.Position = ConvertUnits.ToSimUnits(mousePos);
                            mouseFarseer.BodyType = BodyType.Dynamic;
                            mouseFarseer.IsSensor = true;
                        }
                    }
                    break;
                case "Right":
                    if (currentState == CurrentState.None)
                    {
                        if (mouseFarseer == null)
                        {
                            clickPos = mousePos;
                            mouseFarseer = BodyFactory.CreateCircle(EditorVariables.world, 0.1f, 10f);
                            mouseFarseer.Position = ConvertUnits.ToSimUnits(mousePos);
                            mouseFarseer.BodyType = BodyType.Dynamic;
                            mouseFarseer.IsSensor = true;
                        }
                        //clickPos = mousePos;

                        //mouseFarseer = BodyFactory.CreateCircle(EditorVariables.world, 0.1f, 10f);
                        //mouseFarseer.Position = ConvertUnits.ToSimUnits(new Vector2(mousePos.X, mousePos.Y));
                        //mouseFarseer.BodyType = BodyType.Dynamic;
                        //mouseFarseer.IsSensor = true;
                    }
                    break;
                case "Middle":
                    break;
            }
        }

        private void XNAWindow_MouseMove(object sender, MouseEventArgs e)
        { // When the mouse moves over the XNA Window (clicking or not)
            Vector2 mousePos = CameraEditor.ScreenToWorld(new Vector2(e.X, e.Y));

            PosLabel.Text = mousePos.ToString();

            if (currentState == CurrentState.Circle && e.Button.Equals(MouseButtons.Left))
            { // creating a circle
                Vector2 dif = new Vector2(Math.Abs(mousePos.X - clickPos.X), Math.Abs(mousePos.Y - clickPos.Y));
                float radius = dif.Length() == 0.0f ? 0.1f : dif.Length();
                switch (currentMode)
                {
                    case CurrentMode.Collisions:
                        if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                        {
                            LevelProperties.Collisions.CircleList.Last().Radius = ConvertUnits.ToSimUnits(radius);
                            LevelProperties.Collisions.CircleList.Last().BodyPosition = ConvertUnits.ToSimUnits(clickPos);
                        }
                        break;
                    case CurrentMode.Sectors:
                        if (currentView == CurrentView.Scenario)
                        {
                            ((ObjCircle)CurrentScenario?.Sectors.Last().CollisionObject).Radius = ConvertUnits.ToSimUnits(radius);
                            ((ObjCircle)CurrentScenario?.Sectors.Last().CollisionObject).BodyPosition = ConvertUnits.ToSimUnits(clickPos);
                        }
                        else MessageBox.Show("Sectors Only allowed on Scenarios.");
                        break;
                    case CurrentMode.TCC:
                        if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions
                            && TCCToolform.CurrentState?.CollisionObject.objectType == ObjectClass.ObjectType.Circle)
                        {
                            ((ObjCircle)TCCToolform.CurrentState.CollisionObject).Radius = ConvertUnits.ToSimUnits(radius);
                            ((ObjCircle)TCCToolform.CurrentState.CollisionObject).BodyPosition = ConvertUnits.ToSimUnits(clickPos);
                        }
                        break;
                    case CurrentMode.IAAux:
                        if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                        { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) circle
                            if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone
                                && ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() == typeof(ObjCircle))
                            {//if the ennemy needs an ActivationZone and if the current Object is a circle (because if it's not, the right piece of code needs to move the object)
                                ((ObjCircle)((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).Radius
                                    = ConvertUnits.ToSimUnits(radius);
                                ((ObjCircle)((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).BodyPosition
                                    = ConvertUnits.ToSimUnits(clickPos);
                            }
                        }
                        if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                        { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) circle
                            if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone
                                && ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() == typeof(ObjCircle))
                            {//if the ennemy needs an ActivationZone and if the current Object is a circle (because if it's not, the right piece of code needs to move the object)
                                ((ObjCircle)((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).Radius
                                    = ConvertUnits.ToSimUnits(radius);
                                ((ObjCircle)((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).BodyPosition
                                    = ConvertUnits.ToSimUnits(clickPos);
                            }
                        }
                        break;
                }
            }
            if (currentState == CurrentState.Rectangle && e.Button.Equals(MouseButtons.Left))
            {

                Vector2 dif = mousePos - clickPos;

                switch (currentMode)
                {
                    case CurrentMode.Collisions: //replace the last rectangle with a new one containing the new size
                        if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                        {
                            LevelProperties.Collisions.RectList.Last().Size = ConvertUnits.ToSimUnits(new Vector2(dif.X == 0.0f ? 0.1f : Math.Abs(dif.X),
                                                                                                        dif.Y == 0.0f ? 0.1f : Math.Abs(dif.Y)));
                            LevelProperties.Collisions.RectList.Last().BodyPosition = ConvertUnits.ToSimUnits(clickPos + dif / 2);
                        }
                        break;
                    case CurrentMode.Sectors:
                        if (currentView == CurrentView.Scenario && CurrentScenario?.Sectors.Last().CollisionObject.objectType == ObjectClass.ObjectType.Rectangle)
                        {
                            CurrentScenario.Sectors.Last().CollisionObject.BodyPosition = ConvertUnits.ToSimUnits(clickPos + dif / 2);
                            ((ObjRectangle)CurrentScenario.Sectors.Last().CollisionObject).Size 
                                = ConvertUnits.ToSimUnits(new Vector2(dif.X == 0.0f ? 0.1f : Math.Abs(dif.X), dif.Y == 0.0f ? 0.1f : Math.Abs(dif.Y)));
                        }
                        else MessageBox.Show("Sectors Only allowed on Scenarios.");
                        break;
                    case CurrentMode.TCC:
                        if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions
                            && TCCToolform?.CurrentState?.CollisionObject.objectType == ObjectClass.ObjectType.Rectangle)
                        {
                            ((ObjRectangle)TCCToolform.CurrentState.CollisionObject).Size = new Vector2(ConvertUnits.ToSimUnits(dif.X == 0.0f ? 0.1f : Math.Abs(dif.X)),
                            ConvertUnits.ToSimUnits(dif.Y == 0.0f ? 0.1f : Math.Abs(dif.Y)));
                            ((ObjRectangle)TCCToolform.CurrentState.CollisionObject).BodyPosition = ConvertUnits.ToSimUnits(clickPos + dif / 2);
                        }
                        break;
                    case CurrentMode.IAAux:
                        if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                        { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                            if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone
                                && ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() == typeof(ObjRectangle))
                            {//if the ennemy needs an ActivationZone and if the current Object is a rectangle (because if it's not, the right piece of code needs to move the object)
                                ((ObjRectangle)((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).Size 
                                    = new Vector2(ConvertUnits.ToSimUnits(dif.X == 0.0f ? 0.1f : Math.Abs(dif.X)),
                                                  ConvertUnits.ToSimUnits(dif.Y == 0.0f ? 0.1f : Math.Abs(dif.Y)));
                                ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.BodyPosition 
                                    = ConvertUnits.ToSimUnits(clickPos + dif / 2);
                            }
                        }
                        if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                        { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                            if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone
                                && ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.GetType() == typeof(ObjRectangle))
                            {//if the ennemy needs an ActivationZone and if the current Object is a rectangle (because if it's not, the right piece of code needs to move the object)
                                ((ObjRectangle)((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody).Size
                                    = new Vector2(ConvertUnits.ToSimUnits(dif.X == 0.0f ? 0.1f : Math.Abs(dif.X)),
                                                  ConvertUnits.ToSimUnits(dif.Y == 0.0f ? 0.1f : Math.Abs(dif.Y)));
                                ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody.BodyPosition
                                    = ConvertUnits.ToSimUnits(clickPos + dif / 2);
                            }
                        }
                        break;
                }

            }
            if (currentState == CurrentState.Edge && e.Button.Equals(MouseButtons.Left))
            {
                switch (currentMode)
                {
                    case CurrentMode.Collisions:
                        if(TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                            LevelProperties.Collisions.EdgeList.Last().Vert2Pos = ConvertUnits.ToSimUnits(mousePos);
                        break;
                    case CurrentMode.Sectors:
                        if(currentView == CurrentView.Scenario)
                        {
                            if (CurrentScenario?.Sectors.Last().CollisionObject.objectType == ObjectClass.ObjectType.Edge)
                            {
                                ((ObjEdge)CurrentScenario.Sectors.Last().CollisionObject).Vert2Pos = ConvertUnits.ToSimUnits(mousePos);
                            }
                        }
                        else MessageBox.Show("Sectors Only allowed on Scenarios.");
                        break;
                    case CurrentMode.TCC:
                        if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions
                            && TCCToolform.CurrentState?.CollisionObject.objectType == ObjectClass.ObjectType.Edge 
                            )
                        {
                            ((ObjEdge)TCCToolform.CurrentState.CollisionObject).Vert2Pos = ConvertUnits.ToSimUnits(mousePos);
                        }
                        break;
                }

            }

            #region Move
            if (currentState == CurrentState.None && e.Button.Equals(MouseButtons.Left))
            {
                /*if (mouseObject.objCollidesWith != null)
                        {
                            
                            mouseObject.objCollidesWith.body.Position += dif;// new Vector2(mousePos.X, mousePos.Y);
                            mouseObject.objCollidesWith.Position += dif;// new Vector2(mousePos.X, mousePos.Y);
                            clickPos.X = mousePos.X;
                            clickPos.Y = mousePos.Y;
                        }*/

                Vector2 dif = mousePos - clickPos;
                clickPos = mousePos;
                if (mouseFarseer != null)
                {
                    var c = mouseFarseer.ContactList;
                    while (c != null)
                    {
                        if (c.Contact.IsTouching)
                        {
                            object uf;
                            if (c.Contact.FixtureB.Body.Equals(mouseFarseer))
                                uf = c.Contact.FixtureA.Body.UserData;
                            else uf = c.Contact.FixtureB.Body.UserData;
                            if (!Helpers.ClassInheritsFrom(uf.GetType(), typeof(ObjectClass))) goto cancelCollision; //if we're not dealing with a regular object (e.g. WorldOrigin)

                            if (((ObjectClass)uf).UserData == "IAAux" && Helpers.ClassInheritsFrom(((ObjectClass)uf).Parent?.GetType(), typeof(IAAuxiliary.IAAux))
                                && !((IAAuxiliary.IAAux)((ObjectClass)uf).Parent).CanBeMoved)
                                    goto cancelCollision; //if we are trying to move an ennemy's IAAux that we can't move, cancel it

                            if (((ObjectClass)uf)?.Parent?.GetType() == typeof(EnemyProperties))
                            {// if we move an ennemy (ennemy's rectangle)
                                ((EnemyProperties)((ObjectClass)uf).Parent).Position += ConvertUnits.ToSimUnits(dif); //General ennemy position
                                goto cancelCollision;
                            }
                            //else if (((ObjectClass)uf)?.Parent?.GetType() == typeof(PassiveTrapsProperties))
                            //{// if we move an ennemy (ennemy's rectangle)
                            //    ((PassiveTrapsProperties)((ObjectClass)uf).Parent).Position += ConvertUnits.ToSimUnits(dif); //General ennemy position
                            //    goto cancelCollision;
                            //}
                            else if (((ObjectClass)uf)?.Parent?.GetType() == typeof(PassiveTrapsProperties))
                            {// if we move a trap (trap's rectangle)
                                int newCurrentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);
                                //int dif = newCurrentZone - currentZone;
                                
                                if (currentView == CurrentView.Scenario)
                                {
                                    if (newCurrentZone != currentZone && newCurrentZone < CurrentScenario.Zones.Count)
                                    { //if we went to another zone
                                        PassiveTrapsProperties backup = (PassiveTrapsProperties)((ObjectClass)uf).Parent;
                                        CurrentScenario.Zones[currentZone].PassiveTraps.Remove((PassiveTrapsProperties)((ObjectClass)uf).Parent); //remove the trap in the old zone
                                        CurrentScenario.Zones[newCurrentZone].PassiveTraps.Add(backup); //add the new one

                                        CurrentScenario.Zones[newCurrentZone].PassiveTraps.Last().Position += ConvertUnits.ToSimUnits(dif);
                                        currentZone = newCurrentZone;
                                    }
                                    else if (newCurrentZone == currentZone && newCurrentZone < CurrentScenario.Zones.Count)//if nothing changed (we're still in the same zone)
                                    {
                                        ((PassiveTrapsProperties)((ObjectClass)uf).Parent).Position += ConvertUnits.ToSimUnits(dif);
                                        currentZone = newCurrentZone;
                                    }
                                    else CurrentScenario.Zones.Last().PassiveTraps.Last().Position += ConvertUnits.ToSimUnits(dif);
                                    //((PassiveTrapsProperties)((ObjectClass)uf).Parent).Position += ConvertUnits.ToSimUnits(dif); //General trap position
                                    goto cancelCollision;
                                }
                                else if(currentView == CurrentView.Precision)
                                {
                                    ((PassiveTrapsProperties)((ObjectClass)uf).Parent).Position += ConvertUnits.ToSimUnits(dif); //General trap position
                                    goto cancelCollision;
                                }
                            }
                            else if (uf != null && ((ObjectClass)uf).Parent != null && ((ObjectClass)uf).Parent.GetType() == typeof(DecorationProperties))
                            {// if we move a decoration (deco's rectangle)
                                int newCurrentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);
                                
                                
                                if (currentView == CurrentView.Scenario)
                                {
                                    if (newCurrentZone != currentZone && newCurrentZone < CurrentScenario.Zones.Count)
                                    { //if we went to another zone
                                        DecorationProperties backup = CurrentScenario.Zones[currentZone].Decoration.Last();
                                        CurrentScenario.Zones[currentZone].Decoration.Remove(CurrentScenario.Zones[currentZone].Decoration.Last()); //remove the deco in the old zone
                                        CurrentScenario.Zones[newCurrentZone].Decoration.Add(backup); //add the new one

                                        CurrentScenario.Zones[newCurrentZone].Decoration.Last().Position += ConvertUnits.ToSimUnits(dif);
                                        currentZone = newCurrentZone;
                                    }
                                    else if (newCurrentZone == currentZone && newCurrentZone < CurrentScenario.Zones.Count)//if nothing changed (we're still in the same zone)
                                    {
                                        CurrentScenario.Zones[currentZone].Decoration.Last().Position += ConvertUnits.ToSimUnits(dif);
                                        currentZone = newCurrentZone;
                                    }
                                    else CurrentScenario.Zones.Last().Decoration.Last().Position += ConvertUnits.ToSimUnits(dif);
                                    
                                    goto cancelCollision;
                                }
                            }
                            if (((ObjectClass)uf).Parent?.GetType() == typeof(ObjEdgeChain))
                            { // if we are trying to move an EdgeChain
                                List<Vector2> backupVerts = ((ObjEdgeChain)((ObjEdge)uf).Parent).VerticesList;
                                List<Vector2> newVerts = new List<Vector2>();

                                for (int i = 0; i < ((ObjEdgeChain)((ObjEdge)uf).Parent).VerticesList.Count; i++)
                                    newVerts.Add(backupVerts[i] + ConvertUnits.ToSimUnits(dif));
                                ((ObjEdgeChain)((ObjectClass)uf).Parent).VerticesList = newVerts;
                                if (mouseFarseer != null) mouseFarseer.SleepingAllowed = false;
                            }
                            else if (uf.GetType() == typeof(ObjEdge))
                            { // if we are trying to move an EdgeChain
                                ((ObjEdge)uf).Vert1Pos += ConvertUnits.ToSimUnits(dif);
                                ((ObjEdge)uf).Vert2Pos += ConvertUnits.ToSimUnits(dif);
                                if (mouseFarseer != null) mouseFarseer.SleepingAllowed = false;
                            }
                            else if (uf.GetType() == typeof(ObjPolygon))
                            { // if we are trying to move a polygon
                                List<Vector2> verts = new List<Vector2>();
                                for (int i = 0; i < ((ObjPolygon)uf).VerticesList.Count; i++)
                                {
                                    verts.Add(((ObjPolygon)uf).VerticesList[i] + ConvertUnits.ToSimUnits(dif));
                                }
                            ((ObjPolygon)uf).VerticesList = verts;
                                if (mouseFarseer != null) mouseFarseer.SleepingAllowed = false;
                            }
                            else // moving anything else
                                ((ObjectClass)uf).BodyPosition += ConvertUnits.ToSimUnits(dif);
                        }
                        cancelCollision:
                        c = c.Next;
                    }
                    mouseFarseer.SetTransform(ConvertUnits.ToSimUnits(mousePos), 0f);
                }
            }
            #endregion

            if ((currentState == CurrentState.Spawn && currentDict != CurrentDict.Collisions || currentState == CurrentState.PlayerSpawn || currentState == CurrentState.EndSpawn
                  || currentState == CurrentState.GoalSpawn) && e.Button.Equals(MouseButtons.Left))
            {
                switch (currentState)
                {
                    case CurrentState.PlayerSpawn:
                        LevelProperties.Spawns.playerSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                        goto skip;
                    case CurrentState.EndSpawn:
                        LevelProperties.Spawns.EndSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                        goto skip;
                    case CurrentState.GoalSpawn:
                        LevelProperties.Spawns.GoalSpawn.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                        goto skip;
                }
                switch (currentDict)
                {
                    case CurrentDict.Ennemies:
                        if (spawning)//protection
                        {
                            EnemyProperties LastEnnemy;
                            if (currentView == CurrentView.Precision)
                                LastEnnemy = LevelProperties.TravelEnemies.Last();
                            else LastEnnemy = CurrentScenario.Enemies.Last();

                            LastEnnemy.Position = ConvertUnits.ToSimUnits(mousePos);

                            //when spawning an ennemy that needs a DualWaypoint, move those waypoints with the ennemy when spawning.
                            if (LastEnnemy.IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.DualWaypoints)
                            {
                                LastEnnemy.IAAuxNeeded.Delete();
                                LastEnnemy.IAAuxNeeded =
                                        new IAAuxiliary.DualWaypoints(
                                            new PreciseRectangle(LastEnnemy.spawnRect.BodyPosition.X - LastEnnemy.spawnRect.Bounds.Width / 2,
                                                                 LastEnnemy.spawnRect.BodyPosition.Y - LastEnnemy.spawnRect.Bounds.Height / 2, 1, 1),
                                            new PreciseRectangle(LastEnnemy.spawnRect.BodyPosition.X + LastEnnemy.spawnRect.Bounds.Width / 2,
                                                                 LastEnnemy.spawnRect.BodyPosition.Y - LastEnnemy.spawnRect.Bounds.Height / 2, 1, 1));
                            }

                            //when spawning an ennemy that needs an ActivationZone, move the default rect with the ennemy when spawning.
                            if (LastEnnemy.IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                ((IAAuxiliary.ActivationZone)LastEnnemy.IAAuxNeeded).ActivationBody.BodyPosition = ConvertUnits.ToSimUnits(mousePos);

                            if (LastEnnemy.IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.Path)
                            {
                                ((IAAuxiliary.IAPath)LastEnnemy.IAAuxNeeded).PathObject.Delete();
                                LastEnnemy.IAAuxNeeded = 
                                    new IAAuxiliary.IAPath(new Vector2(LastEnnemy.spawnRect.BodyPosition.X - (LastEnnemy.spawnRect.Bounds.Width / 2) - 1, 
                                                                       LastEnnemy.spawnRect.BodyPosition.Y - (LastEnnemy.spawnRect.Bounds.Height / 2) - 1),
                                                           new Vector2(LastEnnemy.spawnRect.BodyPosition.X + (LastEnnemy.spawnRect.Bounds.Width / 2) + 1,
                                                                       LastEnnemy.spawnRect.BodyPosition.Y + (LastEnnemy.spawnRect.Bounds.Height / 2) + 1));
                            }
                        }
                        break;
                    case CurrentDict.Items:
                        if (spawning)//protection
                        {
                            if (currentView == CurrentView.Scenario)
                            {
                                int newCurrentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);
                                int dif = newCurrentZone - currentZone;
                                if (dif != 0 && CurrentScenario.Zones.Count > newCurrentZone)
                                { //if we went to the right
                                    ItemProperties backup = CurrentScenario.Zones[currentZone].Items.Last();
                                    CurrentScenario.Zones[currentZone].Items.Remove(CurrentScenario.Zones[currentZone].Items.Last()); //remove the trap in the old zone
                                    CurrentScenario.Zones[newCurrentZone].Items.Add(backup); //add the new one

                                    CurrentScenario.Zones[newCurrentZone].Items.Last().spawnRect.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                }
                                else //if nothing changed (we're still in the same zone)
                                {
                                    if (CurrentScenario.Zones.Count > newCurrentZone)
                                        CurrentScenario.Zones[currentZone].Items.Last().spawnRect.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                }
                                currentZone = newCurrentZone;
                            }
                        }
                        break;
                    case CurrentDict.Traps:
                        if (spawning)//protection
                        {
                            if (currentView == CurrentView.Scenario)
                            {
                                int newCurrentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);
                                //int dif = newCurrentZone - currentZone;
                                if (newCurrentZone != currentZone && newCurrentZone < CurrentScenario.Zones.Count)
                                { //if we went to another zone
                                    PassiveTrapsProperties backup = CurrentScenario.Zones[currentZone].PassiveTraps.Last();
                                    CurrentScenario.Zones[currentZone].PassiveTraps.Remove(CurrentScenario.Zones[currentZone].PassiveTraps.Last()); //remove the trap in the old zone
                                    CurrentScenario.Zones[newCurrentZone].PassiveTraps.Add(backup); //add the new one

                                    CurrentScenario.Zones[newCurrentZone].PassiveTraps.Last().Position = ConvertUnits.ToSimUnits(mousePos);

                                    //when spawning an ennemy that needs an ActivationZone, move the default rect with the ennemy when spawning.
                                    if (CurrentScenario.Zones[newCurrentZone].PassiveTraps.Last().IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                        ((IAAuxiliary.ActivationZone)CurrentScenario.Zones[newCurrentZone].PassiveTraps.Last().IAAuxNeeded).ActivationBody.BodyPosition
                                            = ConvertUnits.ToSimUnits(mousePos);

                                    currentZone = newCurrentZone;
                                }
                                else if (newCurrentZone == currentZone && newCurrentZone < CurrentScenario.Zones.Count)//if nothing changed (we're still in the same zone)
                                {
                                    CurrentScenario.Zones[currentZone].PassiveTraps.Last().Position = ConvertUnits.ToSimUnits(mousePos);

                                    //when spawning an ennemy that needs an ActivationZone, move the default rect with the ennemy when spawning.
                                    if (CurrentScenario.Zones[currentZone].PassiveTraps.Last().IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                        ((IAAuxiliary.ActivationZone)CurrentScenario.Zones[currentZone].PassiveTraps.Last().IAAuxNeeded).ActivationBody.BodyPosition = ConvertUnits.ToSimUnits(mousePos);

                                    currentZone = newCurrentZone;
                                }
                                else CurrentScenario.Zones.Last().PassiveTraps.Last().Position = ConvertUnits.ToSimUnits(mousePos);
                            }
                            else if (currentView == CurrentView.Precision)
                            {
                                LevelProperties.TravelTraps.Last().Position = ConvertUnits.ToSimUnits(mousePos);

                                //when spawning an ennemy that needs an ActivationZone, move the default rect with the ennemy when spawning.
                                if (LevelProperties.TravelTraps.Last().IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                    ((IAAuxiliary.ActivationZone)LevelProperties.TravelTraps.Last().IAAuxNeeded).ActivationBody.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                            }
                        }
                        break;
                    case CurrentDict.Decoration:
                        if (spawning)//protection
                        {
                            if (currentView == CurrentView.Scenario)
                            {
                                int newCurrentZone = (int)(mousePos.X / LevelProperties.zoneDimensions.X);
                                //int dif = newCurrentZone - currentZone;
                                if (newCurrentZone != currentZone && newCurrentZone < CurrentScenario.Zones.Count)
                                { //if we went to another zone
                                    DecorationProperties backup = CurrentScenario.Zones[currentZone].Decoration.Last();
                                    CurrentScenario.Zones[currentZone].Decoration.Remove(CurrentScenario.Zones[currentZone].Decoration.Last()); //remove the trap in the old zone
                                    CurrentScenario.Zones[newCurrentZone].Decoration.Add(backup); //add the new one

                                    CurrentScenario.Zones[newCurrentZone].Decoration.Last().spawnRect.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                    currentZone = newCurrentZone;
                                }
                                else if (newCurrentZone == currentZone && newCurrentZone < CurrentScenario.Zones.Count)//if nothing changed (we're still in the same zone)
                                {
                                    CurrentScenario.Zones[currentZone].Decoration.Last().spawnRect.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                                    currentZone = newCurrentZone;
                                }
                                else CurrentScenario.Zones.Last().Decoration.Last().spawnRect.BodyPosition = ConvertUnits.ToSimUnits(mousePos);
                            }
                            else if (currentView == CurrentView.Precision)
                            {
                                LevelProperties.TravelDecos.Last().Position = ConvertUnits.ToSimUnits(mousePos);
                            }
                        }
                        break;
                }

            }
        skip:
            if (currentState == CurrentState.EditVertices && e.Button.Equals(MouseButtons.Left))
            {
                if (mouseFarseer != null)
                {
                    Vector2 dif = mousePos - clickPos;
                    clickPos = mousePos;

                    var c = mouseFarseer.ContactList;
                    while (c != null)
                    {
                        if (c.Contact.IsTouching)
                        {
                            object uf;
                            if (c.Contact.FixtureB.Body.Equals(mouseFarseer))
                                uf = c.Contact.FixtureA.Body.UserData;
                            else uf = c.Contact.FixtureB.Body.UserData;

                            if (PropertyGrid.SelectedObject.GetType() == typeof(ObjPolygon))
                            {
                                if (uf != null && uf.GetType() == typeof(ObjCircle))
                                {
                                    for (int i = 0; i < ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList.Count; i++)
                                    { // search in which position is the currently touched circle of the VertList
                                        if (((ObjectClass)uf).BodyPosition == ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList[i])
                                        {
                                            int pos = i;
                                            List<Vector2> verts = new List<Vector2>();
                                            for (int j = 0; j < ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList.Count; j++)
                                                verts.Add(((ObjPolygon)PropertyGrid.SelectedObject).VerticesList[j]);
                                            verts[pos] += ConvertUnits.ToSimUnits(dif);
                                            ((ObjPolygon)PropertyGrid.SelectedObject).VerticesList = verts;

                                            ((ObjCircle)uf).BodyPosition += ConvertUnits.ToSimUnits(dif);
                                        }
                                    }
                                }
                            }
                            if (PropertyGrid.SelectedObject.GetType() == typeof(ObjEdgeChain))
                            {
                                if (uf != null && uf.GetType() == typeof(ObjCircle))
                                {
                                    for (int i = 0; i < ((ObjEdgeChain)PropertyGrid.SelectedObject).VerticesList.Count; i++)
                                    { // search in which position is the currently touched circle of the VertList
                                        if (((ObjectClass)uf).BodyPosition == ((ObjEdgeChain)PropertyGrid.SelectedObject).VerticesList[i])
                                        {
                                            int pos = i;
                                            List<Vector2> verts = new List<Vector2>();
                                            for (int j = 0; j < ((ObjEdgeChain)PropertyGrid.SelectedObject).VerticesList.Count; j++)
                                                verts.Add(((ObjEdgeChain)PropertyGrid.SelectedObject).VerticesList[j]);
                                            verts[pos] += ConvertUnits.ToSimUnits(dif);
                                            ((ObjEdgeChain)PropertyGrid.SelectedObject).VerticesList = verts;

                                            ((ObjCircle)uf).BodyPosition += ConvertUnits.ToSimUnits(dif);
                                        }
                                    }
                                }
                            }
                        }
                        c = c.Next;
                    }
                    mouseFarseer.SetTransform(ConvertUnits.ToSimUnits(mousePos), 0f);
                }
            }

            if(currentState == CurrentState.None && e.Button.Equals(MouseButtons.Right))
            {
                if(mouseFarseer != null)
                {
                    mouseFarseer.SetTransform(ConvertUnits.ToSimUnits(mousePos), 0f);
                    mouseFarseer.SleepingAllowed = false;
                }
            }


            //when moving around with the right button, move the canvas
            if(e.Button.Equals(MouseButtons.Right))
            {
                Vector2 dif = mousePos - clickPos;
                clickPos = mousePos;
                CameraEditor.position += dif;
            }
        }

        private void XNAWindow_MouseUp(object sender, MouseEventArgs e)
        { // When a mouse button is released
            switch (e.Button.ToString())
            { // depending on which button we press
                case "Left":
                    if (currentState == CurrentState.Circle)
                    {
                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if(TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                    PropertyGrid.SelectedObject = LevelProperties.Collisions.CircleList.Last();
                                break;
                            case CurrentMode.Sectors:
                                if(currentView == CurrentView.Scenario)
                                    PropertyGrid.SelectedObject = CurrentScenario.Sectors.Last();
                                break;
                            case CurrentMode.TCC:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions 
                                    && TCCToolform?.CurrentState?.CollisionObject.objectType == ObjectClass.ObjectType.Circle)
                                    PropertyGrid.SelectedObject = TCCToolform.CurrentState.CollisionObject;
                                break;
                            case CurrentMode.IAAux:
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                                    if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                        PropertyGrid.SelectedObject = ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded;
                                }
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                                    if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                                        PropertyGrid.SelectedObject = ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded;
                                }
                                break;
                        }
                    }
                    if (currentState == CurrentState.Rectangle)
                    {
                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if(TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                    PropertyGrid.SelectedObject = LevelProperties.Collisions.RectList.Last();
                                break;
                            case CurrentMode.Sectors:
                                if(currentView == CurrentView.Scenario)
                                    PropertyGrid.SelectedObject = CurrentScenario.Sectors.Last();
                                break;
                            case CurrentMode.TCC:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions 
                                    && TCCToolform.CurrentState?.CollisionObject.objectType == ObjectClass.ObjectType.Rectangle)
                                    PropertyGrid.SelectedObject = TCCToolform.CurrentState.CollisionObject;
                                break;
                            case CurrentMode.IAAux:
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                                    if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                    {
                                        PropertyGrid.SelectedObject = ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded;
                                    }
                                }
                                if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                { //when having an ennemy that needs an ActivationZone, move its IAauxNeeded(ActivationZone) rectangle
                                    if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                                    {
                                        PropertyGrid.SelectedObject = ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded;
                                    }
                                }
                                break;
                        }
                    }
                    if(currentState == CurrentState.Edge)
                    {
                        //If the edge is too smal, delete it
                        switch (currentMode)
                        {
                            case CurrentMode.Collisions:
                                if(TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions)
                                {
                                    Vector2 A = LevelProperties.Collisions.EdgeList.Last().Vert1Pos;
                                    Vector2 B = LevelProperties.Collisions.EdgeList.Last().Vert2Pos;

                                    float distance = (float)Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
                                    Console.WriteLine(distance);
                                    if (distance < 0.5f)
                                    {
                                        LevelProperties.Collisions.EdgeList.Last().Delete();
                                        LevelProperties.Collisions.EdgeList.Remove(LevelProperties.Collisions.EdgeList.Last());
                                    }
                                    else PropertyGrid.SelectedObject = LevelProperties.Collisions.EdgeList.Last();
                                }
                                break;
                            case CurrentMode.TCC:
                                if (TreeView.SelectedNode != null && currentDict == CurrentDict.Collisions 
                                    && TCCToolform.CurrentState != null && TCCToolform.CurrentState.CollisionObject.objectType == ObjectClass.ObjectType.Edge)
                                {
                                    Vector2 A = ((ObjEdge)TCCToolform.CurrentState.CollisionObject).Vert1Pos;
                                    Vector2 B = ((ObjEdge)TCCToolform.CurrentState.CollisionObject).Vert2Pos;

                                    float distance = (float)Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
                                    if (distance < 0.5f)
                                    {
                                        TCCToolform.CurrentState.CollisionObject.Delete();
                                        TCCToolform.CurrentState.CollisionObject = null;
                                    }
                                    else PropertyGrid.SelectedObject = TCCToolform.CurrentState.CollisionObject;

                                    TCCToolform.RefreshPropertyGrid();
                                }
                                break;
                            case CurrentMode.Sectors:
                                if (currentView == CurrentView.Scenario)
                                {
                                    Vector2 A = ((ObjEdge)CurrentScenario.Sectors.Last().CollisionObject).Vert1Pos;
                                    Vector2 B = ((ObjEdge)CurrentScenario.Sectors.Last().CollisionObject).Vert2Pos;

                                    float distance = (float)Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
                                    if (distance < 0.5f)
                                    {
                                        CurrentScenario.Sectors.Last().CollisionObject.Delete();
                                        CurrentScenario.Sectors.Last().CollisionObject = null;
                                    }
                                    else PropertyGrid.SelectedObject = CurrentScenario.Sectors.Last().CollisionObject;
                                }
                                break;
                        }
                    }
                    if ((currentState == CurrentState.Spawn || currentState == CurrentState.PlayerSpawn || currentState == CurrentState.EndSpawn || currentState == CurrentState.GoalSpawn))
                    {
                        currentState = CurrentState.Spawn;
                        switch (currentDict)
                        {
                            case CurrentDict.Ennemies:
                                if (spawning) // protection
                                {
                                    if (currentView == CurrentView.Scenario)
                                    {
                                        PropertyGrid.SelectedObject = CurrentScenario.Enemies.Last();
                                        foreach (EnemyProperties en in CurrentScenario.Enemies)
                                        { // hide all the IAAuxs of the enemies we already spawned, except the one we just spawned
                                            if (en.Equals(CurrentScenario.Enemies.Last())) break;
                                            en.IAAuxNeeded?.Hide();
                                        }
                                    }
                                    else if(currentView == CurrentView.Precision)
                                    {
                                        PropertyGrid.SelectedObject = LevelProperties.TravelEnemies.Last();
                                        foreach (EnemyProperties en in LevelProperties.TravelEnemies)
                                        { // hide all the IAAuxs of the enemies we already spawned, except the one we just spawned
                                            if (en.Equals(LevelProperties.TravelEnemies.Last())) break;
                                            en.IAAuxNeeded?.Hide();
                                        }
                                    }
                                }
                                break;
                            case CurrentDict.Traps:
                                if (spawning) // protection
                                {
                                    if (currentView == CurrentView.Scenario)
                                    {
                                        PropertyGrid.SelectedObject = CurrentScenario.Zones[currentZone].PassiveTraps.Last();

                                        foreach (PassiveTrapsProperties t in CurrentScenario.Zones[currentZone].PassiveTraps)
                                        { // hide all the IAAuxs of the enemies we already spawned, except the one we just spawned
                                            if (t.Equals(CurrentScenario.Zones[currentZone].PassiveTraps.Last())) break;
                                            t.IAAuxNeeded?.Hide();
                                        }
                                    }
                                    else if (currentView == CurrentView.Precision)
                                    {
                                        PropertyGrid.SelectedObject = LevelProperties.TravelTraps.Last();

                                        foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
                                        { // hide all the IAAuxs of the enemies we already spawned, except the one we just spawned
                                            if (t.Equals(LevelProperties.TravelTraps.Last())) break;
                                            t.IAAuxNeeded?.Hide();
                                        }
                                    }
                                }
                                break;
                            case CurrentDict.Decoration:
                                if (spawning) // protection
                                {
                                    if (currentView == CurrentView.Scenario)
                                        PropertyGrid.SelectedObject = CurrentScenario.Zones[currentZone].Decoration.Last();
                                    else if (currentView == CurrentView.Precision)
                                        PropertyGrid.SelectedObject = LevelProperties.TravelDecos.Last();
                                }
                                break;
                        }
                        spawning = false;
                    }
                    if (currentState == CurrentState.None)
                    {
                        //select the body the mouse is touching and put it in the PropertyGrid
                        if (mouseFarseer != null)
                        {
                            // if we were touching an ennemy, hide its IAAux
                            if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties))
                                ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded?.Hide();
                            if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties))
                                ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded?.Hide();

                            //if we were touching an ennemy/trap's IAAux, hide it for now (and if it CanBeMoved, show it again later)
                            Type type = PropertyGrid.SelectedObject?.GetType();
                            if (type != null && Helpers.ClassInheritsFrom(type, typeof(IAAuxiliary.IAAux)))
                                ((IAAuxiliary.IAAux)PropertyGrid.SelectedObject).Hide();

                            PropertyGrid.SelectedObject = null; //////////////////////

                            var c = mouseFarseer.ContactList;
                            while (c != null)
                            {
                                if (c.Contact.IsTouching)
                                {
                                    object uf;
                                    if (c.Contact.FixtureB.Body.Equals(mouseFarseer))
                                        uf = c.Contact.FixtureA.Body.UserData;
                                    else uf = c.Contact.FixtureB.Body.UserData;
                                    if (!Helpers.ClassInheritsFrom(uf.GetType(), typeof(ObjectClass))) goto cancelCollision;

                                    if (((ObjectClass)uf).Parent != null) // if we clicked on an object that has a parent, select the parent object
                                    {
                                        PropertyGrid.SelectedObject = ((ObjectClass)uf).Parent; // and not the rect

                                        if (PropertyGrid.SelectedObject.GetType() == typeof(ObjEdgeChain) && ((ObjEdgeChain)PropertyGrid.SelectedObject).Parent != null)
                                            PropertyGrid.SelectedObject = ((ObjEdgeChain)PropertyGrid.SelectedObject).Parent;

                                        if ((PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) || PropertyGrid.SelectedObject?.GetType() == typeof(ObjEdgeChain)) && ((ObjectClass)uf).UserData == "IAAux")
                                            PropertyGrid.SelectedObject = ((ObjectClass)uf).Parent;

                                        if (Helpers.ClassInheritsFrom(((ObjectClass)uf).Parent.GetType(), typeof(IAAuxiliary.IAAux)))
                                        { // if we just clicked on an ennemy's IAAux, show it again
                                            ((IAAuxiliary.IAAux)((ObjectClass)uf).Parent).Show();
                                        }
                                        if(((ObjectClass)uf).Parent.GetType() == typeof(ObjEdgeChain) && Helpers.ClassInheritsFrom(((ObjEdgeChain)((ObjectClass)uf).Parent).Parent?.GetType(), typeof(IAAuxiliary.IAAux)))
                                        {
                                            ((IAAuxiliary.IAAux)((ObjEdgeChain)((ObjectClass)uf).Parent).Parent).Show();
                                        }

                                        if (((ObjectClass)uf).Parent.GetType() == typeof(EnemyProperties))
                                        { // if we just clicked on an ennemy, show its IAAux again
                                            ((EnemyProperties)((ObjectClass)uf).Parent).IAAuxNeeded?.Show();
                                            break;
                                        }
                                        if (((ObjectClass)uf).Parent.GetType() == typeof(PassiveTrapsProperties))
                                        { // if we just clicked on an ennemy, show its IAAux again
                                            ((PassiveTrapsProperties)((ObjectClass)uf).Parent).IAAuxNeeded?.Show();
                                            break;
                                        }
                                    }
                                    else { PropertyGrid.SelectedObject = uf; } //select the clicked object if it was a simple object.
                                }
                                cancelCollision:
                                c = c.Next;
                            }

                            if (mouseFarseer != null)
                                EditorVariables.world.RemoveBody(mouseFarseer);
                            mouseFarseer = null;
                        }
                    }

                    if (currentState == CurrentState.Polygon || currentState == CurrentState.EdgeChain)
                    {
                        Vector2 mousePos = CameraEditor.ScreenToWorld(new Vector2(e.X, e.Y));


                        if(currentState == CurrentState.Polygon)
                        { //only let us create a polygon if we actually can do it (skip the polygon code if we can't)
                            switch(currentMode)
                            {
                                case CurrentMode.Collisions:
                                    if (TreeView.SelectedNode == null) goto cancelCreation;
                                    break;
                                case CurrentMode.Sectors:
                                    if (currentView != CurrentView.Scenario) { MessageBox.Show("Sectors only allowed on Scenarios."); goto cancelCreation; }
                                    break;
                                case CurrentMode.TCC:
                                    if (TreeView.SelectedNode == null || TCCToolform.CurrentState == null) goto cancelCreation;
                                    break;
                                case CurrentMode.IAAux:
                                    if ((PropertyGrid.SelectedObject?.GetType() != typeof(EnemyProperties) &&
                                        PropertyGrid.SelectedObject?.GetType() != typeof(PassiveTrapsProperties)) || PropertyGrid.SelectedGridItem?.Label != "IAAuxNeeded")
                                        goto cancelCreation;
                                        break;
                            }
                        }
                        else if (currentState == CurrentState.EdgeChain)
                        { //only let us create an EdgeChain if we actually can do it (skip the polygon code if we can't)
                            if (currentMode == CurrentMode.Collisions)
                            {
                                if (TreeView.SelectedNode == null) goto cancelCreation;
                            }
                            else if(currentMode == CurrentMode.TCC)
                            {
                                if (TreeView.SelectedNode == null) goto cancelCreation;
                            }
                            else if(currentMode == CurrentMode.IAAux) //BUG take this if out and only put else (like before)? 
                            {
                                if ((PropertyGrid.SelectedObject?.GetType() != typeof(EnemyProperties) &&
                                    PropertyGrid.SelectedObject?.GetType() != typeof(PassiveTrapsProperties)) || PropertyGrid.SelectedGridItem?.Label != "IAAuxNeeded")
                                    goto cancelCreation;
                            }
                        }
                        
                        clickPos = mousePos;
                        if (nbrPointPol == 0)
                            EditorVariables.polyCircleList.Add(new ObjCircle(ConvertUnits.ToSimUnits(mousePos), 0.1f, 0.0f));
                        else
                        {
                            if (nbrPointPol <= 2 || (ConvertUnits.ToDisplayUnits(EditorVariables.polyCircleList.First().body.Position) - new Vector2(mousePos.X, mousePos.Y)).Length() >= 5.0f)
                            {
                                EditorVariables.polyCircleList.Add(new ObjCircle(ConvertUnits.ToSimUnits(new Vector2(mousePos.X, mousePos.Y)),
                                0.1f, 0.0f));
                                EditorVariables.polyEdgeList.Add(new ObjEdge(EditorVariables.polyCircleList.Last().body.Position,
                                EditorVariables.polyCircleList.ElementAt(EditorVariables.polyCircleList.Count - 2).body.Position));
                            }
                            else
                            {
                                List<Vector2> vertlist = new List<Vector2>();
                                foreach (ObjCircle c in EditorVariables.polyCircleList)
                                {
                                    vertlist.Add(c.body.Position);
                                    EditorVariables.world.RemoveBody(c.body);
                                }
                                foreach (ObjEdge ed in EditorVariables.polyEdgeList)
                                    EditorVariables.world.RemoveBody(ed.body);






                                ////////////////// CREATING THE POLYGON /////////////////////
                                if (currentState == CurrentState.Polygon)
                                {
                                    switch (currentMode)
                                    {
                                        case CurrentMode.Collisions:
                                            if (TreeView.SelectedNode == null) TreeView.SelectedNode = TreeView.Nodes[0];

                                                LevelProperties.Collisions.PolyList.Add(new ObjPolygon(vertlist, TreeView.SelectedNode.Text)
                                                        { Color = setCollisionColor(TreeView.SelectedNode.Text) });
                                                PropertyGrid.SelectedObject = LevelProperties.Collisions.PolyList.Last();//EditorVariables.PolyList.Last();
                                            break;
                                        case CurrentMode.Sectors:
                                            if (currentView == CurrentView.Scenario)
                                            {
                                                CurrentScenario.Sectors.Add(new Sector(new ObjPolygon(vertlist), null));
                                                PropertyGrid.SelectedObject = CurrentScenario.Sectors.Last();
                                            }
                                            break;
                                        case CurrentMode.TCC:
                                            // creating the collisions of the selected TCC State in TCCTool
                                            if (TreeView.SelectedNode != null && TCCToolform.CurrentState != null)
                                            {
                                                TCCToolform.CurrentState.CollisionObject = new ObjPolygon(vertlist, TreeView.SelectedNode.Text)
                                                            { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                                PropertyGrid.SelectedObject = TCCToolform.CurrentState.CollisionObject;
                                                TCCToolform.Refresh();
                                            }
                                            break;
                                        case CurrentMode.IAAux:
                                            if(PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                            {
                                                if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.ActivationZone)
                                                { //do it only on the enemies on which we can add a polygon
                                                    ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody?.Delete();
                                                    ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody
                                                        = new ObjPolygon(vertlist, "IAAux")
                                                        {
                                                            Color = IAAuxiliary.IAAux.IAAuxColor,
                                                            Parent = ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded
                                                        };
                                                    PropertyGrid.SelectedObject = ((IAAuxiliary.ActivationZone)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody;
                                                }
                                            }
                                            if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                            {
                                                if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                                                { //do it only on the enemies on which we can add a polygon
                                                    ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody?.Delete();
                                                    ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody
                                                        = new ObjPolygon(vertlist, "IAAux")
                                                        {
                                                            Color = IAAuxiliary.IAAux.IAAuxColor,
                                                            Parent = ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded
                                                        };
                                                    PropertyGrid.SelectedObject = ((IAAuxiliary.ActivationZone)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).ActivationBody;
                                                }
                                            }
                                            break;
                                    }
                                }
                                else if (currentState == CurrentState.EdgeChain)
                                {
                                    if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                    { //when having an ennemy selected that needs a path, create an EdgeChain object in it's path body.
                                        if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == Enemy.SecondaryInfos.Path)
                                        {
                                            ((IAAuxiliary.IAPath)((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded).PathObject?.Delete();
                                            ((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded
                                                = new IAAuxiliary.IAPath(vertlist);
                                        }
                                    }
                                    if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties) && PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
                                    { //when having an ennemy selected that needs a path, create an EdgeChain object in it's path body.
                                        if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.Path)
                                        {
                                            ((IAAuxiliary.IAPath)((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded).PathObject?.Delete();
                                            ((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded
                                                = new IAAuxiliary.IAPath(vertlist);
                                        }
                                    }
                                    else if(currentMode == CurrentMode.Collisions)
                                    {
                                        LevelProperties.Collisions.EdgeChainList.Add(new ObjEdgeChain(vertlist, TreeView.SelectedNode.Text)
                                                    { EdgesColor = setCollisionColor(TreeView.SelectedNode.Text) });
                                        PropertyGrid.SelectedObject = LevelProperties.Collisions.EdgeChainList.Last();
                                    }
                                    else if(currentMode == CurrentMode.TCC)
                                    {
                                        if (TCCToolform?.CurrentState != null)
                                            TCCToolform.CurrentState.CollisionObject = new ObjEdgeChain(vertlist, TreeView.SelectedNode.Text)
                                                        { Color = setCollisionColor(TreeView.SelectedNode.Text) };
                                    }
                                }



                                nbrPointPol = -1;                           // reset
                                EditorVariables.polyCircleList.Clear();     // all the
                                EditorVariables.polyEdgeList.Clear();       // stuff
                            }
                        }
                        nbrPointPol++;
                    }
                    cancelCreation:
                    if (currentState == CurrentState.EditVertices)
                    {
                        if(mouseFarseer != null)
                            EditorVariables.world.RemoveBody(mouseFarseer);
                        mouseFarseer = null;
                    }
                    break;
                case "Right":
                    if (currentState == CurrentState.None && mouseFarseer != null)
                    {
                        // Remove the body the mouse is touching.
                        var c = mouseFarseer.ContactList;
                        while (c != null)
                        {
                            if (c.Contact.IsTouching)
                            {
                                object uf;
                                if (c.Contact.FixtureB.Body.Equals(mouseFarseer)) uf = c.Contact.FixtureA.Body.UserData;
                                else uf = c.Contact.FixtureB.Body.UserData;
                                if (!Helpers.ClassInheritsFrom(uf.GetType(), typeof(ObjectClass))) goto cancelDeletion;

                                if (((ObjectClass)uf).UserData == "IAAux") goto cancelDeletion; //do not let us delete an ennemy's IAAux

                                if (((ObjectClass)uf).Parent?.GetType() == typeof(ObjEdgeChain)) //delete the entire objedgechain if it is one
                                    ((ObjEdgeChain)((ObjectClass)uf).Parent).Delete();

                                else ((ObjectClass)uf).Delete(); //if it is a normal object, delete it simply


                                PropertyGrid.SelectedObject = null; // if we deleted the selected object, then remove it from the PropertyGrid also

                                TCCToolform?.RefreshPropertyGrid();
                            }
                            cancelDeletion:
                            c = c.Next;
                        }
                        if (mouseFarseer != null)
                            EditorVariables.world.RemoveBody(mouseFarseer);
                        mouseFarseer = null;

                    }
                    break;
                case "Middle":
                    break;
            }
        }

        #region Helpers
        private void XNAWindow_MouseEnter(object sender, EventArgs e)
        {
            MouseInXNA = true;
        }

        private void XNAWindow_MouseLeave(object sender, EventArgs e)
        {
            MouseInXNA = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (MouseInXNA) e.SuppressKeyPress = true; // prevents the user from typing in unwanted situations (text / shortcuts)

            if (e.Shift) ShiftPressed = true;
            else ShiftPressed = false;

            if (e.Control) CtrlPressed = true;
            else CtrlPressed = false;

            if (CtrlPressed && e.KeyCode == Keys.S) { FileSave_Click(null, null); }
            if (CtrlPressed && e.KeyCode == Keys.O) { FileOpen_Click(null, null); }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift) ShiftPressed = true;
            else ShiftPressed = false;

            if (e.Control) CtrlPressed = true;
            else CtrlPressed = false;
        }
        #endregion

        #endregion

        #region ModesButtons
        private void CollisionsModeButton_Click(object sender, EventArgs e)
        { //When we click on the collisions mode button
            SelectMode(CurrentMode.Collisions);

            ResetStateButtonsColor();
            btnNoState.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.None;

            BtnPolygon.Enabled = true;
            BtnRectangle.Enabled = true;
            BtnCircle.Enabled = true;
            BtnEdge.Enabled = true;
            btnSpawn.Enabled = false;
        }

        private void SectorsModeButton_Click(object sender, EventArgs e)
        { //When we click on the Sectors mode button
            SelectMode(CurrentMode.Sectors);

            ResetStateButtonsColor();
            btnNoState.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.None;

            BtnPolygon.Enabled = true;
            BtnRectangle.Enabled = true;
            BtnCircle.Enabled = true;
            BtnEdge.Enabled = true;
            btnSpawn.Enabled = false;
        }

        private void SpawnsModeButton_Click(object sender, EventArgs e)
        { //When we click on the Spawns mode button
            SelectMode(CurrentMode.Spawns);

            currentState = CurrentState.Spawn;
            ResetStateButtonsColor();
            btnSpawn.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);

            BtnPolygon.Enabled = false;
            BtnRectangle.Enabled = false;
            BtnCircle.Enabled = false;
            BtnEdge.Enabled = false;
            btnSpawn.Enabled = true;
        }


        private void TCCModeButton_Click(object sender, EventArgs e)
        { //When we click on the changing collisions mode button
            SelectMode(CurrentMode.TCC);

            launchTTCTools();

            BtnPolygon.Enabled = true;
            BtnRectangle.Enabled = true;
            BtnCircle.Enabled = true;
            BtnEdge.Enabled = true;
            btnSpawn.Enabled = false;
            //create new object if you're not creating one
            //show object properties
        }

        private void PropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (PropertyGrid.SelectedGridItem?.Label == "IAAuxNeeded")
            {
                if (PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties))
                {
                    if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded?.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                    {
                        // when having an ennemy selected, and when we select its IAAux, put the CurrentMode to IAAux and disable
                        // all mode buttons / enable State buttons
                        SelectMode(CurrentMode.IAAux);
                        BtnPolygon.Enabled = true;
                        BtnRectangle.Enabled = true;
                        BtnCircle.Enabled = true;
                        BtnEdge.Enabled = false;
                        btnSpawn.Enabled = false;
                    }
                    else if (((EnemyProperties)PropertyGrid.SelectedObject).IAAuxNeeded?.NeededInfos == IAActivatedObject.SecondaryInfos.Path)
                    { // same, but only let us put an EdgeChain in the ennemy wants a path.
                        SelectMode(CurrentMode.IAAux);
                        BtnPolygon.Enabled = false;
                        BtnRectangle.Enabled = false;
                        BtnCircle.Enabled = false;
                        BtnEdge.Enabled = true;
                        btnSpawn.Enabled = false;
                    }
                }
                else if (PropertyGrid.SelectedObject?.GetType() == typeof(PassiveTrapsProperties))
                {
                    if (((PassiveTrapsProperties)PropertyGrid.SelectedObject).IAAuxNeeded?.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                    {
                        // when having an ennemy selected, and when we select its IAAux, put the CurrentMode to IAAux and disable
                        // all mode buttons / enable State buttons
                        SelectMode(CurrentMode.IAAux);
                        BtnPolygon.Enabled = true;
                        BtnRectangle.Enabled = true;
                        BtnCircle.Enabled = true;
                        BtnEdge.Enabled = false;
                        btnSpawn.Enabled = false;
                    }
                }
            }
            else if((PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties) || PropertyGrid.SelectedObject?.GetType() == typeof(EnemyProperties))
                    && currentMode == CurrentMode.IAAux)
            {//if we are still in the ennemy's properties but went out of the IAAux edition mode, go back to normal
                if (currentMode == CurrentMode.IAAux) CollisionsModeButton_Click(null, null);
            }
        }

        public void SelectMode(CurrentMode cm)
        {
            ResetModeButtonsColor();
            currentMode = cm;

            CollisionsModeButton.Enabled = true;
            SectorsModeButton.Enabled = true;
            SpawnsModeButton.Enabled = true;
            TCCModeButton.Enabled = true;

            switch (cm)
            {
                case CurrentMode.Collisions:
                    CollisionsModeButton.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                    break;
                case CurrentMode.IAAux://NOTHING
                    CollisionsModeButton.Enabled = false;
                    SectorsModeButton.Enabled = false;
                    SpawnsModeButton.Enabled = false;
                    TCCModeButton.Enabled = false;
                    break;
                case CurrentMode.None://NOTHING
                    break;
                case CurrentMode.Sectors:
                    SectorsModeButton.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                    break;
                case CurrentMode.Spawns:
                    SpawnsModeButton.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                    break;
                case CurrentMode.TCC:
                    TCCModeButton.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                    break;
            }
        }
        #endregion

        #region StatesButtons
        private void btnNoState_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();
            btnNoState.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.None;
        }

        private void BtnPolygon_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();
            BtnPolygon.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.Polygon;
        }

        private void BtnRectangle_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();
            BtnRectangle.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.Rectangle;
        }

        private void BtnCircle_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();
            BtnCircle.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.Circle;
        }

        private void btnEditVertices_Click(object sender, EventArgs e)
        {
            if (currentState != CurrentState.EditVertices)
            {
                ResetStateButtonsColor();
                btnEditVertices.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                currentState = CurrentState.EditVertices;
                if (PropertyGrid.SelectedObject?.GetType() == typeof(IAAuxiliary.ActivationZone)) //if we have an ennemy/trap's IAAux selected, put it's body in and not the whole object
                    PropertyGrid.SelectedObject = ((IAAuxiliary.ActivationZone)PropertyGrid.SelectedObject).ActivationBody; //(otherwise it won't work)
                EnableEditVerts(PropertyGrid.SelectedObject);

                btnNoState.Enabled = false;
                BtnPolygon.Enabled = false;
                BtnRectangle.Enabled = false;
                BtnCircle.Enabled = false;
                BtnEdge.Enabled = false;
                btnSpawn.Enabled = false;
            }
            else
            {
                ResetStateButtonsColor();
                btnNoState.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                currentState = CurrentState.None;
                DisableEditVerts(PropertyGrid.SelectedObject);
                btnNoState.Enabled = true;
                BtnPolygon.Enabled = true;
                BtnRectangle.Enabled = true;
                BtnCircle.Enabled = true;
                BtnEdge.Enabled = true;
                btnSpawn.Enabled = false;
            }
        }

        private void BtnEdge_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();

            if (currentState == CurrentState.Edge)
            {
                BtnEdge.BackColor = System.Drawing.Color.FromArgb(50, 200, 50);
                currentState = CurrentState.EdgeChain;
            }
            else if (currentState == CurrentState.EdgeChain)
            {
                currentState = CurrentState.None;
            }
            else
            {
                BtnEdge.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
                currentState = CurrentState.Edge;
            }
        }

        private void btnSpawn_Click(object sender, EventArgs e)
        {
            ResetStateButtonsColor();
            btnSpawn.BackColor = System.Drawing.Color.FromArgb(180, 180, 180);
            currentState = CurrentState.Spawn;
        }
        #endregion

        #region Helpers
        //private Vector2 ConvertMousePosToWorld(Vector2 initialPos)
        //{
        //    // find the pos on the real world (use the Camera)

        //    return initialPos;
        //}
        private void ResetStateButtonsColor()
        {
            btnNoState.BackColor = System.Drawing.Color.FromName("Control");
            BtnPolygon.BackColor = System.Drawing.Color.FromName("Control");
            BtnRectangle.BackColor = System.Drawing.Color.FromName("Control");
            BtnCircle.BackColor = System.Drawing.Color.FromName("Control");
            BtnEdge.BackColor = System.Drawing.Color.FromName("Control");
            btnSpawn.BackColor = System.Drawing.Color.FromName("Control");
            btnEditVertices.BackColor = System.Drawing.Color.FromName("Control");
        }
        private void ResetModeButtonsColor()
        {
            CollisionsModeButton.BackColor = System.Drawing.Color.FromName("Control");
            SectorsModeButton.BackColor = System.Drawing.Color.FromName("Control");
            TCCModeButton.BackColor = System.Drawing.Color.FromName("Control");
            SpawnsModeButton.BackColor = System.Drawing.Color.FromName("Control");
        }

        private Color backupColor = Color.TransparentBlack;
        private bool CtrlPressed;

        private void EnableEditVerts(object o)
        {
            if (o != null)
            {
                if (o.GetType() == typeof(ObjPolygon))
                {
                    ObjPolygon obj = (ObjPolygon)o; //backup
                    
                    backupColor = ((ObjPolygon)o).Color;
                    ((ObjPolygon)o).Color = new Color(120, 120, 120);

                    if (EditorVariables.polyCircleList.Count > 0) EditorVariables.polyCircleList.Clear(); //in case we were creating a polygon, take that off
                    for (int i = 0; i < obj.VerticesList.Count; i++)
                    {
                        Vector2 vert = obj.VerticesList[i];
                        EditorVariables.polyCircleList.Add(new ObjCircle(vert, 0.1f, 0f));
                        EditorVariables.polyCircleList.Last().Color = Color.Red;
                        EditorVariables.polyCircleList.Last().body.IsSensor = true;
                    }
                }

                if (o.GetType() == typeof(ObjEdgeChain))
                {
                    ObjEdgeChain obj = (ObjEdgeChain)o; //backup
                    
                    backupColor = ((ObjEdgeChain)o).Color;
                    ((ObjEdgeChain)o).EdgesColor = new Color(120, 120, 120);

                    if (EditorVariables.polyCircleList.Count > 0) EditorVariables.polyCircleList.Clear(); //in case we were creating a polygon, take that off
                    for (int i = 0; i < obj.VerticesList.Count; i++)
                    {
                        Vector2 vert = obj.VerticesList[i];
                        EditorVariables.polyCircleList.Add(new ObjCircle(vert, 0.1f, 0f));
                        EditorVariables.polyCircleList.Last().Color = Color.Red;
                        EditorVariables.polyCircleList.Last().body.IsSensor = true;
                    }
                }
            }
        }
        private void DisableEditVerts(object o)
        {
            if (o != null)
            {
                if (o.GetType() == typeof(ObjPolygon))
                {
                    foreach (ObjCircle c in EditorVariables.polyCircleList)      //
                        EditorVariables.world.RemoveBody(c.body);                // cleanup
                    EditorVariables.polyCircleList.Clear();                      //

                    ((ObjPolygon)o).Color = backupColor;
                    backupColor = Color.TransparentBlack;
                }
                if (o.GetType() == typeof(ObjEdgeChain))
                {
                    foreach (ObjCircle c in EditorVariables.polyCircleList)      //
                        EditorVariables.world.RemoveBody(c.body);                // cleanup
                    EditorVariables.polyCircleList.Clear();                      //

                    ((ObjEdgeChain)o).EdgesColor = backupColor;
                    backupColor = Color.TransparentBlack;
                }
            }
        }

        private void TCCTools_Click(object sender, EventArgs e)
        {
            launchTTCTools();
        }
        private void launchTTCTools()
        {
            if (TCCToolform == null || TCCToolform.FormClosed)
            {
                TCCToolform = new TCCTool();
                TCCToolform.StartPosition = FormStartPosition.WindowsDefaultLocation;
                TCCToolform.TopMost = true;
                TCCToolform.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                TCCToolform.MaximizeBox = false;
                TCCToolform.MinimizeBox = false;
                TCCToolform.Show();
            }
        }
        #endregion

        #region Dictionaries
        private void collisionsToolStripMenuItem_Click(object sender, EventArgs e)
        { //when we want to show the collisions dictionnary
            EnnemiesDict.Unload(); // unload previous dictionaries
            DecoDict.Unload();
            TrapsDict.Unload();
            ItemsDict.Unload();

            currentDict = CurrentDict.Collisions;
            loadCollisionsTree();
        }

        private void DictionaryItems_Click(object sender, EventArgs e)
        {
            EnnemiesDict.Unload(); // unload previous dictionaries
            DecoDict.Unload();
            TrapsDict.Unload();

            currentDict = CurrentDict.Items;

            ItemsDict.Initialize();
            ItemsDict.LoadItemsDict();
            loadItemsTree();
        }

        private void DictionaryEnnemies_Click(object sender, EventArgs e)
        {
            DecoDict.Unload();
            TrapsDict.Unload();
            ItemsDict.Unload();

            currentDict = CurrentDict.Ennemies;
            EnnemiesDict.Initialize();
            EnnemiesDict.LoadEnnemiesDict();
            ImageList imgList = new ImageList();

            foreach(string eName in EnnemiesDict.names)
                imgList.Images.Add(System.Drawing.Image.FromFile("Content/Entities/Enemies/" + eName + "Thumbnail.png"));

            TreeView.ImageList = imgList;
            TreeView.ItemHeight = 20;

            TreeView.Nodes.Clear();


            int groupTrack = 0, ennemyTrack = 0; //know where we are in the loops
            foreach (KeyValuePair<string, Dictionary<string, System.Drawing.Image>> group in EnnemiesDict.ennemies)
            { // for each group
                TreeNode groupNode = new TreeNode(group.Key);
                groupNode.ImageIndex = groupTrack + ennemyTrack;

                TreeView.Nodes.Add(groupNode);

                foreach (KeyValuePair<string, System.Drawing.Image> ennemy in group.Value)
                { //
                    ennemyTrack++;
                    TreeNode node = new TreeNode(ennemy.Key);
                    node.ImageIndex = ennemyTrack + groupTrack; //add image
                    TreeView.Nodes[groupTrack].Nodes.Add(node);


                }
                groupTrack++;
            }
        }

        private void DictionaryDecoration_Click(object sender, EventArgs e)
        {
            EnnemiesDict.Unload();
            ItemsDict.Unload();
            currentDict = CurrentDict.Decoration;
            loadDecoTree();
        }

        private void DictionaryTraps_Click(object sender, EventArgs e)
        {
            EnnemiesDict.Unload(); // unload previous dictionaries
            DecoDict.Unload();
            ItemsDict.Unload();

            currentDict = CurrentDict.Traps;
            loadTrapsTree();
        }

        private void loadDecoTree()
        {
            DecoDict.Initialize();
            DecoDict.LoadDecoDict();
            ImageList imgList = new ImageList();


            foreach (System.Drawing.Image img in DecoDict.images)
            {
                imgList.Images.Add(img);
            }
            TreeView.ImageList = imgList;
            TreeView.ItemHeight = 20;

            TreeView.Nodes.Clear();


            int groupTrack = 0, objectTrack = 0; //know where we are in the loops
            foreach (KeyValuePair<string, Dictionary<string, System.Drawing.Image>> group in DecoDict.decos)
            { // for each group
                TreeNode groupNode = new TreeNode(group.Key);
                groupNode.ImageIndex = groupTrack + objectTrack;

                TreeView.Nodes.Add(groupNode);

                foreach (KeyValuePair<string, System.Drawing.Image> obj in group.Value)
                { //
                    objectTrack++;
                    TreeNode node = new TreeNode(obj.Key);
                    node.ImageIndex = objectTrack + groupTrack; //add image
                    TreeView.Nodes[groupTrack].Nodes.Add(node);
                }
                groupTrack++;
            }
        }
        private void loadCollisionsTree()
        {
            //CAUTION : IF CHANGING THIS LIST, UPDATE THE HideCollisions METHOD IN THE LAYERS FORM (more description there)
            currentDict = CurrentDict.Collisions;

            ImageList imgList = new ImageList();            
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/dirt.png"));
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/semi.png"));
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/ladder.png"));
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/water.png"));
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/sand.png"));
            imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Entities/LevelEditorIcons/Collisions/movingPlatform.png"));
            TreeView.ImageList = imgList;

            TreeView.ItemHeight = 20;

            TreeView.Nodes.Clear();
            TreeNode node;

            node = new TreeNode("Solid");
            node.ImageIndex = 0;
            TreeView.Nodes.Add(node);

            node = new TreeNode("Semi");
            node.ImageIndex = 1;
            TreeView.Nodes.Add(node);

            node = new TreeNode("Ladder");
            node.ImageIndex = 2;
            TreeView.Nodes.Add(node);

            node = new TreeNode("Water");
            node.ImageIndex = 3;
            TreeView.Nodes.Add(node);

            node = new TreeNode("Sand");
            node.ImageIndex = 4;
            TreeView.Nodes.Add(node);

            node = new TreeNode("Moving Platform");
            node.ImageIndex = 5;
            TreeView.Nodes.Add(node);
        }
        private void loadItemsTree()
        {
            //currentDict = CurrentDict.Items;

            //ImageList imgList = new ImageList();
            //imgList.Images.Add(System.Drawing.Image.FromFile(@"Content/Icons/Checkpoint.png"));
            //TreeView.ImageList = imgList;

            //TreeView.ItemHeight = 20;

            //TreeView.Nodes.Clear();
            //TreeNode node;

            //node = new TreeNode("Checkpoint");
            //node.ImageIndex = 0;
            //TreeView.Nodes.Add(node);
            ItemsDict.Initialize();
            ItemsDict.LoadItemsDict();
            ImageList imgList = new ImageList();


            foreach (System.Drawing.Image img in ItemsDict.images)
            {
                imgList.Images.Add(img);
            }
            TreeView.ImageList = imgList;
            TreeView.ItemHeight = 20;

            TreeView.Nodes.Clear();


            int groupTrack = 0, trapTrack = 0; //know where we are in the loops
            foreach (KeyValuePair<string, Dictionary<string, System.Drawing.Image>> group in ItemsDict.items)
            { // for each group
                TreeNode groupNode = new TreeNode(group.Key);
                groupNode.ImageIndex = groupTrack + trapTrack;

                TreeView.Nodes.Add(groupNode);

                foreach (KeyValuePair<string, System.Drawing.Image> trap in group.Value)
                { //
                    trapTrack++;
                    TreeNode node = new TreeNode(trap.Key);
                    node.ImageIndex = trapTrack + groupTrack; //add image
                    TreeView.Nodes[groupTrack].Nodes.Add(node);


                }
                groupTrack++;
            }
        }

        private void loadTrapsTree()
        {
            TrapsDict.Initialize();
            TrapsDict.LoadTrapsDict();
            ImageList imgList = new ImageList();


            foreach (System.Drawing.Image img in TrapsDict.images)
            {
                imgList.Images.Add(img);
            }
            TreeView.ImageList = imgList;
            TreeView.ItemHeight = 20;

            TreeView.Nodes.Clear();


            int groupTrack = 0, trapTrack = 0; //know where we are in the loops
            foreach (KeyValuePair<string, Dictionary<string, System.Drawing.Image>> group in TrapsDict.traps)
            { // for each group
                TreeNode groupNode = new TreeNode(group.Key);
                groupNode.ImageIndex = groupTrack + trapTrack;

                TreeView.Nodes.Add(groupNode);

                foreach (KeyValuePair<string, System.Drawing.Image> trap in group.Value)
                { //
                    trapTrack++;
                    TreeNode node = new TreeNode(trap.Key);
                    node.ImageIndex = trapTrack + groupTrack; //add image
                    TreeView.Nodes[groupTrack].Nodes.Add(node);


                }
                groupTrack++;
            }
        }


        #endregion

        #region Effects
        private void effectsPickerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showEffectsPicker(null);
        }
        private void showEffectsPicker(List<string> s)
        {
            effectsForm = new EffectsForm(s);
            effectsForm.btnDone.Click += (sender, e) => { effectsForm.Close(); };
            effectsForm.AcceptButton = effectsForm.btnDone;
            effectsForm.StartPosition = FormStartPosition.CenterParent;
            effectsForm.FormClosing += effectsForm_FormClosing;
            effectsForm.Show();
        }
        void effectsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (PropertyGrid.SelectedObject != null && PropertyGrid.SelectedObject.GetType() == typeof(Sector))
                ((Sector)PropertyGrid.SelectedObject).Effects = effectsForm.FinalEffects;
        }
        #endregion

        #region SliderView
        private void rBtnPrecision_CheckedChanged(object sender, EventArgs e)
        { //Handling the change of the View Mode (with the radio buttons)
            if (rBtnPrecision.Checked)
            { //if we clicked on the PrecisionView radio button
                currentView = CurrentView.Precision;
                SliderTime.Enabled = true;
                UpDownScenario.Enabled = false;
                LblCurrentYear.Text = "Current Year : " + SliderTime.Value;

                UpdateObjectsVisibility();
            }
            else if (rBtnScenario.Checked)
            { // if we clicked on the ScenarioView radio button
                currentView = CurrentView.Scenario;
                SliderTime.Enabled = false;
                UpDownScenario.Enabled = true;
                LblCurrentYear.Text = "Current Scenario : " + UpDownScenario.Value;

                UpdateObjectsVisibility();
                setSliderMiddlePos(CurrentScenario.StartTime, CurrentScenario.EndTime);
            }

            if (EditorVariables.DevMode) Editor.reloadImages = true;
        }

        private void UpDownScenario_ValueChanged(object sender, EventArgs e)
        {
            //only allow to go between scenario 0 and LP.Scenarios.Count (- 1)
            if (UpDownScenario.Value < LevelProperties.Scenarios.Count)
            { // When changing the scenario
                LblCurrentYear.Text = "Current Scenario : " + UpDownScenario.Value;

                EditorVariables.CurrentScenario = (int)UpDownScenario.Value;

                setSliderMiddlePos(CurrentScenario.StartTime, CurrentScenario.EndTime);
                EditorVariables.CurrentYear = (SliderTime.Value);
                
                UpdateObjectsVisibility();

                if (EditorVariables.DevMode) Editor.reloadImages = true;
            }
            else UpDownScenario.Maximum = LevelProperties.Scenarios.Count - 1; //if we go too far, set the max where it should be again

            EditorVariables.CurrentScenario = (int)UpDownScenario.Value;
        }

        public static void UpdateObjectsVisibility()
        { //when changing the time, only show the objects that are alive.
            if (currentView == CurrentView.Precision)
            { //TODO hide and show items
                //hide all the scenarios
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    sc.Hide();

                //show the objects that are alive in the current year.
                //TODO put the real entities lifespans instead of the current 40 years.
                foreach (EnemyProperties en in LevelProperties.TravelEnemies)
                { //Enemies
                    if (EditorVariables.CurrentYear < en.SpawnDate || EditorVariables.CurrentYear > en.SpawnDate + 40 /*ennemy lifespan CHANGER*/)
                        en.Hide();     //if the ennemy is living during this year, show him. Hide him otherwise
                    else { if (LayersDisplay.Enemies) en.Show(); }
                }

                foreach (DecorationProperties d in LevelProperties.TravelDecos)
                { //Enemies
                    if (EditorVariables.CurrentYear < d.SpawnDate || EditorVariables.CurrentYear > d.SpawnDate + 40 /*deco lifespan CHANGER*/)
                        d.Hide();     //if the ennemy is living during this year, show him. Hide him otherwise
                    else { if (LayersDisplay.Decoration) d.Show(); }
                }

                foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
                {
                    if (EditorVariables.CurrentYear < t.SpawnDate || EditorVariables.CurrentYear > t.SpawnDate + 40 /*trap lifespan CHANGER*/)
                        t.Hide();     //if the trap is living during this year, show him. Hide him otherwise
                    else { if (LayersDisplay.Traps) t.Show(); }
                }

                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                    if (EditorVariables.CurrentYear >= sc.StartTime && EditorVariables.CurrentYear <= sc.EndTime)
                        sc.Show();
                //TODO apply the layers restrictions in sc.hide and sc.show too
            }
            else if (currentView == CurrentView.Scenario)
            {
                //hide all the precision objects
                foreach (EnemyProperties en in LevelProperties.TravelEnemies)
                    en.Hide();
                foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
                    t.Hide();
                foreach (DecorationProperties d in LevelProperties.TravelDecos)
                    d.Hide();

                //show only the current scenario
                foreach (ScenarioProperties sc in LevelProperties.Scenarios)
                { //hide all the scenarios unless the currentOne (show it)
                    if (!LevelProperties.Scenarios[EditorVariables.CurrentScenario].Equals(sc)) sc.Hide();
                    else sc.Show();
                }
            }

            
            foreach(TCCProperties TCC in LevelProperties.TCCs)
            { // only show the currently visible TCC's states
                foreach (TCCState state in TCC.States)
                {
                    if(state.TimeType == TCCState.TimeTypeEnum.Precision)
                        if (EditorVariables.CurrentYear >= state.startTime && EditorVariables.CurrentYear < state.endTime)
                            state.Show();
                        else { if (LayersDisplay.TCCs) state.Hide(); }

                    else if(state.TimeType == TCCState.TimeTypeEnum.Scenario)
                        if (EditorVariables.CurrentYear >= LevelProperties.Scenarios[state.startTime].StartTime 
                         && EditorVariables.CurrentYear  < LevelProperties.Scenarios[state.endTime].StartTime)
                            state.Show();
                        else { if (LayersDisplay.TCCs) state.Hide(); }
                }
            }

            if (TCCToolform != null && !TCCToolform.FormClosed)
                TCCToolform.RefreshData();


        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        { // Draw the SliderTimes scenarios guidelines
            DrawScenariosGuidelines();
        }

        public void DrawScenariosGuidelines()
        { // Draw the SliderTimes scenarios guidelines
            System.Drawing.Color color = System.Drawing.Color.White;
            if (currentView == CurrentView.Precision)
                color = System.Drawing.Color.Gray;
            else if (currentView == CurrentView.Scenario)
                color = System.Drawing.Color.DarkSeaGreen;

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.Graphics formGraphics;
            formGraphics = SliderTime.CreateGraphics();


            int sliderLeft = SliderTime.Location.X;
            int sliderRight = SliderTime.Location.X + SliderTime.Size.Width;
            try
            {
                //if (TCCToolform != null && !TCCToolform.FormClosed && TCCToolform.CurrentState != null)
                //{ //TODO correct code not doing the right thing (show slider guidelines of the selected TCC)
                //    int i = 0;
                //    foreach (TCCState state in TCCToolform.CurrentTCC.States)
                //    {
                //        Vector2 time = TCCToolform.CurrentTCC.GetLifeTime(i);

                //        int reste = i % 2;
                //        myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 50 + 150 * reste, 50, 100 + 150 * reste));
                //        //Console.WriteLine(System.Drawing.Color.FromArgb(255, 50 + 150 * reste, 50, 100 + 150 * reste) + "    " + i);
                //        int posLeft = ((int)time.X * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                //        int posRight = ((int)time.Y * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                //        formGraphics.FillRectangle(myBrush, new System.Drawing.Rectangle(posLeft, 20, posRight - posLeft + 5, 6));

                //        Console.WriteLine(time + "   " + i);

                //        i++;
                //    }
                //}
                //else
                {
                    foreach (ScenarioProperties scenario in LevelProperties.Scenarios)
                    {
                        int posLeft = (scenario.StartTime * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                        int posRight = (scenario.EndTime * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                        formGraphics.FillRectangle(myBrush, new System.Drawing.Rectangle(posLeft, 20, posRight - posLeft + 5, 6));
                    }

                    foreach (DecorationProperties d in LevelProperties.TravelDecos)
                    {
                        int posX = (d.SpawnDate * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                        formGraphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.DarkCyan), new System.Drawing.Rectangle(posX, 20, 5, 6));
                    }
                    foreach (EnemyProperties e in LevelProperties.TravelEnemies)
                    {
                        int posX = (e.SpawnDate * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                        formGraphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.MediumVioletRed), new System.Drawing.Rectangle(posX, 20, 5, 6));
                    }
                    foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
                    {
                        int posX = (t.SpawnDate * (SliderTime.Size.Width - 25)) / LevelProperties.NYears + 10;
                        formGraphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.OrangeRed), new System.Drawing.Rectangle(posX, 20, 5, 6));
                    }

                    if(saveSucceeded)
                        formGraphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Green), new System.Drawing.Rectangle(0, 20, SliderTime.Size.Width, 6));
                }
            }
            catch { }

            myBrush.Dispose();
            formGraphics.Dispose();
        }
        
        private void setSliderMiddlePos(int pos1, int pos2)
        {
            SliderTime.Value = (pos1 + pos2) / 2;
        }
        #endregion

        private void btnLayers_Click(object sender, EventArgs e)
        {
            LayersForm layers = new LayersForm();
            layers.TopMost = true;
            layers.Show();
        }

        public static Color setCollisionColor(string ud)
        {
            if(ud != null)
            {
                Color color = Color.TransparentBlack;
                if (ud == "Solid") color = Color.SandyBrown;
                else if (ud == "Semi") color = Color.DimGray;
                else if (ud == "Ladder") color = Color.Purple;
                else if (ud == "Water") color = Color.DeepSkyBlue;
                else if (ud == "Sand") color = Color.Beige;
                else if (ud == "Moving Platform") color = Color.DarkOliveGreen;

                //if (obj.GetType() == typeof(ObjEdgeChain)) ((ObjEdgeChain)obj).EdgesColor = color;
                //else obj.Color = color;
                return color;
            }
            return Color.TransparentBlack;
        }

        private void FileSave_Click(object sender, EventArgs e)
        {
            saveSucceeded = LevelLoader.SaveLevel();
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            PropertyGrid.SelectedObject = null;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            string result = LevelLoader.LoadLevel(dialog.FileName);
            if (result != "success")
                MessageBox.Show(result);
            else saveSucceeded = true;
        }
    }
}