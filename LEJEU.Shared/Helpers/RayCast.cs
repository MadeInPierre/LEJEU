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
        private List<float> CollisionFractions;

        public RayCast(World world, Vector2 start, Vector2 end)
        {
            CollisionPoints = new List<Vector2>();
            CollisionFractions = new List<float>();

            world.RayCast(ray, start, end);
        }

        public void Refresh(World world, Vector2 start, Vector2 end)
        {
            CollisionPoints.Clear();
            CollisionFractions.Clear();

            world.RayCast(ray, start, end);
        }

        public List<Vector2> GetContacts()
        {
            if (CollisionPoints.Count == 0) CollisionPoints.Add(Vector2.Zero);
            return CollisionPoints;
        }

        public Vector2 GetClosest()
        {
            if(CollisionPoints.Count != 0)
            {
                float record = CollisionFractions[0];
                int index = 0;
                for (int i = 0; i < CollisionFractions.Count; i++)
                {
                    if (CollisionFractions[i] < record)
                    {
                        record = CollisionFractions[i];
                        index = i;
                    }
                };
                return CollisionPoints[index];
            }
            return Vector2.Zero;
        }

        private float ray(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            //Console.WriteLine(point.X + " " + point.Y + " " + normal.X + " " + normal.Y + " " + fraction);

            CollisionPoints.Add(point);
            CollisionFractions.Add(fraction);
            return -1;
        }
    }
}
