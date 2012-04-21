using System;
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
        Color color;
        Rectangle clip;

        public SpriteAnimation(Texture2D texture, int spriteWidth, int spriteHeight, int updateFreq)
        {
            this.texture = texture;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.color = Color.White;

            int textWidth = this.texture.Bounds.Width;
            int textHeight = this.texture.Bounds.Height;

            numFrames = (textWidth / spriteWidth) * (textHeight / spriteHeight);
            currFrame = 0;
            setClip(currFrame);
        }

        public void update(GameTime gameTime)
        {
            
        }

        public void setClip(int frameNum)
        {

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
