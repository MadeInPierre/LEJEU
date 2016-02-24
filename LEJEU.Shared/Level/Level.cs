#region usings
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
//using Microsoft.Xna.Framework.GamerServices;

using FarseerPhysics.Collision;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Controllers;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
#endregion

namespace LEJEU.Shared
{
	public class Level
	{

		DebugViewXNA debugView;
		float TexturesAlpha = 1;
		MapZonesManager mapZonesManager;

		Texture2D blackPause;
		Player player;
		Camera camera;
		
		LevelProperties LP;
		
		World world;
		List<Platform> floors;
		Platform ladder;

		float debugScale = 1;


		public Level ()
		{

		}
		
		public void Initialize()
		{
			ConvertUnits.SetDisplayUnitToSimUnitRatio(42.2f);
			LP = LevelLoader.LoadLevelProperties(LP, "Maps/Level1.1/properties1.xml");

			world = new World(new Vector2(0f, 9.82f));
			player = new Player(world, LP.playerSpawnPos);
			camera = new Camera();
			camera.Initialize((int)LP.playerSpawnPos.X);

			mapZonesManager = new MapZonesManager(LP);

			
			
			
			floors = new List<Platform>();

			Vertices verts = new Vertices();
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(0,844)));   //SOL PRINCIPAL
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(110,844))); 
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(191,806)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(278,780)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(335,783)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(429,837)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(460,842))); 
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(500,831)));    
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(515,817)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(621,826)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(630,834)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(952,830)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1004,786)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1021,779)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1073,772)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1113,727)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1148,713)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1185,666)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1205,659)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1315,669)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1348,691)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1376,746)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1445,815)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1569,830)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1570, 900)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(0, 900)));
			floors.Add(new Platform(world, Vector2.Zero, verts, BodyType.Static, 0.2f, 2f, 10f, "floor", false, true, true));

			verts.Clear();
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1570,445))); //HAUT DE LA GROTTE
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1531,441))); 
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1485,442)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1431,459)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1349,505)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1294,526)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1248,546))); 
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1222,573)));    
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1225,581)));  
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1310,584)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1353,593)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1379,599)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1402,592)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1431,579)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1455,582)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1496,566)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(1570,546)));
		   floors.Add(new Platform(world, Vector2.Zero, verts, BodyType.Static, 0.2f, 2f, 10f, "floor", false, true, true));

			verts.Clear();
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(364,502))); //FEUILLE DE DROITE
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(471,523)));
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(531,532)));
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(590,516)));
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(558,533)));
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(498,540)));
				 verts.Add(ConvertUnits.ToSimUnits(new Vector2(372,520)));
			floors.Add(new Platform(world,Vector2.Zero,verts,BodyType.Static,0.2f,2f,10f,"floor",false,true,true));

			verts.Clear();
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(216,313))); //FEUILLE DE GAUCHE
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(62,367)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(8,370)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(48,383)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(125,370)));
			floors.Add(new Platform(world, Vector2.Zero, verts, BodyType.Static, 0.2f, 2f, 10f, "floor", false, true, true));



			/////////////////////////////////////////////////////////////////////////////////////////////
			verts.Clear();
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(341,515))); //ECHELLE
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(304,777)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(278,776)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(319,502)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(290,380)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(359,190)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(399,222)));
				verts.Add(ConvertUnits.ToSimUnits(new Vector2(319,402)));
			ladder = new Platform(world, Vector2.Zero, verts, BodyType.Static, 0f, 2f, 10f, "ladder", true, true, true);
			/////////////////////////////////////////////////////////////////////////////////////////////
			verts.Clear();


			Path semiPath = new Path();
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(234,0))); //SEMI
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(295,87)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(399,207)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(485,275)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(603,342)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(766,372)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(875,356)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(993,310)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1113,257)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1191,212)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1316,136)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1415,54)));
				semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1460, 0))); semiPath.Add(ConvertUnits.ToSimUnits(new Vector2(1460, 0)));
			semiPath.Closed = false;

			Body semi = BodyFactory.CreateBody(world);
			semi.BodyType = BodyType.Static;

			//Static shape made up of edges
			PathManager.ConvertPathToEdges(semiPath, semi, semiPath.ControlPoints.Count);
			semi.Position -= new Vector2(0, 0);
			semi.UserData = "semi";
			semi.Friction = 10f;


			BodyFactory.CreateEdge(world, new Vector2(0, 0), ConvertUnits.ToSimUnits(new Vector2(0,ResolutionManager.GameRes.Y))); //edge a la zone 0
			BodyFactory.CreateEdge(world, ConvertUnits.ToSimUnits(new Vector2((LP.NZones + 1) * ResolutionManager.GameRes.X, 0)),  //edge a la derniere zone
				ConvertUnits.ToSimUnits(new Vector2((LP.NZones + 1) * ResolutionManager.GameRes.X, ResolutionManager.GameRes.Y)));

			BodyFactory.CreateEdge(world, ConvertUnits.ToSimUnits(new Vector2(0, ResolutionManager.GameRes.Y)), // edge par terre
				ConvertUnits.ToSimUnits(new Vector2((LP.NZones + 1) * ResolutionManager.GameRes.X, ResolutionManager.GameRes.Y)));

		}
		public void LoadContent(ContentManager Content)
		{

			blackPause = Content.Load<Texture2D>("blackPause");

			player.LoadContent(Content);
			mapZonesManager.LoadContent(Content, LP);  /* Load Level */

			debugView = new DebugViewXNA(world);
				debugView.AppendFlags(DebugViewFlags.Shape);
				debugView.AppendFlags(DebugViewFlags.PolygonPoints);
			debugView.LoadContent(Game1.graphicsDevice, Content);
			
		}
		public void UnloadContent()
		{
			mapZonesManager.UnloadContent();
		}
		public void Update(GameTime gameTime, InputManager input)
		{
			/* if(play.!moving) temps défile (fonction) + effets sur ecran
			 * 
			 * - gérer stack (a partir de CameraPosition) - check posX et posX + width s'ils sont ds l'ecran
			 *   de ts les objets (pr afficher objets et ennemis)
			 * - gérer mini-stack (a partir de playerPos et stack - prendre que qui est a un rayon de x metres) Farseer ?
			 * - Collisions:
			 * 		-mini-stack: player/terrain + ennemis + projectiles + items + objectifs
			 * 		- Level entier: ennemis mobiles + objets + projectiles / terrain
			 * - Update position des ennemis, projectiles
			 * - update player position
			 * - gérer caméra
			 * - gérer animations du stack
			 * - gérer animations terrain fixes/locales dans les zones
			 * - gérer météo
			 */

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


			//player.handleInput(inputManager,gameTime);
			//player.Update(camera, gameTime, inputManager, mapZonesManager.CurrentZone, LP.NZones/*, typeCollision*/);

			if (input.KeyDown(Keys.Q)) debugScale *= 1.05f;
			if (input.KeyDown(Keys.A)) debugScale *= 0.95f;
			if (input.KeyPressed(Keys.D)) { if (TexturesAlpha == 1) TexturesAlpha = 0.1f; else TexturesAlpha = 1f; }


			world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f)));
			player.Update(input);
			mapZonesManager.Update(input, camera, player, LP);

			camera.Update(input, ConvertUnits.ToDisplayUnits(Player.torso.Position), LP, debugScale);
			
			
			
			
		}
		public void Draw(SpriteBatch sb)
		{
				/*  Drawing order
				 * >Eau<
					Map Background layers
					Map Platforms
					Objects, Decos
					Lights
					Objective
					Ennemies Imm
					Ennemies Mob
					Player
					Map Foreground
					Météo
					HUD
					(Pause)

					n'afficher que les elements faisant partie du stack
			 */
			sb.End();
			//Vector2 screenCenter = new Vector2(ScreenManager.Instance.Dimensions.X / 2, ScreenManager.Instance.Dimensions.Y / 2);

			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
				camera.ViewMatrix);
					//* Matrix.CreateTranslation(new Vector3(screenCenter.X, screenCenter.Y, 0f)));
			
				

					mapZonesManager.DrawBackGrounds(sb, camera, TexturesAlpha);

					mapZonesManager.DrawPlatforms(sb, TexturesAlpha);

					player.Draw(sb, TexturesAlpha);

					mapZonesManager.DrawForeground(sb, TexturesAlpha);
			
				sb.End();
			sb.Begin();
				renderDebugView();
		}

		public void drawPause(SpriteBatch sb)
		{
			Game1.graphicsDevice.Clear(Color.Black * 0.7f);

			sb.Draw(blackPause, new Vector2(0, 0), Color.White * 0.7f);
		}

		void renderDebugView()
		{
			Matrix projection = Matrix.CreateOrthographicOffCenter(0f, ResolutionManager.WindowRes.X / ConvertUnits.ToDisplayUnits(1) / ResolutionManager.WindowScale,
				ResolutionManager.WindowRes.Y / ConvertUnits.ToDisplayUnits(1) / ResolutionManager.WindowScale, 0f, 0f, 1f);
			
			Vector2 screenCenter = new Vector2(ResolutionManager.WindowRes.X / 2, ResolutionManager.WindowRes.Y / 2);
			Matrix view = Matrix.CreateTranslation(new Vector3((-camera.CameraPosition / ConvertUnits.ToDisplayUnits(1)) - (screenCenter / ConvertUnits.ToDisplayUnits(1)), 0f))
							  * Matrix.CreateTranslation(new Vector3((screenCenter / ConvertUnits.ToDisplayUnits(1)), 0f))
							  * Matrix.CreateScale(new Vector3(debugScale, debugScale, 0f));

			debugView.RenderDebugData(ref projection, ref view);
		}
	}
}
