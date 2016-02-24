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
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Collision.Shapes;
#endregion

namespace LEJEU.Shared
{
    public class Player
    {

        public static Body torso, wheel, sensor, feet;
        public RevoluteJoint motorJoint;
        public Texture2D playerImg;

        public bool SimpleMoving = false;
        public bool OnGround = false;
        public bool CanClimb = true;

        public Player(World world, Vector2 pos)
        {
            #region Player's bodies/sensors
            sensor = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(54f), ConvertUnits.ToSimUnits(100f), 0.000001f);
            sensor.Position = ConvertUnits.ToSimUnits(pos);
            sensor.BodyType = BodyType.Dynamic;
            sensor.IsSensor = true;
            sensor.FixedRotation = true;
            sensor.UserData = "playerSensor";

            feet = BodyFactory.CreateRectangle(world, 0.6f, 0.2f, 0.000001f);
            feet.Position = ConvertUnits.ToSimUnits(pos) + new Vector2(0, 1.18f);
            feet.BodyType = BodyType.Dynamic;
            feet.IsSensor = true;
            feet.FixedRotation = true;
            feet.UserData = "playerFeet";

            torso = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(49), ConvertUnits.ToSimUnits(47.5), 0.7f);
            wheel = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(24), 0.7f);
            torso.Position = ConvertUnits.ToSimUnits(pos);
            torso.BodyType = BodyType.Dynamic;
            wheel.BodyType = BodyType.Dynamic;
            torso.UserData = "torso";
            wheel.UserData = "wheel";
            wheel.Position = torso.Position + ConvertUnits.ToSimUnits(new Vector2(0, 47.5f / 2f));
            wheel.Friction = 2.8f;
            torso.FixedRotation = true;
            motorJoint = JointFactory.CreateRevoluteJoint(world, torso, wheel, Vector2.Zero);
            motorJoint.CollideConnected = false;
            motorJoint.MotorEnabled = true;
            motorJoint.MotorSpeed = 0;
            motorJoint.MotorImpulse = 3.0f;
            motorJoint.MaxMotorTorque = 10;


            JointFactory.CreateRevoluteJoint(world, torso, sensor, Vector2.Zero);
            JointFactory.CreateRevoluteJoint(world, sensor, feet,  Vector2.Zero);

            sensor.IgnoreCollisionWith(torso);
            sensor.IgnoreCollisionWith(wheel);

            sensor.OnCollision += new OnCollisionEventHandler(sensorOnCollision);
            sensor.OnSeparation += new OnSeparationEventHandler(sensorOnSeparation);

            feet.IgnoreCollisionWith(sensor); feet.IgnoreCollisionWith(wheel); feet.IgnoreCollisionWith(torso);

            wheel.Mass = 12f;
#endregion
        }
        public void LoadContent(ContentManager Content)
        {
            playerImg = Content.Load<Texture2D>("player");
        }

        public void Update(InputManager input)
        {
            #region ResetPosition
            if (input.KeyPressed(Keys.R))
            {
                torso.Position = new Vector2(1, 1);
                wheel.Position = new Vector2(1, 1);
                sensor.Position = new Vector2(1, 1);
                feet.Position = new Vector2(1, 1);
            }
            #endregion

            if (SimpleMoving)
            {
                torso.IsStatic = true; wheel.IsStatic = true; sensor.IsStatic = true;
                if (input.KeyDown(Keys.Up) || input.PadState.ThumbSticks.Left.Y > 0.5f)
                {
                    float amount = input.PadState.ThumbSticks.Left.Y;
                    if (input.KeyDown(Keys.Up)) amount = 1;
                    torso.SetTransform(new Vector2(torso.Position.X, torso.Position.Y - 0.1f * amount), 0);
                    wheel.SetTransform(new Vector2(wheel.Position.X, wheel.Position.Y - 0.1f * amount), 0);
                }

                if (input.KeyDown(Keys.Down) || input.PadState.ThumbSticks.Left.Y < -0.5f)
                {
                    float amount = -input.PadState.ThumbSticks.Left.Y;
                    if (input.KeyDown(Keys.Down)) amount = 1;
                    torso.SetTransform(new Vector2(torso.Position.X, torso.Position.Y + 0.1f * amount), 0);
                    wheel.SetTransform(new Vector2(wheel.Position.X, wheel.Position.Y + 0.1f * amount), 0);
                }

                if (input.KeyDown(Keys.Left) || input.PadState.ThumbSticks.Left.X < -0.5f)
                {
                    float amount = -input.PadState.ThumbSticks.Left.X;
                    if (input.KeyDown(Keys.Left)) amount = 1;
                    torso.SetTransform(new Vector2(torso.Position.X - 0.1f * amount, torso.Position.Y), 0);
                    wheel.SetTransform(new Vector2(wheel.Position.X - 0.1f * amount, wheel.Position.Y), 0);
                }

                if (input.KeyDown(Keys.Right) || input.PadState.ThumbSticks.Left.X > 0.5f)
                {
                    float amount = input.PadState.ThumbSticks.Left.X;
                    if (input.KeyDown(Keys.Right)) amount = 1;
                    torso.SetTransform(new Vector2(torso.Position.X + 0.1f * amount, torso.Position.Y), 0);
                    wheel.SetTransform(new Vector2(wheel.Position.X + 0.1f * amount, wheel.Position.Y), 0);
                }
                torso.IsStatic = false; wheel.IsStatic = false; sensor.IsStatic = false;
            }
            else
            {

                motorJoint.MotorSpeed = 0f;

                if ((input.KeyPressed(Keys.Up) || input.ButtonPressed(Buttons.A)) /*&& feet.ContactList != null*/)
                {
                    wheel.ApplyLinearImpulse(new Vector2(0, -8.5f));
                    CanClimb = true;
                }
                else if (input.KeyReleased(Keys.Up) || input.ButtonReleased(Buttons.A))
                {
                    CanClimb = false;
                }
                if (input.KeyState.IsKeyDown(Keys.Left) || input.PadState.ThumbSticks.Left.X < -0.5f)
                {
                    float amount = -input.PadState.ThumbSticks.Left.X;
                    if (input.KeyState.IsKeyDown(Keys.Left)) amount = 1;
                    motorJoint.MotorSpeed = -MathHelper.TwoPi * 2.5f * amount;
                    wheel.ApplyLinearImpulse(new Vector2(-0.5f * amount, 0));
                }
                if (input.KeyState.IsKeyDown(Keys.Right) || input.PadState.ThumbSticks.Left.X > 0.5f)
                {
                    float amount = input.PadState.ThumbSticks.Left.X;
                    if (input.KeyState.IsKeyDown(Keys.Right)) amount = 1;
                    motorJoint.MotorSpeed = MathHelper.TwoPi * 2.5f * amount;
                    wheel.ApplyLinearImpulse(new Vector2(0.5f * amount, 0));
                }
                if (input.KeyDown(Keys.Down) || input.ButtonPressed(Buttons.B))
                {
                    var c = sensor.ContactList;
                    while (c != null)
                    {
                        string ufA = "", ufB = "";
                            if (c.Contact.FixtureA.Body.UserData != null)               //Protection against nullException
                                ufA = c.Contact.FixtureA.Body.UserData.ToString();
                            if (c.Contact.FixtureB.Body.UserData != null)
                                ufB = c.Contact.FixtureB.Body.UserData.ToString();
                        if (c.Contact.IsTouching && (ufA == "semi" || ufB == "semi"))
                        {
                            torso.IgnoreCollisionWith(c.Contact.FixtureA.Body);
                            torso.IgnoreCollisionWith(c.Contact.FixtureB.Body);
                            wheel.IgnoreCollisionWith(c.Contact.FixtureA.Body);
                            wheel.IgnoreCollisionWith(c.Contact.FixtureB.Body);
                        }
                        c = c.Next;
                    }
                }
            }

        }
        public void Draw(SpriteBatch sb, float alpha)
        {
            sb.Draw(playerImg, ConvertUnits.ToDisplayUnits(torso.Position) - new Vector2(24, 24), Color.White * alpha);
        }

        
        public bool sensorOnCollision(Fixture fA, Fixture fB, Contact contact)
        {
            if(fB.Body.UserData != null)
            {
                string ufA = fA.Body.UserData.ToString();
                string ufB = fB.Body.UserData.ToString();
                if (ufB == "floor")
                {
                    OnGround = true;
                }
                if (ufB == "semi")
                {
                    Vector2 v1 = ((EdgeShape)fB.Shape).Vertex1;
                    Vector2 v2 = ((EdgeShape)fB.Shape).Vertex2;

                    if (v1.X > v2.X)
                    {
                        Vector2 temp;

                        temp = v1;
                        v1 = v2;
                        v2 = temp;
                    }

                    float a = (v2.Y - v1.Y) / (v2.X - v1.X);
                    float b = v1.Y - (a * v1.X);

                    //on a donc y = ax + b

                    if (sensor.Position.Y > (a * sensor.Position.X + b)) //si le player est en bas
                    {
                        torso.IgnoreCollisionWith(fB.Body);
                        wheel.IgnoreCollisionWith(fB.Body);
                        //OnSemi = false;
                    }
                    //else OnSemi = true;
                }
                if (ufB == "ladder")
                {
                    SimpleMoving = true;
                }
        }
            return true;
        }

        public void sensorOnSeparation(Fixture fA, Fixture fB)
        {
            if (fB.Body.UserData != null)
            {
                string ufA = fA.Body.UserData.ToString();
                string ufB = fB.Body.UserData.ToString();
                if (ufB == "floor")
                    OnGround = false;
                if (ufB == "semi")
                {
                    var c = sensor.ContactList;
                    bool touchingSemi = false;
                    while (c != null)
                    {
                        if (c.Contact.IsTouching && (c.Contact.FixtureA.Body.UserData.ToString() == "semi" || c.Contact.FixtureB.Body.UserData.ToString() == "semi"))
                        {
                            touchingSemi = true;
                            break;
                        }
                        c = c.Next;
                    }


                    if (touchingSemi == false)
                    {
                        torso.RestoreCollisionWith(fB.Body);
                        wheel.RestoreCollisionWith(fB.Body);
                    }
                }
                if (ufB == "ladder")
                {
                        SimpleMoving = false;
                }
            }
        }


        /*public bool feetOnCollision(Fixture fA, Fixture fB, Contact contact)
        {
            CanJump = true;
            return true;
        }
        public void feetOnSeparation(Fixture fA, Fixture fB)
        {
            CanJump = false;
        }*/



    }
}