using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace AntOnslaught
{
    class Map : Drawable
    {
        private int numOfYCells = 100;
        private int numOfXCells = 100;
        Cell[,] grid;
        List<Cell> openList;
        List<Cell> closedList;
        public Map()
        {
            TextReader infoReader = new StreamReader("infoout.txt");
            String[] infoTokens = infoReader.ReadLine().Split(',');

            grid = new Cell[int.Parse(infoTokens[1]), int.Parse(infoTokens[2])];

            TextReader mapReader = new StreamReader("mapout.txt");
            String mapFile = mapReader.ReadToEnd();
            String[] mapTokens = mapFile.Split(',');
            int currentXCoord = 0;
            int currentYCoord = 0;
            foreach(String cellStr in mapTokens)
            {
                if(numOfXCells % currentXCoord == 0)
                {
                    currentYCoord++;
                    currentXCoord = 0;
                }
                grid[currentXCoord, currentYCoord] = new Cell(cellStr);
                grid[currentXCoord, currentYCoord].yCoord = currentYCoord;
                grid[currentXCoord, currentYCoord].xCoord = currentXCoord;
                currentXCoord++;
            }

            String nextLine = infoReader.ReadLine();
            Boolean EOF = false;
            while (!infoReader.ReadLine().Equals("tiles"))
            {
            }
            while (!EOF)
            {
                infoTokens = nextLine.Split(',');
                foreach(Cell c in grid)
                {
                    if (c.tileType == int.Parse(infoTokens[0]))
                    {
                        c.texCoordX = int.Parse(infoTokens[1]);
                        c.texCoordY = int.Parse(infoTokens[2]);
                        String passable = infoTokens[3];
                        if (passable.Equals("1"))
                        {
                            c.passable = false;
                        }
                        else
                        {
                            c.passable = true;
                        }
                    }
                }
                try{

                    nextLine = infoReader.ReadLine();
                }
                catch{
                    EOF = true;
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

        public Rectangle getClip()
        {
            throw new NotImplementedException();
        }

        public void setClip(Rectangle clip)
        {
            throw new NotImplementedException();
        }
        public List<Cell> getPath(Cell start, Cell end)
        {
            List<Cell> path = new List<Cell>();
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
            Cell lowestScoreCell = new Cell("1");
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
                if(currentNode.Equals(endCell))
                {
                    //reconstruct Path
                }
                else
                {
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);
                    List<Cell> adjacentCells = getAdjacentCells(currentNode);
                    foreach (Cell c in getAdjacentCells(currentNode))
                    {
                        if (closedList.Contains(c))
                        {
                            c.g = currentNode.g + distanceBetween(currentNode, c);
                        }
                        if (!openList.Contains(c))
                        {
                        }
                    }
                }
            }
            return false;
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
