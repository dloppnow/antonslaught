using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    class Food : Drawable
    {
        private Texture2D texture;
        private Color color;
        private Rectangle clip;
        private int amountFoodLeft = 50;
        private float delayFoodCounter = 100;
        private float delayFoodTimer = 0;
        private List<Cell> pathToQueen;

        public void setAmountOfFoodLeft(int amount)
        {
            amountFoodLeft = amount;
        }
        public int getAmountOfFoodLeft()
        {
            return amountFoodLeft;
        }
        public void reduceFoodBy(int foodTaken)
        {
            amountFoodLeft -= foodTaken;
        }
        public void setPathToQueen(List<Cell> path)
        {
            pathToQueen = path;
        }
        public List<Cell> getPathToQueen()
        {
            return pathToQueen;
        }
        public void resetTimer()
        {
            delayFoodTimer = delayFoodCounter;
        }
        public bool canPickUp()
        {
            bool canPickUp = false;
            if(delayFoodTimer < 0)
            {
                canPickUp = true;
            }
            return canPickUp;
        }
        public void update(GameTime timer)
        {
            if (delayFoodTimer >= 0)
            {
                delayFoodTimer -= timer.ElapsedGameTime.Milliseconds;
            }
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
