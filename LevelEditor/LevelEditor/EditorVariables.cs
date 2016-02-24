using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace LevelEditor
{
    public static class EditorVariables
    {
        public static bool DevMode; //when activated, the zones images correspond to the schema of the current scenario

        public static World world;
        public static int CurrentYear;
        public static int CurrentScenario;
        /* 
         * listes de edges, circles pour la creation de polygones (temp)
         * ennemies
         * collisions
         * sectors
         * timechangingcollidables
         * spawns (decos/traps/items)
         */
        //public static List<ObjCircle>   CircleList;
        //public static List<ObjEdge>     EdgeList;
        //public static List<ObjRectangle> RectList;
        //public static List<ObjPolygon> PolyList;
        public static List<ObjPoint> PointList;
        public static string ContentBasePath;


        public static List<ObjCircle> polyCircleList; // temporary bodies when
        public static List<ObjEdge> polyEdgeList;   // building polygons

        public static void Initialize()
        { // initializing all the lists (in the Game1)
            //CircleList = new List<ObjCircle>();
            //EdgeList = new List<ObjEdge>();
            //RectList = new List<ObjRectangle>();
            //PolyList = new List<ObjPolygon>();
            PointList = new List<ObjPoint>();


            //ContentBasePath = new Uri(AppDomain.CurrentDomain.BaseDirectory); //the directory where the LevelEditor .exe is
            Uri ContentBaseUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../../../../LEJEU.Content");
            ContentBasePath = ContentBaseUri.AbsolutePath.Replace("%20", " ") + "/"; //get the absolute path of where the LEJEU.Content is.

            polyCircleList = new List<ObjCircle>();
            polyEdgeList = new List<ObjEdge>();
        }
    }

    ////////////////////////////DICTIONARIES LOADERS AND HANDLERS//////////////////////////
    #region EnnemiesDict
    public static class EnnemiesDict
    {
        public static List<string> names;
        public static List<Image> images;
        //Dictionary<string, Image> decos;
        public static Dictionary<string, Dictionary<string, Image>> ennemies;

        public static void Initialize()
        {
            names = new List<string>();
            images = new List<Image>();
            ennemies = new Dictionary<string, Dictionary<string, Image>>();
        }
        public static void Unload()
        {
            if (names != null)
                names.Clear();
            if (images != null)
                images.Clear();
            if (ennemies != null)
                ennemies.Clear();
        }
        public static void LoadEnnemiesDict()
        {
            //load the xml file with the ennemies list NOT IMPLEMENTED
            System.IO.Stream streamLevel = TitleContainer.OpenStream(@"Content/Entities/LevelEditorDictionaries/EnemiesDB.xml"); //load xml
            XDocument list = XDocument.Load(streamLevel);                                            //document

            foreach (XElement g in list.Element("Ennemies").Descendants("Group"))
            { // for each group of ennemies

                images.Add(Image.FromFile(@"Content/Entities/Enemies/" + g.Attribute("file").Value.ToString() + "Thumbnail.png"));
                names.Add(g.Attribute("file").Value.ToString());
                Console.WriteLine(names.Last());
                Dictionary<string, Image> group = new Dictionary<string, Image>();

                foreach (XElement e in g.Descendants("Ennemy"))
                { // for each ennemy in each group
                    names.Add(e.Attribute("id").Value.ToString());
                    images.Add(Image.FromFile(@"Content/Entities/Enemies/" + e.Attribute("id").Value.ToString() + "/" + e.Attribute("id").Value.ToString() + "Thumbnail.png"));
                    group.Add(e.Attribute("id").Value.ToString(), images.Last());
                }

                ennemies.Add(g.Attribute("name").Value.ToString(), group);
            }
        }
    }
    #endregion

    #region DecoDict
    public static class DecoDict
    {
        public static List<string> names;
        public static List<Image> images;
        //Dictionary<string, Image> decos;
        public static Dictionary<string, Dictionary<string, Image>> decos;

        public static void Initialize()
        {
            names = new List<string>();
            images = new List<Image>();
            decos = new Dictionary<string, Dictionary<string, Image>>();
        }
        public static void Unload()
        {
            if (names != null)
                names.Clear();
            if (images != null)
                images.Clear();
            if (decos != null)
                decos.Clear();
        }
        public static void LoadDecoDict()
        {
            //load the xml file with the ennemies list NOT IMPLEMENTED
            System.IO.Stream streamLevel = TitleContainer.OpenStream(@"Content/Entities/LevelEditorDictionaries/DecoDB.xml"); //load xml
            XDocument list = XDocument.Load(streamLevel);                                                    //document

            foreach (XElement g in list.Element("Objects").Descendants("Group"))
            { // for each group of decos

                images.Add(Image.FromFile(@"Content/Entities/Decos/" + g.Attribute("file").Value.ToString() + ".png"));
                names.Add(g.Attribute("name").Value.ToString());
                Dictionary<string, Image> group = new Dictionary<string, Image>();

                foreach (XElement o in g.Descendants("Object"))
                { // for each object in each group
                    names.Add(o.Attribute("id").Value.ToString());
                    images.Add(Image.FromFile(@"Content/Entities/Decos/" + o.Attribute("id").Value.ToString() + ".png"));
                    group.Add(o.Attribute("id").Value.ToString(), images.Last());
                }

                decos.Add(g.Attribute("name").Value.ToString(), group);
            }
        }
    }
    #endregion

    #region TrapsDict
    public static class TrapsDict
    {
        public static List<string> names;
        public static List<Image> images;
        public static Dictionary<string, Dictionary<string, Image>> traps;

        public static void Initialize()
        {
            names = new List<string>();
            images = new List<Image>();
            traps = new Dictionary<string, Dictionary<string, Image>>();
        }
        public static void Unload()
        {
            if (names != null)
                names.Clear();
            if (images != null)
                images.Clear();
            if (traps != null)
                traps.Clear();
        }
        public static void LoadTrapsDict()
        {
            //load the xml file with the ennemies list NOT IMPLEMENTED
            System.IO.Stream streamLevel = TitleContainer.OpenStream(@"Content/Entities/LevelEditorDictionaries/TrapsDB.xml"); //load xml
            XDocument list = XDocument.Load(streamLevel);                                                    //document

            foreach (XElement g in list.Element("Traps").Descendants("Group"))
            { // for each group of decos

                images.Add(Image.FromFile(@"Content/Entities/Traps/" + g.Attribute("file").Value.ToString() + ".png"));
                names.Add(g.Attribute("name").Value.ToString());
                Dictionary<string, Image> group = new Dictionary<string, Image>();

                foreach (XElement t in g.Descendants("Trap"))
                { // for each object in each group
                    names.Add(t.Attribute("id").Value.ToString());
                    images.Add(Image.FromFile(@"Content/Entities/Traps/" + t.Attribute("id").Value.ToString() + "/" + t.Attribute("id").Value.ToString() + "Thumbnail.png"));
                    group.Add(t.Attribute("id").Value.ToString(), images.Last());
                }

                traps.Add(g.Attribute("name").Value.ToString(), group);
            }
        }
    }
    #endregion

    #region ItemsDict
    public static class ItemsDict
    {
        public static List<string> names;
        public static List<Image> images;
        public static Dictionary<string, Dictionary<string, Image>> items;

        public static void Initialize()
        {
            names = new List<string>();
            images = new List<Image>();
            items = new Dictionary<string, Dictionary<string, Image>>();
        }
        public static void Unload()
        {
            if (names != null)
                names.Clear();
            if (images != null)
                images.Clear();
            if (items != null)
                items.Clear();
        }
        public static void LoadItemsDict()
        {
            //load the xml file with the ennemies list NOT IMPLEMENTED
            System.IO.Stream streamLevel = TitleContainer.OpenStream(@"Content/Entities/LevelEditorDictionaries/ItemsDB.xml"); //load xml
            XDocument list = XDocument.Load(streamLevel);                                                    //document

            foreach (XElement g in list.Element("Items").Descendants("Group"))
            { // for each group of decos

                images.Add(Image.FromFile(@"Content/Entities/Items/" + g.Attribute("file").Value.ToString() + "Thumbnail.png"));
                Dictionary<string, Image> group = new Dictionary<string, Image>();

                foreach (XElement t in g.Descendants("Item"))
                { // for each object in each group
                    names.Add(t.Attribute("id").Value.ToString());
                    images.Add(Image.FromFile(@"Content/Entities/Items/" + t.Attribute("id").Value.ToString() + "Thumbnail.png"));
                    group.Add(t.Attribute("id").Value.ToString(), images.Last());
                }

                items.Add(g.Attribute("name").Value.ToString(), group);
            }
        }
    }
    #endregion
}
