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
        public enum GameState
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }
        private Vector2 position;
        private float speed;
        private void move(GameState direction)
        {
            if (direction == GameState.UP)
            {
                position.Y -= speed;
            }
            else if (direction == GameState.DOWN)
            {
                position.Y += speed;
            }
            else if (direction == GameState.LEFT)
            {
                position.X -= speed;
            }
            else if (direction == GameState.RIGHT)
            {
                position.X += speed;
            }
        }
    }
}
