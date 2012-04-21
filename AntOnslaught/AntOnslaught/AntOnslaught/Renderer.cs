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
        private Vector2 viewCenter; //Which cell to center the map on.

        public Renderer(SpriteBatch sb, Viewport viewport, Vector2 viewCenter)
        {
            this.sb = sb;
            this.viewport = viewport;
            this.viewCenter = viewCenter;
        }

        public void setViewCenter(Vector2 viewCenter)
        {
            this.viewCenter = viewCenter;
        }

        public void Draw(MovableObject obj)
        {
            
        }

        public void Draw(Map map)
        {
            int mapWidth = map.getWidth();
            int mapHeight = map.getHeight();

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; i < mapHeight; j++)
                {
                    float x = (viewport.Width / 2) - (16) - ((i - viewCenter.X) * 32);
                    float y = (viewport.Height / 2) - (16) - ((j - viewCenter.Y) * 32);
                    Vector2 pos = new Vector2(x, y);
                    
                }
            }
        }
    }
}
