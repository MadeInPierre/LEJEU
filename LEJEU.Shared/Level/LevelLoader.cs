using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LEJEU.Shared
{
    public class LevelLoader
    {
        public static LevelProperties LoadLevelProperties(LevelProperties Level, string path)
        {
            System.IO.Stream streamLevel = TitleContainer.OpenStream("Content/" + path); //load xml
            XDocument doc = XDocument.Load(streamLevel);                              //document

            ////////////////////////////////////////////////////////////////////////////////////////////
            Level = new LevelProperties()
            {
                Weather = doc.Element("Level").Element("Weather").Attribute("id").Value,
                SoundtrackPath = doc.Element("Level").Element("Soundtrack").Attribute("path").Value,
                NZones = Convert.ToInt32(doc.Element("Level").Element("NZones").Value),
                playerSpawnPos = new Vector2((float)Convert.ToDecimal(doc.Element("Level").Element("playerSpawn").Attribute("x").Value),
                                             (float)Convert.ToDecimal(doc.Element("Level").Element("playerSpawn").Attribute("y").Value)),

                Zones = (from zone in doc.Element("Level").Element("Zones").Descendants("Zone")
                         select new ZonesProperties()
                         {
                             id = Convert.ToInt32(zone.Attribute("id").Value),
                             platformPath = zone.Element("platform").Attribute("path").Value,
                             foregroundPath = zone.Element("foreground").Attribute("path").Value,
                             Collisions = new CollisionsProperties()
                             {
                                 physicalsPath = zone.Element("Collisions").Element("physicals").Attribute("path").Value,
                                 sectorsPath = zone.Element("Collisions").Element("sectors").Attribute("path").Value
                             },
                             Ennemies = (from ennemy in zone.Element("Ennemies").Descendants("Ennemy")
                                         select new EnnemyProperties()
                                         {
                                             id = ennemy.Attribute("id").Value,
                                             pos = new Vector3((float)Convert.ToDecimal(ennemy.Element("pos").Attribute("x").Value),
                                                               (float)Convert.ToDecimal(ennemy.Element("pos").Attribute("y").Value),
                                                               (float)Convert.ToDecimal(ennemy.Element("pos").Attribute("direction").Value)),
                                             spawndate = Convert.ToInt32(ennemy.Element("spawndate").Attribute("year").Value),
                                             randAge = Convert.ToBoolean(ennemy.Element("age").Attribute("rand").Value),
                                             age = Convert.ToInt32(ennemy.Element("age").Value)
                                         }).ToList(),
                             Objects = (from o in zone.Element("Objects").Descendants("Object")
                                        select new ObjectProperties()
                                        {
                                            id = o.Attribute("id").Value,
                                            pos = new Vector3((float)Convert.ToDecimal(o.Element("pos").Attribute("x").Value),
                                                              (float)Convert.ToDecimal(o.Element("pos").Attribute("y").Value),
                                                              (float)Convert.ToDecimal(o.Element("pos").Attribute("rotation").Value)),
                                            spawndate = Convert.ToInt32(o.Element("spawndate").Attribute("year").Value),
                                            randAge = Convert.ToBoolean(o.Element("age").Attribute("rand").Value),
                                            age = Convert.ToInt32(o.Element("age").Value)
                                        }).ToList(),
                             SafeZones = (from sz in zone.Element("SafeZones").Descendants("safeZone")
                                          select new SafeZoneProperties()
                                          {
                                              pos = new Vector2((float)Convert.ToDecimal(sz.Element("pos").Attribute("x").Value),
                                                                (float)Convert.ToDecimal(sz.Element("pos").Attribute("y").Value)),
                                              inactiveImg = sz.Element("inactive").Attribute("img").Value,
                                              activeImg = sz.Element("active").Attribute("img").Value,
                                              deadImg = sz.Element("dead").Attribute("img").Value,

                                              inactiveCollisions = sz.Element("inactive").Attribute("collision").Value,
                                              activeCollisions = sz.Element("active").Attribute("collision").Value,
                                              deadCollisions = sz.Element("dead").Attribute("collision").Value,

                                              spawnDate = Convert.ToInt32(sz.Element("lifespan").Attribute("spawnDate").Value),
                                              activationDate = Convert.ToInt32(sz.Element("lifespan").Attribute("activationDate").Value),
                                              deathDate = Convert.ToInt32(sz.Element("lifespan").Attribute("deathDate").Value),
                                          }).ToList(),
                             Items = (from i in zone.Element("Items").Descendants("Item")
                                      select new ItemProperties()
                                      {
                                          id = i.Attribute("id").Value,
                                          pos = new Vector2((float)Convert.ToDecimal(i.Element("pos").Attribute("x").Value),
                                                            (float)Convert.ToDecimal(i.Element("pos").Attribute("y").Value))
                                      }).ToList()

                         }).ToList(),

                //BackSpeed = (float)Convert.ToDouble(doc.Element("Level").Element("Backgrounds").Attribute("speed").Value),
                Backgrounds = (from background in doc.Element("Level").Element("Backgrounds").Descendants("img")
                         select new BackgroundProperties()
                         {
                             id = Convert.ToInt32(background.Attribute("id").Value),
                             path = background.Attribute("path").Value
                         }).ToList()
            };

            streamLevel.Close();
            return Level;
        }

    }





        public class LevelProperties
        {
            public string Weather { get; set; }
            public string SoundtrackPath { get; set; }
            public int NZones { get; set; }
            public Vector2 playerSpawnPos { get; set; }
            public int playerSpawnZone { get; set; }
            public List<ZonesProperties> Zones { get; set; }
            public float BackSpeed { get; set; }
            public List<BackgroundProperties> Backgrounds { get; set; }
        }
        public class ZonesProperties
        {
            public int id { get; set; }
            public string platformPath { get; set; }
            public string foregroundPath { get; set; }
            public CollisionsProperties Collisions { get; set; }
            public List<EnnemyProperties> Ennemies { get; set; }
            public List<ObjectProperties> Objects { get; set; }
            public List<SafeZoneProperties> SafeZones { get; set; }
            public List<ItemProperties> Items { get; set; }
        }
        public class CollisionsProperties
        {
            public string physicalsPath { get; set; } //plateformes, eau.. XML
            public string sectorsPath { get; set; } //secteurs (sable, grotte..) XML
        }
        public class EnnemyProperties
        {
            public string id { get; set; } //voir dictionnaire XML
            public Vector3 pos { get; set; } //posX, posY, direction
            public int spawndate { get; set; }
            public Boolean randAge { get; set; }
            public int age { get; set; }
        }
        public class ObjectProperties
        {
            public string id { get; set; }
            public Vector3 pos { get; set; } //posX, posY, Rotation(radians)
            public int spawndate { get; set; }
            public Boolean randAge { get; set; }
            public int age { get; set; }
        }

        public class SafeZoneProperties
        {
            public Vector2 pos { get; set; }

            public string inactiveImg { get; set; }
            public string activeImg { get; set; }
            public string deadImg { get; set; }

            public string inactiveCollisions { get; set; }
            public string activeCollisions { get; set; }
            public string deadCollisions { get; set; }

            public int spawnDate { get; set; }
            public int activationDate { get; set; }
            public int deathDate { get; set; }


        }
        public class ItemProperties
        {
            public string id { get; set; }
            public Vector2 pos { get; set; }
        }


        public class BackgroundProperties
        {
            public int id { get; set; }
            public string path { get; set; }
        }
    }

