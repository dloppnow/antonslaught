﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    class Map : Drawable
    {
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

    }
}