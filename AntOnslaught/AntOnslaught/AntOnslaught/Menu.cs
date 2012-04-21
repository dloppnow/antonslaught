using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace AntOnslaught
{
    class Menu
    {
        SpriteBatch sb;
        KeyboardState prevKBState;
        MouseState prevMState;
        SpriteFont font;
        Rectangle quitButton;
        Rectangle playButton;
        bool paused;
        bool quit;
        bool justStarted;

        Texture2D dummyTexture;

        public Menu(SpriteBatch sb, ContentManager content)
        {
            dummyTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            this.sb = sb;
            paused = true;
            quit = false;
            justStarted = true;
            font = content.Load<SpriteFont>("Font");
            Viewport vp = sb.GraphicsDevice.Viewport;
            quitButton = new Rectangle(0, vp.Height - 25, 100, 25);
            playButton = new Rectangle(0, 0, 50, 25);
        }

        public void update(GameTime gameTime, KeyboardState kbState, MouseState mState)
        {

            if (prevKBState.IsKeyUp(Keys.Escape) && kbState.IsKeyDown(Keys.Escape))
            {
                if (paused)
                {
                    paused = false;
                    justStarted = false;
                }
                else
                {
                    paused = true;
                }
            }
            

            if (paused)
            {
                if (prevMState.LeftButton == ButtonState.Released && mState.LeftButton == ButtonState.Pressed) //button was just clicked
                {
                    if (mState.X >= quitButton.Left && mState.X <= quitButton.Right)
                    {
                        if (mState.Y >= quitButton.Top && mState.Y <= quitButton.Bottom)
                        { //quit button was pressed
                            quit = true;
                        }
                    }
                    if (mState.X >= playButton.Left && mState.X <= playButton.Right)
                    {
                        if (mState.Y >= playButton.Top && mState.Y <= playButton.Bottom)
                        { //quit button was pressed
                            paused = false;
                        }
                    }
                }
            }
            prevKBState = kbState;
            prevMState = mState;
        }

        public bool isPaused()
        {
            return paused;
        }

        public bool shouldQuit()
        {
            return quit;
        }

        public void draw()
        {
            sb.GraphicsDevice.Clear(Color.Black);
            sb.DrawString(font, "QUIT", new Vector2(quitButton.X, quitButton.Y), Color.White);
            sb.Draw(dummyTexture, playButton, Color.Orange);
            sb.DrawString(font, "PLAY", new Vector2(playButton.X, playButton.Y), Color.White);
            
        }
    }
}
