﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Enemy : MovableObject, Drawable
    {
       public Enemy(Vector2 position)
        {
            this.position = position;
        }

        public void update(GameTime gameTime)
        {

        }

        public Texture2D getTexture()
        {
            throw new NotImplementedException();
        }

        public void setTexture(Texture2D texture)
        {
            throw new NotImplementedException();
        }

        public Color getColor()
        {
            throw new NotImplementedException();
        }

        public void setColor(Color color)
        {
            throw new NotImplementedException();
        }

        public Rectangle getClip()
        {
            throw new NotImplementedException();
        }

        public void setClip(Rectangle clip)
        {
            throw new NotImplementedException();
        }
    }
}
