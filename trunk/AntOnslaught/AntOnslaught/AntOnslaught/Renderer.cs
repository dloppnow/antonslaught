using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AntOnslaught
{
    class Renderer
    {
        private SpriteBatch sb;
        private Viewport viewport;
        public Renderer(SpriteBatch sb, Viewport viewport)
        {
            this.sb = sb;
            this.viewport = viewport;
        }

        public void Draw(Drawable obj, Vector2 Pos) 
        {

        }
    }
}
