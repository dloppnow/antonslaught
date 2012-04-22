using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    abstract class Enemy : MovableObject
    {
        protected SpriteAnimation sAnimation;

        public Enemy(Vector2 position, SpriteAnimation sAnimation)
        {
            this.position = position * 32;
            this.sAnimation = sAnimation;
            this.speed = speed;
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
