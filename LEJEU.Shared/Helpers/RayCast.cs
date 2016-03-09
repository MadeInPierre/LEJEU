using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LEJEU.Shared
{
    public class RayCast
    {
        private List<Vector2> CollisionPoints;
        public RayCast(World world, Vector2 start, Vector2 end)
        {
            CollisionPoints = new List<Vector2>();

            world.RayCast(ray, start, end);
        }

        public List<Vector2>GetClosestContacts()
        {
            if (CollisionPoints.Count == 0) CollisionPoints.Add(Vector2.Zero);
            return CollisionPoints;
        }

        private float ray(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            //Console.WriteLine(point.X + " " + point.Y + " " + normal.X + " " + normal.Y + " " + fraction);

            CollisionPoints.Add(point);

            return -1;
        }
    }
}
