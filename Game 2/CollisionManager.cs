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

        public bool CollidesWith(Vector2 v, Rectangle r)
        {
            return (r.X <= v.X && v.X <= r.X + r.Width)
                && (r.Y <= v.Y && v.Y <= r.Y + r.Height);
        }

        public bool IsWithinRange(Vector2 v, Rectangle r, float range)
        {
            float nearestX = Clamp(v.X, r.X, r.X + r.Width);
            float nearestY = Clamp(v.Y, r.Y, r.Y + r.Height);
            return (Math.Pow(range, 2) > (Math.Pow(v.X - nearestX, 2) + Math.Pow(v.Y - nearestY, 2)));
        }

        private float Clamp(float point, float min, float max)
        {
            if (point < min) return min;
            else if (point > max) return max;
            else return point;
        }
    }
}
