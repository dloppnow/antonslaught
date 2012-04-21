using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace AntOnslaught
{
    class movableObject
    {
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        private Vector2 position;
        private float speed;
        private void move(Direction direction)
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
    }
}
