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
        private int tileWidth = 32;
        private int mapWidth;
        private int mapHeight;

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

        public Vector2 getViewCenter()
        {
            return viewCenter;
        }

        public void Draw(Drawable obj, Vector2 pos)
        {
            sb.Draw(obj.getTexture(), pos, obj.getClip(), obj.getColor());
        }

        public void Draw(MovableObject obj)
        {
            Vector2 pos = obj.getPosition(); //position relative to the map
            Rectangle clip = obj.getClip();

            float x = (viewport.Width / 2) - (tileWidth / 2) + (pos.X - (viewCenter.X * clip.Width));
            float y = (viewport.Height / 2) - (tileWidth / 2) + (pos.Y - (viewCenter.Y * clip.Height));
            sb.Draw(obj.getTexture(), new Vector2(x, y), clip, obj.getColor());
        }

        public void Draw(Map map)
        {
            tileWidth = 32;
            mapWidth = map.getWidth();
            mapHeight = map.getHeight();

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    float x = (viewport.Width / 2) - (tileWidth / 2) + ((i - viewCenter.X) * tileWidth);
                    float y = (viewport.Height / 2) - (tileWidth / 2) + ((j - viewCenter.Y) * tileWidth);
                    Cell cell = map.getCell(i, j);
                    sb.Draw(map.getTexture(), new Vector2(x, y), new Rectangle(cell.texCoordY * tileWidth, cell.texCoordX * tileWidth, tileWidth, tileWidth), Color.White);
                }
            }
        }
    }
}
