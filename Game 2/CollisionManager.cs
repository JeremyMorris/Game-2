using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game_2
{
    public class CollisionManager
    {
        public bool CollidesWith(Rectangle a, Rectangle b)
        {
            return !(a.X > a.X + b.Width
                  || a.X + a.Width < b.X
                  || a.Y > b.Y + b.Height
                  || a.Y + a.Height < b.Y);
        }

        public bool CollidesWith(Vector2 v, Vector2 other)
        {
            return v == other;
        }

        public static bool CollidesWith(Vector2 v, Rectangle r)
        {
            return (r.X <= v.X && v.X <= r.X + r.Width)
                && (r.Y <= v.Y && v.Y <= r.Y + r.Height);
        }
    }
}
