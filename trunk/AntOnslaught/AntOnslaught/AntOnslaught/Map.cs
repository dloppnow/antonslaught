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
        private int numOfYCells = 100;
        private int numOfXCells = 100;
        Cell[,] grid;
        List<Cell> openList;
        List<Cell> closedList;
        struct Cell
        {
            public Texture2D texture;
            public Boolean passable;
            public int g;
            public int f;
            public int h;

            public int xCoord;
            public int yCoord;
        }
        public Map()
        {
            grid = new Cell[numOfYCells, numOfXCells];
            for (int y = 0; y < numOfYCells; y++)
            {
                for (int x = 0; x < numOfXCells; x++)
                {
                    Cell c = new Cell();
                    c.yCoord = y;
                    c.xCoord = x;
                }
            }

        }
        public int getWidth()
        {
            return numOfXCells;
        }
        public int getHeight()
        {
            return numOfYCells;
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
            removeFromOpenList(lowestCost);
            closedList.Add(lowestCost);

            if (lowestCost.Equals(endCell))
            {
                return true;
            }
            List<Cell> adjacentCells = getAdjacentCells(lowestCost);
            foreach (Cell c in adjacentCells)
            {
                if (c.passable)
                {

                }
            }
            return true;
        }
        private void removeFromOpenList(Cell c)
        {
            openList.Remove(c);
        }
        private List<Cell> getAdjacentCells(Cell c)
        {
            List<Cell> adjacentCells = new List<Cell>();
            if (c.xCoord > 0)
            {
                adjacentCells.Add(grid[c.yCoord, c.xCoord - 1]);
            }
            if (c.xCoord < numOfXCells)
            {
                adjacentCells.Add(grid[c.yCoord, c.xCoord + 1]);
            }
            if (c.yCoord > 0)
            {
                adjacentCells.Add(grid[c.yCoord - 1, c.xCoord]);
            }
            if (c.yCoord < numOfYCells)
            {
                adjacentCells.Add(grid[c.yCoord + 1, c.xCoord]);
            }
            return adjacentCells;
        }
    }
}
