using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AntOnslaught
{
    abstract class MovableObject : Drawable
    {
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        protected Vector2 position;
        public Vector2 getPosition()
        {
            return position;
        }
        protected float speed;
        protected void move(Direction direction)
        {
            if (direction == Direction.UP)
            {
                position.Y -= speed;
            }
            else if (direction == Direction.DOWN)
            {
                position.Y += speed;
            }
            else if (direction == Direction.LEFT)
            {
                position.X -= speed;
            }
            else if (direction == Direction.RIGHT)
            {
                position.X += speed;
            }
        }
        public void findPath(Vector2 goal)
        {
             
        }
        public abstract Texture2D getTexture();
        public abstract void setTexture(Texture2D texture);
        public abstract Color getColor();
        public abstract void setColor(Color color);
        public abstract Rectangle getClip();
        public abstract void setClip(Rectangle clip);
    }
}
