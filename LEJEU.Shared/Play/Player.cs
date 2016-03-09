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
            Line line = new Line(new Vector2(playerPos.X - 40, playerPos.Y + 128 / 2), new Vector2(playerPos.X - 40, playerPos.Y + 128 / 2 + 100), 1, Color.White, GD);
            line.Update(); line.Draw(sb);
            line      = new Line(new Vector2(playerPos.X,      playerPos.Y + 128 / 2), new Vector2(playerPos.X,      playerPos.Y + 128 / 2 + 100), 1, Color.White, GD);
            line.Update(); line.Draw(sb);
            line      = new Line(new Vector2(playerPos.X + 40, playerPos.Y + 128 / 2), new Vector2(playerPos.X + 40, playerPos.Y + 128 / 2 + 100), 1, Color.White, GD);
            line.Update(); line.Draw(sb);

            // Drawing the points detected by the bottom raycasts
            foreach (List<Vector2> rays in rayWeb.GetBottomContacts())
            {
                foreach (Vector2 point in rays)
                {
                    Line collisionLine = new Line(new Vector2(point.X - 2, point.Y - 5), new Vector2(point.X + 2, point.Y - 5), 10, Color.Red, GD);
                    collisionLine.Update();
                    collisionLine.Draw(sb);
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
