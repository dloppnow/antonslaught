using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Map : Drawable
    {
        List<Cell> openList;
        List<Cell> closedList;
        private int height;
        private int width;
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
        public void setWidth(int width)
        {
            this.width = width;
        }
        public void setHeight(int height)
        {
            this.height = height;
        }
        struct Cell
        {
            public Texture2D texture;
            public Boolean passable;
            public int g;
            public int f;
            public int h;
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

        public List<Cell> getPath(Cell start, Cell end)
        {
            List<Cell> path = new List<Cell>;
            openList.Clear();
            closedList.Clear();
            openList.Add(start);
            if (calculatePath(start, end))
			{

			}
            return path;
        }
        private Boolean calculatePath(Cell startCell, Cell endCell)
        {
            Cell lowestCost = new Cell();
            lowestCost.g = 10000;
            lowestCost.f = 10000;
            lowestCost.h = 10000;
            foreach (Cell c in openList)
            {
                if ((lowestCost.g + lowestCost.h) > (c.g + c.h))
                {
                    lowestCost = c;
                }
            }
            removeFromOpenList(startCell);
            closedList.Add(lowestCost);
            if (lowestCost.Equals(endCell))
            {

            }
            return true;
        }
        private void removeFromOpenList(Cell c)
        {
            openList.Remove(c);
        }
    }
}
