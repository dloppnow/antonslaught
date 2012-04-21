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
        Texture2D getTexture();
        void setTexture(Texture2D texture);
        Color getColor();
        void setColor(Color color);
        Rectangle getClip();
        void setClip(Rectangle clip);
    }
}
