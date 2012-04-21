using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AntOnslaught
{
    interface Drawable
    {
        public Texture2D getTexture();
        public void setTexture(Texture2D texture);
        public Color getColor();
        public void setColor(Color color);
        public Vector2 getVector();
    }
}
