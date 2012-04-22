using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace AntOnslaught
{
    class Map : Drawable
    {
        private Texture2D texture;
        Random rand;
        private int numOfYCells = 100;
        private int numOfXCells = 100;
        private Cell[,] grid;
        private List<Cell> openList = new List<Cell>();
        private List<Cell> closedList = new List<Cell>();
        private List<MovableObject> newObjects = new List<MovableObject>();
        private Cell soldierWaypoint = null;
        private Cell workerWaypoint = null;
        public Map(ContentManager content)
        {
            rand = new Random();
            TextReader infoReader = new StreamReader("infoout.txt");
            String[] infoTokens = infoReader.ReadLine().Split(',');
            numOfXCells = int.Parse(infoTokens[1]);
            numOfYCells = int.Parse(infoTokens[2]);

            grid = new Cell[numOfXCells, numOfYCells];

            TextReader mapReader = new StreamReader("mapout.txt");
            String mapFile = mapReader.ReadToEnd();
            String[] mapTokens = mapFile.Split(',');
            int currentXCoord = 0;
            int currentYCoord = 0;
            foreach(String cellStr in mapTokens)
            {
                if (currentXCoord == numOfXCells)
                {
                    currentYCoord++;
                    currentXCoord = 0;
                }
                grid[currentXCoord, currentYCoord] = new Cell(cellStr);
                grid[currentXCoord, currentYCoord].coord.Y = currentYCoord;
                grid[currentXCoord, currentYCoord].coord.X = currentXCoord;
                currentXCoord++;
            }

            String nextLine = infoReader.ReadLine();
            Boolean EOF = false;
            while (!nextLine.Equals("tiles"))
            {
                infoTokens = nextLine.Split(',');
                if (infoTokens[0].Equals("Worker"))
                {
                    newObjects.Add(new WorkerAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])), 
                        new SpriteAnimation(content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Soldier"))
                {
                    newObjects.Add(new SolderAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                        new SpriteAnimation(content.Load<Texture2D>("soldier_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Queen"))
                {
                    newObjects.Add(new QueenAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                        new SpriteAnimation(content.Load<Texture2D>("queen_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Seed"))
                {
                    grid[int.Parse(infoTokens[1]), int.Parse(infoTokens[2])].food = new Food();
                    grid[int.Parse(infoTokens[1]), int.Parse(infoTokens[2])].food.setTexture(content.Load<Texture2D>("seed_small"));
                    //newObjects.Add(new QueenAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                    //    new SpriteAnimation(content.Load<Texture2D>("queen_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Soldier_Waypoint"))
                {
                    soldierWaypoint = grid[int.Parse(infoTokens[1]), int.Parse(infoTokens[2])];
                    //newObjects.Add(new QueenAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                    //    new SpriteAnimation(content.Load<Texture2D>("queen_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Worker_Waypoint"))
                {
                    workerWaypoint = grid[int.Parse(infoTokens[1]), int.Parse(infoTokens[2])];
                    //newObjects.Add(new QueenAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                    //    new SpriteAnimation(content.Load<Texture2D>("queen_sprite_sheet"), 32, 32, 100)));
                }
                else if (infoTokens[0].Equals("Spider"))
                {
                    newObjects.Add(new Spider(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                        new SpriteAnimation(content.Load<Texture2D>("spider_sprite_sheet"), 32, 64, 100)));
                    //newObjects.Add(new Spider(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                    //    new SpriteAnimation(content.Load<Texture2D>("spider_sprite_sheet"), 32, 64, 100)));
                    //newObjects.Add(new QueenAnt(new Vector2(int.Parse(infoTokens[1]), int.Parse(infoTokens[2])),
                    //    new SpriteAnimation(content.Load<Texture2D>("queen_sprite_sheet"), 32, 32, 100)));
                }
                nextLine = infoReader.ReadLine();
            }
            nextLine = infoReader.ReadLine();
            while (nextLine != null)
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
                nextLine = infoReader.ReadLine();
            }
        }
        public List<MovableObject> getNewObjects()
        {
            return newObjects;
        }
        public Cell getCell(int x, int y)
        {
            return grid[x, y];
        }
        public int getWidth()
        {
            return numOfXCells;
        }
        public Cell[,] getGrid()
        {
            return grid;
        }
        public Cell getSoldierWaypoint()
        {
            return soldierWaypoint;
        }
        public Cell getWorkerWaypoint()
        {
            return workerWaypoint;
        }
        public int getHeight()
        {
            return numOfYCells;
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
        public Cell findUnoccupiedClosestCell(Cell c)
        {
            foreach (Cell cell in grid)
            {
                cell.visited = false;
            }
            Cell result = null;
            List<Cell> checkableCells = new List<Cell>();
            checkableCells.Add(c);
            List<Cell> possibleCells = new List<Cell>();
            while (result == null && checkableCells.Count > 0)
            {
                if (checkableCells[0].occupied == false && checkableCells[0].passable == true)
                {
                    result = checkableCells[0];
                }
                else
                {
                    possibleCells = getAdjacentCells(checkableCells[0]);
                    checkableCells.RemoveAt(0);
                    foreach(Cell cell in possibleCells)
                    {
                        if(!cell.visited)
                        {
                            cell.visited = true;
                            checkableCells.Add(cell);
                        }
                    }
                }
            }
            return result;
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
        public List<Cell> getPath(Cell start, Cell end)
		{
            foreach (Cell c in grid)
            {
                c.g = 0;
                c.f = 0;
                c.h = 0;
                c.next = null;
            }
			List<Cell> path = new List<Cell>();

			openList.Clear();
			closedList.Clear();

			openList.Add(start);
			if(calculatePath(start, end))
			{
				Cell node = end;
				while(node != null)
				{
					path.Add(node);
					node = node.next;
				}
			}
			openList.Clear();
			closedList.Clear();

			return path;
		}
		bool calculatePath(Cell startNode, Cell endNode)
		{
			Cell lowestCost = null;
			for(int i = 0; i < openList.Count(); i++)
			{
				if(lowestCost == null ||
				   (lowestCost.g + lowestCost.h) > (openList[i].g + openList[i].h))
				{
					lowestCost = openList[i];
				}
			}
			removeFromOpenList(lowestCost);
			if(lowestCost == endNode)
			{
				//endNode->parentCell = lowestCost;
				return true;
			}
			closedList.Add(lowestCost);
            foreach (Cell c in getAdjacentCells(lowestCost))
            {
			//for(int i = 0; i < lowestCost->adjacentCells.size(); i++)
			//{
                if (c.passable)
				{
					if(isOnClosedList(c) == false)
					{
						Cell aNode = isOnOpenList(c);
						if(aNode != null)
						{
							if(aNode.g > lowestCost.g + 1)
							{
								aNode.next = lowestCost;
								aNode.g = lowestCost.g + 1;
							}
						}
						else
						{
							c.next = lowestCost;
							c.g = lowestCost.g + 1;
                            c.h = distanceBetween(c, endNode);
							openList.Add(c);
						}
					}
				}
			}
			if(openList.Count() <= 0)
			{
				return false;
			}
			calculatePath(startNode, endNode);
			return true;
		}
		bool isOnClosedList(Cell node)
		{
			bool onClosedList = false;
			for(int i = 0; i < closedList.Count(); i++)
			{
				if(closedList[i] == node)
				{
					onClosedList = true;
					break;
				}
			}
			return onClosedList;
		}
		Cell isOnOpenList(Cell node)
		{
			Cell onOpenListNode = null;
			for(int i = 0; i < openList.Count(); i++)
			{
				if(openList[i] == node)
				{
					onOpenListNode = openList[i];
					break;
				}
			}
			return onOpenListNode;
		}
		void removeFromOpenList(Cell node)
		{
            openList.Remove(node);
		}
        private int distanceBetween(Cell c1, Cell c2)
        {
            return (int)Math.Abs(c1.coord.X - c2.coord.X) + (int)Math.Abs(c1.coord.Y - c2.coord.Y);
        }
        public List<Cell> getAdjacentCells(Cell c)
        {
            List<Cell> adjacentCells = new List<Cell>();
            if (c.coord.X > 0)
            {
                adjacentCells.Add(grid[(int)c.coord.X - 1, (int)c.coord.Y]);
            }
            if (c.coord.X < numOfXCells - 1)
            {
                adjacentCells.Add(grid[(int)c.coord.X + 1, (int)c.coord.Y]);
            }
            if (c.coord.Y > 0)
            {
                adjacentCells.Add(grid[(int)c.coord.X, (int)c.coord.Y - 1]);
            }
            if (c.coord.Y < numOfYCells - 1)
            {
                adjacentCells.Add(grid[(int)c.coord.X, (int)c.coord.Y + 1]);
            }


            if (c.coord.X > 0 && c.coord.Y > 0)
            {
                adjacentCells.Add(grid[(int)c.coord.X - 1, (int)c.coord.Y - 1]);
            }
            if (c.coord.X < numOfXCells - 1 && c.coord.Y < numOfYCells - 1)
            {
                adjacentCells.Add(grid[(int)c.coord.X + 1, (int)c.coord.Y + 1]);
            }
            if (c.coord.X > 0 && c.coord.Y < numOfYCells - 1)
            {
                adjacentCells.Add(grid[(int)c.coord.X - 1, (int)c.coord.Y + 1]);
            }
            if (c.coord.Y > 0 && c.coord.X < numOfXCells - 1)
            {
                adjacentCells.Add(grid[(int)c.coord.X + 1, (int)c.coord.Y - 1]);
            }
            return adjacentCells;
        }

        public Cell getRandomCell(Cell center, int range)
        {
            int x = (int)center.coord.X;
            int y = (int)center.coord.Y;
            List<Cell> cells = new List<Cell>();
            for (int i = -range; i <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    if (x + i >= 0 && x + i < numOfXCells && y + j >= 0 && y + j < numOfYCells)
                    {
                        if (grid[x + i, y + j].passable == true && grid[x + i, y + j].occupied == false)
                        {
                            cells.Add(grid[x + i,  y + j]);
                        }
                    }
                }
            }

            if (cells.Count <= 0)
                return null;
            int index = rand.Next(0, cells.Count);
            return cells[index];
        }
    }
}
