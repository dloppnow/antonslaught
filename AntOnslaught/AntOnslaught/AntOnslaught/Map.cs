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

        struct Cell
        {
            Texture2D texture;
            Boolean passable;
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
            openList.Clear();
            closedList.Clear();
            Cell startNode = start;
            Cell endNode = end;
        }
    }
}
