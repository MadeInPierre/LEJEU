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
        public class Platform
        {
            public Body platformBody;

            /// <summary>
            /// Creer un polygone
            /// </summary>
            public Platform(World world, Vector2 pos, Vertices verts, BodyType bodyType, //CREATION DUN POLYGONE
                            float restitution, float friction, float density, string userData, bool isSensor,
                            bool onCollisionEnabled, bool onSeparationEnabled)
            {

                if (verts.IsConvex()) platformBody = BodyFactory.CreatePolygon(world, verts, density);
                else
                {
                    List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                    platformBody = BodyFactory.CreateCompoundPolygon(world, list, density);
                }

                platformBody.Position = pos;
                platformBody.BodyType = bodyType;
                platformBody.Restitution = restitution;
                platformBody.Friction = friction;
                platformBody.UserData = userData;
                platformBody.IsSensor = isSensor;

                if (onCollisionEnabled) platformBody.OnCollision += new OnCollisionEventHandler(onCollision);
                if (onSeparationEnabled) platformBody.OnSeparation += new OnSeparationEventHandler(onSeparation);
            }

            /// <summary>
            /// Creer un rectangle
            /// </summary>
            public Platform(World world, Vector2 pos, Vector2 size, BodyType bodyType, //CREATION DUN RECTANGLE
                            float restitution, float friction, float density, string userData, bool isSensor,
                            bool onCollisionEnabled, bool onSeparationEnabled)
            {


                platformBody = BodyFactory.CreateRectangle(world, size.X, size.Y, density);

                platformBody.Position = pos;
                platformBody.BodyType = bodyType;
                platformBody.Restitution = restitution;
                platformBody.Friction = friction;
                platformBody.UserData = userData;
                platformBody.IsSensor = isSensor;

                if (onCollisionEnabled) platformBody.OnCollision += new OnCollisionEventHandler(onCollision);
                if (onSeparationEnabled) platformBody.OnSeparation += new OnSeparationEventHandler(onSeparation);
            }

            //public bool inPlatform;
            public bool onCollision(Fixture fA, Fixture fB, FarseerPhysics.Dynamics.Contacts.Contact contact)
            {
                string ufA = fA.Body.UserData.ToString();
                string ufB = fB.Body.UserData.ToString();
                return true;
            }
            public void onSeparation(Fixture fA, Fixture fB)
            {
                string ufA = fA.Body.UserData.ToString();
                string ufB = fB.Body.UserData.ToString();
            }
        }
    }
