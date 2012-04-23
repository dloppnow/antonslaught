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
        Rectangle textBackground;
        Texture2D beetleText;
        Texture2D roachText;
        Texture2D queenText;
        Texture2D spiderText;
        Texture2D workerText;
        Texture2D soldierText;
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
            textBackground =    new Rectangle(130, 30, vp.Width - 140, vp.Height - 40);
            playButton =        new Rectangle(0, 30, 125, 25);
            optionButton =      new Rectangle(0, 60, 125, 25);
            howtoPlayButton =   new Rectangle(0, 90, 125, 25);
            aboutUsButton =     new Rectangle(0, 120, 125, 25);
            quitButton =        new Rectangle(0, 150, 125, 25);

            beetleText = content.Load<Texture2D>("beetle");
            roachText = content.Load<Texture2D>("cockroach");
            queenText = content.Load<Texture2D>("queen");
            spiderText = content.Load<Texture2D>("spider");
            workerText = content.Load<Texture2D>("worker");
            soldierText = content.Load<Texture2D>("soldier");
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
            sb.Draw(dummyTexture, textBackground, textBackColor);
            sb.DrawString(font, "Welcome to Ant Onslaught!", new Vector2(textBackground.X + 5, textBackground.Y + 5), textColor);
        }

        private void drawAbout()
        {
            sb.Draw(dummyTexture, textBackground, textBackColor);
            sb.DrawString(font, "Ant Onslaught is a game made in 72 hours for the Ludum Dare. (ludumdare.com)", new Vector2(textBackground.X + 5, textBackground.Y + 5), textColor);
            sb.DrawString(font, "It was created by: Evad, Laremere, PirateCove, and ebuch.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 30), textColor);
            sb.DrawString(font, "Programmers: Evad and PrirateCove.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 60), textColor);
            sb.DrawString(font, "Map assets: Laremere.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 90), textColor);
            sb.DrawString(font, "Character sprites and audio: ebuch.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 120), textColor);
        }

        private void drawHow()
        {
            //Text stuff
            sb.Draw(dummyTexture, textBackground, textBackColor);
            sb.DrawString(font, "There are 2 types of ants. Worker ants (Black) and soldier ants (Red).", new Vector2(textBackground.X + 5, textBackground.Y + 5), textColor);
            sb.DrawString(font, "Build them by pressing the buttons on the bottom of the screen or", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 30), textColor);
            sb.DrawString(font, "by press 1 to build workers and 2 to build soldiers.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 60), textColor);
            sb.DrawString(font, "You can select up to 13 ants by left-click-dragging across the screen. ", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 90), textColor);
            sb.DrawString(font, "Worker ants can pick up food to bring to the Queen ant,", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 120), textColor);
            sb.DrawString(font, "but, they are no good in a fight.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 150), textColor);
            sb.DrawString(font, "Soldier ants can't pick up food but they can fight against", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 180), textColor);
            sb.DrawString(font, "the larger insects that want to kill you.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 210), textColor);
            sb.DrawString(font, "Right-click to move your selected ants around.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 240), textColor);
            sb.DrawString(font, "Right-clicking on food will have make worker ants grab it and", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 270), textColor);
            sb.DrawString(font, "bring it back to the Queen while right-clicking near larger insects", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 300), textColor);
            sb.DrawString(font, "will cause soldier ants to attack.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 330), textColor);
            sb.DrawString(font, "WASD or arrow keys move the camera around.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 360), textColor);
            sb.DrawString(font, "Once larger insects die they can be picked up as food.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 390), textColor);
            sb.DrawString(font, "Collect all of the food on the map (including dead enemies) to win.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 420), textColor);
            sb.DrawString(font, "If you do not have enough nutrients to purchase a new worker and", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 450), textColor);
            sb.DrawString(font, "all of your workers die, or your queen dies you will lose.", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 480), textColor);

            //Images
            sb.DrawString(font, "Queen", new Vector2(textBackground.X + 5 + 5, textBackground.Y + 5 + 600), textColor);
            sb.Draw(queenText, new Vector2(textBackground.X + 5 + 15, textBackground.Y + 5 + 630), Color.White);

            sb.DrawString(font, "Worker", new Vector2(textBackground.X + 5 + 80, textBackground.Y + 5 + 600), textColor);
            sb.Draw(workerText, new Vector2(textBackground.X + 5 + 95, textBackground.Y + 5 + 630), Color.White);

            sb.DrawString(font, "Soldier", new Vector2(textBackground.X + 5 + 160, textBackground.Y + 5 + 600), textColor);
            sb.Draw(soldierText, new Vector2(textBackground.X + 5 + 175, textBackground.Y + 5 + 630), Color.White);

            sb.DrawString(font, "Beetle", new Vector2(textBackground.X + 5 + 245, textBackground.Y + 5 + 600), textColor);
            sb.Draw(beetleText, new Vector2(textBackground.X + 5 + 255, textBackground.Y + 5 + 630), Color.White);

            sb.DrawString(font, "Roach", new Vector2(textBackground.X + 5 + 325, textBackground.Y + 5 + 600), textColor);
            sb.Draw(roachText, new Vector2(textBackground.X + 5 + 335, textBackground.Y + 5 + 630), Color.White);

            sb.DrawString(font, "Spider", new Vector2(textBackground.X + 5 + 400, textBackground.Y + 5 + 600), textColor);
            sb.Draw(spiderText, new Vector2(textBackground.X + 5 + 410, textBackground.Y + 5 + 620), Color.White);
        }

        private void drawOption()
        {
            sb.Draw(dummyTexture, textBackground, textBackColor);
            sb.DrawString(font, "You think we had time to do options?", new Vector2(textBackground.X + 5, textBackground.Y + 5), textColor);
            sb.DrawString(font, "HAHAHAHAHAHA!!!", new Vector2(textBackground.X + 5, textBackground.Y + 5 + 30), textColor);
        }
    }
}
