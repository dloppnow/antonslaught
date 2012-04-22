using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace AntOnslaught
{
    class WorkerAnt : Ant
    {
        float defaultSpeed = 0.5f;
        public WorkerAnt(Vector2 position, SpriteAnimation sAnimation) : base(position, sAnimation)
        {
            speed = defaultSpeed;
        }
    }
}
