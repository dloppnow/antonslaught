using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class SolderAnt : Ant
    {
        float defaultSpeed = 0.5f;
        public SolderAnt(Vector2 position, SpriteAnimation sAnimation): base(position, sAnimation)
        {
            speed = defaultSpeed;
        }
    }
}
