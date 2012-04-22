using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    abstract class Ant : MovableObject
    {
        protected SpriteAnimation sAnimation;
        protected int health;

        public Ant(Vector2 position, SpriteAnimation sAnimation)
        {
            health = 10;
            this.position = position * 32;
            this.sAnimation = sAnimation;
            this.speed = speed;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }

        public int getHealth()
        {
            return health;
        }

        public void update(GameTime gameTime)
        {
            if (isMoving)
            {
                sAnimation.setRepeatable(true);
            }
            else
            {
                sAnimation.setRepeatable(false);
            }
            sAnimation.update(gameTime);
        }

        public override Texture2D getTexture()
        {
            return sAnimation.getTexture();
        }

        public override void setTexture(Texture2D texture)
        {
            sAnimation.setTexture(texture);
        }

        public override Color getColor()
        {
            return sAnimation.getColor();
        }

        public override void setColor(Color color)
        {
            sAnimation.setColor(color); ;
        }

        public override Rectangle getClip()
        {
            return sAnimation.getClip();
        }

        public override void setClip(Rectangle clip)
        {
            sAnimation.setClip(clip);
        }
    }
}
