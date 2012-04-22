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
        Rectangle optionButton;
        Rectangle howtoPlayButton;
        Rectangle aboutUsButton;
        Rectangle header;
        bool paused;
        bool quit;
        bool justStarted;
        Color backgroundColor = Color.Black;
        Color textColor = Color.White;
        Color textBackColor = Color.Orange;
        TextState textState = TextState.NONE;

        Texture2D dummyTexture;

        private enum TextState
        {
            NONE,
            OPTION,
            HOWTO,
            ABOUT
        }

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
            header =            new Rectangle(vp.Width / 2 - 74, 0, 148, 25);
            playButton =        new Rectangle(0, 30, 125, 25);
            optionButton =      new Rectangle(0, 60, 125, 25);
            howtoPlayButton =   new Rectangle(0, 90, 125, 25);
            aboutUsButton =     new Rectangle(0, 120, 125, 25);
            quitButton =        new Rectangle(0, 150, 125, 25);
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
                    //Quit Button
                    if (mState.X >= quitButton.Left && mState.X <= quitButton.Right)
                    {
                        if (mState.Y >= quitButton.Top && mState.Y <= quitButton.Bottom)
                        {
                            quit = true;
                        }
                    }
                    //Play/Resume Button
                    if (mState.X >= playButton.Left && mState.X <= playButton.Right)
                    {
                        if (mState.Y >= playButton.Top && mState.Y <= playButton.Bottom)
                        {
                            paused = false;
                            justStarted = false;
                        }
                    }
                    //Option Button
                    if (mState.X >= optionButton.Left && mState.X <= optionButton.Right)
                    {
                        if (mState.Y >= optionButton.Top && mState.Y <= optionButton.Bottom)
                        {
                            if (textState == TextState.OPTION)
                            {
                                textState = TextState.NONE;
                            }
                            else
                            {
                                textState = TextState.OPTION;
                            }
                        }
                    }
                    //How to play Button
                    if (mState.X >= howtoPlayButton.Left && mState.X <= howtoPlayButton.Right)
                    {
                        if (mState.Y >= howtoPlayButton.Top && mState.Y <= howtoPlayButton.Bottom)
                        {
                            if (textState == TextState.HOWTO)
                            {
                                textState = TextState.NONE;
                            }
                            else
                            {
                                textState = TextState.HOWTO;
                            }
                        }
                    }
                    //About us Button
                    if (mState.X >= aboutUsButton.Left && mState.X <= aboutUsButton.Right)
                    {
                        if (mState.Y >= aboutUsButton.Top && mState.Y <= aboutUsButton.Bottom)
                        {
                            if (textState == TextState.ABOUT)
                            {
                                textState = TextState.NONE;
                            }
                            else
                            {
                                textState = TextState.ABOUT;
                            }
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
            sb.Begin();
            sb.GraphicsDevice.Clear(backgroundColor);
            //Draw Header
            sb.Draw(dummyTexture, header, textBackColor);
            sb.DrawString(font, "ANT ONSLAUGHT", new Vector2(header.X, header.Y), textColor);
            //Draw the buttons.
            sb.Draw(dummyTexture, quitButton, textBackColor);
            sb.DrawString(font, "QUIT", new Vector2(quitButton.X, quitButton.Y), textColor);
            sb.Draw(dummyTexture, playButton, textBackColor);
            sb.DrawString(font, justStarted ? "PLAY" : "RESUME", new Vector2(playButton.X, playButton.Y), textColor);
            sb.Draw(dummyTexture, optionButton, textBackColor);
            sb.DrawString(font, "OPTIONS", new Vector2(optionButton.X, optionButton.Y), textColor);
            sb.Draw(dummyTexture, howtoPlayButton, textBackColor);
            sb.DrawString(font, "HOW TO PLAY", new Vector2(howtoPlayButton.X, howtoPlayButton.Y), textColor);
            sb.Draw(dummyTexture, aboutUsButton, textBackColor);
            sb.DrawString(font, "ABOUT US", new Vector2(aboutUsButton.X, aboutUsButton.Y), textColor);
            //Draw other text
            switch (textState)
            {
                case TextState.NONE: drawNone(); break;
                case TextState.ABOUT: drawAbout(); break;
                case TextState.HOWTO: drawHow(); break;
                case TextState.OPTION: drawOption(); break;
            }
            sb.End();
        }

        private void drawNone()
        {

        }

        private void drawAbout()
        {

        }

        private void drawHow()
        {

        }

        private void drawOption()
        {
            Rectangle rect = new Rectangle(155, 30, 100, 25);
            sb.Draw(dummyTexture, rect, textBackColor);
            sb.DrawString(font, "ABOUT US", new Vector2(rect.X, rect.Y), textColor);
        }
    }
}
