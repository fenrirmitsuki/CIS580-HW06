using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingCircle
    {
        public float X;
        public float Y;
        public float Radius;

        //Create new BoundingCircle
        public Vector2 Center
        {
            get => new Vector2(X,Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        //Convert the BoundingCircle to a Rectangle
        public static implicit operator Rectangle(BoundingCircle c)
        {
            return new Rectangle(
                (int)(c.X - c.Radius),
                (int)(c.Y - c.Radius),
                (int)(2 * c.Radius),
                (int)(2 * c.Radius)
                );
        }
    }
}
