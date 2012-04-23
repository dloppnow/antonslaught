using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    class WorkerAnt : Ant
    {
        Texture2D carryingFoodText;
        Texture2D normalText;
        float defaultSpeed = 0.25f;
        public WorkerAnt(Vector2 position, SpriteAnimation sAnimation, SpriteAnimation carryingFoodTex)
            : base(position, sAnimation)
        {
            carryingFoodText = carryingFoodTex.getTexture();
            normalText = sAnimation.getTexture();
            speed = defaultSpeed;
            health = 500000;
            canCarryFood = true;
        }
        public override Texture2D getTexture()
        {
            if (hasFood())
            {
                return carryingFoodText;
            }
            else
            {
                return normalText;
            }
        }
    }
}
