using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Beetle : Enemy
    {
        private float defaultSpeed = 0.1f;
        public Beetle(Vector2 position, SpriteAnimation sAnimation)
            : base(position, sAnimation)
        {
            speed = defaultSpeed;
            //Set combat variables
            aggroRange = 200;
            attackRange = 50;
            damage = 2;
            attackInterval = 200;
            health = 20;
        }
    }
}
