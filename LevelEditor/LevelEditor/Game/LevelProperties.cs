#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LEJEU.Entities.Enemies;
using LevelEditor.IAAuxiliary;
using FarseerPhysics.Factories;
using System.ComponentModel;
using System.Drawing.Design;
using LEJEU.Entities;
using System.Xml;
using FarseerPhysics;
using System.Globalization;
#endregion

namespace LevelEditor
{

    public static class LevelProperties
    {
        public static string LevelBasePath { get; set; }

        public static Vector2 zoneDimensions = new Vector2(1600, 900);
        public static string Weather { get; set; }
        public static string SoundtrackPath { get; set; }
        public static int NZones { get; set; } //FORGET IT
        public static int TTS { get; set; } //Time Travelling Speed
        public static int NYears { get; set; }
        public static int NBacks { get; set; }

        public static List<ScenarioProperties> Scenarios { get; set; }

        public static List<EnemyProperties> TravelEnemies { get; set; } // enemies and traps that are spawned 
        public static List<PassiveTrapsProperties> TravelTraps { get; set; } // outside the scenarios (when travelling)
        public static List<DecorationProperties> TravelDecos { get; set; }

        public static CollisionsProperties Collisions { get; set; }
        public static MainSpawns Spawns { get; set; }
        public static List<TCCProperties> TCCs { get; set; }



        public static float BackSpeed { get; set; }
        public static List<string> Backgrounds { get; set; }

        public static void Initialize()
        {
            NBacks = 1;
            NZones = 1;
            NYears = 1000;
            Backgrounds = new List<string>();
            Backgrounds.Add("");

            LevelBasePath = @"Maps/Chapter1/LevelTemplate/";
            SoundtrackPath = "";
            Weather = "Sunny";

            TravelEnemies = new List<EnemyProperties>();
            TravelTraps = new List<PassiveTrapsProperties>();
            TravelDecos = new List<DecorationProperties>();

            Scenarios = new List<ScenarioProperties>();
            Scenarios.Add(new ScenarioProperties(0, 50));
            Scenarios.Add(new ScenarioProperties(200, 250));
            Scenarios.Add(new ScenarioProperties(950, 1000));

            Collisions = new CollisionsProperties();
            Collisions.CircleList = new List<ObjCircle>();
            Collisions.EdgeList = new List<ObjEdge>();
            Collisions.EdgeChainList = new List<ObjEdgeChain>();
            Collisions.RectList = new List<ObjRectangle>();
            Collisions.PolyList = new List<ObjPolygon>();

            Spawns = new MainSpawns();

            TCCs = new List<TCCProperties>();
            TCCs.Add(new TCCProperties() { TCCName = "Cascade" });
            TCCs.Last().States.Add(new TCCState()
            {
                startTime = 12,
                endTime = 120
            });
        }
    }

    public class ZoneProperties
    {
        public List<DecorationProperties> Decoration { get; set; }
        public List<ItemProperties> Items { get; set; } //items and checkpoints
        public List<PassiveTrapsProperties> PassiveTraps { get; set; }

        public ZoneProperties()
        {
            Decoration = new List<DecorationProperties>();
            Items = new List<ItemProperties>();
            PassiveTraps = new List<PassiveTrapsProperties>();
        }
    }

    public class MainSpawns
    {
        public ObjRectangle playerSpawn { get; set; } // take the rect's position
        public ObjRectangle EndSpawn { get; set; } // take the rect's position
        public ObjRectangle GoalSpawn { get; set; } // take the rect's position
    }

    public class CollisionsProperties
    {
        public List<ObjCircle> CircleList;
        public List<ObjEdge> EdgeList;
        public List<ObjEdgeChain> EdgeChainList;
        public List<ObjRectangle> RectList;
        public List<ObjPolygon> PolyList;
    }

    public class Sector
    {
        // Sectors are only present on Scenarios, so they don't have startTime.
        public static Color SectorColor = Color.Violet;
        public ObjectClass CollisionObject { get; set; }
        public ObjectClass.ObjectType objectType
        {
            get
            {
                if (CollisionObject != null)
                    return CollisionObject.objType;
                else return ObjectClass.ObjectType.Null;
            }
        }

        public List<string> Effects { get; set; } //Example : "DAMAGE 20", "KILL", "POISON 30 13 true" (integrate arguments inside the string itself)

        public Sector(ObjectClass obj, List<string> args)
        {
            Effects = new List<string>();

            CollisionObject = obj;
            CollisionObject.Parent = this;
            CollisionObject.Color = SectorColor;

            if(args != null)
                Effects = args;
        }

        public void Hide()
        {
            CollisionObject.Hide();
        }
        public void Show()
        {
            CollisionObject.Show();
        }
    }

    public class DecorationProperties
    {
        public string id { get; set; }
        private float angle { get; set; }
        public SpriteEffects Direction { get; set; }

        public int SpawnDate { get; set; }

        public ObjRectangle spawnRect { get; set; }
        public Texture2D thumbnail { get; set; }
        public bool Hidden;

        public Vector2 Position
        {
            get { return spawnRect.BodyPosition; }
            set { spawnRect.BodyPosition = value; }
        }
        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                spawnRect.Angle = angle;
            }
        }

        public DecorationProperties(string id, Vector2 pos, ObjRectangle spawnRect, int spawndate, SpriteEffects dir = SpriteEffects.None)
        { //Contructor when we want to add an ennemy from the Editor
            this.id = id;
            SpawnDate = spawndate;

            this.spawnRect = spawnRect;
            this.spawnRect.Parent = this;
            Direction = dir;

            Hidden = false;
        }

        public void Hide()
        {
            Hidden = true;
            spawnRect.Hide();
        }
        public void Show()
        {
            Hidden = false;
            spawnRect.Show();
        }
    }

    public class EnemyProperties
    {
        public string id { get; set; } //voir dictionnaire XML
        public Vector2 Position
        {
            get { return spawnRect.BodyPosition; }
            set
            {
                spawnRect.BodyPosition = value;
                if (IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.Radius)
                    ((Radius)IAAuxNeeded).Position = value; //if we move the ennemy that is needing a radius, move the circle as well
            }
        }
        public SpriteEffects Direction { get; set; } // None = Left and SplitHorizontally = Right.

        public int SpawnDate { get; set; }
        public int Age { get; set; }

        public ObjRectangle spawnRect { get; set; }
        public Texture2D thumbnail { get; set; }
        public IAAux IAAuxNeeded { get; set; } //stores the elements the ennemy's IA needs (path, Waypoints..)
        public Enemy Instance;

        public bool Hidden;

        public EnemyProperties(string id, Vector2 pos, int spawndate, ObjRectangle spawnRect, SpriteEffects dir = SpriteEffects.None)
        { //Contructor when we want to add an ennemy from the Editor
            this.id = id;
            this.SpawnDate = spawndate;
            Direction = dir;

            this.spawnRect = spawnRect;
            this.spawnRect.Parent = this;
            Hidden = false;

            Instance = (Enemy)Activator.CreateInstance(Type.GetType("LEJEU.Entities.Enemies." + id));

            switch (Instance.NeededInfos)
            { // create the IA's NeededAux needed based on the ennemy's needs
                case IAActivatedObject.SecondaryInfos.DualWaypoints:
                    IAAuxNeeded = new DualWaypoints(new PreciseRectangle(spawnRect.BodyPosition.X - spawnRect.Bounds.Width / 2, spawnRect.BodyPosition.Y - spawnRect.Bounds.Height / 2, 1, 1),
                                                    new PreciseRectangle(spawnRect.BodyPosition.X + spawnRect.Bounds.Width / 2, spawnRect.BodyPosition.Y - spawnRect.Bounds.Height / 2, 1, 1));
                    break;
                case IAActivatedObject.SecondaryInfos.Radius:
                    IAAuxNeeded = new Radius(spawnRect.BodyPosition, 5f /*RADIUS*/);
                    break;
                case IAActivatedObject.SecondaryInfos.ActivationZone:
                    IAAuxNeeded = new ActivationZone(new PreciseRectangle(spawnRect.BodyPosition, spawnRect.Size.X + 2f, spawnRect.Size.Y + 2));
                    break;
                case IAActivatedObject.SecondaryInfos.Path:
                    IAAuxNeeded = new IAPath(new Vector2(spawnRect.BodyPosition.X - (spawnRect.Bounds.Width / 2) - 1, spawnRect.BodyPosition.Y - (spawnRect.Bounds.Height / 2) - 1),
                                             new Vector2(spawnRect.BodyPosition.X + (spawnRect.Bounds.Width / 2) + 1, spawnRect.BodyPosition.Y + (spawnRect.Bounds.Height / 2) + 1));

                    break;
            }
        }

        public void Hide()
        {
            Hidden = true;
            spawnRect.Hide();
            if (IAAuxNeeded != null) //if the ennemy doesn't need any aux
                IAAuxNeeded.Hide();
        }
        public void Show()
        {
            Hidden = false;
            spawnRect.Show();
        }

        public void Delete()
        {
            IAAuxNeeded?.Delete();
        }
    }

    public class PassiveTrapsProperties
    {
        public string id { get; set; }
        private float angle { get; set; }
        public Vector2 Position
        {
            get { return spawnRect.BodyPosition; }
            set
            {
                spawnRect.BodyPosition = value;
                if (IAAuxNeeded?.NeededInfos == Enemy.SecondaryInfos.Radius)
                    ((Radius)IAAuxNeeded).Position = value; //if we move the ennemy that is needing a radius, move the circle as well
            }
        }
        public SpriteEffects Direction { get; set; }

        List<string> EffectArgs { get; set; }
        List<string> Effects { get; set; }
        public int SpawnDate { get; set; }
        public int age { get; set; }

        public ObjRectangle spawnRect { get; set; }
        public Texture2D thumbnail { get; set; }
        public IAAux IAAuxNeeded { get; set; } //stores the elements the trap's IA needs (path, Waypoints..)
        public bool Hidden;
        public Trap Instance;

        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                spawnRect.Angle = angle;
            }
        }

        public PassiveTrapsProperties(string id, Vector2 pos, int spawndate, ObjRectangle spawnRect, SpriteEffects dir = SpriteEffects.None)
        { //Contructor when we want to add an ennemy from the Editor
            this.id = id;
            this.SpawnDate = spawndate;

            this.spawnRect = spawnRect;
            this.spawnRect.Parent = this;
            Direction = dir;

            Hidden = false;

            Instance = (Trap)Activator.CreateInstance(Type.GetType("LEJEU.Entities.Traps." + id));

            switch (Instance.NeededInfos)
            { // create the IA's NeededAux needed based on the ennemy's needs
                case IAActivatedObject.SecondaryInfos.Radius:
                    IAAuxNeeded = new Radius(spawnRect.BodyPosition, 5f /*RADIUS*/);
                    break;
                case IAActivatedObject.SecondaryInfos.ActivationZone:
                    IAAuxNeeded = new ActivationZone(new PreciseRectangle(spawnRect.BodyPosition, spawnRect.Size.X + 2f, spawnRect.Size.Y + 2));
                    break;
            }
        }

        public void Hide()
        {
            Hidden = true;
            spawnRect.Hide();
            IAAuxNeeded?.Hide();
        }
        public void Show()
        {
            Hidden = false;
            spawnRect.Show();
        }

        public void Delete()
        {
            IAAuxNeeded?.Delete();
        }
    }

    public class SafeZoneProperties
    {// outdated
        public Vector2 pos { get; set; }

        public string inactiveImg { get; set; }
        public string activeImg { get; set; }
        public string deadImg { get; set; }

        public string inactiveCollisions { get; set; } //List<Vector2> Verts
        public string activeCollisions { get; set; }
        public string deadCollisions { get; set; }

        public int spawnDate { get; set; }
        public int activationDate { get; set; }
        public int deathDate { get; set; }


    }

    public class ItemProperties
    {
        public string id { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public Vector2 Position
        {
            get { return spawnRect.BodyPosition; }
            set { spawnRect.BodyPosition = value; }
        }

        public ObjRectangle spawnRect; //take the Rect's pos for the item's position
        public Texture2D thumbnail;

        public ItemProperties(string id, Vector2 Position, float width, float height, Color color)
        {
            this.id = id;
            spawnRect = new ObjRectangle(Position, width, height, 0f, color);
        }

        public void Hide()
        {
            spawnRect.Hide();
        }
        public void Show()
        {
            spawnRect.Show();
        }
    }

    public class BackgroundProperties
    {
        public int id { get; set; }
        public string path { get; set; }
    }

    public class TCCProperties
    {
        public bool IsSafeZone;

        public string TCCName;
        public List<TCCState> States;
        public TCCProperties()
        {
            States = new List<TCCState>();
        }

        public Vector2 GetLifeTime(int stateIndex)
        { // gets the start and end time of the state we want. it returns Vector2(start time, end time)
            if (stateIndex >= States.Count) return Vector2.Zero;

            int startTime = 0;
            int endtime = 0;
            if (States[stateIndex].TimeType == TCCState.TimeTypeEnum.Precision)
            {
                startTime = States[stateIndex].startTime;
                if (stateIndex + 1 <= States.Count - 1)
                {
                    if (States[stateIndex + 1].TimeType == TCCState.TimeTypeEnum.Precision)
                        endtime = States[stateIndex + 1].startTime;

                    else endtime = (int)GetLifeTime(stateIndex + 1).Y;
                }
                else endtime = LevelProperties.NYears;
            }
            else if (States[stateIndex].TimeType == TCCState.TimeTypeEnum.Scenario)
            {
                startTime = LevelProperties.Scenarios[stateIndex].StartTime;
                endtime = (int)GetLifeTime(stateIndex + 1).Y;
            }
            return new Vector2(startTime, endtime);
        }
    }

    public class TCCState
    {
        public bool IsActiveState; //if we are dealing with a SafeZone, the ActiveState is when the player can come in and use the SF.

        public bool Hidden;
        public enum TimeTypeEnum
        { // determines if the state is attached to a scenario or if it is free. (so the type of startTime)
            Precision,
            Scenario
        }

        [Description("Determines if the state is attached to a scenario or if it is free in time. (year per year basis)")]
        public TimeTypeEnum TimeType { get; set; }

        public string ImagePath { get; set; }
        public Vector2 ImagePosition { get; set; }
        public Texture2D Image { get; set; }

        public ObjectClass CollisionObject { get; set; }

        [Description("Can be either a scenario ID or a year, depending on the selected TimeType option. START YEAR NOT INCLUDED ([startTime, endTime[)")]
        public int startTime { get; set; }
        [Description("Can be either a scenario ID or a year, depending on the selected TimeType option. END YEAR NOT INCLUDED ([startTime, endTime[)")]
        public int endTime { get; set; }



        public TCCState()
        {
            ImagePath = "";
            Hidden = false;
        }

        public void Hide()
        {
            if (!Hidden)
            {
                CollisionObject?.Hide();
                Hidden = true;
            }
        }

        public void Show()
        {
            if (Hidden)
            {
                Hidden = false;
                CollisionObject?.Show();
            }
        }
    }

    public class ScenarioProperties
    { // USELESS
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        /*
            Enemies
            Zones
                PassiveTraps
                Items
                Decoration
            TCCs ou pas ?
        */

        public List<ZoneProperties> Zones { get; set; }
        public List<EnemyProperties> Enemies { get; set; }
        public List<Sector> Sectors { get; set; }

        public ScenarioProperties(int start, int end)
        {
            StartTime = start;
            EndTime = end;

            Zones = new List<ZoneProperties>();
            //UpdateZonesCount(); //BUG put this back?

            Enemies = new List<EnemyProperties>();
            Sectors = new List<Sector>();
        }
        public ScenarioProperties()
        {
            Zones = new List<ZoneProperties>();
            Enemies = new List<EnemyProperties>();
            Sectors = new List<Sector>();
        }

        public void UpdateZonesCount()
        { //when changing the number of zones, refresh the number of zones of this scenario too
            if (Zones.Count > LevelProperties.NZones) //
            {
            rs:
                for (int i = Zones.Count - 1; i < LevelProperties.NZones - 1; i++)
                {
                    Zones.RemoveAt(i);
                    goto rs;
                }
            }
            else
            {
                for (int i = 0; i < LevelProperties.NZones; i++)
                {
                    if (i >= Zones.Count) //if the zone does not exists yet
                        Zones.Add(new ZoneProperties());
                }
            }
        }

        //hide and show all the elements in the scenario
        public void Hide()
        {
            foreach (ZoneProperties z in Zones)
            {
                foreach (PassiveTrapsProperties t in z.PassiveTraps)
                    t.Hide();
                foreach (ItemProperties i in z.Items)
                    i.Hide();
                foreach (DecorationProperties d in z.Decoration)
                    d.Hide();
            }
            foreach (EnemyProperties e in Enemies)
                e.Hide();
        }
        public void Show()
        {
            foreach (ZoneProperties z in Zones)
            {
                foreach (PassiveTrapsProperties t in z.PassiveTraps)
                    if (LayersDisplay.Traps)
                        t.Show();
                foreach (ItemProperties i in z.Items)
                    if (LayersDisplay.Items)
                        i.Show();
                foreach (DecorationProperties d in z.Decoration)
                    if (LayersDisplay.Decoration)
                        d.Show();
            }
            foreach (EnemyProperties e in Enemies)
                if (LayersDisplay.Enemies)
                    e.Show();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////// LEVEL //////////////////////////////////////////////////////
    /////////////////////////////////////////// LOADER //////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static class LevelLoader
    {
        public static XmlTextWriter w;
        private static ScenarioProperties lsc //last scenario
        {
            get { return LevelProperties.Scenarios.Last(); }
        }

        public static string LoadLevel(string path)
        { //returns "success" if it loaded the level successfully, and the specific error message if it couldn't load it.
            string resultMessage = "success"; // this will be returned if no error happened. if one error fired up, the string will be replaced by the error info.

            ClearLevel();
            //Load all the level with a given XML File
            if(!path.Contains(".xml")) { MessageBox.Show("Invalid file type. select an xml."); resultMessage = "invalid file format."; goto cancelLoad; }

            XElement doc;
            try {          doc = XDocument.Load(path).Element("Level"); }                              //document
            catch { resultMessage = "File corrupted or incomplete."; goto cancelLoad; }

            #region GeneralSettings
            LevelProperties.LevelBasePath = doc.Element("GeneralSettings").Element("LevelBasePath").Value.ToString();
                //MainSpawns
                XElement MainSpawns = doc.Element("GeneralSettings").Element("MainSpawns");
            try { LevelProperties.Spawns.playerSpawn = new ObjRectangle(new Vector2((float)Convert.ToDouble(MainSpawns.Element("PlayerSpawn").Attribute("X").Value),
                                                                              (float)Convert.ToDouble(MainSpawns.Element("PlayerSpawn").Attribute("Y").Value)),
                                                                              1f, 1f, 0f) { UserData = "PlayerSpawn" }; }
            catch { resultMessage = "No PlayerSpawn."; goto cancelLoad; }


            try { LevelProperties.Spawns.EndSpawn = new ObjRectangle(new Vector2((float)Convert.ToDouble(MainSpawns.Element("EndSpawn").Attribute("X").Value),
                                                                               (float)Convert.ToDouble(MainSpawns.Element("EndSpawn").Attribute("Y").Value)),
                                                                               1f, 1f, 0f) { UserData = "EndSpawn" }; }
            catch { resultMessage = "No EndSpawn."; goto cancelLoad; }
            try { LevelProperties.Spawns.GoalSpawn = new ObjRectangle(new Vector2((float)Convert.ToDouble(MainSpawns.Element("GoalSpawn").Attribute("X").Value),
                                                                              (float)Convert.ToDouble(MainSpawns.Element("GoalSpawn").Attribute("Y").Value)),
                                                                              1f, 1f, 0f) { UserData = "GoalSpawn" }; }
            catch { MessageBox.Show("Info : no Goal in this level."); } //do not cancel the loading, just inform that no goal is present.

            try { LevelProperties.SoundtrackPath = doc.Element("GeneralSettings").Element("Sound").Element("SoundtrackPath").Value.ToString(); }
            catch { resultMessage = "Could not find the music path."; goto cancelLoad; }

            try { LevelProperties.Weather = doc.Element("GeneralSettings").Element("Weather").Value; }
            catch { resultMessage = "No weather set."; goto cancelLoad; }

            LevelProperties.NZones = Convert.ToInt32(doc.Element("GeneralSettings").Element("NZones").Value);
            LevelProperties.NBacks = Convert.ToInt32(doc.Element("GeneralSettings").Element("NBacks").Value);
            LevelProperties.BackSpeed = Convert.ToInt32(doc.Element("GeneralSettings").Element("BackSpeed").Value);
            LevelProperties.NYears = Convert.ToInt32(doc.Element("GeneralSettings").Element("NYears").Value);
            LevelProperties.TTS = Convert.ToInt32(doc.Element("GeneralSettings").Element("NYears").Value);
            #endregion

            #region BaseCollisions
            XElement Collisions = doc.Element("BaseCollisions");

            foreach (XElement circle in Collisions.Element("Circles").Descendants("Circle"))
            {
                LevelProperties.Collisions.CircleList.Add(new ObjCircle(new Vector2((float)Convert.ToDouble(circle.Attribute("X").Value),
                                                                                    (float)Convert.ToDouble(circle.Attribute("Y").Value)),
                                                                                    (float)Convert.ToDouble(circle.Attribute("radius").Value), 0f)
                { UserData = circle.Element("CollisionType").Value, Color = MainForm.setCollisionColor(circle.Element("CollisionType").Value) });
            }
            foreach (XElement rect in Collisions.Element("Rectangles").Descendants("Rectangle"))
            {
                LevelProperties.Collisions.RectList.Add(new ObjRectangle(new Vector2((float)Convert.ToDouble(rect.Attribute("X").Value), 
                                                                                     (float)Convert.ToDouble(rect.Attribute("Y").Value)),
                                                                                     (float)Convert.ToDouble(rect.Attribute("width").Value),
                                                                                     (float)Convert.ToDouble(rect.Attribute("height").Value),
                                                                                     (float)Convert.ToDouble(rect.Attribute("angle").Value))
                { UserData = rect.Element("CollisionType").Value, Color = MainForm.setCollisionColor(rect.Element("CollisionType").Value) });
            }
            foreach (XElement poly in Collisions.Element("Polygons").Descendants("Polygon"))
            {
                List<Vector2> vertlist = new List<Vector2>();
                foreach (XElement v in poly.Element("Vertices").Descendants("vert"))
                {
                    Console.WriteLine("\"" + v.Attribute("X").Value + "\"   \"" + v.Attribute("Y").Value + "\"");
                    vertlist.Add(new Vector2(StringToFloat(v.Attribute("X").Value), StringToFloat(v.Attribute("Y").Value)));
                }
                LevelProperties.Collisions.PolyList.Add(new ObjPolygon(vertlist)
                { UserData = poly.Element("CollisionType").Value, Color = MainForm.setCollisionColor(poly.Element("CollisionType").Value) });
            }

            foreach (XElement edge in Collisions.Element("Edges").Descendants("Edge"))
            {
                LevelProperties.Collisions.EdgeList.Add(new ObjEdge(new Vector2((float)Convert.ToDouble(edge.Element("Vertices").Element("WP1").Attribute("X").Value),
                                                                                (float)Convert.ToDouble(edge.Element("Vertices").Element("WP1").Attribute("Y").Value)),
                                                                    new Vector2((float)Convert.ToDouble(edge.Element("Vertices").Element("WP2").Attribute("X").Value),
                                                                                (float)Convert.ToDouble(edge.Element("Vertices").Element("WP2").Attribute("Y").Value)))
                { UserData = edge.Element("CollisionType").Value, Color = MainForm.setCollisionColor(edge.Element("CollisionType").Value) });
            }
            foreach (XElement ec in Collisions.Element("EdgeChains").Descendants("EdgeChain"))
            {
                List<Vector2> vertlist = new List<Vector2>();
                foreach (XElement edge in ec.Element("Vertices").Descendants("vert"))
                {
                    vertlist.Add(new Vector2((float)Convert.ToDouble(edge.Attribute("X").Value),
                                             (float)Convert.ToDouble(edge.Attribute("Y").Value)));
                }

                LevelProperties.Collisions.EdgeChainList.Add(new ObjEdgeChain(vertlist)
                { UserData = ec.Element("CollisionType").Value, EdgesColor = MainForm.setCollisionColor(ec.Element("CollisionType").Value) });
            }
            #endregion

            #region Scenarios
            LevelProperties.Scenarios.Clear();
            foreach (XElement sc in doc.Element("Scenarios").Descendants("Scenario"))
            {
                LevelProperties.Scenarios.Add(new ScenarioProperties(Convert.ToInt32(sc.Element("StartTime").Value), Convert.ToInt32(sc.Element("EndTime").Value)));

                #region Zones
                //zones (traps, decos, items)
                foreach (XElement zone in sc.Element("Zones").Descendants("Zone"))
                {
                    lsc.Zones.Add(new ZoneProperties());
                    //Traps
                    foreach (XElement trap in zone.Element("Traps").Descendants("Trap"))
                    {
                        Vector2 position = new Vector2(StringToFloat(trap.Element("Position").Attribute("X").Value),
                                                       StringToFloat(trap.Element("Position").Attribute("Y").Value));
                        
                        System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Traps/" 
                                                                          + trap.Attribute("id").Value + "/" + trap.Attribute("id").Value + "Thumbnail.png");

                        SpriteEffects inverted;
                        if (Convert.ToBoolean(trap.Element("Direction").Attribute("inverted").Value))
                            inverted = SpriteEffects.FlipHorizontally;
                        else inverted = SpriteEffects.None;

                        lsc.Zones.Last().PassiveTraps.Add(new PassiveTrapsProperties(trap.Attribute("id").Value,
                                                                       position,
                                                                       100,
                                                                       new ObjRectangle(position, ConvertUnits.ToSimUnits(thumbnail.Width),
                                                                                                  ConvertUnits.ToSimUnits(thumbnail.Height), 0f)
                                                                       { Color = Color.Orange })
                        { Angle = StringToFloat(trap.Element("Rotation").Value), Direction = inverted });
                        lsc.Zones.Last().PassiveTraps.Last().thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice);

                        #region Trap's IAAux
                        lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded?.Show();
                        switch (lsc.Zones.Last().PassiveTraps.Last().Instance.NeededInfos)
                        {
                            case IAActivatedObject.SecondaryInfos.None:
                                //nothing
                                break;
                            case IAActivatedObject.SecondaryInfos.DualWaypoints:
                                break;
                            case IAActivatedObject.SecondaryInfos.Radius:
                                ((IAAuxiliary.Radius)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).radius 
                                     = (float)Convert.ToDouble(trap.Element("IAAux").Element("Radius").Value);
                                break;
                            case IAActivatedObject.SecondaryInfos.Path:
                                break;
                            case IAActivatedObject.SecondaryInfos.ActivationZone:
                                ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody?.Delete();

                                switch (trap.Element("IAAux").Element("Object").Attribute("type").Value)
                                {
                                    case "Circle":
                                        XElement c = trap.Element("IAAux").Element("Object").Element("Circle");

                                        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                            new ObjCircle(new Vector2((float)Convert.ToDouble(c.Attribute("X").Value),
                                                                      (float)Convert.ToDouble(c.Attribute("Y").Value)),
                                                         (float)Convert.ToDouble(c.Attribute("radius").Value), 0f)
                                            { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                        break;
                                    case "Rectangle":
                                        XElement r = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                            new ObjRectangle(new Vector2(StringToFloat(r.Attribute("X").Value),
                                                                         StringToFloat(r.Attribute("Y").Value)),
                                                             StringToFloat(r.Attribute("width").Value),
                                                             StringToFloat(r.Attribute("height").Value),
                                                             StringToFloat(r.Attribute("angle").Value))
                                            { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                        break;
                                    case "Polygon":
                                        { // isolate the variables scope
                                            XElement p = trap.Element("IAAux").Element("Object").Element("Polygon");

                                            List<Vector2> vertlist = new List<Vector2>();
                                            foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                                vertlist.Add(new Vector2(StringToFloat(v.Attribute("X").Value),
                                                                         StringToFloat(v.Attribute("Y").Value)));

                                            ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                                new ObjPolygon(vertlist) { UserData = "IAAux", Color = IAAux.IAAuxColor,
                                                                           Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                        }
                                        break;
                                        //case "Edge":
                                        //    XElement e = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                        //    ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                        //        new ObjEdge(new Vector2((float)Convert.ToDouble(e.Element("WP1").Attribute("X").Value),
                                        //                                (float)Convert.ToDouble(e.Element("WP1").Attribute("Y").Value)),
                                        //                    new Vector2((float)Convert.ToDouble(e.Element("WP2").Attribute("X").Value),
                                        //                                (float)Convert.ToDouble(e.Element("WP2").Attribute("Y").Value)))
                                        //        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                        //    break;
                                        //case "EdgeChain":
                                        //    { //isolate the variables scope
                                        //        XElement p = trap.Element("IAAux").Element("Object").Element("EdgeChain");

                                        //        List<Vector2> vertlist = new List<Vector2>();
                                        //        foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                        //            vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                        //                                     (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                        //        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                        //            new ObjEdgeChain(vertlist, "IAAux") { UserData = "IAAux", EdgesColor = IAAux.IAAuxColor
                                         //                                                    , Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded};
                                        //    }
                                        //    break;
                                }
                                break;
                            case IAActivatedObject.SecondaryInfos.SpawnPoint:
                                break;
                            default:
                                break;
                        }
                        lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded?.Hide();
                        #endregion
                    }

                    //Decos
                    foreach (XElement deco in zone.Element("Decorations").Descendants("Decoration"))
                    {
                        Vector2 position = new Vector2((float)Convert.ToDouble(deco.Element("Position").Attribute("X").Value),
                                                       (float)Convert.ToDouble(deco.Element("Position").Attribute("Y").Value));

                        SpriteEffects inverted;
                        if (Convert.ToBoolean(deco.Element("Direction").Attribute("inverted").Value))
                            inverted = SpriteEffects.FlipHorizontally;
                        else inverted = SpriteEffects.None;

                        System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Decos/" + deco.Attribute("id").Value + ".png");

                        lsc.Zones.Last().Decoration.Add(
                            new DecorationProperties(deco.Attribute("id").Value,
                                                     position,
                                                     new ObjRectangle(position, ConvertUnits.ToSimUnits(thumbnail.Width), ConvertUnits.ToSimUnits(thumbnail.Width),
                                                                      (float)Convert.ToDouble(deco.Element("Rotation").Value)) { Color = Color.AliceBlue },
                                                     0)
                                                     { Angle = (float)Convert.ToDouble(deco.Element("Rotation").Value), Direction = inverted });
                        lsc.Zones.Last().Decoration.Last().thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice);
                    }
                    //Items
                    foreach (XElement item in zone.Element("Items").Descendants("Item"))
                    {
                        Vector2 position = new Vector2((float)Convert.ToDouble(item.Element("Position").Attribute("X").Value),
                                                       (float)Convert.ToDouble(item.Element("Position").Attribute("Y").Value));

                        System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Decos/" + item.Attribute("id").Value + ".png");

                        lsc.Zones.Last().Items.Add(new ItemProperties(item.Attribute("id").Value,
                            position,
                            ConvertUnits.ToSimUnits(thumbnail.Width), ConvertUnits.ToSimUnits(thumbnail.Width),
                                             Color.GreenYellow));
                        lsc.Zones.Last().Items.Last().thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice);
                    }
                }
                #endregion

                #region Enemies
                //Enemies
                foreach (XElement e in sc.Element("Enemies").Descendants("Enemy"))
                {
                    Vector2 position = new Vector2(StringToFloat(e.Element("Position").Attribute("X").Value),
                                                   StringToFloat(e.Element("Position").Attribute("Y").Value));

                    System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Enemies/" + e.Attribute("id").Value 
                                                                                                + "/" + e.Attribute("id").Value + "Thumbnail.png");

                    SpriteEffects inverted;
                    if (Convert.ToBoolean(e.Element("Direction").Attribute("inverted").Value))
                        inverted = SpriteEffects.FlipHorizontally;
                    else inverted = SpriteEffects.None;

                    lsc.Enemies.Add(new EnemyProperties(e.Attribute("id").Value, position, 0,
                                                        new ObjRectangle(position, ConvertUnits.ToSimUnits(thumbnail.Width),
                                                                                   ConvertUnits.ToSimUnits(thumbnail.Height),
                                                                         0f) { Color = Color.Red })
                    { Age = Convert.ToInt32(e.Element("Age").Value), thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice), Direction = inverted });


                    #region Enemy's IAAux
                    lsc.Enemies.Last().IAAuxNeeded?.Show();
                    switch (lsc.Enemies.Last().Instance.NeededInfos)
                    {
                        case IAActivatedObject.SecondaryInfos.None:
                            //nothing
                            break;
                        case IAActivatedObject.SecondaryInfos.DualWaypoints:
                            ((DualWaypoints)lsc.Enemies.Last().IAAuxNeeded).WP1Position 
                                = new Vector2(StringToFloat(e.Element("IAAux").Element("WP1").Attribute("X").Value),
                                              StringToFloat(e.Element("IAAux").Element("WP1").Attribute("Y").Value));

                            ((DualWaypoints)lsc.Enemies.Last().IAAuxNeeded).WP2Position
                                = new Vector2(StringToFloat(e.Element("IAAux").Element("WP2").Attribute("X").Value),
                                              StringToFloat(e.Element("IAAux").Element("WP2").Attribute("Y").Value));
                            break;
                        case IAActivatedObject.SecondaryInfos.Radius:
                            ((IAAuxiliary.Radius)lsc.Enemies.Last().IAAuxNeeded).radius
                                 = (float)Convert.ToDouble(e.Element("IAAux").Element("Radius").Value);
                            break;
                        case IAActivatedObject.SecondaryInfos.Path:
                            {
                                ((IAAuxiliary.IAPath)lsc.Enemies.Last().IAAuxNeeded).PathObject.Delete();

                                List<Vector2> vertlist = new List<Vector2>();
                                foreach (XElement v in e.Element("IAAux").Element("Vertices").Descendants("vert"))
                                {
                                    vertlist.Add(new Vector2(StringToFloat(v.Attribute("X").Value),
                                                             StringToFloat(v.Attribute("Y").Value)));
                                }

                                ((IAAuxiliary.IAPath)lsc.Enemies.Last().IAAuxNeeded).PathObject =
                                    new ObjEdgeChain(vertlist) { userData = "IAAux", EdgesColor = IAAux.IAAuxColor, Parent = lsc.Enemies.Last().IAAuxNeeded };
                            }
                            break;
                        case IAActivatedObject.SecondaryInfos.ActivationZone:
                            ((IAAuxiliary.ActivationZone)lsc.Enemies.Last().IAAuxNeeded).ActivationBody?.Delete();

                            switch (e.Element("IAAux").Element("Object").Attribute("type").Value)
                            {
                                case "Circle":
                                    XElement c = e.Element("IAAux").Element("Object").Element("Circle");

                                    ((IAAuxiliary.ActivationZone)lsc.Enemies.Last().IAAuxNeeded).ActivationBody =
                                        new ObjCircle(new Vector2((float)Convert.ToDouble(c.Attribute("X").Value),
                                                                  (float)Convert.ToDouble(c.Attribute("Y").Value)),
                                                     (float)Convert.ToDouble(c.Attribute("radius").Value), 0f)
                                        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Enemies.Last().IAAuxNeeded };
                                    break;
                                case "Rectangle":
                                    XElement r = e.Element("IAAux").Element("Object").Element("Rectangle");

                                    ((IAAuxiliary.ActivationZone)lsc.Enemies.Last().IAAuxNeeded).ActivationBody =
                                        new ObjRectangle(new Vector2(StringToFloat(r.Attribute("X").Value),
                                                                     StringToFloat(r.Attribute("Y").Value)),
                                                         StringToFloat(r.Attribute("width").Value),
                                                         StringToFloat(r.Attribute("height").Value),
                                                         StringToFloat(r.Attribute("angle").Value))
                                        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Enemies.Last().IAAuxNeeded };
                                    break;
                                case "Polygon":
                                    { // isolate the variables scope
                                        {
                                            XElement p = e.Element("IAAux").Element("Object").Element("Polygon");

                                            List<Vector2> vertlist = new List<Vector2>();
                                            foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                                vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                                                         (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                            ((IAAuxiliary.ActivationZone)lsc.Enemies.Last().IAAuxNeeded).ActivationBody =
                                                new ObjPolygon(vertlist)
                                                {
                                                    UserData = "IAAux",
                                                    Color = IAAux.IAAuxColor,
                                                    Parent = lsc.Enemies.Last().IAAuxNeeded
                                                };
                                        }
                                    }
                                    break;
                                    //case "Edge":
                                    //    XElement e = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                    //    ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                    //        new ObjEdge(new Vector2((float)Convert.ToDouble(e.Element("WP1").Attribute("X").Value),
                                    //                                (float)Convert.ToDouble(e.Element("WP1").Attribute("Y").Value)),
                                    //                    new Vector2((float)Convert.ToDouble(e.Element("WP2").Attribute("X").Value),
                                    //                                (float)Convert.ToDouble(e.Element("WP2").Attribute("Y").Value)))
                                    //        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                    //    break;
                                    //case "EdgeChain":
                                    //    { //isolate the variables scope
                                    //        XElement p = trap.Element("IAAux").Element("Object").Element("EdgeChain");

                                    //        List<Vector2> vertlist = new List<Vector2>();
                                    //        foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                    //            vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                    //                                     (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                    //        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                    //            new ObjEdgeChain(vertlist, "IAAux") { UserData = "IAAux", EdgesColor = IAAux.IAAuxColor
                                    //                                                    , Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded};
                                    //    }
                                    //    break;
                            }
                            break;
                        case IAActivatedObject.SecondaryInfos.SpawnPoint:
                            break;
                        default:
                            break;
                    }
                    lsc.Enemies.Last().IAAuxNeeded?.Hide();
                    #endregion
                }
                #endregion

                #region
                //Sectors
                foreach (XElement sector in sc.Element("Sectors").Descendants("Sector"))
                {

                    ObjectClass obj = new ObjectClass();
                    switch (sector.Element("CollisionObject").Attribute("type").Value)
                    {
                        case "Circle":
                            XElement c = sector.Element("CollisionObject").Element("Circle");

                            obj = new ObjCircle(new Vector2((float)Convert.ToDouble(c.Attribute("X").Value),
                                                            (float)Convert.ToDouble(c.Attribute("Y").Value)),
                                                (float)Convert.ToDouble(c.Attribute("radius").Value), 0f);
                            break;
                        case "Rectangle":
                            XElement r = sector.Element("CollisionObject").Element("Rectangle");

                            obj =
                                new ObjRectangle(new Vector2((float)Convert.ToDouble(r.Attribute("X").Value),
                                                             (float)Convert.ToDouble(r.Attribute("Y").Value)),
                                                 (float)Convert.ToDouble(r.Attribute("width").Value),
                                                 (float)Convert.ToDouble(r.Attribute("height").Value),
                                                 (float)Convert.ToDouble(r.Attribute("angle").Value));
                            break;
                        case "Polygon":
                            { // isolate the variables scope
                                {
                                    XElement p = sector.Element("CollisionObject").Element("Polygon");

                                    List<Vector2> vertlist = new List<Vector2>();
                                    foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                        vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                                                 (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                    obj = new ObjPolygon(vertlist);
                                }
                            }
                            break;
                        case "Edge":
                            XElement e = sector.Element("CollisionObject").Element("Edge");

                            obj =
                                new ObjEdge(new Vector2((float)Convert.ToDouble(e.Element("Vertices").Element("WP1").Attribute("X").Value),
                                                        (float)Convert.ToDouble(e.Element("Vertices").Element("WP1").Attribute("Y").Value)),
                                            new Vector2((float)Convert.ToDouble(e.Element("Vertices").Element("WP2").Attribute("X").Value),
                                                        (float)Convert.ToDouble(e.Element("Vertices").Element("WP2").Attribute("Y").Value)));
                            break;
                        case "EdgeChain":
                            { //isolate the variables scope
                                XElement p = sector.Element("CollisionObject").Element("EdgeChain");

                                List<Vector2> vertlist = new List<Vector2>();
                                foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                    vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                                             (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                obj = new ObjEdgeChain(vertlist);
                            }
                            break;
                        default:
                            resultMessage = "Invalid Sector collision object.";
                            goto cancelLoad;
                    }

                    List<string> effects = new List<string>();
                    foreach (XElement effect in sector.Element("Effects").Descendants("Effect"))
                    {
                        effects.Add(effect.Value);
                    }

                    lsc.Sectors.Add(new Sector(obj, effects));
                }
                #endregion

            }
            #endregion

            #region TravelEntities

            #region TravelEnemies
            foreach (XElement enemy in doc.Element("TravelEntities").Element("TravelEnemies").Descendants("Enemy"))
            {
                //Vector2 position = new Vector2((float)Convert.ToDouble(enemy.Element("Position").Attribute("X").Value),
                //                             (float)Convert.ToDouble(enemy.Element("Position").Attribute("Y").Value));

                Vector2 position = new Vector2(StringToFloat(enemy.Element("Position").Attribute("X").Value),
                                               StringToFloat(enemy.Element("Position").Attribute("Y").Value));


                System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Enemies/" + enemy.Attribute("id").Value
                                                                                            + "/" + enemy.Attribute("id").Value + "Thumbnail.png");

                SpriteEffects inverted;
                if (Convert.ToBoolean(enemy.Element("Direction").Attribute("inverted").Value))
                    inverted = SpriteEffects.FlipHorizontally;
                else inverted = SpriteEffects.None;

                LevelProperties.TravelEnemies.Add(new EnemyProperties(enemy.Attribute("id").Value, position, Convert.ToInt32(enemy.Element("SpawnDate").Value),
                                                    new ObjRectangle(position, ConvertUnits.ToSimUnits(thumbnail.Width),
                                                                               ConvertUnits.ToSimUnits(thumbnail.Height),
                                                                     0f)
                                                    { Color = Color.Red })
                { thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice), Direction = inverted });


                #region Enemy's IAAux
                LevelProperties.TravelEnemies.Last().IAAuxNeeded?.Show();
                switch (LevelProperties.TravelEnemies.Last().Instance.NeededInfos)
                {
                    case IAActivatedObject.SecondaryInfos.None:
                        //nothing
                        break;
                    case IAActivatedObject.SecondaryInfos.DualWaypoints:
                        ((DualWaypoints)LevelProperties.TravelEnemies.Last().IAAuxNeeded).WP1Position
                              //= new Vector2((float)Convert.ToDouble(enemy.Element("IAAux").Element("WP1").Attribute("X").Value),
                              //            (float)Convert.ToDouble(enemy.Element("IAAux").Element("WP1").Attribute("Y").Value));
                              = new Vector2(StringToFloat(enemy.Element("IAAux").Element("WP1").Attribute("X").Value),
                                            StringToFloat(enemy.Element("IAAux").Element("WP1").Attribute("Y").Value));

                        ((DualWaypoints)LevelProperties.TravelEnemies.Last().IAAuxNeeded).WP2Position
                            = new Vector2(StringToFloat(enemy.Element("IAAux").Element("WP2").Attribute("X").Value),
                                          StringToFloat(enemy.Element("IAAux").Element("WP2").Attribute("Y").Value));
                        break;
                    case IAActivatedObject.SecondaryInfos.Radius:
                        ((IAAuxiliary.Radius)LevelProperties.TravelEnemies.Last().IAAuxNeeded).radius
                             = StringToFloat(enemy.Element("IAAux").Element("Radius").Value);
                        break;
                    case IAActivatedObject.SecondaryInfos.Path:
                        {
                            ((IAAuxiliary.IAPath)LevelProperties.TravelEnemies.Last().IAAuxNeeded).PathObject.Delete();

                            List<Vector2> vertlist = new List<Vector2>();
                            foreach (XElement v in enemy.Element("IAAux").Element("Vertices").Descendants("vert"))
                            {
                                vertlist.Add(new Vector2(StringToFloat(v.Attribute("X").Value), StringToFloat(v.Attribute("Y").Value)));
                            }

                            ((IAAuxiliary.IAPath)LevelProperties.TravelEnemies.Last().IAAuxNeeded).PathObject =
                                new ObjEdgeChain(vertlist) { userData = "IAAux", EdgesColor = IAAux.IAAuxColor, Parent = LevelProperties.TravelEnemies.Last().IAAuxNeeded };
                        }
                        break;
                    case IAActivatedObject.SecondaryInfos.ActivationZone:
                        ((IAAuxiliary.ActivationZone)LevelProperties.TravelEnemies.Last().IAAuxNeeded).ActivationBody?.Delete();

                        switch (enemy.Element("IAAux").Element("Object").Attribute("type").Value)
                        {
                            case "Circle":
                                XElement c = enemy.Element("IAAux").Element("Object").Element("Circle");

                                ((IAAuxiliary.ActivationZone)LevelProperties.TravelEnemies.Last().IAAuxNeeded).ActivationBody =
                                    new ObjCircle(new Vector2((float)Convert.ToDouble(c.Attribute("X").Value),
                                                              (float)Convert.ToDouble(c.Attribute("Y").Value)),
                                                 (float)Convert.ToDouble(c.Attribute("radius").Value), 0f)
                                    { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = LevelProperties.TravelEnemies.Last().IAAuxNeeded };
                                break;
                            case "Rectangle":
                                XElement r = enemy.Element("IAAux").Element("Object").Element("Rectangle");

                                ((IAAuxiliary.ActivationZone)LevelProperties.TravelEnemies.Last().IAAuxNeeded).ActivationBody =
                                    new ObjRectangle(new Vector2(StringToFloat(r.Attribute("X").Value),
                                                                 StringToFloat(r.Attribute("Y").Value)),
                                                     StringToFloat(r.Attribute("width").Value),
                                                     StringToFloat(r.Attribute("height").Value),
                                                     StringToFloat(r.Attribute("angle").Value))
                                    { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = LevelProperties.TravelEnemies.Last().IAAuxNeeded };
                                break;
                            case "Polygon":
                                { // isolate the variables scope
                                    {
                                        XElement p = enemy.Element("IAAux").Element("Object").Element("Polygon");

                                        List<Vector2> vertlist = new List<Vector2>();
                                        foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                            vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                                                     (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                        ((IAAuxiliary.ActivationZone)LevelProperties.TravelEnemies.Last().IAAuxNeeded).ActivationBody =
                                            new ObjPolygon(vertlist)
                                            {
                                                UserData = "IAAux",
                                                Color = IAAux.IAAuxColor,
                                                Parent = LevelProperties.TravelEnemies.Last().IAAuxNeeded
                                            };
                                    }
                                }
                                break;
                                //case "Edge":
                                //    XElement e = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                //    ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                //        new ObjEdge(new Vector2((float)Convert.ToDouble(e.Element("WP1").Attribute("X").Value),
                                //                                (float)Convert.ToDouble(e.Element("WP1").Attribute("Y").Value)),
                                //                    new Vector2((float)Convert.ToDouble(e.Element("WP2").Attribute("X").Value),
                                //                                (float)Convert.ToDouble(e.Element("WP2").Attribute("Y").Value)))
                                //        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                //    break;
                                //case "EdgeChain":
                                //    { //isolate the variables scope
                                //        XElement p = trap.Element("IAAux").Element("Object").Element("EdgeChain");

                                //        List<Vector2> vertlist = new List<Vector2>();
                                //        foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                //            vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                //                                     (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                //        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                //            new ObjEdgeChain(vertlist, "IAAux") { UserData = "IAAux", EdgesColor = IAAux.IAAuxColor
                                //                                                    , Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded};
                                //    }
                                //    break;
                        }
                        break;
                    case IAActivatedObject.SecondaryInfos.SpawnPoint:
                        break;
                    default:
                        break;
                }
                LevelProperties.TravelEnemies.Last().IAAuxNeeded?.Hide();
                #endregion
            }
            #endregion

            #region TravelTraps
            foreach (XElement trap in doc.Element("TravelEntities").Element("TravelTraps").Descendants("Trap"))
            {
                Vector2 position = new Vector2((float)Convert.ToDouble(trap.Element("Position").Attribute("X").Value),
                                                       (float)Convert.ToDouble(trap.Element("Position").Attribute("Y").Value));

                System.Drawing.Image thumbnail = System.Drawing.Image.FromFile("Content/Entities/Traps/"
                                                                  + trap.Attribute("id").Value + "/" + trap.Attribute("id").Value + "Thumbnail.png");

                SpriteEffects inverted;
                if (Convert.ToBoolean(trap.Element("Direction").Attribute("inverted").Value))
                    inverted = SpriteEffects.FlipHorizontally;
                else inverted = SpriteEffects.None;

                LevelProperties.TravelTraps.Add(new PassiveTrapsProperties(trap.Attribute("id").Value,
                                                               position,
                                                               Convert.ToInt32(trap.Element("SpawnDate").Value),
                                                               new ObjRectangle(position, ConvertUnits.ToSimUnits(thumbnail.Width),
                                                                                          ConvertUnits.ToSimUnits(thumbnail.Height), 0f)
                                                               { Color = Color.Orange })
                {  Direction = inverted });
                LevelProperties.TravelTraps.Last().thumbnail = Helpers.ImageToTexture2D(thumbnail, Game1.graphicsDevice);

                #region Trap's IAAux
                LevelProperties.TravelTraps.Last().IAAuxNeeded?.Show();
                switch (LevelProperties.TravelTraps.Last().Instance.NeededInfos)
                {
                    case IAActivatedObject.SecondaryInfos.None:
                        //nothing
                        break;
                    case IAActivatedObject.SecondaryInfos.DualWaypoints:
                        break;
                    case IAActivatedObject.SecondaryInfos.Radius:
                        ((IAAuxiliary.Radius)LevelProperties.TravelTraps.Last().IAAuxNeeded).radius
                             = (float)Convert.ToDouble(trap.Element("IAAux").Element("Radius").Value);
                        break;
                    case IAActivatedObject.SecondaryInfos.Path:
                        break;
                    case IAActivatedObject.SecondaryInfos.ActivationZone:
                        ((IAAuxiliary.ActivationZone)LevelProperties.TravelTraps.Last().IAAuxNeeded).ActivationBody?.Delete();

                        switch (trap.Element("IAAux").Element("Object").Attribute("type").Value)
                        {
                            case "Circle":
                                XElement c = trap.Element("IAAux").Element("Object").Element("Circle");

                                ((IAAuxiliary.ActivationZone)LevelProperties.TravelTraps.Last().IAAuxNeeded).ActivationBody =
                                    new ObjCircle(new Vector2((float)Convert.ToDouble(c.Attribute("X").Value),
                                                              (float)Convert.ToDouble(c.Attribute("Y").Value)),
                                                 (float)Convert.ToDouble(c.Attribute("radius").Value), 0f)
                                    { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = LevelProperties.TravelTraps.Last().IAAuxNeeded };
                                break;
                            case "Rectangle":
                                XElement r = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                ((IAAuxiliary.ActivationZone)LevelProperties.TravelTraps.Last().IAAuxNeeded).ActivationBody =
                                    new ObjRectangle(new Vector2((float)Convert.ToDouble(r.Attribute("X").Value),
                                                                 (float)Convert.ToDouble(r.Attribute("Y").Value)),
                                                     (float)Convert.ToDouble(r.Attribute("width").Value),
                                                     (float)Convert.ToDouble(r.Attribute("height").Value),
                                                     (float)Convert.ToDouble(r.Attribute("angle").Value))
                                    { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = LevelProperties.TravelTraps.Last().IAAuxNeeded };
                                break;
                            case "Polygon":
                                { // isolate the variables scope
                                    XElement p = trap.Element("IAAux").Element("Object").Element("Polygon");

                                    List<Vector2> vertlist = new List<Vector2>();
                                    foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                        vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                                                 (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                    ((IAAuxiliary.ActivationZone)LevelProperties.TravelTraps.Last().IAAuxNeeded).ActivationBody =
                                        new ObjPolygon(vertlist)
                                        {
                                            UserData = "IAAux",
                                            Color = IAAux.IAAuxColor,
                                            Parent = LevelProperties.TravelTraps.Last().IAAuxNeeded
                                        };
                                }
                                break;
                                //case "Edge":
                                //    XElement e = trap.Element("IAAux").Element("Object").Element("Rectangle");

                                //    ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                //        new ObjEdge(new Vector2((float)Convert.ToDouble(e.Element("WP1").Attribute("X").Value),
                                //                                (float)Convert.ToDouble(e.Element("WP1").Attribute("Y").Value)),
                                //                    new Vector2((float)Convert.ToDouble(e.Element("WP2").Attribute("X").Value),
                                //                                (float)Convert.ToDouble(e.Element("WP2").Attribute("Y").Value)))
                                //        { UserData = "IAAux", Color = IAAux.IAAuxColor, Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded };
                                //    break;
                                //case "EdgeChain":
                                //    { //isolate the variables scope
                                //        XElement p = trap.Element("IAAux").Element("Object").Element("EdgeChain");

                                //        List<Vector2> vertlist = new List<Vector2>();
                                //        foreach (XElement v in p.Element("Vertices").Descendants("vert"))
                                //            vertlist.Add(new Vector2((float)Convert.ToDouble(v.Attribute("X").Value),
                                //                                     (float)Convert.ToDouble(v.Attribute("Y").Value)));

                                //        ((IAAuxiliary.ActivationZone)lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded).ActivationBody =
                                //            new ObjEdgeChain(vertlist, "IAAux") { UserData = "IAAux", EdgesColor = IAAux.IAAuxColor
                                //                                                    , Parent = lsc.Zones.Last().PassiveTraps.Last().IAAuxNeeded};
                                //    }
                                //    break;
                        }
                        break;
                    case IAActivatedObject.SecondaryInfos.SpawnPoint:
                        break;
                    default:
                        break;
                }
                LevelProperties.TravelTraps.Last().IAAuxNeeded?.Hide();
                #endregion
            }
            #endregion

            #endregion

            #region TCCs
            LevelProperties.TCCs.Clear();
            foreach (XElement TCC in doc.Element("TCCs").Descendants("TCC"))
            {
                LevelProperties.TCCs.Add(new TCCProperties());
                LevelProperties.TCCs.Last().IsSafeZone = TCC.ToString().Contains("<IsSafeZone />");
                LevelProperties.TCCs.Last().TCCName = TCC.Attribute("name").Value;

                foreach (XElement state in TCC.Element("States").Descendants("State"))
                {
                    LevelProperties.TCCs.Last().States.Add(new TCCState());
                    LevelProperties.TCCs.Last().States.Last().IsActiveState = state.ToString().Contains("<IsActiveState />");

                    if(state.Element("Time").Element("TimeType").Value == "Precision")
                         LevelProperties.TCCs.Last().States.Last().TimeType = TCCState.TimeTypeEnum.Precision;
                    else LevelProperties.TCCs.Last().States.Last().TimeType = TCCState.TimeTypeEnum.Scenario;
                    LevelProperties.TCCs.Last().States.Last().startTime = Convert.ToInt32(state.Element("Time").Element("StartTime").Value);
                    LevelProperties.TCCs.Last().States.Last().endTime   = Convert.ToInt32(state.Element("Time").Element("EndTime").Value);

                    if(state.Element("Image").Element("Path").Value != "None")
                    {
                        LevelProperties.TCCs.Last().States.Last().ImagePath = state.Element("Image").Element("Path").Value;
                        LevelProperties.TCCs.Last().States.Last().ImagePosition = new Vector2((float)Convert.ToDouble(state.Element("Image").Element("Position").Attribute("X").Value),
                                                                                              (float)Convert.ToDouble(state.Element("Image").Element("Position").Attribute("Y").Value));
                        LevelProperties.TCCs.Last().States.Last().Image 
                            = Helpers.ImageToTexture2D(System.Drawing.Image.FromFile(EditorVariables.ContentBasePath + LevelProperties.TCCs.Last().States.Last().ImagePath), Game1.graphicsDevice);
                    }


                    switch (state.Element("CollisionObject").Attribute("type").Value)
                    {
                        case "Circle":
                            XElement circle = state.Element("CollisionObject").Element("Circle");

                            LevelProperties.TCCs.Last().States.Last().CollisionObject
                                                                    = new ObjCircle(new Vector2((float)Convert.ToDouble(circle.Attribute("X").Value),
                                                                                    (float)Convert.ToDouble(circle.Attribute("Y").Value)),
                                                                                    (float)Convert.ToDouble(circle.Attribute("radius").Value), 0f)
                                                                    { UserData = circle.Element("CollisionType").Value, Color = MainForm.setCollisionColor(circle.Element("CollisionType").Value) };
                            break;
                        case "Rectangle":
                            XElement rect = state.Element("CollisionObject").Element("Rectangle");

                            LevelProperties.TCCs.Last().States.Last().CollisionObject = new ObjRectangle(new Vector2(StringToFloat(rect.Attribute("X").Value),
                                                                                     StringToFloat(rect.Attribute("Y").Value)),
                                                                                     StringToFloat(rect.Attribute("width").Value),
                                                                                     StringToFloat(rect.Attribute("height").Value),
                                                                                     StringToFloat(rect.Attribute("angle").Value))
                            { UserData = rect.Element("CollisionType").Value, Color = MainForm.setCollisionColor(rect.Element("CollisionType").Value) };
                            break;
                        case "Polygon":
                            {
                                XElement poly = state.Element("CollisionObject").Element("Polygon");

                                List<Vector2> vertlist = new List<Vector2>();
                                foreach (XElement v in poly.Element("Vertices").Descendants("vert"))
                                {
                                    vertlist.Add(new Vector2(StringToFloat(v.Attribute("X").Value), StringToFloat(v.Attribute("Y").Value)));
                                }
                                LevelProperties.TCCs.Last().States.Last().CollisionObject = new ObjPolygon(vertlist)
                                { UserData = poly.Element("CollisionType").Value, Color = MainForm.setCollisionColor(poly.Element("CollisionType").Value) };
                            }
                            break;
                        case "Edge":
                            {
                                XElement edge = state.Element("CollisionObject").Element("Edge");

                                LevelProperties.TCCs.Last().States.Last().CollisionObject = new ObjEdge(new Vector2((float)Convert.ToDouble(edge.Element("Vertices").Element("WP1").Attribute("X").Value),
                                                                                    (float)Convert.ToDouble(edge.Element("Vertices").Element("WP1").Attribute("Y").Value)),
                                                                        new Vector2((float)Convert.ToDouble(edge.Element("Vertices").Element("WP2").Attribute("X").Value),
                                                                                    (float)Convert.ToDouble(edge.Element("Vertices").Element("WP2").Attribute("Y").Value)))
                                { UserData = edge.Element("CollisionType").Value, Color = MainForm.setCollisionColor(edge.Element("CollisionType").Value) };
                            }
                            break;
                        case "EdgeChain":
                            {
                                XElement ec = state.Element("CollisionObject").Element("EdgeChain");

                                List<Vector2> vertlist = new List<Vector2>();
                                foreach (XElement edge in ec.Element("Vertices").Descendants("vert"))
                                {
                                    vertlist.Add(new Vector2(StringToFloat(edge.Attribute("X").Value),
                                                             StringToFloat(edge.Attribute("Y").Value)));
                                }

                                LevelProperties.TCCs.Last().States.Last().CollisionObject = new ObjEdgeChain(vertlist)
                                { UserData = ec.Element("CollisionType").Value, EdgesColor = MainForm.setCollisionColor(ec.Element("CollisionType").Value) };
                            }
                            break;
                    }
                }
            }
            #endregion

            Editor.reloadImages = true;
            MainForm.UpdateObjectsVisibility(); //filter what should and should not be shown

            cancelLoad:
            return resultMessage;
        }
        private static float StringToFloat(string s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static void ClearLevel()
        {
            EditorVariables.world.Clear();

            LevelProperties.Weather = "";
            LevelProperties.SoundtrackPath = "";
            LevelProperties.NZones = 0;
            LevelProperties.TTS = 0;
            LevelProperties.NYears = 1000;
            LevelProperties.NBacks = 0;

            LevelProperties.Scenarios.Clear();
            LevelProperties.Collisions = null;
            LevelProperties.Spawns = null;
            LevelProperties.TCCs.Clear();

            LevelProperties.BackSpeed = 0;
            LevelProperties.Backgrounds.Clear();

            LevelProperties.Initialize();

            BodyFactory.CreateRectangle(EditorVariables.world, 0.1f, 2f, 0f, Vector2.Zero, "WorldOrigin");
            BodyFactory.CreateRectangle(EditorVariables.world, 2f, 0.1f, 0f, Vector2.Zero, "WorldOrigin");
        }

        public static bool SaveLevel() //returns if yes or no the save was successful;
        {
            w = new XmlTextWriter(EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "properties.xml", Encoding.UTF8);

            w.Formatting = Formatting.Indented;
            w.Indentation = 4;
            w.WriteStartDocument();

            #region Level
            OpenE("Level");

            #region GeneralSetings
            OpenE("GeneralSettings");
            ElementWithValue("LevelBasePath", LevelProperties.LevelBasePath);

            OpenE("MainSpawns");
            if (LevelProperties.Spawns.playerSpawn != null)
                ElementWithAttributes("PlayerSpawn", new string[] { "X", "Y" },
                    new string[] { LevelProperties.Spawns.playerSpawn.BodyPosition.X.ToString(),
                                           LevelProperties.Spawns.playerSpawn.BodyPosition.Y.ToString()});
            //else { MessageBox.Show("PlayerSpawn doesn't exists."); goto cancel; }

            if (LevelProperties.Spawns.EndSpawn != null)
                ElementWithAttributes("EndSpawn", new string[] { "X", "Y" },
                    new string[] { LevelProperties.Spawns.EndSpawn.BodyPosition.X.ToString(),
                                           LevelProperties.Spawns.EndSpawn.BodyPosition.Y.ToString()});
            //else { MessageBox.Show("EndSpawn doesn't exists."); goto cancel; }

            if (LevelProperties.Spawns.GoalSpawn != null)
                ElementWithAttributes("GoalSpawn", new string[] { "X", "Y" },
                    new string[] { LevelProperties.Spawns.GoalSpawn.BodyPosition.X.ToString(),
                                           LevelProperties.Spawns.GoalSpawn.BodyPosition.Y.ToString()});
            CloseE();//MainSpawns

            OpenE("Sound");
            if (LevelProperties.SoundtrackPath?.ToString() != "")
                ElementWithValue("SoundtrackPath", LevelProperties.SoundtrackPath);
            //else { MessageBox.Show("No music set."); goto cancel; }
            CloseE();
            ElementWithValue("Weather", LevelProperties.Weather);

            ElementWithValue("NZones", LevelProperties.NZones.ToString());
            ElementWithValue("NBacks", LevelProperties.NBacks.ToString());
            ElementWithValue("BackSpeed", LevelProperties.BackSpeed.ToString());
            ElementWithValue("NYears", LevelProperties.NYears.ToString());
            ElementWithValue("TTS", LevelProperties.TTS.ToString());
            CloseE();//GeneralSettings
            #endregion

            #region BaseCollisions
            OpenE("BaseCollisions");
            OpenE("Circles");
            foreach (ObjCircle c in LevelProperties.Collisions.CircleList)
            {
                OpenE("Circle");
                w.WriteAttributeString("X", c.BodyPosition.X.ToString());
                w.WriteAttributeString("Y", c.BodyPosition.Y.ToString());
                w.WriteAttributeString("radius", c.Radius.ToString());

                ElementWithValue("CollisionType", c.UserData);
                CloseE();//Circle
            }
            CloseE();

            OpenE("Rectangles");
            foreach (ObjRectangle r in LevelProperties.Collisions.RectList)
            {
                OpenE("Rectangle");
                w.WriteAttributeString("X", r.BodyPosition.X.ToString());
                w.WriteAttributeString("Y", r.BodyPosition.Y.ToString());
                w.WriteAttributeString("width", r.Size.X.ToString());
                w.WriteAttributeString("height", r.Size.Y.ToString());
                w.WriteAttributeString("angle", r.Angle.ToString());

                ElementWithValue("CollisionType", r.UserData);
                CloseE();//Rectangle
            }
            CloseE();

            OpenE("Polygons");
            foreach (ObjPolygon p in LevelProperties.Collisions.PolyList)
            {
                OpenE("Polygon");
                OpenE("Vertices");
                foreach (Vector2 v in p.VerticesList)
                {
                    ElementWithAttributes("vert", new string[] { "X", "Y" },
                            new string[] { v.X.ToString(), v.Y.ToString() });
                }
                CloseE();//Vertices

                ElementWithValue("CollisionType", p.UserData);
                CloseE();//Polygon
            }
            CloseE();

            OpenE("Edges");
            foreach (ObjEdge e in LevelProperties.Collisions.EdgeList)
            {
                OpenE("Edge");
                OpenE("Vertices");
                ElementWithAttributes("WP1", new string[] { "X", "Y" },
                        new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString() });
                ElementWithAttributes("WP2", new string[] { "X", "Y" },
                            new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString() });
                CloseE();//Vertices

                ElementWithValue("CollisionType", e.UserData);
                CloseE();//Edge
            }
            CloseE();//Edges

            OpenE("EdgeChains");
            foreach (ObjEdgeChain ec in LevelProperties.Collisions.EdgeChainList)
            {
                OpenE("EdgeChain");
                OpenE("Vertices");
                foreach (Vector2 v in ec.VerticesList)
                {
                    ElementWithAttributes("vert", new string[] { "X", "Y" },
                            new string[] { v.X.ToString(), v.Y.ToString() });
                }
                CloseE();//Vertices
                ElementWithValue("CollisionType", ec.UserData);
                //foreach (ObjEdge e in ec.edges)
                //{
                //    OpenE("Edge");
                //    OpenE("Vertices");
                //    foreach (Vector2 v in collection)
                //    {

                //    }
                //    ElementWithAttributes("vert", new string[] { "X", "Y" },
                //            new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString() });
                //    ElementWithAttributes("WP2", new string[] { "X", "Y" },
                //            new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString() });
                //    CloseE();//Vertices

                //    ElementWithValue("CollisionType", e.UserData);
                //    CloseE();//Edge
                //}
                CloseE();//EdgeChain
            }
            CloseE();//EdgeChains
            
            CloseE();//BaseCollisions
            #endregion

            #region Scenarios
            OpenE("Scenarios");
            foreach (ScenarioProperties sc in LevelProperties.Scenarios)
            {
                OpenE("Scenario");
                ElementWithValue("StartTime", sc.StartTime.ToString());
                ElementWithValue("EndTime", sc.EndTime.ToString());

                #region Zones
                OpenE("Zones");
                foreach (ZoneProperties z in sc.Zones)
                {
                    OpenE("Zone");
                    OpenE("Traps");
                    foreach (PassiveTrapsProperties t in z.PassiveTraps)
                    {
                        OpenE("Trap");
                        w.WriteAttributeString("id", t.id);

                        ElementWithValue("Age", t.age.ToString());

                        ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { t.Position.X.ToString(), t.Position.Y.ToString() });
                        ElementWithValue("Rotation", t.Angle.ToString());

                        if (t.Direction == SpriteEffects.None)
                            OneAttributeElement("Direction", "inverted", "false");
                        else OneAttributeElement("Direction", "inverted", "true");
                        
                        WriteIAAux(t);

                        CloseE();//Trap
                    }
                    CloseE();//Traps

                    OpenE("Decorations");
                    foreach (DecorationProperties d in z.Decoration)
                    {
                        OpenE("Decoration");
                        w.WriteAttributeString("id", d.id);

                        ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { d.Position.X.ToString(), d.Position.Y.ToString() });
                        ElementWithValue("Rotation", d.Angle.ToString());

                        if (d.Direction == SpriteEffects.None)
                            OneAttributeElement("Direction", "inverted", "false");
                        else OneAttributeElement("Direction", "inverted", "true");

                        CloseE();//Decoration
                    }
                    CloseE();//Decorations

                    OpenE("Items");
                    foreach (ItemProperties i in z.Items)
                    {
                        OpenE("Item");
                        w.WriteAttributeString("id", i.id);

                        ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { i.Position.X.ToString(), i.Position.Y.ToString() });

                        CloseE();//Item
                    }
                    CloseE();//Items
                    CloseE();//Zone
                }
                CloseE();//Zones
                #endregion

                OpenE("Enemies");
                foreach (EnemyProperties e in sc.Enemies)
                {
                    OpenE("Enemy");
                    w.WriteAttributeString("id", e.id);

                    ElementWithValue("Age", e.Age.ToString());

                    ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { e.Position.X.ToString(), e.Position.Y.ToString() });

                    if (e.Direction == SpriteEffects.None)
                        OneAttributeElement("Direction", "inverted", "false");
                    else OneAttributeElement("Direction", "inverted", "true");
                    
                    WriteIAAux(e);

                    CloseE();//Enemy
                }
                CloseE();//Enemies

                OpenE("Sectors");
                foreach(Sector s in sc.Sectors)
                {
                    OpenE("Sector");
                    w.WriteAttributeString("id", "IMPLEMENT A SECTOR DICTIONARY + ID ?");
                    OpenE("CollisionObject");
                    if (s.CollisionObject != null)
                    {
                        w.WriteAttributeString("type", s.CollisionObject.objectType.ToString());

                        switch (s.CollisionObject.objType)
                        {
                            case ObjectClass.ObjectType.Circle:
                                ObjCircle c = (ObjCircle)s.CollisionObject;

                                OpenE("Circle");
                                w.WriteAttributeString("X", c.BodyPosition.X.ToString());
                                w.WriteAttributeString("Y", c.BodyPosition.Y.ToString());
                                w.WriteAttributeString("radius", c.Radius.ToString());

                                CloseE();//Circle
                                break;
                            case ObjectClass.ObjectType.Rectangle:
                                ObjRectangle r = (ObjRectangle)s.CollisionObject;

                                OpenE("Rectangle");
                                w.WriteAttributeString("X", r.BodyPosition.X.ToString());
                                w.WriteAttributeString("Y", r.BodyPosition.Y.ToString());
                                w.WriteAttributeString("width", r.Size.X.ToString());
                                w.WriteAttributeString("height", r.Size.Y.ToString());
                                w.WriteAttributeString("angle", r.Angle.ToString());

                                CloseE();//Rectangle
                                break;
                            case ObjectClass.ObjectType.Polygon:
                                ObjPolygon p = (ObjPolygon)s.CollisionObject;

                                OpenE("Polygon");
                                OpenE("Vertices");
                                foreach (Vector2 v in p.VerticesList)
                                {
                                    ElementWithAttributes("vert", new string[] { "X", "Y" },
                                            new string[] { v.X.ToString(), v.Y.ToString() });
                                }
                                CloseE();//Vertices
                                CloseE();//Polygon
                                break;
                            case ObjectClass.ObjectType.Edge:
                                {
                                    ObjEdge e = (ObjEdge)s.CollisionObject;

                                    OpenE("Edge");
                                    OpenE("Vertices");
                                    ElementWithAttributes("WP1", new string[] { "X", "Y" },
                                            new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString() });
                                    ElementWithAttributes("WP2", new string[] { "X", "Y" },
                                                new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString() });
                                    CloseE();//Vertices
                                    CloseE();//Edge
                                }
                                break;
                            case ObjectClass.ObjectType.EdgeChain:
                                ObjEdgeChain ec = (ObjEdgeChain)s.CollisionObject;

                                foreach (ObjEdge e in ec.edges)
                                {
                                    OpenE("Edge");
                                    OpenE("Vertices");
                                    ElementWithAttributes("WP1", new string[] { "X", "Y" },
                                            new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString() });
                                    ElementWithAttributes("WP2", new string[] { "X", "Y" },
                                            new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString() });
                                    CloseE();//Vertices
                                    CloseE();//Edge
                                }
                                break;
                            default:
                                MessageBox.Show("Error with a sector's object. Type is not valid.");
                                goto cancel;
                        }
                    }
                    else w.WriteAttributeString("type", "None");
                    CloseE();//CollisionObject

                    OpenE("Effects");
                    foreach (string effect in s.Effects)
                    {
                        ElementWithValue("Effect", effect);
                    }
                    CloseE();

                    CloseE();//Sector
                }
                CloseE();//Sectors

                CloseE();//Scenario
            }
            CloseE();//Scenarios
            #endregion

            #region TravelEntities
            OpenE("TravelEntities");

            OpenE("TravelEnemies");
            foreach (EnemyProperties e in LevelProperties.TravelEnemies)
            {
                OpenE("Enemy");
                w.WriteAttributeString("id", e.id);

                ElementWithValue("SpawnDate", e.SpawnDate.ToString());

                ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { e.Position.X.ToString(), e.Position.Y.ToString() });

                if (e.Direction == SpriteEffects.None)
                    OneAttributeElement("Direction", "inverted", "false");
                else OneAttributeElement("Direction", "inverted", "true");

                WriteIAAux(e);

                CloseE();//Enemy
            }
            CloseE();//TravelEnemies

            OpenE("TravelTraps");
            foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
            {
                OpenE("Trap");
                w.WriteAttributeString("id", t.id);

                ElementWithValue("SpawnDate", t.SpawnDate.ToString());

                ElementWithAttributes("Position", new string[] { "X", "Y" },
                            new string[] { t.Position.X.ToString(), t.Position.Y.ToString() });
                ElementWithValue("Rotation", t.Angle.ToString());

                if (t.Direction == SpriteEffects.None)
                    OneAttributeElement("Direction", "inverted", "false");
                else OneAttributeElement("Direction", "inverted", "true");

                WriteIAAux(t);

                CloseE();//Trap
            }
            CloseE();//TravelTraps

            OpenE("TravelDecos");
            foreach (DecorationProperties d in LevelProperties.TravelDecos)
            {
                OpenE("Decoration");
                w.WriteAttributeString("id", d.id);

                ElementWithValue("SpawnDate", d.SpawnDate.ToString());

                ElementWithAttributes("Position", new string[] { "X", "Y" },
                            new string[] { d.Position.X.ToString(), d.Position.Y.ToString() });
                ElementWithValue("Rotation", d.Angle.ToString());

                if (d.Direction == SpriteEffects.None)
                    OneAttributeElement("Direction", "inverted", "false");
                else OneAttributeElement("Direction", "inverted", "true");

                CloseE();//Decoration
            }
            CloseE();//TravelDecos

            CloseE();//TravelEntities
            #endregion

            #region TCCs
            OpenE("TCCs");
            foreach (TCCProperties TCC in LevelProperties.TCCs)
            {
                OpenE("TCC");
                w.WriteAttributeString("name", TCC.TCCName);

                if(TCC.IsSafeZone) { OpenE("IsSafeZone"); CloseE(); }

                OpenE("States");
                foreach (TCCState state in TCC.States)
                {
                    OpenE("State");
                    if(TCC.IsSafeZone && state.IsActiveState) { OpenE("IsActiveState"); CloseE(); }

                    OpenE("Time");
                    ElementWithValue("TimeType", state.TimeType.ToString());
                    ElementWithValue("StartTime", state.startTime.ToString());
                    ElementWithValue("EndTime", state.endTime.ToString());
                    CloseE();//Time

                    OpenE("Image");
                    if(state.ImagePath != "")
                        ElementWithValue("Path", state.ImagePath.ToString());
                    else ElementWithValue("Path", "None");//if there isn't any image, put none instead.
                    ElementWithAttributes("Position", new string[] { "X", "Y" },
                                    new string[] { state.ImagePosition.X.ToString(), state.ImagePosition.Y.ToString() });
                    CloseE();//Image

                    OpenE("CollisionObject");
                    if (state.CollisionObject != null)
                    {
                        w.WriteAttributeString("type", state.CollisionObject.objType.ToString());

                        switch (state.CollisionObject.objType)
                        {
                            case ObjectClass.ObjectType.Circle:
                                ObjCircle c = (ObjCircle)state.CollisionObject;

                                OpenE("Circle");
                                w.WriteAttributeString("X", c.BodyPosition.X.ToString());
                                w.WriteAttributeString("Y", c.BodyPosition.Y.ToString());
                                w.WriteAttributeString("radius", c.Radius.ToString());

                                ElementWithValue("CollisionType", c.UserData);
                                CloseE();//Circle
                                break;
                            case ObjectClass.ObjectType.Rectangle:
                                ObjRectangle r = (ObjRectangle)state.CollisionObject;

                                OpenE("Rectangle");
                                w.WriteAttributeString("X", r.BodyPosition.X.ToString());
                                w.WriteAttributeString("Y", r.BodyPosition.Y.ToString());
                                w.WriteAttributeString("width", r.Size.X.ToString());
                                w.WriteAttributeString("height", r.Size.Y.ToString());
                                w.WriteAttributeString("angle", r.Angle.ToString());

                                ElementWithValue("CollisionType", r.UserData);
                                CloseE();//Rectangle
                                break;
                            case ObjectClass.ObjectType.Polygon:
                                ObjPolygon p = (ObjPolygon)state.CollisionObject;

                                OpenE("Polygon");
                                OpenE("Vertices");
                                foreach (Vector2 v in p.VerticesList)
                                {
                                    ElementWithAttributes("vert", new string[] { "X", "Y" },
                                            new string[] { v.X.ToString(), v.Y.ToString() });
                                }
                                CloseE();//Vertices

                                ElementWithValue("CollisionType", p.UserData);
                                CloseE();//Polygon
                                break;
                            case ObjectClass.ObjectType.Edge:
                                {
                                    ObjEdge e = (ObjEdge)state.CollisionObject;

                                    OpenE("Edge");
                                    OpenE("Vertices");
                                    ElementWithAttributes("WP1", new string[] { "X", "Y" },
                                            new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString() });
                                    ElementWithAttributes("WP2", new string[] { "X", "Y" },
                                                new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString() });
                                    CloseE();//Vertices

                                    ElementWithValue("CollisionType", e.UserData);
                                    CloseE();//Edge
                                }
                                break;
                            case ObjectClass.ObjectType.EdgeChain:
                                ObjEdgeChain ec = (ObjEdgeChain)state.CollisionObject;
                                OpenE("EdgeChain");
                                    OpenE("Vertices");
                                        foreach (Vector2 v in ec.VerticesList)
                                        {
                                            ElementWithAttributes("vert", new string[] { "X", "Y" }, new string[]
                                                {
                                                    v.X.ToString(),
                                                    v.Y.ToString()
                                                });
                                        }
                                    CloseE();//Vertices

                                ElementWithValue("CollisionType", ec.UserData);

                                CloseE();//EdgeChain
                                break;
                            default:
                                MessageBox.Show("Error with a sector's object. Type is not valid.");
                                goto cancel;
                        }
                    }
                    else w.WriteAttributeString("type", "None");
                    CloseE();//CollisionObject

                    CloseE();//State
                }
                CloseE();//States
                CloseE();//TCC
            }
            CloseE();//TCCs
            #endregion

            CloseE();//Level
            #endregion

            w.WriteEndDocument();


            ////////////////
            //Save successful
            w.Close();
            w = null;
            return true;

        cancel:
            w.Close();
            w = null;
            return false;
        }

        #region XmlWriter Helpers
        private static void OpenE(string info)
        {
            w.WriteStartElement(info);
        }
        private static void CloseE()
        {
            w.WriteEndElement();
        }

        private static void OneAttributeElement(string ElementName, string AttributeName, string Value)
        {
            OpenE(ElementName);
            w.WriteStartAttribute(AttributeName);
            w.WriteString(Value);
            w.WriteEndAttribute();
            CloseE();
        }
        private static void ElementWithValue(string ElementName, string Value)
        {
            OpenE(ElementName);
            w.WriteString(Value);
            CloseE();
        }
        private static void ElementWithAttributes(string ElementName, string[] AttributesNames, string[] Values)
        {
            OpenE(ElementName);

            for (int i = 0; i < AttributesNames.Length; i++)
            {
                w.WriteStartAttribute(AttributesNames[i]);
                w.WriteString(Values[i]);
                w.WriteEndAttribute();
            }

            CloseE();
        }
        private static void WriteIAAux(object o)
        {
            OpenE("IAAux");
            if (o.GetType() == typeof(PassiveTrapsProperties))
            {
                PassiveTrapsProperties O = (PassiveTrapsProperties)o;
                if (O.IAAuxNeeded == null) goto cancelIAAux; //do not write anything if the enemy doesn't have any iaaux

                w.WriteAttributeString("type", O.IAAuxNeeded.NeededInfos.ToString());
                if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                {
                    OpenE("Object");
                    w.WriteAttributeString("type", ((ActivationZone)O.IAAuxNeeded).ActivationBody.objType.ToString());

                    switch (((ActivationZone)O.IAAuxNeeded).ActivationBody.objType)
                    {
                        case ObjectClass.ObjectType.Circle:
                            ElementWithAttributes("Circle", new string[] { "X", "Y", "radius" },
                                new string[] { ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.X.ToString(),
                                               ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.Y.ToString(),
                                               ((ObjCircle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Radius.ToString()});
                            break;
                        case ObjectClass.ObjectType.Rectangle:
                            ElementWithAttributes("Rectangle", new string[] { "X", "Y", "width", "height", "angle" },
                                new string[] { ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.X.ToString(),
                                               ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.Y.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Size.X.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Size.Y.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Angle.ToString()});
                            break;
                        case ObjectClass.ObjectType.Polygon:
                            OpenE("Polygon");
                            OpenE("Vertices");
                            foreach (Vector2 v in ((ObjPolygon)((ActivationZone)O.IAAuxNeeded).ActivationBody).VerticesList)
                            {
                                ElementWithAttributes("vert", new string[] { "X", "Y" }, new string[] { v.X.ToString(), v.Y.ToString() });
                            }
                            CloseE();//Vertices
                            CloseE();//Polygon
                            break;
                        #region useless
                        //case ObjectClass.ObjectType.Edge:
                        //    OpenE("Edge");
                        //        ElementWithAttributes("WP1", new string[] { "X", "Y" }, 
                        //            new string[] { ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert1Pos.X.ToString(),
                        //                           ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert1Pos.Y.ToString()});
                        //        ElementWithAttributes("WP2", new string[] { "X", "Y" },
                        //            new string[] { ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert2Pos.X.ToString(),
                        //                           ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert2Pos.Y.ToString()});
                        //    CloseE();//Edge
                        //    break;
                        //case ObjectClass.ObjectType.EdgeChain:
                        //    OpenE("EdgeChain");
                        //        foreach (ObjEdge e in ((ObjEdgeChain)((ActivationZone)O.IAAuxNeeded).ActivationBody).edges)
                        //        {
                        //            OpenE("Edge");
                        //            ElementWithAttributes("WP1", new string[] { "X", "Y" },
                        //                new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString()});
                        //            ElementWithAttributes("WP2", new string[] { "X", "Y" },
                        //                new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString()});
                        //            CloseE();//Edge
                        //        }
                        //    CloseE();//EdgeChain
                        //    break;
                        #endregion
                        default:
                            MessageBox.Show("One Trap does not have a valid object in his ActivationZone's IAAux.");
                            return;
                    }
                    CloseE();//Object
                }
                else if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.Radius)
                {
                    ElementWithValue("Radius", ((Radius)O.IAAuxNeeded).RadiusCircle.Radius.ToString());
                }
            cancelIAAux:
                CloseE();//IAAux
            }
            else if (o.GetType() == typeof(EnemyProperties))
            {
                EnemyProperties O = (EnemyProperties)o;
                if (O.IAAuxNeeded == null) goto cancelIAAux; //do not write anything if the enemy doesn't have any iaaux

                w.WriteAttributeString("type", O.IAAuxNeeded.NeededInfos.ToString());
                if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.ActivationZone)
                {
                    OpenE("Object");
                    w.WriteAttributeString("type", ((ActivationZone)O.IAAuxNeeded).ActivationBody.objType.ToString());

                    switch (((ActivationZone)O.IAAuxNeeded).ActivationBody.objType)
                    {
                        case ObjectClass.ObjectType.Circle:
                            ElementWithAttributes("Circle", new string[] { "X", "Y", "radius" },
                                new string[] { ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.X.ToString(),
                                               ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.Y.ToString(),
                                               ((ObjCircle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Radius.ToString()});
                            break;
                        case ObjectClass.ObjectType.Rectangle:
                            ElementWithAttributes("Rectangle", new string[] { "X", "Y", "width", "height", "angle" },
                                new string[] { ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.X.ToString(),
                                               ((ActivationZone)O.IAAuxNeeded).ActivationBody.BodyPosition.Y.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Size.X.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Size.Y.ToString(),
                                               ((ObjRectangle)((ActivationZone)O.IAAuxNeeded).ActivationBody).Angle.ToString()});
                            break;
                        case ObjectClass.ObjectType.Polygon:
                            OpenE("Polygon");
                            OpenE("Vertices");
                            foreach (Vector2 v in ((ObjPolygon)((ActivationZone)O.IAAuxNeeded).ActivationBody).VerticesList)
                            {
                                ElementWithAttributes("vert", new string[] { "X", "Y" }, new string[] { v.X.ToString(), v.Y.ToString() });
                            }
                            CloseE();//Vertices
                            CloseE();//Polygon
                            break;
                        #region useless
                        //case ObjectClass.ObjectType.Edge:
                        //    OpenE("Edge");
                        //        ElementWithAttributes("WP1", new string[] { "X", "Y" }, 
                        //            new string[] { ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert1Pos.X.ToString(),
                        //                           ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert1Pos.Y.ToString()});
                        //        ElementWithAttributes("WP2", new string[] { "X", "Y" },
                        //            new string[] { ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert2Pos.X.ToString(),
                        //                           ((ObjEdge)((ActivationZone)O.IAAuxNeeded).ActivationBody).Vert2Pos.Y.ToString()});
                        //    CloseE();//Edge
                        //    break;
                        //case ObjectClass.ObjectType.EdgeChain:
                        //    OpenE("EdgeChain");
                        //        foreach (ObjEdge e in ((ObjEdgeChain)((ActivationZone)O.IAAuxNeeded).ActivationBody).edges)
                        //        {
                        //            OpenE("Edge");
                        //            ElementWithAttributes("WP1", new string[] { "X", "Y" },
                        //                new string[] { e.Vert1Pos.X.ToString(), e.Vert1Pos.Y.ToString()});
                        //            ElementWithAttributes("WP2", new string[] { "X", "Y" },
                        //                new string[] { e.Vert2Pos.X.ToString(), e.Vert2Pos.Y.ToString()});
                        //            CloseE();//Edge
                        //        }
                        //    CloseE();//EdgeChain
                        //    break;
                        #endregion
                        default:
                            MessageBox.Show("One Trap does not have a valid object in his ActivationZone's IAAux.");
                            return;
                    }
                    CloseE();//Object
                }
                else if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.Radius)
                {
                    ElementWithValue("Radius", ((Radius)O.IAAuxNeeded).RadiusCircle.Radius.ToString());
                }
                else if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.DualWaypoints)
                {
                    ElementWithAttributes("WP1", new string[] { "X", "Y" },
                        new string[] { ((DualWaypoints)O.IAAuxNeeded).WP1.BodyPosition.X.ToString(),
                                       ((DualWaypoints)O.IAAuxNeeded).WP1.BodyPosition.Y.ToString() });
                    ElementWithAttributes("WP2", new string[] { "X", "Y" },
                        new string[] { ((DualWaypoints)O.IAAuxNeeded).WP2.BodyPosition.X.ToString(),
                                       ((DualWaypoints)O.IAAuxNeeded).WP2.BodyPosition.Y.ToString() });
                }
                else if (O.IAAuxNeeded.NeededInfos == IAActivatedObject.SecondaryInfos.Path)
                {
                    if (((IAPath)O.IAAuxNeeded).PathObject == null) { MessageBox.Show("an ennemy's IAPath is empty."); goto cancelIAAux; }

                    OpenE("Vertices");
                    foreach (Vector2 v in ((IAPath)O.IAAuxNeeded).PathObject.VerticesList)
                    {
                        ElementWithAttributes("vert", new string[] { "X", "Y" }, new string[] { v.X.ToString(), v.Y.ToString() });
                    }
                    CloseE();//Vertices
                }
            cancelIAAux:
                CloseE();//IAAux
            }
        }
        #endregion
    }
}