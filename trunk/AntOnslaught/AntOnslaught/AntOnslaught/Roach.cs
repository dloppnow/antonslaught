using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Roach : Enemy
    {
        private float defaultSpeed = 0.3f;
        public Roach(Vector2 position, SpriteAnimation sAnimation)
            : base(position, sAnimation)
        {
            speed = defaultSpeed;
            //Set combat variables
            aggroRange = 200;
            attackRange = 50;
            damage = 3;
            attackInterval = 200;
            health = 30;
        }
    }
}
