using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class QueenAnt: Ant
    {
        float defaultSpeed = 0.5f;
        public QueenAnt(Vector2 position, SpriteAnimation sAnimation) : base(position, sAnimation)
        {
            speed = defaultSpeed;
        }
    }
}
