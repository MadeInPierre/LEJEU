using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LEJEU.Entities;
using Microsoft.Xna.Framework;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

namespace LevelEditor.IAAuxiliary
{
    public class IAAux // stores the elements the ennemy's IA needs (path, Waypoints..)
    {
        protected bool Hidden;
        public EnemyProperties Parent; 

        public static Color IAAuxColor = new Color(100, 20, 100);

        public IAActivatedObject.SecondaryInfos NeededInfos;
        public string Text
        {
            get { return NeededInfos.ToString(); }
        }
        public bool CanBeMoved; // whether or not we can move the IAAuxs in the LevelEditor (yes for the DualWyapoints, but no for Radius e.g.)

        public IAAux() { }
        public virtual void Hide() { }
        public virtual void Show() { }
        public virtual void Delete() { }
    }
    
    class DualWaypoints : IAAux
    { // WHEN EXPORTING TO XML, USE WP1.BODYPOSITION AND NOT WP1pos/WP2pos
        public ObjRectangle WP1 { get; set; }
        public ObjRectangle WP2 { get; set; }

        private Vector2 WP1pos;
        public Vector2 WP1Position
        {
            get { return WP1pos; }
            set { WP1pos = value; WP1.BodyPosition = value; }
        }

        private Vector2 WP2pos;
        public Vector2 WP2Position
        {
            get { return WP2pos; }
            set { WP2pos = value; WP2.BodyPosition = value; }
        }

        public DualWaypoints(PreciseRectangle WP1rect, PreciseRectangle WP2rect)
        {
            NeededInfos = Enemy.SecondaryInfos.DualWaypoints;
            CanBeMoved = true;
            WP1 = new ObjRectangle(WP1rect.Position, WP1rect.Width, WP1rect.Height, 0f, "IAAux") { Color = IAAuxColor, Parent = this };
            WP1pos = WP1.BodyPosition;
            WP2 = new ObjRectangle(WP2rect.Position, WP2rect.Width, WP2rect.Height, 0f, "IAAux") { Color = IAAuxColor, Parent = this };
            WP2pos = WP2.BodyPosition;

            Hidden = false;
        }

        public override void Hide()
        {
            if (!Hidden)
            {
                WP1.Hide(); WP1pos = WP1.BodyPosition;
                WP2.Hide(); WP2pos = WP2.BodyPosition;
                Hidden = true;
            }
        }
        public override void Show()
        {
            if (Hidden)
            {
                WP1.Show();
                if (WP1.Hidden) WP1.BodyPosition = WP1pos;
                WP2.Show();
                if (WP2.Hidden) WP2.BodyPosition = WP2pos;
                Hidden = false;
            }
        }

        public override void Delete()
        {
            if (!Hidden)
            {
                WP1.Delete();
                WP2.Delete();
            }
        }
    }

    class Radius : IAAux
    {
        public ObjCircle RadiusCircle;
        public float radius { get { return RadiusCircle.Radius; } set { RadiusCircle.Radius = value; } }

        private Vector2 pos;
        public Vector2 Position
        {
            get { return RadiusCircle.BodyPosition; }
            set
            {
                pos = value;
                if (!RadiusCircle.Hidden)
                    RadiusCircle.BodyPosition = value;
            }
        }


        public Radius(Vector2 Position, float radius)
        {
            NeededInfos = Enemy.SecondaryInfos.Radius;
            CanBeMoved = false;
            RadiusCircle = new ObjCircle(Position, radius, 0f, "IAAux") { Color = IAAuxColor, Parent = this };
            pos = Position;

            Hidden = false;
        }

        public override void Hide()
        {
            if (!Hidden)
            {
                RadiusCircle.Hide();
                Hidden = true;
            }
        }
        public override void Show()
        {
            if (Hidden)
            {
                RadiusCircle.Show();
                RadiusCircle.BodyPosition = pos;
                Hidden = false;
            }
        }

        public override void Delete()
        {
            if(!Hidden)
                RadiusCircle.Delete();
        }
    }

    class ActivationZone : IAAux
    {
        /* MANUAL
            we create an ennemy. when spawned, is has a basic rectangle.
            we can right click on it to delete this rectangle if we don't want it
            In order to create a new one, select the ennemy (we must have it on the propertyGrid), select the IAAuxNeeded GridTiem.
                The collisionMode buttons will fade out (disable), and we can select the right State to create (polygon, rectangle, circle, edge)
                Create the body as always, but it will be stored in the selected ennemy's IAAux body.
        */
        public ObjectClass ActivationBody;

        public ActivationZone(PreciseRectangle spawnRect)
        { //create a rectangle by default. The user will be able to delete it in order to create another type of object.
            NeededInfos = Enemy.SecondaryInfos.ActivationZone;
            CanBeMoved = true;
            ActivationBody = new ObjRectangle(spawnRect.Position, spawnRect.Width, spawnRect.Height, 0f, "IAAux") { Color = IAAuxColor, Parent = this };

            Hidden = false;
        }

        public override void Hide()
        {
            if (!Hidden)
            {
                ActivationBody?.Hide();
                Hidden = true;
            }
        }
        public override void Show()
        {
            if (Hidden)
            {
                ActivationBody?.Show();
                Hidden = false;
            }
        }
        public override void Delete()
        {
            if(!Hidden)
                ActivationBody?.Delete();
        }
    }

    class IAPath : IAAux
    {
        public ObjEdgeChain PathObject { get; set; }

        public IAPath(List<Vector2> vertices)
        {
            NeededInfos = Enemy.SecondaryInfos.Path;
            CanBeMoved = true;

            PathObject = new ObjEdgeChain(vertices, "IAAux") { EdgesColor = IAAuxColor, Parent = this };

            Hidden = false;
        }
        public IAPath(Vector2 start, Vector2 end)
        {
            NeededInfos = Enemy.SecondaryInfos.Path;
            CanBeMoved = true;

            PathObject = new ObjEdgeChain(start, end) { UserData = "IAAux", EdgesColor = IAAuxColor, Parent = this };

            Hidden = false;
        }

        public override void Hide()
        {
            if (!Hidden)
            {
                PathObject?.Hide();
                Hidden = true;
            }
        }
        public override void Show()
        {
            if (Hidden)
            {
                PathObject?.Show();
                if (PathObject != null)
                    PathObject.UserData = "IAAux";

                Hidden = false;
            }
        }
        public override void Delete()
        {
            if(!Hidden)
                PathObject?.Delete();
        }
    }
}

