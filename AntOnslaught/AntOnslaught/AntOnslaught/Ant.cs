﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    abstract class Ant : MovableObject, Drawable
    {
        private Texture2D texture;
        private Color color;
        private Rectangle clip;

        public Ant(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
            this.color = Color.White;
        }

        public void update(GameTime gameTime)
        {

        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public Color getColor()
        {
            return color;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public Rectangle getClip()
        {
            return clip;
        }

        public void setClip(Rectangle clip)
        {
            this.clip = clip;
        }
    }
}
