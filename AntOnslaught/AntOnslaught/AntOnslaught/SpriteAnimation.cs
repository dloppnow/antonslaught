﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class SpriteAnimation : Drawable
    {
        int numFrames;
        int currFrame;
        int spriteWidth;
        int spriteHeight;
        Texture2D texture;
        int timeSinceLastUpdate;
        int updateFreq;
        Color color;
        Rectangle clip;
        bool repeatable;

        public SpriteAnimation(Texture2D texture, int spriteWidth, int spriteHeight, int updateFreq)
        {
            this.texture = texture;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.updateFreq = updateFreq;
            this.color = Color.White;

            int textWidth = this.texture.Bounds.Width;
            int textHeight = this.texture.Bounds.Height;

            numFrames = (textWidth / spriteWidth) * (textHeight / spriteHeight);
            currFrame = 0;
            setClip(currFrame);
        }

        public int getSpriteHeight()
        {
            return spriteHeight;
        }
        public int getSpriteWidth()
        {
            return spriteWidth;
        }

        public void setRepeatable(bool repeatable)
        {
            this.repeatable = repeatable;
        }

        public bool isRepeatable()
        {
            return repeatable;
        }

        public void update(GameTime gameTime)
        {
            timeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastUpdate >= updateFreq && repeatable)
            {
                timeSinceLastUpdate = 0;
                currFrame++;
                if (currFrame >= numFrames)
                {
                    currFrame = 0;
                }
                setClip(currFrame);
            }
        }

        public void setClip(int frameNum)
        {
            clip = new Rectangle(currFrame * spriteWidth, 0, spriteWidth, spriteHeight);
        }

        #region Drawable Members

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

        #endregion
    }
}
