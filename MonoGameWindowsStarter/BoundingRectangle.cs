using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        //Crerate new BoundingRectangle
        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        //Convert BoundingRectangle to Rectangle
        public static implicit operator Rectangle(BoundingRectangle r)
        {
            return new Rectangle(
                (int)r.X,
                (int)r.Y,
                (int)r.Width,
                (int)r.Height
                );
        }
    }
}
