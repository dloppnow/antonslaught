using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    class Cell
    {
        public Texture2D texture;
        public Boolean passable;
        public Boolean occupied = false;
        public Boolean visited = false;
        public float g;
        public float f;
        public float h;
        public Food food = null;

        //public int xCoord;
        //public int yCoord;

        public Vector2 coord;
        public int texCoordX;
        public int texCoordY;

        public Cell next;

        public int tileType;
        public Cell(String tileType)
        {
            this.tileType = int.Parse(tileType);
        }
        public Cell(Cell c)
        {
            texCoordX = c.texCoordX;
            texCoordY = c.texCoordY;
            passable = c.passable;
            g = c.g;
            f = c.f;
            h = c.h;
            food = c.food;
            occupied = c.occupied;
            visited = c.visited;
            coord = c.coord;
            tileType = c.tileType;
        }
    }
}
