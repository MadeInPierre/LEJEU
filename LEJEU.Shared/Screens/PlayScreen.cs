using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.DebugView;
using Microsoft.Xna.Framework.Content;
using FarseerPhysics.Common.Decomposition;
using Microsoft.Xna.Framework.Input;

namespace LEJEU.Shared
{
    public class PlayScreen : Screen
    {
        DebugViewXNA debugView;

        World world;
        List<List<int>> PolyList;
        List<Body> PolyBodies;

        Texture2D bg;

        Player player;

        public PlayScreen()
        {
            
        }

        public override void Initialize()
        {
            ScreenMessage = null;
            ScreenStatus = "RUNNING";
            
            world = new World(new Vector2(0, 9.81f));
            debugView = new DebugViewXNA(world);
            //ConvertUnits.SetDisplayUnitToSimUnitRatio(42.2f);

            PolyList = new List<List<int>>();
            PolyList.Add(new List<int>() { 1, 1011, 138, 1011, 242, 963, 338, 933, 414, 939, 518, 999, 557, 1007, 614, 994, 629, 975, 761, 990, 774, 999, 880, 1079, 1, 1079 });
            PolyList.Add(new List<int>() { 772, 998, 987, 992, 1149, 994, 1186, 984, 1225, 944, 1241, 932, 1314, 924, 1364, 869, 1405, 850, 1445, 799, 1476, 786, 1609, 797, 1646,
                                           821, 1682, 868, 1686, 888, 1716, 928, 1767, 973, 1793, 980, 1920, 993, 1920, 1079, 723, 1079 });
            PolyList.Add(new List<int>() { 1919, 657, 1786, 700, 1754, 692, 1706, 716, 1678, 716, 1615, 702, 1506, 700, 1493, 693, 1494, 682, 1519, 652, 1626, 616, 1713, 565, 1803, 532, 1862, 528, 1920, 535 });
            PolyBodies = new List<Body>();
            foreach (var poly in PolyList)
            {
                Vertices verts = new Vertices();
                for (int i = 0; i < poly.Count; i += 2)
                    verts.Add(new Vector2(poly[i], poly[i + 1]));

                if (verts.IsConvex()) PolyBodies.Add(BodyFactory.CreatePolygon(world, verts, 1f));
                else
                {
                    List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                    PolyBodies.Add(BodyFactory.CreateCompoundPolygon(world, list, 1f));
                }
            }

            player = new Player(world);
        }

        public override void LoadContent(ContentManager Content, GraphicsDevice GD)
        {
            debugView = new DebugViewXNA(world);
            debugView.AppendFlags(DebugViewFlags.Shape);
            debugView.AppendFlags(DebugViewFlags.PolygonPoints);
            debugView.LoadContent(GD, Content);

            bg = Content.Load<Texture2D>("Maps/Chapter1/level1.1/zone1");

            player.LoadContent(Content, GD);
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
            player.Update(gameTime, input, world);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(bg, Vector2.Zero, Color.White);

            sb.End();
            sb.Begin();

            var projection = Matrix.CreateOrthographicOffCenter(0f, 1920, 1080, 0f, 0f, 1f);
            debugView.RenderDebugData(ref projection);

            sb.End();
            sb.Begin();

            player.Draw(sb);
        }
    }
}
