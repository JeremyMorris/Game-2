using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_2
{
    static class BoomerangModel
    {
        public static Dictionary<string, Animation> animations;
        public static float maxSpeed = 0.5f;
        public static float framesToTurn = 40;
        public static Player player;
        public static Game1 game;

        public static void SetValues(ref Game1 g, ref Player p, Dictionary<string, Animation> a)
        {
            game = g;
            player = p;
            animations = a;
        }
    }
}
