#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
#endregion

namespace LevelEditor
{ // All the Object Classes :

    #region General Object
    public class ObjectClass
    {
        public Body body;
        public string userData;
        bool alive;
        bool hidden;

        public object Parent;

        private Color color;

        //protected Vector2 position;

        public enum ObjectType { Circle, Rectangle, Polygon, Point, Edge, EdgeChain, Mouse, Null };
        public ObjectType objType;

        public ObjectType objectType
        {
            get { return objType; }
        }

        public Vector2 BodyPosition
        {
            get { return body.Position; }
            set { body.Position = value; }
        }

        public string UserData
        {
            get { return userData; }
            set { userData = value; }
        }

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public bool Hidden
        {
            get { return hidden; }
            set {
                hidden = value; }
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;

                //if (objType == ObjectType.EdgeChain)
                //{
                //    foreach (ObjEdge e in ((ObjEdgeChain)this).edges)
                //    {
                //        e.Color = value;
                //    }
                //}
                //else
                if(body != null)
                    body.BodyColor = value;
            }
        }

        public void Delete()
        {
            if (Alive && !Hidden)
            {
                if (objType == ObjectType.EdgeChain)
                { //if we are hiding an edgeChain, hide all of the sub-edges
                    foreach (ObjEdge e in ((ObjEdgeChain)this).edges)
                    {
                        try { EditorVariables.world.RemoveBody(e.body); }
                        catch { }
                    }
                }
                else
                {
                    try { if (!hidden) EditorVariables.world.RemoveBody(body); }
                    catch { }
                }

                Alive = false;
            }
        }

        public void Hide()
        {
            if (!hidden)
            {
                if(objType == ObjectType.EdgeChain)
                { //if we are hiding an edgeChain, hide all of the sub-edges
                    foreach (ObjEdge e in ((ObjEdgeChain)this).edges)
                    {
                        try { EditorVariables.world.RemoveBody(e.body); }
                        catch { }
                        e.hidden = true;
                    }
                    hidden = true;
                }
                else
                {
                    try
                    {
                        EditorVariables.world.RemoveBody(body); hidden = true;
                    }
                    catch { }
                }

                
            }
        }
        public virtual void Show() { }
    }
    #endregion

    #region Circle
    public class ObjCircle : ObjectClass
    {
        private float radius;

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                Body backup = body;
                try { EditorVariables.world.RemoveBody(body); }
                catch { }

                body = BodyFactory.CreateCircle(EditorVariables.world, radius, 1f);
                body.Rotation = backup.Rotation;
                body.Position = backup.Position;
                body.BodyColor = backup.BodyColor;
                body.BodyType = BodyType.Static;
                body.UserData = this;
            }
        }

        public ObjCircle(Vector2 InitialPosition, float Radius, float Angle)
        {
            body = BodyFactory.CreateCircle(EditorVariables.world, Radius, 1.0f, InitialPosition);
            //ObjectPhysic.Last().Body.Rotation = InitialAngle;
            body.Rotation = Angle;
            //Give it some bounce and friction
            body.Restitution = 0.5f;
            body.Friction = 1.0f;
            body.Position = InitialPosition;
            body.UserData = this;

            Alive = true;
            radius = Radius;
            //position = InitialPosition;

            objType = ObjectType.Circle;
        }

        public ObjCircle(Vector2 InitialPosition, float Radius, float Angle, string uD)
        {
            body = BodyFactory.CreateCircle(EditorVariables.world, Radius, 1.0f, InitialPosition);
            //ObjectPhysic.Last().Body.Rotation = InitialAngle;
            body.Rotation = Angle;
            //Give it some bounce and friction
            body.Restitution = 0.5f;
            body.Friction = 1.0f;
            body.Position = InitialPosition;
            body.UserData = this;
            userData = uD;
            Alive = true;
            radius = Radius;
            //position = InitialPosition;

            objType = ObjectType.Circle;
        }

        public override void Show()
        {
            if(Hidden/* && !EditorVariables.world.BodyList.Contains(body)*/)
            {
                body = BodyFactory.CreateCircle(EditorVariables.world, radius, 1f, BodyPosition);
                body.UserData = this;
                body.BodyColor = Color;

                Hidden = false;
            }
        }
    }
    #endregion

    #region Rect
    public class ObjRectangle : ObjectClass
    {
        Vector2 size;
        private float angle;

        public Vector2 CenterPosition //useless ?
        {
            get { return body.Position - Size / 2; }
            set { body.Position = value + Size / 2; }
        }

        public PreciseRectangle Bounds
        {
            get { return new PreciseRectangle(body.Position.X, body.Position.Y, size.X, size.Y); }
        }

        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                body.Rotation = value /** (float)Math.PI / 180.0f*/;
            }
        }

        public Vector2 Size
        { // recreate a rectngle with the new size, as we can't get/set the size of a rectangle in farseer.
            get { return size; }
            set
            {
                size = value;
                EditorVariables.world.RemoveBody(body);
                Body backup = body;
                body = BodyFactory.CreateRectangle(EditorVariables.world, size.X, size.Y, 1f);
                body.Rotation = backup.Rotation;
                body.Position = backup.Position;
                body.BodyType = BodyType.Static;
                body.BodyColor = backup.BodyColor;
                body.UserData = this;
                Rectangle r = new Rectangle(2, 3, 4, 5);
            }
        }
        public ObjRectangle(Vector2 InitialPosition, float width, float height, float Angle)
        {
            angle = Angle;
            //ObjectPhysic.Add(FixtureFactory.CreateRectangle(EditorVariables.worldPhysic, width, height, 1.0f, InitialPosition));
            body = BodyFactory.CreateRectangle(EditorVariables.world, width, height, 1f);
            body.Rotation = Angle * (float)Math.PI / 180.0f;
            body.Position = InitialPosition;



            body.UserData = this;
            Alive = true;
            //position = InitialPosition;
            angle = Angle;
            size.X = width;
            size.Y = height;
            Hidden = false;
            objType = ObjectType.Rectangle;
        }

        public ObjRectangle(Vector2 InitialPosition, float width, float height, float InitialAngle, Color color)
        { //when we want to have an object with a different color
            angle = InitialAngle;
            //ObjectPhysic.Add(FixtureFactory.CreateRectangle(EditorVariables.worldPhysic, width, height, 1.0f, InitialPosition));
            body = BodyFactory.CreateRectangle(EditorVariables.world, width, height, 1f, Vector2.Zero, color);
            body.Rotation = InitialAngle * (float)Math.PI / 180.0f;
            body.Position = InitialPosition;


            body.UserData = this;
            Alive = true;
            //position = InitialPosition;
            size.X = width;
            size.Y = height;
            Hidden = false;
            objType = ObjectType.Rectangle;
        }

        public ObjRectangle(Vector2 InitialPosition, float width, float height, float InitialAngle, string uD)
        {
            angle = InitialAngle;
            body = BodyFactory.CreateRectangle(EditorVariables.world, width, height, 1f);
            body.Rotation = InitialAngle * (float)Math.PI / 180.0f;
            body.Position = InitialPosition;

            body.UserData = this;
            userData = uD;
            Alive = true;
            //position = InitialPosition;
            size.X = width;
            size.Y = height;
            Hidden = false;
            objType = ObjectType.Rectangle;
        }

        public ObjRectangle(Vector2 InitialPosition, float width, float height, float InitialAngle, object parent, string uD)
        {
            angle = InitialAngle;
            body = BodyFactory.CreateRectangle(EditorVariables.world, width, height, 1f);
            body.Rotation = InitialAngle * (float)Math.PI / 180.0f;
            body.Position = InitialPosition;

            body.UserData = this;
            userData = uD;
            Alive = true;
            //position = InitialPosition;
            size.X = width;
            size.Y = height;
            Hidden = false;
            Parent = parent;
            objType = ObjectType.Rectangle;
        }


        public override void Show()
        {
            if (Hidden )
            {
                body = BodyFactory.CreateRectangle(EditorVariables.world, size.X, size.Y, 10f, BodyPosition, Color);
                body.Rotation = angle;
                body.UserData = this;
                body.BodyColor = Color;

                Hidden = false;
            }
        }
    }
    #endregion

    #region Polygon
    public class ObjPolygon : ObjectClass
    {
        List<Vector2> vertlist;

        public List<Vector2> VerticesList
        {
            get { return vertlist; }
            set
            {
                vertlist = value;
                EditorVariables.world.RemoveBody(body);
                Body backup = body;

                FarseerPhysics.Common.Vertices verts = new FarseerPhysics.Common.Vertices(vertlist);

                if (verts.IsConvex()) body = BodyFactory.CreatePolygon(EditorVariables.world, verts, 1f);
                else
                {
                    List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                    body = BodyFactory.CreateCompoundPolygon(EditorVariables.world, list, 1f);
                }

                body.Rotation = backup.Rotation;
                body.Position = backup.Position;
                body.BodyType = BodyType.Static;
                body.BodyColor = backup.BodyColor;
                body.UserData = this;
            }
        }


        public ObjPolygon(List<Vector2> vertlist)
        {

            this.vertlist = vertlist;
            Vertices verts = new Vertices(vertlist);

            if (verts.IsConvex()) body = BodyFactory.CreatePolygon(EditorVariables.world, verts, 1f);
            else
            {
                List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                body = BodyFactory.CreateCompoundPolygon(EditorVariables.world, list, 1f);
            }

            body.BodyType = BodyType.Static;
            body.UserData = this;
            body.Position = Vector2.Zero;
            //position = Vector2.Zero;
            Alive = true;
            objType = ObjectType.Polygon;
        }

        public ObjPolygon(List<Vector2> vertlist, string userData)
        {

            this.vertlist = vertlist;
            Vertices verts = new Vertices(vertlist);

            if (verts.IsConvex()) body = BodyFactory.CreatePolygon(EditorVariables.world, verts, 1f);
            else
            {
                List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                body = BodyFactory.CreateCompoundPolygon(EditorVariables.world, list, 1f);
            }

            body.BodyType = BodyType.Static;
            body.UserData = this;
            body.Position = Vector2.Zero;
            //position = Vector2.Zero;

            objType = ObjectType.Polygon;
            Alive = true;
            this.userData = userData;
        }

        public void setFriction(float friction)
        {
            body.Friction = friction;
        }

        public override void Show()
        {
            if (Hidden)
            {
                Vertices verts = new Vertices(vertlist);
                if (verts.IsConvex()) body = BodyFactory.CreatePolygon(EditorVariables.world, verts, 1f);
                else
                {
                    List<Vertices> list = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Bayazit);
                    body = BodyFactory.CreateCompoundPolygon(EditorVariables.world, list, 1f);
                }
                body.UserData = this;
                body.BodyColor = Color;

                Hidden = false;
            }
        }
    }
    #endregion

    #region Edge
    public class ObjEdge : ObjectClass
    {
        Vector2 vert1Pos, vert2Pos;

        public Vector2 Vert1Pos
        {
            get { return vert1Pos; }
            set
            {
                vert1Pos = value;
                EditorVariables.world.RemoveBody(body);
                Body backup = body;
                body = BodyFactory.CreateEdge(EditorVariables.world, vert1Pos, vert2Pos);
                body.Rotation = backup.Rotation;
                body.Position = backup.Position;
                body.BodyType = BodyType.Static;
                body.BodyColor = backup.BodyColor;
                body.UserData = this;
            }
        }

        public Vector2 Vert2Pos
        {
            get { return vert2Pos; }
            set
            {
                vert2Pos = value;
                EditorVariables.world.RemoveBody(body);
                Body backup = body;
                body = BodyFactory.CreateEdge(EditorVariables.world, vert1Pos, vert2Pos);
                body.Rotation = backup.Rotation;
                body.Position = backup.Position;
                body.BodyColor = backup.BodyColor;
                body.BodyType = BodyType.Static;
                body.UserData = this;
            }
        }

        public ObjEdge(Vector2 start, Vector2 end)
        {
            objType = ObjectType.Edge;
            body = BodyFactory.CreateEdge(EditorVariables.world, start, end);
            vert1Pos = start;
            vert2Pos = end;
            body.UserData = this;
            Alive = true;
        }
        public ObjEdge(Vector2 start, Vector2 end, string uD)
        {
            objType = ObjectType.Edge;
            body = BodyFactory.CreateEdge(EditorVariables.world, start, end, Color.Purple);//BodyFactory.CreateEdge(EditorVariables.world, start, end);
            vert1Pos = start;
            vert2Pos = end;
            body.UserData = this;
            userData = uD;
            Alive = true;
        }

        public override void Show()
        {
            if (Hidden)
            {
                body = BodyFactory.CreateEdge(EditorVariables.world, vert1Pos, vert2Pos);
                body.UserData = this;
                body.BodyColor = Color;

                Hidden = false;
            }
        }
    }
    #endregion

    #region Point
    public class ObjPoint : ObjectClass
    {
        public Vector2 position;
        //private Fixture fixtureCollidesWith;
        public Object objCollidesWith;
        //public string UD = "";
        public Color pointColor = new Color(250, 0, 0);
        public float Size;

        public ObjPoint(Vector2 InitialPosition, float InitialRadius)
        {

            objType = ObjectType.Point;
            position = InitialPosition;
            Size = InitialRadius;
            Alive = true;
            //this.ObjectPhysic.Add(FixtureFactory.CreateCircle(EditorVariables.worldPhysic, InitialRadius, 1.0f, InitialPosition));
            // À METTRE DANS ObjMouse      ObjectPhysic.Last().OnCollision += new OnCollisionEventHandler(CollisionPointMouse);
        }

        public ObjPoint(Vector2 InitialPosition, float InitialRadius, string userData, Color color)
        {

            objType = ObjectType.Point;
            position = InitialPosition;
            Size = InitialRadius;
            this.userData = userData;
            this.pointColor = color;
            Alive = true;
            //this.ObjectPhysic.Add(FixtureFactory.CreateCircle(EditorVariables.worldPhysic, InitialRadius, 1.0f, InitialPosition));
            // À METTRE DANS ObjMouse      ObjectPhysic.Last().OnCollision += new OnCollisionEventHandler(CollisionPointMouse);
        }
    }
    #endregion

    #region EdgeChain
    public class ObjEdgeChain : ObjectClass
    {
        private Color edgesColor;
        public Color EdgesColor
        {
            get { return edgesColor; }
            set
            {
                edgesColor = value;
                Color = value;
                foreach (ObjEdge item in edges)
                {
                    item.Color = value;
                }
            }
        }

        public List<Vector2> vertlist;
        public List<ObjEdge> edges;

        public List<Vector2> VerticesList
        {
            get { return vertlist; }
            set
            {
                vertlist = value;
                //List<Body> backup = new List<Body>();
                foreach (ObjEdge edge in edges)
                {
                    //backup.Add(edge.body);
                    EditorVariables.world.RemoveBody(edge.body);
                }
                edges.Clear();

                for (int i = 0; i < vertlist.Count - 1; i++)
                {
                    edges.Add(new ObjEdge(vertlist[i], vertlist[i + 1]));
                    edges.Last().body.UserData = edges.Last();
                    edges.Last().Parent = this;
                    edges.Last().Color = EdgesColor;
                }
            }
        }


        public ObjEdgeChain(List<Vector2> vertlist, string uD = null)
        {
            edges = new List<ObjEdge>();
            objType = ObjectType.EdgeChain;
            this.vertlist = vertlist;
            Alive = true;
            for (int i = 0; i < vertlist.Count - 1; i++)
            {
                edges.Add(new ObjEdge(vertlist[i], vertlist[i + 1]) { Parent = this, userData = uD });
                edges.Last().body.UserData = edges.Last();
            }
            userData = uD;
        }

        /// <summary>
        /// Create a chain with only one edge.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public ObjEdgeChain(Vector2 start, Vector2 end)
        {
            edges = new List<ObjEdge>();
            objType = ObjectType.EdgeChain;
            Alive = true;
            edges.Add(new ObjEdge(start, end));
            edges.Last().body.UserData = edges.Last();

            vertlist = new List<Vector2>();
            vertlist.Add(start);
            vertlist.Add(end);
        }


        public override void Show()
        {
            if(Hidden)
            {
                foreach (ObjEdge e in edges)
                {
                    e.Show();
                    e.Parent = this;
                }
                Hidden = false;
            }
        }
    }
    #endregion
}