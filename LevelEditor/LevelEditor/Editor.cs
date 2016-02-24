using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using System.IO;
using FarseerPhysics.Common;

namespace LevelEditor
{
    public class Editor
    {
        DebugViewXNA debugView;
        public static bool reloadImages = true;

        public CameraEditor camera;
        ContentManager Content;

        Texture2D img;
        SpriteFont font;



        Texture2D[] platformImgs, backgroundImgs; //array des textures des plateformes

        public Editor()
        {

        }


        public void Initialize()
        {
            EditorVariables.world = new World(/*new Vector2(0, 9.1f)*/Vector2.Zero);
            camera = new CameraEditor();
            camera.Initialize(Vector2.Zero);

            //Type type = Type.GetType("LevelEditorTest1.ObjEdge");
            //object instance = Activator.CreateInstance(type);
            //Console.WriteLine(instance);
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = Content;

            debugView = new DebugViewXNA(EditorVariables.world);
            debugView.AppendFlags(DebugViewFlags.Shape);
            debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            debugView.LoadContent(Game1.graphicsDevice, Content);

            font = Content.Load<SpriteFont>("Font");

            // img = Texture2D.FromStream(Game1.graphicsDevice, File.OpenRead(@"C:\Users\_Jack_-_Daxter_0\Google Drive\LEJEU\RES\Gameplay\Ennemis\Oiseaux\OISEAU ANIM.gif"));

            BodyFactory.CreateRectangle(EditorVariables.world, 0.1f, 2f, 0f, Vector2.Zero, "WorldOrigin");
            BodyFactory.CreateRectangle(EditorVariables.world, 2f, 0.1f, 0f, Vector2.Zero, "WorldOrigin");
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            if (reloadImages)
            {
                platformImgs = new Texture2D[LevelProperties.NZones];                              // reload all
                for (int i = 0; i < LevelProperties.NZones; i++)                                   // the platform Images
                {
                    string path = "";

                    path = EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "Zones/Platforms/zone" + i + ".png";
                    if(EditorVariables.DevMode)
                        if (MainForm.currentView == MainForm.CurrentView.Scenario)
                            path = EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "dev/SC" + EditorVariables.CurrentScenario + "/zone" + i + ".png";
                    try
                    {
                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        platformImgs[i] = Texture2D.FromStream(Game1.graphicsDevice, fs);
                        fs.Flush();
                    }
                    catch { System.Windows.Forms.MessageBox.Show("Could not load the file " + path + ". Try again before doing anything else.",
                                                                 "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error); }
                }

                backgroundImgs = new Texture2D[LevelProperties.NBacks];
                for (int i = 0; i < LevelProperties.NBacks; i++)
                {
                    string path = EditorVariables.ContentBasePath + LevelProperties.LevelBasePath + "Backgrounds/back" + i + ".png";
                    try
                    {
                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        backgroundImgs[i] = Texture2D.FromStream(Game1.graphicsDevice, fs);
                        fs.Flush();
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("Could not load the file " + path + ". Try again before doing anything else.",
                                                             "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                reloadImages = false;
            }
            //listCleanup() //look if each element in LevelPerperties is alive, and delete the ones that are at false;

            camera.Update(input);
            EditorVariables.world.Step(1f / 30f);
            handleDebugViewInput(input);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix);
            for (int i = 0; i < platformImgs.Length; i++)
            { // draw all the platform images
                try { sb.Draw(platformImgs[i], Vector2.UnitX * LevelProperties.zoneDimensions.X * i, Color.White); }
                catch { } //in case we couldn't load an image
            }
            for (int i = 0; i < backgroundImgs.Length; i++)
            { // draw all the platform images
                try { sb.Draw(backgroundImgs[i], new Vector2(1 * LevelProperties.zoneDimensions.X * i, -LevelProperties.zoneDimensions.Y), Color.White); }
                catch { } //in case we couldn't load an image
            }
            sb.End();

            sb.Begin();
            renderDebugView(sb);
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.ViewMatrix); //used for ennemies thumbnails

            foreach (TCCProperties TCC in LevelProperties.TCCs)
            {
                foreach (TCCState state in TCC.States)
                {
                    if(!state.Hidden && state.ImagePath != null && state.ImagePath != "")
                    {
                        sb.Draw(state.Image, ConvertUnits.ToDisplayUnits(state.ImagePosition), Color.White);
                    }
                }
            }


            foreach (ScenarioProperties scenario in LevelProperties.Scenarios)
            {
                foreach (EnemyProperties e in scenario.Enemies)
                {
                    Vector2 ImageMiddle = new Vector2(e.thumbnail.Width / 2, e.thumbnail.Height / 2);
                    Vector2 TextPos = new Vector2(ConvertUnits.ToDisplayUnits(e.spawnRect.BodyPosition.X) - font.MeasureString(e.id).X / 2,
                                                  ConvertUnits.ToDisplayUnits(e.spawnRect.BodyPosition.Y - (e.spawnRect.Size.Y / 2)));

                    if (!e.Hidden)
                    {
                        sb.Draw(e.thumbnail, ConvertUnits.ToDisplayUnits(e.spawnRect.BodyPosition),
                            null, Color.White, 0f, ImageMiddle, 1f, e.Direction, 0f);
                        sb.DrawString(font, e.id, TextPos, Color.White);
                    }
                }
                foreach (ZoneProperties zone in scenario.Zones)
                { // take each trap il all zones (time doesn't matter TEMPORAIRE)
                    foreach (PassiveTrapsProperties pt in zone.PassiveTraps)
                    { //Draw the description
                        Vector2 midSize = new Vector2(pt.thumbnail.Width / 2, pt.thumbnail.Height / 2);
                        Vector2 TextPos = new Vector2(ConvertUnits.ToDisplayUnits(pt.spawnRect.BodyPosition.X) - font.MeasureString(pt.id).X / 2,
                                              ConvertUnits.ToDisplayUnits(pt.spawnRect.BodyPosition.Y - (pt.spawnRect.Size.Y / 2)));

                        if (!pt.Hidden)
                        {
                            sb.Draw(pt.thumbnail, ConvertUnits.ToDisplayUnits(pt.spawnRect.BodyPosition),
                            null, Color.White, pt.Angle, midSize, 1f, pt.Direction, 0f);
                            sb.DrawString(font, pt.id, TextPos, Color.White);
                        }
                    }
                    foreach (DecorationProperties d in zone.Decoration)
                    { //Draw the description
                        Vector2 midSize = new Vector2(d.thumbnail.Width / 2, d.thumbnail.Height / 2);
                        Vector2 TextPos = new Vector2(ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition.X) - font.MeasureString(d.id).X / 2,
                                              ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition.Y - (d.spawnRect.Size.Y / 2)));

                        if (!d.Hidden)
                        {
                            sb.Draw(d.thumbnail, ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition),
                            null, Color.White, d.Angle, midSize, 1f, d.Direction, 0f);
                            sb.DrawString(font, d.id, TextPos, Color.White);
                        }
                    }
                    foreach (ItemProperties item in zone.Items)
                    { //draw the Item's images in each zone
                        if (!item.spawnRect.Hidden)
                        {
                            sb.Draw(item.thumbnail, ConvertUnits.ToDisplayUnits(item.spawnRect.BodyPosition) - new Vector2(item.thumbnail.Width / 2, item.thumbnail.Height / 2), Color.White);
                            Vector2 TextPos = new Vector2(ConvertUnits.ToDisplayUnits(item.spawnRect.BodyPosition.X) - font.MeasureString(item.id).X / 2,
                                                          ConvertUnits.ToDisplayUnits(item.spawnRect.BodyPosition.Y  - (item.spawnRect.Size.Y / 2)));
                            sb.DrawString(font, item.id, TextPos, Color.White);
                        }
                    }
                }
            }
            
            foreach (EnemyProperties e in LevelProperties.TravelEnemies)
            {
                Vector2 middle = new Vector2(e.thumbnail.Width / 2, e.thumbnail.Height / 2);

                if (!e.Hidden)
                    sb.Draw(e.thumbnail, ConvertUnits.ToDisplayUnits(e.spawnRect.BodyPosition),
                    null, Color.White, 0f, middle, 1f, e.Direction, 0f);
            }
            foreach (PassiveTrapsProperties t in LevelProperties.TravelTraps)
            {
                Vector2 middle = new Vector2(t.thumbnail.Width / 2, t.thumbnail.Height / 2);

                if (!t.Hidden)
                    sb.Draw(t.thumbnail, ConvertUnits.ToDisplayUnits(t.spawnRect.BodyPosition),
                    null, Color.White, t.Angle, middle, 1f, t.Direction, 0f);
            }
            foreach (DecorationProperties d in LevelProperties.TravelDecos)
            { //Draw the description
                Vector2 midSize = new Vector2(d.thumbnail.Width / 2, d.thumbnail.Height / 2);
                Vector2 TextPos = new Vector2(ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition.X) - font.MeasureString(d.id).X / 2,
                                      ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition.Y - (d.spawnRect.Size.Y / 2)));

                if (!d.Hidden)
                {
                    sb.Draw(d.thumbnail, ConvertUnits.ToDisplayUnits(d.spawnRect.BodyPosition),
                    null, Color.White, d.Angle, midSize, 1f, d.Direction, 0f);
                    sb.DrawString(font, d.id, TextPos, Color.White);
                }
            }





            foreach (ObjPoint p in EditorVariables.PointList)
            {
                Vector2 pos = new Vector2(ConvertUnits.ToDisplayUnits(p.position.X) - font.MeasureString(p.UserData).X / 2,
                                          ConvertUnits.ToDisplayUnits(p.position.Y) + 8);
                sb.DrawString(font, p.UserData, pos, Color.White);
            }

            #region playerSpawn, endspawn, goalspawn descriptions
            if (LevelProperties.Spawns.playerSpawn != null)
            {
                Vector2 pos = new Vector2(ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.playerSpawn.BodyPosition.X) - font.MeasureString(LevelProperties.Spawns.playerSpawn.UserData).X / 2,
                                                      ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.playerSpawn.BodyPosition.Y) + 8);
                sb.DrawString(font, LevelProperties.Spawns.playerSpawn.UserData, pos, Color.White);
            }
            if (LevelProperties.Spawns.EndSpawn != null)
            {
                Vector2 pos = new Vector2(ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.EndSpawn.BodyPosition.X) - font.MeasureString(LevelProperties.Spawns.EndSpawn.UserData).X / 2,
                                                      ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.EndSpawn.BodyPosition.Y) + 8);
                sb.DrawString(font, LevelProperties.Spawns.EndSpawn.UserData, pos, Color.White);
            }
            if (LevelProperties.Spawns.GoalSpawn != null)
            {
                Vector2 pos = new Vector2(ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.GoalSpawn.BodyPosition.X) - font.MeasureString(LevelProperties.Spawns.GoalSpawn.UserData).X / 2,
                                                      ConvertUnits.ToDisplayUnits(LevelProperties.Spawns.GoalSpawn.BodyPosition.Y) + 8);
                sb.DrawString(font, LevelProperties.Spawns.GoalSpawn.UserData, pos, Color.White);
            }
            #endregion
            sb.End();
        }

        #region debugViewInput
        public void handleDebugViewInput(InputManager input)
        {
            if (Game1.form.MouseInXNA)
            {
                if (input.KeyPressed(Keys.F1))
                    debugView.AppendFlags(DebugViewFlags.Shape);
                else if (input.KeyPressed(Keys.F2))
                    debugView.AppendFlags(DebugViewFlags.DebugPanel);
                else if (input.KeyPressed(Keys.F3))
                    debugView.AppendFlags(DebugViewFlags.PerformanceGraph);
                else if (input.KeyPressed(Keys.F4))
                    debugView.AppendFlags(DebugViewFlags.AABB);
                else if (input.KeyPressed(Keys.F5))
                    debugView.AppendFlags(DebugViewFlags.CenterOfMass);
                else if (input.KeyPressed(Keys.F6))
                    debugView.AppendFlags(DebugViewFlags.Joint);
                else if (input.KeyPressed(Keys.F7))
                {
                    debugView.AppendFlags(DebugViewFlags.ContactPoints);
                    debugView.AppendFlags(DebugViewFlags.ContactNormals);
                }
                else if (input.KeyPressed(Keys.F8))
                    debugView.AppendFlags(DebugViewFlags.PolygonPoints);
                else if (input.KeyPressed(Keys.F9))
                    debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            }
        }
        #endregion

        #region renderDebugView
        void renderDebugView(SpriteBatch sb)
        {
            Matrix projection = Matrix.CreateOrthographicOffCenter(0f, Game1.Dimensions.X / ConvertUnits.ToDisplayUnits(1) / CameraEditor.scale,
                                                                       Game1.Dimensions.Y / ConvertUnits.ToDisplayUnits(1) / CameraEditor.scale, 0f, 0f,
                                                                   1f);

            Vector2 screenCenter = new Vector2(Game1.Dimensions.X / 2, Game1.Dimensions.Y / 2);
            Matrix view = Matrix.CreateTranslation(new Vector3((-camera.CameraPosition / ConvertUnits.ToDisplayUnits(1)) - (screenCenter / ConvertUnits.ToDisplayUnits(1)), 0f))
                                                    * Matrix.CreateTranslation(new Vector3((screenCenter / ConvertUnits.ToDisplayUnits(1)), 0f))
                                                    * Matrix.CreateScale(new Vector3(CameraEditor.debugScale, CameraEditor.debugScale, 0f));

            debugView.RenderDebugData(ref projection, ref view);

            debugView.BeginCustomDraw(ref projection, ref view); //render custom points and shapes, with customisable colors
                                                                 //foreach (ObjPoint p in EditorVariables.PointList)
                                                                 //{ // no need anymore
                                                                 //    debugView.DrawPoint(p.position, p.Size, p.pointColor);
                                                                 //}
                                                                 /*foreach (EnemyProperties e in LevelProperties.Spawns.Ennemies)
                                                                 { //Draw the Spawn red points
                                                                     debugView.DrawPoint(e.spawnPoint.position, e.spawnPoint.Size, e.spawnPoint.pointColor);
                                                                 }
                                                                 foreach (ZonesProperties zone in LevelProperties.Zones)
                                                                 { // take each trap il all zones (time doesn't matter TEMPORAIRE)
                                                                     foreach (PassiveTrapsProperties pt in zone.PassiveTraps)
                                                                     { //Draw the Spawn red points
                                                                         debugView.DrawPoint(pt.spawnPoint.position, pt.spawnPoint.Size, pt.spawnPoint.pointColor);
                                                                     }
                                                                 }*/
            debugView.EndCustomDraw();
        }
        #endregion
    }
}