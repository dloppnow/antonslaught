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
        public struct Cell
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
        public Cell getCell(int x, int y)
        {
            return grid[x, y];
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
        private Cell lowestScoreInOpen()
        {
            Cell lowestScoreCell = new Cell();
            lowestScoreCell.f = 1000000;
            foreach (Cell c in openList)
            {
                if (c.f < lowestScoreCell.f)
                {
                    lowestScoreCell = c;
                }
            }
            return lowestScoreCell;
        }
        private Boolean calculatePath(Cell startCell, Cell endCell)
        {

            while (openList.Count() > 0)
            {
                Cell currentNode = lowestScoreInOpen();
                if(currentNode.Equals(endCell)
                {
                    //reconstruct Path
                }
                else
                {
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);
                    List<Cell> adjacentCells = getAdjacentCells(currentNode);
                    for(int i = 0; i < adjacentCells.Count; i++)
                    {
                        if(closedList.Contains(adjacentCells[i]))
                        {
                            adjacentCells[i].g = currentNode.g + distanceBetween(currentNode, adjacentCells[i]);
                        }
                    }
                }
            }
        }
        private int distanceBetween(Cell c1, Cell c2)
        {
            return Math.Abs(c1.xCoord - c2.xCoord) + Math.Abs(c1.yCoord - c2.yCoord);
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
