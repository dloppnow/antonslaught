using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace AntOnslaught
{
    class Renderer
    {
        private SpriteBatch sb;
        private Viewport viewport;
        Texture2D selectionCircle;
        private Vector2 viewCenter; //Which cell to center the map on.
        private int tileWidth = 32;
        private int mapWidth;
        private int mapHeight;
        private float selectionCircleAngle = 0.0f;

        public Renderer(SpriteBatch sb, Viewport viewport, Vector2 viewCenter, ContentManager Content)
        {
            this.sb = sb;
            this.viewport = viewport;
            this.viewCenter = viewCenter;
            selectionCircle = Content.Load<Texture2D>("dashed_circle");
        }

        public void setViewCenter(Vector2 viewCenter)
        {
            this.viewCenter = viewCenter;
        }

        public Vector2 getViewCenter()
        {
            return viewCenter;
        }

        public void DrawSelectionCircles(List<Ant> ants)
        {
            sb.Begin();
            foreach (Ant ant in ants)
            {
                Vector2 pos = ant.getPosition();
                Rectangle clip = ant.getClip();
                float x = (viewport.Width / 2) - (tileWidth / 8) + (pos.X - (viewCenter.X * clip.Width));
                float y = (viewport.Height / 2) - (tileWidth / 8) + (pos.Y - (viewCenter.Y * clip.Height));
                sb.Draw(selectionCircle, new Rectangle((int)x, (int)y, 32, 32), new Rectangle(0, 0, 32, 32), Color.White, selectionCircleAngle, new Vector2(16, 16), SpriteEffects.None, 0.0f);
            }
            sb.End();
            selectionCircleAngle += 0.05f;
        }

        public void Draw(MovableObject obj)
        {
            float angle = -(float)(obj.getDirection() + 1.5 * Math.PI);
            Vector2 pos = obj.getPosition(); //position relative to the map
            Rectangle clip = obj.getClip();
            sb.Begin();
            float x = (viewport.Width / 2) - (tileWidth / 8) + (pos.X - (viewCenter.X * clip.Width));
            float y = (viewport.Height / 2) - (tileWidth / 8) + (pos.Y - (viewCenter.Y * clip.Height));
            sb.Draw(obj.getTexture(), new Rectangle((int)x, (int)y, clip.Width, clip.Height), clip, Color.White, angle, new Vector2(clip.Width / 2, clip.Height / 2), SpriteEffects.None, 0.0f); 
            sb.End();
        }

        public void Draw(Map map)
        {
            sb.Begin();
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
                    Food food = cell.food;
                    sb.Draw(map.getTexture(), new Vector2(x, y), new Rectangle(cell.texCoordY * tileWidth, cell.texCoordX * tileWidth, tileWidth, tileWidth), Color.White);
                    if (food != null)
                        sb.Draw(food.getTexture(), new Vector2(x, y), new Rectangle(cell.texCoordY * tileWidth, cell.texCoordX * tileWidth, tileWidth, tileWidth), Color.White);
                }
            }
            sb.End();
        }
    }
}
