using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class Player
    {
        private GraphicsDevice GD;

        private Body playerBody;
        public  Vector2 playerPos;
        private PlayerRaycastWeb rayWeb;

        private Line lineTool;

        public Player(World world)
        {
            playerPos = new Vector2(970, 780);
            playerBody = BodyFactory.CreateRectangle(world, 80f, 128f, 1f, playerPos);

            rayWeb = new PlayerRaycastWeb();
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager Content, GraphicsDevice GD)
        {
            this.GD = GD;
            lineTool = new Line(GD);
        }

        public void Update(GameTime gameTime, InputManager input, World world)
        {
            if (input.KeyDown(Keys.Up))
                playerPos.Y -= 250 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.KeyDown(Keys.Down))
                playerPos.Y += 250 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.KeyDown(Keys.Left))
                playerPos.X -= 250 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (input.KeyDown(Keys.Right))
                playerPos.X += 250 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerBody.Position = playerPos;

            rayWeb.Update(world, playerPos);
        }

        

        public void Draw(SpriteBatch sb)
        {
            // Drawing the bottom lines that represent the raycasts
            lineTool.SetColor(Color.White); lineTool.SetThickness(1);
            lineTool.SetPosition(playerPos.X - 40, playerPos.Y + 128 / 2, playerPos.X - 40, playerPos.Y + 128 / 2 + 100); lineTool.Draw(sb);
            lineTool.SetPosition(playerPos.X     , playerPos.Y + 128 / 2, playerPos.X     , playerPos.Y + 128 / 2 + 100); lineTool.Draw(sb);
            lineTool.SetPosition(playerPos.X + 40, playerPos.Y + 128 / 2, playerPos.X + 40, playerPos.Y + 128 / 2 + 100); lineTool.Draw(sb);

            // Drawing the points detected by the bottom raycasts
            lineTool.SetColor(Color.Red); lineTool.SetThickness(10);
            foreach (List<Vector2> rays in rayWeb.GetBottomContacts())
            {
                foreach (Vector2 point in rays)
                {
                    lineTool.SetPosition(point.X - 2, point.Y - 5, point.X + 2, point.Y - 5);
                    lineTool.Draw(sb);
                }
            }
            
        }
    }

    public class PlayerRaycastWeb
    {
        RayCast BottomLeft, BottomCenter, BottomRight;

        public PlayerRaycastWeb()
        {

        }

        public void Update(World world, Vector2 playerPos)
        {
            Vector2 basePos = new Vector2(playerPos.X, playerPos.Y + 128 / 2);
            BottomLeft = new RayCast(world, new Vector2(basePos.X - 40, basePos.Y), new Vector2(basePos.X - 40, basePos.Y + 100));
            BottomCenter = new RayCast(world, new Vector2(basePos.X, basePos.Y), new Vector2(basePos.X, basePos.Y + 100));
            BottomRight = new RayCast(world, new Vector2(basePos.X + 40, basePos.Y), new Vector2(basePos.X + 40, basePos.Y + 100));
        } 

        public List<List<Vector2>> GetBottomContacts()
        {
            return new List<List<Vector2>>() { BottomLeft.GetClosestContacts(), BottomCenter.GetClosestContacts(), BottomRight.GetClosestContacts() };
        }
    }
}
