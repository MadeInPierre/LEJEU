using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace LevelEditor
{
    public class Helpers
    {
        public static Texture2D ImageToTexture2D(System.Drawing.Image image, GraphicsDevice gd)
        {
            System.IO.Stream stream = new System.IO.MemoryStream();
            image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            Texture2D texture = Texture2D.FromStream(gd, stream);

            return texture;
        }

        public static Image Texture2DToImage(Texture2D texture)
        {
            System.Drawing.Image img;
            using (MemoryStream MS = new MemoryStream())
            {
                texture.SaveAsPng(MS, texture.Width, texture.Height);
                //Go To the  beginning of the stream.
                MS.Seek(0, SeekOrigin.Begin);
                //Create the image based on the stream.
                img = Image.FromStream(MS);
            }
            return img;
        }

        public static Vector2 StringToVector2(string s)
        {
            string cleanString = s.Replace("{X:", "");   //"512 Y:384}"
            cleanString = cleanString.Replace("Y:", "");  //"512 384}"
            cleanString = cleanString.Replace("}", "");   //"512 384"
            string[] xyVals = cleanString.Split(' ');      //"512" and "384"
            return new Vector2(float.Parse(xyVals[0]), float.Parse(xyVals[1]));
        }

        public static bool ClassInheritsFrom(Type type, Type baseType)
        { //know if one object is a child on another class (e.g. "Schrumbli" is a child of "Ennemy" so it will return true)
            // null does not have base type
            if (type == null)
            {
                return false;
            }

            // only interface can have null base type
            if (baseType == null)
            {
                return type.IsInterface;
            }

            // check implemented interfaces
            if (baseType.IsInterface)
            {
                return type.GetInterfaces().Contains(baseType);
            }

            // check all base types
            var currentType = type;
            while (currentType != null)
            {
                if (currentType.BaseType == baseType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePath"> the base path which the 'path' will be compared to.</param>
        /// <param name="path">the path that will be reduced compared to basePath.</param>
        /// <returns></returns>
        public static string GetRelativePathFromContent(string basePath, string path)
        {
            // transform C://Users/pierrelaclau/Google Drive/LEJEU/CODE/LEJEU/LEJEU.Content/Maps/Level1
            // to        Maps/Level1
            Uri baseUri = new Uri(EditorVariables.ContentBasePath);
            string relative = baseUri.MakeRelativeUri(new Uri(path)).ToString();
            
            return relative;
        }
    }

    public class PreciseRectangle
    { //this class is a remake of the original XNA's Rectangle class because it only accepts integers. The level editor requires float values.
        public float X, Y, Width, Height;

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }
        public Vector2 Size
        {
            get { return new Vector2(Width, Height); }
        }

        public PreciseRectangle(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
        }
        public PreciseRectangle(Vector2 pos, Vector2 size)
        {
            X = pos.X;
            Y = pos.Y;
            Width = size.X;
            Height = size.Y;
        }
        public PreciseRectangle(Vector2 pos, float w, float h)
        {
            X = pos.X;
            Y = pos.Y;
            Width = w;
            Height = h;
        }
        public PreciseRectangle(Microsoft.Xna.Framework.Rectangle sourceRect)
        {
            X = sourceRect.X;
            Y = sourceRect.Y;
            Width = sourceRect.Width;
            Height = sourceRect.Height;
        }
    }
}
