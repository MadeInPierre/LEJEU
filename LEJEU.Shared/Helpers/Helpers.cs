﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    class Helpers
    {
    }

    public class Line
    {
        Texture2D pixel;
        public Vector2 p1, p2; //this will be the position in the center of the line
        int length, thickness; //length and thickness of the line, or width and height of rectangle
        Rectangle rect; //where the line will be drawn
        float rotation; // rotation of the line, with axis at the center of the line
        Color color;


        //p1 and p2 are the two end points of the line
        public Line(Vector2 p1, Vector2 p2, int thickness, Color color, GraphicsDevice GD)
        {
            pixel = new Texture2D(GD, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new Color[] { Color.White});
            this.p1 = p1;
            this.p2 = p2;
            this.thickness = thickness;
            this.color = color;
        }
        public Line(GraphicsDevice GD)
        {
            pixel = new Texture2D(GD, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new Color[] { Color.White });
        }

        public void Update()
        {
            length = (int)Vector2.Distance(p1, p2); //gets distance between the points
            rotation = getRotation(p1.X, p1.Y, p2.X, p2.Y); //gets angle between points(method on bottom)
            rect = new Rectangle((int)p1.X, (int)p1.Y, length, thickness);

            //To change the line just change the positions of p1 and p2
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, rect, null, color, rotation, Vector2.Zero, SpriteEffects.None, 0.0f);
        }

        public void SetPosition(Vector2 start, Vector2 end)
        {
            p1 = start;
            p2 = end;
        }
        public void SetPosition(float startX, float startY, float endX, float endY)
        {
            p1.X = startX;
            p1.Y = startY;
            p2.X = endX;
            p2.Y = endY;

            Update();
        }
        public void SetThickness(int value)
        {
            thickness = value;
        }
        public void SetColor(Color value)
        {
            color = value;
        }

        //this returns the angle between two points in radians 
        private float getRotation(float x, float y, float x2, float y2)
        {
            float adj = x - x2;
            float opp = y - y2;
            float tan = opp / adj;
            float res = MathHelper.ToDegrees((float)Math.Atan2(opp, adj));
            res = (res - 180) % 360;
            if (res < 0) { res += 360; }
            res = MathHelper.ToRadians(res);
            return res;
        }
    }
}
