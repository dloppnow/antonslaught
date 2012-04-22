using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Spider : Enemy
    {
        private float defaultSpeed = 0.2f;
        public Spider(Vector2 position, SpriteAnimation sAnimation)
            : base(position, sAnimation)
        {
            speed = defaultSpeed;
            //Set combat variables
            aggroRange = 250;
            attackRange = 50;
            damage = 5;
            attackTimer = 2000;
            attackInterval = 250;
        }
    }
}
