using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class SolderAnt : Ant
    {
        float defaultSpeed = 0.35f;
        public SolderAnt(Vector2 position, SpriteAnimation sAnimation): base(position, sAnimation)
        {
            speed = defaultSpeed;
            canCarryFood = false;
            aggroRange = 250;
            attackRange = 50;
            damage = 1;
            attackInterval = 100;
        }
    }
}
