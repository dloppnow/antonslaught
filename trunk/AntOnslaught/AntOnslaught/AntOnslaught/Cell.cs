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
        public int g;
        public int f;
        public int h;

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
    }
}
